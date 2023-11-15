using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Pactometro
{
    public partial class MainWindow : Window
    {
        // Definir el evento para notificar la selección
        public event EventHandler<string> ProcesoEleccionSeleccionado;

        private VentanaSecundaria ventanaSecundaria;
        private ObservableCollection<ProcesoElectoral> coleccionElecciones;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
            coleccionElecciones = new ObservableCollection<ProcesoElectoral>();
            Closed += MainWindow_Closed; // Suscribe un controlador para el evento Closed de la ventana principal
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Abre la ventana secundaria automáticamente al cargar la aplicación
            AbrirVentanaSecundaria();
        }

        private void menuVentanaSecundaria(object sender, RoutedEventArgs e)
        {
            AbrirVentanaSecundaria();
        }

        private void AbrirVentanaSecundaria()
        {
            if (ventanaSecundaria == null)
            {
                // Crear una nueva instancia de VentanaSecundaria con la colección de elecciones
                ventanaSecundaria = new VentanaSecundaria(coleccionElecciones);

                double distanciaEntreVentanas = 10; // Puedes ajustar esto a tu preferencia
                double nuevaPosX = Left + Width + distanciaEntreVentanas;
                double nuevaPosY = Top;

                ventanaSecundaria.Left = nuevaPosX;
                ventanaSecundaria.Top = nuevaPosY;

                // Suscribirse al evento de la VentanaSecundaria para recibir notificaciones de selección
                ventanaSecundaria.ProcesoEleccionSeleccionado += VentanaSecundaria_ProcesoEleccionSeleccionado;

                ventanaSecundaria.Closed += VentanaSecundaria_Closed; // Suscribe un controlador para el evento Closed
                ventanaSecundaria.Show(); // Muestra la ventana secundaria
            }
            else
            {
                ventanaSecundaria.Activate(); // Si la ventana ya existe, ábrela y enfócala
            }
        }

        private void VentanaSecundaria_ProcesoEleccionSeleccionado(object sender, string nombreProceso)
        {
            // Actualizar el TextBlock con el nombre del proceso electoral seleccionado
            txtTitulo.Text = nombreProceso;
        }

        private void VentanaSecundaria_Closed(object sender, EventArgs e)
        {
            // Manejar el evento Closed de la ventana secundaria si es necesario
            ventanaSecundaria = null; // Liberar la referencia a la ventana secundaria
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Cierra la aplicación cuando la ventana principal se cierra
        }

        private void menuSalir(object sender, RoutedEventArgs e)
        {
            Close(); // Cierra la ventana principal, lo que también activará el evento Closed
        }
    }
}
