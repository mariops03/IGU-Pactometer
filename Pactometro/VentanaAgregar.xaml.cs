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
        public VentanaAgregar(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
        }

        private void BtnAñadirPartido_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAñadirProceso_Click(object sender, RoutedEventArgs e)
        {

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

        private void dpFecha_LostFocus(object sender, RoutedEventArgs e)
        {
            // Validar el formato después de perder el foco
            if (DateTime.TryParseExact(dpFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                // La fecha tiene el formato correcto
            }
            else
            {
                // Mostrar un mensaje de error
                MessageBox.Show("Por favor, introduce una fecha con fórmato válido: DD/MM/YYYY", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);

                // Restablecer la fecha o realizar otras acciones según sea necesario
                dpFecha.SelectedDate = null;
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


    }
}
