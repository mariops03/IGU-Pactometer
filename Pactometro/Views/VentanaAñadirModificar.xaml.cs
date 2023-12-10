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
    public enum ModoVentana
    {
        Agregar,Modificar
    }

    public partial class VentanaAñadirModificar : Window
    {
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;
        private ObservableCollection<Partido> PartidosTemporales = new ObservableCollection<Partido>();
        private ProcesoElectoral ProcesoSeleccionado;
        private VentanaSecundariaViewModel viewModel;
        private ModoVentana ModoActual;
        Color colorSeleccionado;

        public VentanaAñadirModificar(ModoVentana modo, ObservableCollection<ProcesoElectoral> coleccionElecciones, ProcesoElectoral procesoSeleccionado)
        {
            InitializeComponent();
            ModoActual = modo;
            ColeccionElecciones = coleccionElecciones;
            ProcesoSeleccionado = procesoSeleccionado;
            viewModel = new VentanaSecundariaViewModel(coleccionElecciones);

            ConfigurarVentanaSegunModo();
        }

        private void ConfigurarVentanaSegunModo()
        {
            if (ModoActual == ModoVentana.Agregar)
            {
                btnAñadirModificar.Content = "Añadir Proceso";
            }
            else if (ModoActual == ModoVentana.Modificar)
            {
                btnAñadirModificar.Content = "Actualizar Proceso";
                dpFecha.SelectedDate = ProcesoSeleccionado.fecha;
                cmbTipoProceso.Text = viewModel.ObtenerParteAlfabetica(ProcesoSeleccionado.nombre);
                
                //Crear una copia de la colección de partidos del proceso seleccionado y almacenarla en una nueva colección temporal que crearemos
            }
            lvPartidos.ItemsSource = PartidosTemporales;
            lvPartidos.SelectionChanged += lvPartidos_SelectionChanged;
            dpFecha.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            txtEscaños.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            btnEliminar.IsEnabled = false;
            btnModificar.IsEnabled = false;
            seleccionadorColor.ItemsSource = typeof(Colors).GetProperties();
            seleccionadorColor.SelectedIndex = 0;
            viewModel.EleccionSeleccionada = ProcesoSeleccionado;
            viewModel.Elecciones = ColeccionElecciones;
            foreach (Partido partido in ProcesoSeleccionado.coleccionPartidos)
            {
                PartidosTemporales.Add(partido);
            }
        }
               
        private void BtnActualizarCrearProceso_Click(object sender, RoutedEventArgs e)
        {
            if (ModoActual == ModoVentana.Agregar)
            {
                // Lógica para crear un nuevo proceso electoral
                CrearProcesoElectoral();
            }
            else if (ModoActual == ModoVentana.Modificar)
            {
                // Lógica para actualizar un proceso electoral existente
                ActualizarProcesoElectoral();
            }
        }

        private void CrearProcesoElectoral()
        {
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
                MessageBox.Show("Ya existe un proceso de " + tipoProceso + " en la fecha seleccionada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("La suma de los escaños de todos los partidos debe de ser de " + numEscañosProceso + " escaños", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProcesoSeleccionado.nombre = nombreProceso;
            ProcesoSeleccionado.fecha = fechaProceso;
            ProcesoSeleccionado.numEscaños = numEscañosProceso;
            ProcesoSeleccionado.mayoriaAbsoluta = (numEscañosProceso / 2) + 1;

            //Recorre la colección de partidos temporales y añade cada partido a la colección de partidos del nuevo proceso
            foreach (Partido partido in PartidosTemporales)
            {
                ProcesoSeleccionado.coleccionPartidos.Add(partido);
            }

            ColeccionElecciones.Add(ProcesoSeleccionado);

            // Cerrar la ventana
            this.Close();
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

            // Verificar que el numero de escaños sea mayor que 0
            if (int.Parse(escañosText) <= 0)
            {
                MessageBox.Show("El número de escaños debe ser mayor que 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar la entrada de escaños
            if (!int.TryParse(escañosText, out int escañosPartido))
            {
                MessageBox.Show("Por favor, introduce un número válido para los escaños.", "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar si ya hay un partido con el mismo nombre
            if (PartidosTemporales.Any(partido => partido.Nombre.ToLower() == nombrePartido.ToLower()))
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

            viewModel.OrdenarPorEscañosPartidoDescendente(PartidosTemporales);

            // Puedes realizar otras acciones aquí, como limpiar los campos de entrada
            LimpiarCamposPartido();
        }


        private void BtnEliminarPartido_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si hay un partido seleccionado
            if (lvPartidos.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar el partido seleccionado?", "Eliminar partido político", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result == MessageBoxResult.Yes)
                {
                    // Eliminar el partido seleccionado de la colección temporal
                    Partido partidoSeleccionado = (Partido)lvPartidos.SelectedItem;
                    PartidosTemporales.Remove(partidoSeleccionado);

                    lvPartidos.ItemsSource = PartidosTemporales;
                }
            }
        }

        private void ActualizarProcesoElectoral()
        {
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

            // Verificar que no haya un proceso con la misma fecha y tipo


            string nombreProceso = $"{tipoProceso} {fechaProceso.ToString("dd-MM-yyyy")}";

            if (nombreProceso != ProcesoSeleccionado.nombre && ColeccionElecciones.Any(proceso => proceso.nombre == nombreProceso))
            {
                MessageBox.Show("Ya existe un proceso con la misma fecha y tipo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una instancia de la clase ProcesoElectoral
            ProcesoElectoral nuevoProceso = new ProcesoElectoral(nombreProceso, fechaProceso, numEscañosProceso, mayoriaAbsoluta: (numEscañosProceso / 2) + 1);

            //Recorre la colección de partidos temporales y añade cada partido a la colección de partidos del nuevo proceso
            foreach (Partido partido in PartidosTemporales)
            {
                nuevoProceso.coleccionPartidos.Add(partido);
            }

            // Borrar el proceso que ha llegado como parámetro
            ColeccionElecciones.Remove(ProcesoSeleccionado);

            ColeccionElecciones.Add(nuevoProceso);

            this.Close();

        }

        private void LimpiarCamposPartido()
        {
            txtPartido.Text = string.Empty;
            txtEscaños.Text = string.Empty;
            seleccionadorColor.SelectedIndex = 0;
        }
        private void lvPartidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPartidos != null)
            {
                btnEliminar.IsEnabled = lvPartidos.SelectedItem != null;
                btnModificar.IsEnabled = lvPartidos.SelectedItem != null;
            }
            else
            {
                btnEliminar.IsEnabled = false;
                btnModificar.IsEnabled = false;
            }
        }

        private void BtnModificarPartido_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia de la ventana VentanaModificarPartido pasandole el partido seleccionado y el proceso electoral almacenado en PartidosTemporales
            VentanaModificarPartido ventanaModificarPartido = new VentanaModificarPartido((Partido)lvPartidos.SelectedItem, PartidosTemporales);
            // Haz que sea propiedad de esta ventana
            ventanaModificarPartido.Owner = this;
            ventanaModificarPartido.ShowDialog();
            viewModel.OrdenarPorEscañosPartidoDescendente(PartidosTemporales);
        }

        private void seleccionadorColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el color seleccionado en el ComboBox
            colorSeleccionado = (Color)(seleccionadorColor.SelectedItem as PropertyInfo).GetValue(null, null);
        }

        private void PartidoHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.OrdenarPorNombreAlternar(PartidosTemporales);
            }
        }

        private void EscañosHeader_Click(object sender, MouseButtonEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.OrdenarPorEscañosAlternar(PartidosTemporales);
            }
        }
    }
}
