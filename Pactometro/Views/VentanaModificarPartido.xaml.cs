using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace Pactometro
{
    public partial class VentanaModificarPartido : Window
    {
        Color colorSeleccionado;
        Partido Partido;
        ObservableCollection<Partido> ColeccionPartidos;
        ObservableCollection<Partido> PartidosTemporales; 

        public VentanaModificarPartido(Partido partido, ObservableCollection<Partido> coleccionPartidos)
        {
            InitializeComponent();
            Partido = partido;
            ColeccionPartidos = coleccionPartidos;
            PartidosTemporales = new ObservableCollection<Partido>();
            txtEscaños.PreviewTextInput += Validaciones.AllowOnlyNumbers;
            seleccionadorColor.ItemsSource = typeof(Colors).GetProperties();
            // Establecer el color predeterminado
            seleccionadorColor.SelectedIndex = 0;

            // Establecer los valores iniciales de los campos
            txtNombre.Text = Partido.Nombre;
            txtEscaños.Text = Partido.Escaños.ToString();
            seleccionadorColor.SelectedItem = typeof(Colors).GetProperties().FirstOrDefault(prop => (Color)prop.GetValue(null, null) == Partido.Color);

        }

        private void seleccionadorColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el color seleccionado en el ComboBox
            colorSeleccionado = (Color)(seleccionadorColor.SelectedItem as PropertyInfo).GetValue(null, null);

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            // Obtener datos desde la interfaz de usuario
            string nombrePartido = txtNombre.Text.Trim();
            string escañosText = txtEscaños.Text.Trim();

            // Validar que no haya campos vacíos
            if (string.IsNullOrEmpty(nombrePartido) || string.IsNullOrEmpty(escañosText))
            {
                MessageBox.Show("Por favor, completa todos los campos para modificar el partido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Verificar que el numero de escaños sea mayor que 0
            if (int.Parse(escañosText) <= 0)
            {
                MessageBox.Show("Por favor, introduce un número válido para los escaños.\nSi deseas eliminar el partido, simplemente seleccionalo y pulsa el boton de eliminar", "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar la entrada de escaños
            if (!int.TryParse(escañosText, out int escañosPartido))
            {
                MessageBox.Show("Por favor, introduce un número válido para los escaños.", "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar si ya hay un partido con el mismo color
            

            PartidosTemporales.Clear();


            foreach (Partido partidoProceso in ColeccionPartidos)
            {
                PartidosTemporales.Add(partidoProceso);

            }
                

            PartidosTemporales.Remove(Partido);

            if (PartidosTemporales.Any(partido => partido.Color == colorSeleccionado))
            {
                MessageBox.Show("Ya hay un partido con el mismo color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Verificar si ya hay un partido con el mismo nombre
            if (PartidosTemporales.Any(partido => partido.Nombre.ToLower() == nombrePartido.ToLower()))
            {
                MessageBox.Show("Ya hay un partido con el mismo nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Partido partidoModificado = new Partido(nombrePartido, escañosPartido, colorSeleccionado);
            PartidosTemporales.Add(partidoModificado);

            ColeccionPartidos.Remove(Partido);
            
            ColeccionPartidos.Add(partidoModificado);

            this.Close();
        }
    }
}
