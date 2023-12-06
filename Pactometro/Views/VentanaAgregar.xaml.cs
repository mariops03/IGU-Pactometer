using Pactometro.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
        private ObservableCollection<Partido> PartidosTemporales;
        private bool procesoAñadido;
        private BaseViewModel viewModel;
        public VentanaAgregar(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            ColeccionElecciones = coleccionElecciones;
            PartidosTemporales = new ObservableCollection<Partido>();
            lvPartidos.SelectionChanged += lvPartidos_SelectionChanged;
            dpFecha.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            txtNumEscaños.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            txtEscaños.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            btnEliminar.IsEnabled = false;
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
            if (PartidosTemporales.Any(partido => partido.Nombre.ToLower() == nombrePartido))
            {
                MessageBox.Show("Ya hay un partido con el mismo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar si ya hay un partido con el mismo color
            if (PartidosTemporales.Any(partido => partido.Color == colorSeleccionado))
            {
                MessageBox.Show("Ya hay un partido con el mismo color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una instancia de la clase Partido y agregarla a la colección temporal
            Partido nuevoPartido = new Partido(nombrePartido, escañosPartido, colorSeleccionado);
            PartidosTemporales.Add(nuevoPartido);

            // Ordenar la lista de partidos de mayor a menor por el número de escaños
            PartidosTemporales = new ObservableCollection<Pactometro.Partido>(PartidosTemporales.OrderByDescending(partido => partido.Escaños));

            // Actualizar la lista de partidos en el ListView
            lvPartidos.ItemsSource = PartidosTemporales;

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
                PartidosTemporales.Remove(partidoSeleccionado);

                // Actualizar la lista de partidos en el ListView
                lvPartidos.ItemsSource = null;
                lvPartidos.ItemsSource = PartidosTemporales;
            }
        }


        private void BtnAñadirProceso_Click(object sender, RoutedEventArgs e)
        {
            // Restablecer la variable procesoAñadido
            procesoAñadido = false;

            // Obtener datos desde la interfaz de usuario para el nuevo proceso electoral
            string tipoProceso = (cmbTipoProceso.SelectedItem as ComboBoxItem)?.Content?.ToString()?.Trim();

            // Validar que no haya campos vacíos
            if (string.IsNullOrEmpty(tipoProceso) || dpFecha.SelectedDate == null || string.IsNullOrEmpty(txtNumEscaños.Text))
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

            int numEscañosProceso;

            // Validar la entrada de escaños
            if (!int.TryParse(txtNumEscaños.Text, out numEscañosProceso))
            {
                MessageBox.Show("Por favor, introduce un número válido para el número de escaños.", "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que no haya un proceso con el mismo nombre
            string nombreProceso = $"{tipoProceso} {fechaProceso.ToString("dd-MM-yyyy")}";
            if (ColeccionElecciones.Any(proceso => proceso.nombre == nombreProceso))
            {
                MessageBox.Show("Ya hay un proceso con el mismo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que hay al menos un partido
            if (PartidosTemporales.Count == 0)
            {
                MessageBox.Show("Debes añadir al menos un partido antes de crear un nuevo proceso.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que la suma de escaños de los partidos sea igual al número de escaños del proceso
            if (PartidosTemporales.Sum(partido => partido.Escaños) != numEscañosProceso)
            {
                MessageBox.Show("La suma de los escaños de los partidos no es igual al número de escaños del proceso.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una instancia de la clase ProcesoElectoral
            ProcesoElectoral nuevoProceso = new ProcesoElectoral(nombreProceso, fechaProceso, numEscañosProceso, mayoriaAbsoluta: (numEscañosProceso / 2) + 1);

            // Añadir los partidos a la colección de partidos del nuevo proceso
            foreach (Partido partido in PartidosTemporales)
            {
                nuevoProceso.coleccionPartidos.Add(partido);
            }

            // Limpiar la colección temporal de partidos
            PartidosTemporales.Clear();

            // Agregar el nuevo proceso a la colección principal de forma ordenada por fecha de mayor a menor
            ColeccionElecciones.Add(nuevoProceso);

            // Puedes realizar otras acciones aquí, como limpiar otros campos de entrada
            LimpiarCamposProceso();

            viewModel.OrdenarPorFecha(ColeccionElecciones);
            // Marcar el proceso como añadido
            procesoAñadido = true;
        }

        private void LimpiarCamposProceso()
        {
            cmbTipoProceso.SelectedIndex = -1;
            dpFecha.SelectedDate = null;
            txtNumEscaños.Text = string.Empty;
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
        }

        Color colorSeleccionado;
        private void seleccionadorColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el color seleccionado en el ComboBox
            colorSeleccionado= (Color)(seleccionadorColor.SelectedItem as PropertyInfo).GetValue(null, null);

        }
    }
}
