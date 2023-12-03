using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Lógica de interacción para VentanaModificarPartido.xaml
    /// </summary>
    public partial class VentanaModificarPartido : Window
    {
        Color colorSeleccionado;
        public VentanaModificarPartido(Partido partido)
        {
            InitializeComponent();
            // Establecer el color seleccionado por defecto
            colorSeleccionado = Colors.Black;
        }

        private void seleccionadorColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el color seleccionado en el ComboBox
            colorSeleccionado = (Color)(seleccionadorColor.SelectedItem as PropertyInfo).GetValue(null, null);

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el partido seleccionado en la ventana principal
            Partido partido = (Partido)Application.Current.Properties["partidoSeleccionado"];

            // Modificar los datos del partido
            partido.Nombre = txtNombre.Text;
            partido.Color = colorSeleccionado;
            partido.Escaños = txtEscaños.Text;

            // Actualizar la lista de partidos de la ventana principal
            MainWindow mainWindow = (MainWindow)Application.Current.Properties["ventanaPrincipal"];
            mainWindow.actualizarListaPartidos();

            // Cerrar la ventana
            this.Close();
        }
    }
}
