using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pactometro
{
    public partial class MainWindow : Window
    {
        // Definir el evento para notificar la selección
        public event EventHandler<ProcesoElectoral> ProcesoEleccionSeleccionado;

        private VentanaSecundaria ventanaSecundaria;
        private ObservableCollection<ProcesoElectoral> coleccionElecciones;
        private ProcesoElectoral procesoElectoralActual;

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

        //Hacer que la ventana principal maneje el evento de selección de la ventana secundaria, recibiendo una colección de partidos
        private void VentanaSecundaria_ProcesoEleccionSeleccionado(object sender, ProcesoElectoral procesoElectoral)
        {

            procesoElectoralActual = procesoElectoral;
        // Actualizar el TextBlock con el nombre del proceso electoral seleccionado
        
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

        private void Grafico1_Click(object sender, RoutedEventArgs e)
        {
            if (procesoElectoralActual != null)
            {
                // Llamar al método que crea el gráfico utilizando el proceso electoral actual
                txtTitulo.Text = procesoElectoralActual.nombre;

                // Limpiar el Canvas antes de agregar nuevas barras y la leyenda
                chartCanvas.Children.Clear();

                double barWidth = 30; // Ancho fijo de las barras
                double barSpacing = 10; // Espaciado entre barras

                double xPosition = 30; // Posición inicial de la primera barra
                double legendY = 10; // Altura de cada elemento en la leyenda

                double maxEscaños = procesoElectoralActual.coleccionPartidos.First().Escaños;

                int numEtiquetas = Math.Max(5, (int)Math.Ceiling(maxEscaños / 10.0));

                int paso = Math.Max(1, (int)Math.Ceiling(maxEscaños / (double)(numEtiquetas - 1)));

                for (int i = 0; i <= maxEscaños; i += paso)
                {
                    TextBlock numeroEscaños = new TextBlock();
                    numeroEscaños.Text = i.ToString();
                    numeroEscaños.TextAlignment = TextAlignment.Center;
                    numeroEscaños.FontSize = 10;

                    // Calcular la posición de la etiqueta para evitar superposiciones
                    double yPosition = chartCanvas.ActualHeight - (i * (chartCanvas.ActualHeight / maxEscaños)) - 10;

                    Canvas.SetLeft(numeroEscaños, 0);
                    Canvas.SetTop(numeroEscaños, yPosition);

                    chartCanvas.Children.Add(numeroEscaños);

                    // Asegurar que la última etiqueta esté a la altura del rectángulo del partido con más escaños
                    if (i + paso >= maxEscaños)
                    {
                        TextBlock ultimaEtiqueta = new TextBlock();
                        ultimaEtiqueta.Text = maxEscaños.ToString();
                        ultimaEtiqueta.TextAlignment = TextAlignment.Center;
                        ultimaEtiqueta.FontSize = 10;

                        // Calcular la posición de la última etiqueta a la altura del rectángulo del partido con más escaños
                        double yUltimaEtiqueta = chartCanvas.ActualHeight - (maxEscaños * (chartCanvas.ActualHeight / maxEscaños)) - 10;

                        Canvas.SetLeft(ultimaEtiqueta, 0);
                        Canvas.SetTop(ultimaEtiqueta, yUltimaEtiqueta);

                        chartCanvas.Children.Add(ultimaEtiqueta);
                        break;
                    }

                }

                foreach (var partido in procesoElectoralActual.coleccionPartidos)
                {
                    double barHeight = (partido.Escaños / maxEscaños) * (chartCanvas.ActualHeight - legendY); // Escalar las barras

                    Rectangle barra = new Rectangle();
                    barra.Width = barWidth;
                    barra.Height = barHeight;

                    try
                    {
                        barra.Fill = (Brush)new BrushConverter().ConvertFromString(partido.Color);
                    }
                    catch (FormatException)
                    {
                        barra.Fill = Brushes.Gray;
                    }

                    Canvas.SetLeft(barra, xPosition);
                    Canvas.SetTop(barra, chartCanvas.ActualHeight - barHeight);

                    chartCanvas.Children.Add(barra);

                    // Agregar el nombre del partido debajo de la barra con tamaño de letra ajustado
                    TextBlock nombrePartido = new TextBlock();
                    nombrePartido.Text = partido.Nombre;
                    nombrePartido.TextAlignment = TextAlignment.Center;
                    nombrePartido.FontSize = 10; // Tamaño inicial de la letra
                    nombrePartido.MaxWidth = barWidth; // Ancho máximo para ajustar el tamaño
                    nombrePartido.TextTrimming = TextTrimming.CharacterEllipsis; // Truncar el texto si es demasiado largo

                    // Ajustar el tamaño de la letra
                    while (nombrePartido.DesiredSize.Width > barWidth)
                    {
                        nombrePartido.FontSize--;
                    }

                    Canvas.SetLeft(nombrePartido, xPosition);
                    Canvas.SetTop(nombrePartido, chartCanvas.ActualHeight + 5); // Ajusta según sea necesario
                    chartCanvas.Children.Add(nombrePartido);

                    // Actualizar las posiciones para la siguiente barra
                    xPosition += barWidth + barSpacing;
                }
            }
            else
            {
                // Manejar el caso en el que no hay un proceso electoral actual seleccionado
                // Puedes mostrar un mensaje de error, por ejemplo
                MessageBox.Show("No se ha seleccionado un proceso electoral.");
            }
            

        }

        private void Grafico2_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si hay un proceso electoral actual antes de crear el gráfico
            if (procesoElectoralActual != null)
            {
                // Obtener otros procesos electorales equivalentes para la comparación
                Collection<ProcesoElectoral> procesosEquivalentes = ObtenerProcesosEquivalentesPorNombre(procesoElectoralActual);

                // Llamar al método que crea el gráfico comparativo
                CrearGraficoComparativo(procesoElectoralActual, procesosEquivalentes);
            }
            else
            {
                // Manejar el caso en el que no hay un proceso electoral actual seleccionado
                // Puedes mostrar un mensaje de error, por ejemplo
                MessageBox.Show("No se ha seleccionado un proceso electoral.");
            }
        }

        private Collection<ProcesoElectoral> ObtenerProcesosEquivalentesPorNombre(ProcesoElectoral procesoElectoralBase)
        {
            // Implementa la lógica para obtener otros procesos electorales equivalentes por la parte de caracteres del nombre
            // Aquí puedes retornar una colección ficticia para propósitos de ejemplo
            Collection<ProcesoElectoral> procesosEquivalentes = new Collection<ProcesoElectoral>();

            foreach (ProcesoElectoral proceso in coleccionElecciones)
            {
                // Filtrar los procesos equivalentes por la parte de caracteres del nombre (sin números)
                if (ObtenerParteAlfabética(proceso.nombre) == ObtenerParteAlfabética(procesoElectoralBase.nombre))
                {
                    procesosEquivalentes.Add(proceso);
                }
            }

            return procesosEquivalentes;
        }

        // Función auxiliar para obtener la parte alfabética del nombre
        private string ObtenerParteAlfabética(string nombre)
        {
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

        private void CrearGraficoComparativo(ProcesoElectoral procesoElectoralBase, Collection<ProcesoElectoral> procesosComparativos)
        {
            // Limpiar el Canvas antes de agregar nuevos elementos
            chartCanvas.Children.Clear();
            //Establecer el título del gráfico
            txtTitulo.Text = "Comparación de " + procesoElectoralBase.nombre;
            //Establecer el ancho de las barras
            double barWidth = 20;
            //Establecer el espaciado entre barras
            double barSpacing = 10;
            //Establecer la posición inicial de la primera barra
            double xPosition = 30;

            //Recorrer los procesos electorales comparativos, creando una barra para cada uno


            
        }





        private void Grafico3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
