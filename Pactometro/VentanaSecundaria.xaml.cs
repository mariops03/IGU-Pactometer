using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        public VentanaSecundaria()
        {
            InitializeComponent();
        }

        private void mainTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainTable.ItemsSource = coleccionElecciones;
            // Verificar si se ha seleccionado algún elemento
            if (mainTable.SelectedItem != null)
            {
                // Obtener el objeto ProcesoElectoral seleccionado
                ProcesoElectoral procesoSeleccionado = mainTable.SelectedItem as ProcesoElectoral;

                // Verificar si el objeto no es nulo
                if (procesoSeleccionado != null)
                {
                    // Aquí puedes realizar acciones con el objeto seleccionado, por ejemplo, mostrar información en otros controles
                    string nombre = procesoSeleccionado.nombre;
                    DateTime fecha = procesoSeleccionado.fecha;
                    int numEscaños = procesoSeleccionado.numEscaños;
                    int mayoriaAbsoluta = procesoSeleccionado.mayoriaAbsoluta;

                    // Realizar acciones con los datos seleccionados, como mostrarlos en etiquetas o realizar otros cálculos
                    // Por ejemplo, mostrarlos en etiquetas de texto
                   /* nombreLabel.Content = "Nombre: " + nombre;
                    fechaLabel.Content = "Fecha: " + fecha.ToString();
                    numEscañosLabel.Content = "Número de Escaños: " + numEscaños.ToString();
                    mayoriaAbsolutaLabel.Content = "Mayoría Absoluta: " + mayoriaAbsoluta.ToString();*/
                }
            }
        }

    }
}
