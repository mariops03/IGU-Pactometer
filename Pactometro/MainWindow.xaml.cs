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
        private bool grafico1 = false;
        private bool grafico2 = false;
        private bool grafico3 = false;

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
            // Suscribir al evento SizeChanged de la ventana o del contenedor que contiene el Canvas
            SizeChanged += Window_SizeChanged;

            // Llamar al método que crea el gráfico utilizando el proceso electoral actual
            mostrarGrafico1();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Verificar si se ha deseleccionado el proceso actual
            if (procesoElectoralActual == null)
            {
                // Manejar el caso en el que no hay un proceso electoral actual seleccionado
                // Puedes mostrar un mensaje de error, por ejemplo
                MessageBox.Show("No se ha seleccionado un proceso electoral.");
                return; // Salir del método si no hay proceso seleccionado
            }

            // Actualizar el gráfico cuando cambie el tamaño de la ventana
            if (grafico1 == true)
            {
                mostrarGrafico1();
            }
            else if (grafico2 == true)
            {
                // Obtener otros procesos electorales equivalentes para la comparación
                Collection<ProcesoElectoral> procesosEquivalentes = ObtenerProcesosEquivalentesPorNombre(procesoElectoralActual);

                // Llamar al método que crea el gráfico comparativo
                mostrarGrafico2(procesosEquivalentes);
            }
            else if (grafico3 == true)
            {
                // Agregar código para grafico3 si es necesario
            }
        }


        private void mostrarGrafico1()
        {
            grafico1 = true;
            grafico2 = false;
            grafico3 = false;
            if (procesoElectoralActual != null)
            {
                // Llamar al método que crea el gráfico utilizando el proceso electoral actual
                txtTitulo.Text = procesoElectoralActual.nombre;

                // Limpiar el Canvas antes de agregar nuevas barras y la leyenda
                chartCanvas.Children.Clear();

                double barSpacing = 5; // Espaciado entre barras

                double maxEscaños = procesoElectoralActual.coleccionPartidos.Max(partido => partido.Escaños);

                int numPartidos = procesoElectoralActual.coleccionPartidos.Count;

                // Calcular el ancho de cada barra de manera que ocupen todo el Canvas
                double barWidth = (chartCanvas.ActualWidth - (numPartidos - 1) * barSpacing) / numPartidos;

                for (int i = 0; i < numPartidos; i++)
                {
                    var partido = procesoElectoralActual.coleccionPartidos[i];

                    double barHeight = (partido.Escaños / maxEscaños) * (chartCanvas.ActualHeight);

                    Rectangle barra = new Rectangle();
                    barra.Width = barWidth;
                    barra.Height = barHeight;
                    barra.ToolTip = partido.Nombre + ": " + partido.Escaños + " escaños";

                    try
                    {
                        barra.Fill = (Brush)new BrushConverter().ConvertFromString(partido.Color);
                    }
                    catch (FormatException)
                    {
                        barra.Fill = Brushes.Gray;
                    }

                    Canvas.SetLeft(barra, i * (barWidth + barSpacing));
                    Canvas.SetTop(barra, chartCanvas.ActualHeight - barHeight);

                    chartCanvas.Children.Add(barra);

                    // No agregamos el TextBlock aquí
                }

                // Añadir los nombres de los partidos debajo de las barras después de crear todas las barras
                for (int i = 0; i < numPartidos; i++)
                {
                    var partido = procesoElectoralActual.coleccionPartidos[i];

                    // Agregar el nombre del partido debajo de la barra centrado
                    TextBlock nombrePartido = new TextBlock();
                    nombrePartido.Text = partido.Nombre;
                    nombrePartido.TextAlignment = TextAlignment.Center;
                    nombrePartido.FontSize = 10; // Tamaño inicial de la letra
                    nombrePartido.Width = barWidth; // Ancho igual al de la barra
                    nombrePartido.TextTrimming = TextTrimming.CharacterEllipsis; // Truncar el texto si es demasiado largo

                    // Calcular la posición vertical para que el nombre aparezca debajo de la barra
                    double yPosition = chartCanvas.ActualHeight + 5;

                    Canvas.SetLeft(nombrePartido, i * (barWidth + barSpacing));
                    Canvas.SetTop(nombrePartido, yPosition);
                    chartCanvas.Children.Add(nombrePartido);
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
            SizeChanged += Window_SizeChanged;
            // Verificar si hay un proceso electoral actual antes de crear el gráfico
            if (procesoElectoralActual != null)
            {
                // Obtener otros procesos electorales equivalentes para la comparación
                Collection<ProcesoElectoral> procesosEquivalentes = ObtenerProcesosEquivalentesPorNombre(procesoElectoralActual);

                // Llamar al método que crea el gráfico comparativo
                mostrarGrafico2(procesosEquivalentes);
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

        private void mostrarGrafico2(Collection<ProcesoElectoral> procesosComparativos)
        {
            grafico1 = false;
            grafico2 = true;
            grafico3 = false;

            // Limpiar el Canvas antes de agregar nuevos elementos
            chartCanvas.Children.Clear();

            // Establecer el título del gráfico
            txtTitulo.Text = "Comparación de " + procesoElectoralActual.nombre;

            // Establecer el espaciado entre barras
            double barSpacing = 10;

            double xPosition = 30; // Establecer la posición inicial de la primera barra

            // Recorrer cada proceso electoral en la colección
            foreach (var procesoComparativo in procesosComparativos)
            {
                if (procesoComparativo != null)
                {
                    // Obtener el número total de partidos en el proceso electoral actual
                    int numPartidos = procesoComparativo.coleccionPartidos.Count;

                    // Calcular el ancho de cada barra de manera que se ajusten en el Canvas
                    double barWidth = (chartCanvas.ActualWidth - (numPartidos - 1) * barSpacing) / numPartidos;

                    double maxEscaños =procesoComparativo.coleccionPartidos.Max(partido => partido.Escaños);

                    // Recorrer la colección de partidos y crear una barra para cada uno
                    for (int i = 0; i < numPartidos; i++)
                    {
                        var partido = procesoComparativo.coleccionPartidos[i];

                        double barHeight = (partido.Escaños / maxEscaños) * (chartCanvas.ActualHeight);

                        Rectangle barra = new Rectangle();
                        barra.Width = barWidth;
                        barra.Height = barHeight;
                        barra.ToolTip = partido.Nombre + ": " + partido.Escaños + " escaños";

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

                        // Incrementar la posición horizontal para la siguiente barra
                        xPosition += barWidth + barSpacing;

                        // Agregar el nombre del partido debajo de la barra centrado
                        TextBlock nombrePartido = new TextBlock();
                        nombrePartido.Text = partido.Nombre;
                        nombrePartido.TextAlignment = TextAlignment.Center;
                        nombrePartido.FontSize = 10; // Tamaño inicial de la letra
                        nombrePartido.Width = barWidth; // Ancho igual al de la barra
                        nombrePartido.TextTrimming = TextTrimming.CharacterEllipsis; // Truncar el texto si es demasiado largo

                        // Calcular la posición vertical para que el nombre aparezca debajo de la barra
                        double yPosition = chartCanvas.ActualHeight + 5;

                        Canvas.SetLeft(nombrePartido, xPosition - barWidth - barSpacing); // Alinear el nombre con la barra correspondiente
                        Canvas.SetTop(nombrePartido, yPosition);
                        chartCanvas.Children.Add(nombrePartido);
                    }
                }
            }
        }


        private void Grafico3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
