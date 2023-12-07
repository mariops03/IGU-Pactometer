using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Pactometro.ViewModels;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaModificar.xaml
    /// </summary>
    public partial class VentanaModificar : Window
    {
        private ObservableCollection<Partido> PartidosTemporales;
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;
        private ProcesoElectoral ProcesoSeleccionado;
        private BaseViewModel viewModel;
        Color colorSeleccionado;
        public VentanaModificar(ObservableCollection<ProcesoElectoral> coleccionElecciones, ProcesoElectoral procesoSeleccionado)
        {
            InitializeComponent();
            ColeccionElecciones = coleccionElecciones;
            ProcesoSeleccionado = procesoSeleccionado;
            PartidosTemporales = new ObservableCollection<Partido>();
            PartidosTemporales = procesoSeleccionado.coleccionPartidos;
            lvPartidos.SelectionChanged += lvPartidos_SelectionChanged;
            dpFecha.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            txtEscaños.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            btnEliminar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            seleccionadorColor.ItemsSource = typeof(Colors).GetProperties();
            // Establecer el color predeterminado
            seleccionadorColor.SelectedIndex = 0;
            dpFecha.SelectedDate = procesoSeleccionado.fecha;
            cmbTipoProceso.Text = ObtenerParteAlfabetica(procesoSeleccionado.nombre);
            lvPartidos.ItemsSource = PartidosTemporales;
            lvPartidos.KeyDown += LvPartidos_KeyDown;
            viewModel = new BaseViewModel();
            viewModel.EleccionSeleccionada = procesoSeleccionado;
        }

        private string ObtenerParteAlfabetica(string nombre)
        {
            // Verificar si el nombre es null
            if (nombre == null)
            {
                throw new ArgumentNullException(nameof(nombre));
            }

            // Buscar la posición del último espacio en blanco
            int indiceUltimoEspacio = nombre.LastIndexOf(' ');

            // Verificar si se encontró un espacio en blanco
            if (indiceUltimoEspacio >= 0)
            {
                // Obtener la parte alfabética antes del último espacio en blanco
                return nombre.Substring(0, indiceUltimoEspacio);
            }

            // En caso de que no haya espacio en blanco, devolver el nombre original
            return nombre;
        }

        private void BtnModificarPartido_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia de la ventana VentanaModificarPartido pasandole el partido seleccionado y el proceso electoral almacenado en PartidosTemporales
            VentanaModificarPartido ventanaModificarPartido = new VentanaModificarPartido((Partido)lvPartidos.SelectedItem, ProcesoSeleccionado);
            // Haz que sea propiedad de esta ventana
            ventanaModificarPartido.Owner = this;
            // Muestra la ventana
            ventanaModificarPartido.ShowDialog();
            // Ordenar la lista de partidos de mayor a menor por el número de escaños
            PartidosTemporales = new ObservableCollection<Pactometro.Partido>(PartidosTemporales.OrderByDescending(partido => partido.Escaños));
        }

        
        private void seleccionadorColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el color seleccionado en el ComboBox
            colorSeleccionado = (Color)(seleccionadorColor.SelectedItem as PropertyInfo).GetValue(null, null);

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

        private void LimpiarCamposPartido()
        {
            txtPartido.Text = string.Empty;
            txtEscaños.Text = string.Empty;
            seleccionadorColor.SelectedIndex = 0;
        }

        private void BtnActualizarProceso_Click(object sender, RoutedEventArgs e)
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

            //Si es un proceso de tipo Congreso, validar que el número de escaños sea mayor o igual que 350
            int numEscañosProceso = tipoProceso == "Elecciones Generales" ? 350 : 81;

            // Verificar que hay al menos un partido
            if (PartidosTemporales.Count == 0)
            {
                MessageBox.Show("Debes añadir al menos un partido antes de crear un nuevo proceso.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar que la suma de escaños de los partidos sea igual al número de escaños del proceso
            if (PartidosTemporales.Sum(partido => partido.Escaños) != numEscañosProceso)
            {
                MessageBox.Show("La suma de los escaños de todos los partidos debe de ser de " + numEscañosProceso + " escaños", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string nombreProceso = $"{tipoProceso} {fechaProceso.ToString("dd-MM-yyyy")}";

            ColeccionElecciones.Remove(ColeccionElecciones.Where(proceso => proceso.nombre == nombreProceso).FirstOrDefault());

            if (ColeccionElecciones.Any(proceso => proceso.nombre == nombreProceso))
            {
                MessageBox.Show("Ya hay un proceso con el mismo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una instancia de la clase ProcesoElectoral
            ProcesoElectoral nuevoProceso = new ProcesoElectoral(nombreProceso, fechaProceso, numEscañosProceso, mayoriaAbsoluta: (numEscañosProceso / 2) + 1);

            foreach (Partido partido in PartidosTemporales)
            {
                nuevoProceso.coleccionPartidos.Add(partido);
            }
            

            // Limpiar la colección temporal de partidos
            PartidosTemporales.Clear();

            // Agregar el nuevo proceso a la colección principal de forma ordenada por fecha de mayor a menor

            // Borrar el proceso que ha llegado como parámetro
            ColeccionElecciones.Remove(ProcesoSeleccionado);

            ColeccionElecciones.Add(nuevoProceso);

            this.Close();

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

        private void lvPartidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPartidos != null)
            {
                btnEliminar.IsEnabled = lvPartidos.SelectedItem != null;
                btnModificar.IsEnabled = lvPartidos.SelectedItem != null;
                btnAñadir.IsDefault = false;
            }
            else
            {
                btnEliminar.IsEnabled = false;
                btnModificar.IsEnabled = false;
                btnAñadir.IsDefault = true;
            }
        }

        private void LvPartidos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && lvPartidos.SelectedItem != null)
            {
                BtnEliminarPartido_Click(sender, e);

            }
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnAñadir.IsDefault = true;
            btnModificar.IsDefault = false;
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
