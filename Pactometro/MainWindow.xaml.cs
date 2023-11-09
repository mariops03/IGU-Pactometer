using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            VentanaSecundaria ventanaSecundaria = new VentanaSecundaria();

            //ventanaSecundaria.mainTable.ItemsSource = coleccionElecciones;

            // Calcular la posición de la ventana secundaria en relación con la ventana principal
            double distanciaEntreVentanas = 10; // Puedes ajustar esto a tu preferencia
            double nuevaPosX = Left + Width + distanciaEntreVentanas;
            double nuevaPosY = Top;

            // Establecer la posición de la ventana secundaria
            ventanaSecundaria.Left = nuevaPosX;
            ventanaSecundaria.Top = nuevaPosY;

            // Mostrar la ventana secundaria
            ventanaSecundaria.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
