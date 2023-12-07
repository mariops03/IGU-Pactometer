using Pactometro.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaAgregar.xaml
    /// </summary>
    /// 
    
    public partial class VentanaAgregar : Window
    {
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;
        private BaseViewModel viewModel;
        ProcesoElectoral nuevoProceso;
        public VentanaAgregar(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            ColeccionElecciones = coleccionElecciones;
            nuevoProceso = new ProcesoElectoral();
            lvPartidos.SelectionChanged += lvPartidos_SelectionChanged;
            dpFecha.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            txtEscaños.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            btnEliminar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            seleccionadorColor.ItemsSource = typeof(Colors).GetProperties();
            // Establecer el color predeterminado
            seleccionadorColor.SelectedIndex = 0;
            viewModel = new BaseViewModel();
        }

        private void BtnAñadirPartido_Click(object sender, RoutedEventArgs e)
        {
            // Obtener datos desde la interfaz de usuario
            string nombrePartido = txtPartido.Text.Trim();
            string escañosText = txtEscaños.Text.Trim();

            // Validar que no haya campos vacíos
            if (string.IsNullOrEmpty(nombrePartido) || string.IsNullOrEmpty(escañosText))
            {
                MessageBox.Show("Por favor, completa todos los campos para añadir un partido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar la entrada de escaños
            if (!int.TryParse(escañosText, out int escañosPartido))
            {
                MessageBox.Show("Por favor, introduce un número válido para los escaños.", "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar si ya hay un partido con el mismo nombre
            if (nuevoProceso.coleccionPartidos.Any(partido => partido.Nombre.ToLower() == nombrePartido))
            {
                MessageBox.Show("Ya hay un partido con el mismo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar si ya hay un partido con el mismo color
            if (nuevoProceso.coleccionPartidos.Any(partido => partido.Color == colorSeleccionado))
            {
                MessageBox.Show("Ya hay un partido con el mismo color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una instancia de la clase Partido y agregarla a la colección temporal
            Partido nuevoPartido = new Partido(nombrePartido, escañosPartido, colorSeleccionado);

            nuevoProceso.coleccionPartidos.Add(nuevoPartido);

            nuevoProceso.coleccionPartidos = new ObservableCollection<Partido>(nuevoProceso.coleccionPartidos.OrderByDescending(partido => partido.Escaños));

            viewModel.EleccionSeleccionada = nuevoProceso;

            // Actualizar la lista de partidos en el ListView
            lvPartidos.ItemsSource = nuevoProceso.coleccionPartidos;

            // Puedes realizar otras acciones aquí, como limpiar los campos de entrada
            LimpiarCamposPartido();
        }


        private void BtnEliminarPartido_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si hay un partido seleccionado
            if (lvPartidos.SelectedItem != null)
            {
                // Eliminar el partido seleccionado de la colección temporal
                Partido partidoSeleccionado = (Partido)lvPartidos.SelectedItem;
                nuevoProceso.coleccionPartidos.Remove(partidoSeleccionado);

                nuevoProceso.coleccionPartidos = new ObservableCollection<Partido>(nuevoProceso.coleccionPartidos.OrderByDescending(partido => partido.Escaños));

                lvPartidos.ItemsSource = nuevoProceso.coleccionPartidos;
            }
        }


        private void BtnAñadirProceso_Click(object sender, RoutedEventArgs e)
        {

            // Obtener datos desde la interfaz de usuario para el nuevo proceso electoral
            string tipoProceso = (cmbTipoProceso.SelectedItem as ComboBoxItem)?.Content?.ToString()?.Trim();

            // Validar que no haya campos vacíos
            if (string.IsNullOrEmpty(tipoProceso) || dpFecha.SelectedDate == null)
            {
                MessageBox.Show("Por favor, completa todos los campos para añadir un proceso.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar la fecha
            if (!DateTime.TryParseExact(dpFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaProceso))
            {
                MessageBox.Show("Por favor, introduce una fecha con formato válido: DD/MM/YYYY", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Si es un proceso de elecciones generales, tiene 350 escaños
            int numEscañosProceso = tipoProceso == "Elecciones Generales" ? 350 : 81;


            // Verificar que no haya un proceso con el mismo nombre
            string nombreProceso = $"{tipoProceso} {fechaProceso.ToString("dd-MM-yyyy")}";
            if (ColeccionElecciones.Any(proceso => proceso.nombre == nombreProceso))
            {
                MessageBox.Show("Ya hay un proceso con el mismo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que hay al menos un partido
            if (nuevoProceso.coleccionPartidos.Count == 0)
            {
                MessageBox.Show("Debes añadir al menos un partido antes de crear un nuevo proceso.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que la suma de escaños de los partidos sea igual al número de escaños del proceso
            if (nuevoProceso.coleccionPartidos.Sum(partido => partido.Escaños) != numEscañosProceso)
            {
                MessageBox.Show("La suma de los escaños de todos los partidos debe de ser de "+numEscañosProceso+ " escaños", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            nuevoProceso.nombre = nombreProceso;
            nuevoProceso.fecha = fechaProceso;
            nuevoProceso.numEscaños = numEscañosProceso;
            nuevoProceso.mayoriaAbsoluta = (numEscañosProceso / 2) + 1;

            // Agregar el nuevo proceso a la colección principal de forma ordenada por fecha de mayor a menor
            ColeccionElecciones.Add(nuevoProceso);

            // Puedes realizar otras acciones aquí, como limpiar otros campos de entrada
            LimpiarCamposProceso();

            viewModel.OrdenarPorFecha();

            // Cerrar la ventana
            this.Close();
        }

        private void BtnModificarPartido_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia de la ventana VentanaModificarPartido pasandole el partido seleccionado y el proceso electoral almacenado en PartidosTemporales
            VentanaModificarPartido ventanaModificarPartido = new VentanaModificarPartido((Partido)lvPartidos.SelectedItem, nuevoProceso);
            // Haz que sea propiedad de esta ventana
            ventanaModificarPartido.Owner = this;
            // Muestra la ventana
            ventanaModificarPartido.ShowDialog();
            // Ordenar la lista de partidos de mayor a menor por el número de escaños
            nuevoProceso.coleccionPartidos = new ObservableCollection<Pactometro.Partido>(nuevoProceso.coleccionPartidos.OrderByDescending(partido => partido.Escaños));
        }

        private void LimpiarCamposProceso()
        {
            cmbTipoProceso.SelectedIndex = -1;
            dpFecha.SelectedDate = null;
        }

        private void LimpiarCamposPartido()
        {
            txtPartido.Text = string.Empty;
            txtEscaños.Text = string.Empty;
            seleccionadorColor.SelectedIndex = 0;
        }

        private void lvPartidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEliminar.IsEnabled = lvPartidos.SelectedItem != null;
            btnModificar.IsEnabled = lvPartidos.SelectedItem != null;
        }

        Color colorSeleccionado;
        private void seleccionadorColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el color seleccionado en el ComboBox
            colorSeleccionado= (Color)(seleccionadorColor.SelectedItem as PropertyInfo).GetValue(null, null);

        }

        private void PartidoHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.OrdenarPorNombrePartido();
            }
        }

        private void EscañosHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.OrdenarPorEscañosPartido();
            }
        }

    }
}
