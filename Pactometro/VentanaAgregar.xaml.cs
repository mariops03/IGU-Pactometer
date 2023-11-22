using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        public VentanaAgregar(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            ColeccionElecciones = coleccionElecciones;
            PartidosTemporales = new ObservableCollection<Partido>();
            lvPartidos.SelectionChanged += lvPartidos_SelectionChanged;
            btnEliminar.IsEnabled = false;
        }


        private void BtnAñadirPartido_Click(object sender, RoutedEventArgs e)
        {
            // Obtener datos desde la interfaz de usuario
            string nombrePartido = txtPartido.Text.Trim();
            string escañosText = txtEscaños.Text.Trim();
            string colorPartido = txtColor.Text.Trim();

            // Validar que no haya campos vacíos
            if (string.IsNullOrEmpty(nombrePartido) || string.IsNullOrEmpty(escañosText) || string.IsNullOrEmpty(colorPartido))
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
            if (PartidosTemporales.Any(partido => partido.Color.ToLower() == colorPartido))
            {
                MessageBox.Show("Ya hay un partido con el mismo color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crear una instancia de la clase Partido y agregarla a la colección temporal
            Partido nuevoPartido = new Partido(nombrePartido, escañosPartido, colorPartido);
            PartidosTemporales.Add(nuevoPartido);

            // Ordenar la lista de partidos de mayor a menor por el número de escaños
            PartidosTemporales = new ObservableCollection<Pactometro.Partido>(PartidosTemporales.OrderByDescending(partido => partido.Escaños));

            // Actualizar la lista de partidos en el ListView
            lvPartidos.ItemsSource = PartidosTemporales;

            // Puedes realizar otras acciones aquí, como limpiar los campos de entrada
            LimpiarCamposPartido();
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                System.Windows.Media.Color selectedColor = e.NewValue.Value;
                MessageBox.Show($"Color seleccionado: {selectedColor.ToString()}");
            }
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

            // Verificar que los colores de los partidos no se repitan
            HashSet<string> coloresUnicos = new HashSet<string>();
            foreach (Partido partido in PartidosTemporales)
            {
                if (!coloresUnicos.Add(partido.Color.ToLower())) // Convertir a minúsculas para la comparación
                {
                    MessageBox.Show("Dos partidos no pueden tener el mismo color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                nuevoProceso.coleccionPartidos.Add(partido);
            }

            // Limpiar la colección temporal de partidos
            PartidosTemporales.Clear();

            // Agregar el nuevo proceso a la colección principal
            ColeccionElecciones.Add(nuevoProceso);

            // Puedes realizar otras acciones aquí, como limpiar otros campos de entrada
            LimpiarCamposProceso();

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
            txtColor.Text = string.Empty;
        }

        private void dpFecha_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Permitir números y el carácter '/'
            if (!char.IsDigit(e.Text, 0) && e.Text != "/")
            {
                // Si no es un número ni '/', marcar el evento como manejado para evitar que se escriba
                e.Handled = true;
            }
        }


        private void txtNumEscaños_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Permitir solo números
            if (!char.IsDigit(e.Text, 0))
            {
                // Si no es un número, marcar el evento como manejado para evitar que se escriba
                e.Handled = true;
            }
        }

        private void txtEscaños_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Permitir solo números
            if (!char.IsDigit(e.Text, 0))
            {
                // Si no es un número, marcar el evento como manejado para evitar que se escriba
                e.Handled = true;
            }
        }

        private void lvPartidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEliminar.IsEnabled = lvPartidos.SelectedItem != null;
        }
    }
}
