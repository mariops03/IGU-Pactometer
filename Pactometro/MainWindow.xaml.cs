using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        double flag = 1.0;

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
            // Dependiendo del gráfico seleccionado, llamar al método que crea el gráfico
            procesoSeleccionado();
        
        }

        private void procesoSeleccionado()
        {
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

        private Dictionary<string, bool> estadosMarcado = new Dictionary<string, bool>();

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (grafico2)
            {
                if (procesoElectoralActual == null)
                {
                    // Manejar el caso en el que no hay un proceso electoral actual seleccionado
                    // Puedes mostrar un mensaje de error, por ejemplo
                    MessageBox.Show("No se ha seleccionado un proceso electoral.");
                    return; // Salir del método si no hay proceso seleccionado
                }

            

                // Guardar los estados de marcado antes de cambiar el gráfico
                GuardarEstadosMarcado();

                procesoSeleccionado();

                // Restaurar los estados de marcado después de cambiar el gráfico
                RestaurarEstadosMarcado();
            }
            else
            {
                procesoSeleccionado();
            }
            // Verificar si se ha deseleccionado el proceso actual
            

            
        }

        private void GuardarEstadosMarcado()
        {
            estadosMarcado.Clear();

            // Guardar los estados de marcado de los CheckBox en el checkBoxCanvas
            foreach (CheckBox checkBox in checkBoxCanvas.Children.OfType<CheckBox>())
            {
                estadosMarcado[checkBox.Content.ToString()] = checkBox.IsChecked ?? false;
            }
        }

        private void RestaurarEstadosMarcado()
        {
            // Crear una copia de la colección para evitar la excepción de modificación
            List<CheckBox> checkBoxes = checkBoxCanvas.Children.OfType<CheckBox>().ToList();

            // Restaurar los estados de marcado en el checkBoxCanvas
            foreach (CheckBox checkBox in checkBoxes)
            {
                if (estadosMarcado.ContainsKey(checkBox.Content.ToString()))
                {
                    checkBox.IsChecked = estadosMarcado[checkBox.Content.ToString()];
                }
            } 
        }

        private void mostrarGrafico1()
        {
            grafico1 = true;
            grafico2 = false;
            grafico3 = false;
            if (procesoElectoralActual != null)
            {
                // Eliminar el canvas de los CheckBox si existe
                if (checkBoxCanvas != null)
                {
                    gridPrincipal.Children.Remove(checkBoxCanvas);
                }
                // Llamar al método que crea el gráfico utilizando el proceso electoral actual
                txtTitulo.Text = procesoElectoralActual.nombre;

                // Limpiar el Canvas antes de agregar nuevas barras y la leyenda
                chartCanvas.Children.Clear();

                double barSpacing = 5; // Espaciado entre barras

                double maxEscaños = procesoElectoralActual.coleccionPartidos.Max(partido => partido.Escaños);

                int numPartidos = procesoElectoralActual.coleccionPartidos.Count;

                // Calcular el ancho de cada barra de manera que ocupen todo el Canvas
                double barWidth = (chartCanvas.ActualWidth - (numPartidos - 1) * barSpacing) / (numPartidos + 1);

                double inicioProporcional = barWidth * (numPartidos / (barSpacing*5));

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

                    // Ajusta la posición horizontal para comenzar de manera proporcional al número total de partidos
                    double posicionInicio = i * (barWidth + barSpacing) + inicioProporcional;
                    Canvas.SetLeft(barra, posicionInicio);
                    Canvas.SetTop(barra, (chartCanvas.ActualHeight - barHeight));
                    chartCanvas.Children.Add(barra);

                    // Agregar el nombre del partido debajo de la barra centrado
                    TextBlock nombrePartido = new TextBlock();
                    nombrePartido.Text = partido.Nombre;
                    nombrePartido.TextAlignment = TextAlignment.Center;
                    nombrePartido.FontSize = 10; // Tamaño inicial de la letra
                    nombrePartido.Width = barWidth; // Ancho igual al de la barra
                    nombrePartido.TextTrimming = TextTrimming.CharacterEllipsis; // Truncar el texto si es demasiado largo

                    // Calcular la posición vertical para que el nombre aparezca debajo de la barra
                    double yPosition = chartCanvas.ActualHeight + 5;

                    // Ajusta la posición horizontal para que esté centrado debajo de la barra
                    Canvas.SetLeft(nombrePartido, posicionInicio);
                    Canvas.SetTop(nombrePartido, yPosition);
                    chartCanvas.Children.Add(nombrePartido);
                }

                // Agregar la leyenda indicando el número de escaños
                for (int i = 7; i >= 0; i--)
                {
                    TextBlock leyenda = new TextBlock();

                    if (i == 0)
                    {
                        leyenda.Text = "0";
                        Canvas.SetLeft(leyenda, 0);
                        Canvas.SetBottom(leyenda, 0);
                    }
                    else if (i == 7)
                    {
                        leyenda.Text = maxEscaños.ToString("0");
                        Canvas.SetLeft(leyenda, 0);
                        Canvas.SetTop(leyenda, 0);
                    }
                    else
                    {
                        // Calcular la posición proporcional al número total de escaños
                        double escanosPorPaso = maxEscaños / 7.0;
                        double posicionY = (7 - i) * (chartCanvas.ActualHeight / 7.3);

                        leyenda.Text = (i * escanosPorPaso).ToString("0");
                        Canvas.SetLeft(leyenda, 0);
                        Canvas.SetTop(leyenda, posicionY);
                    }

                    chartCanvas.Children.Add(leyenda);
                }

            }
            else
            {
                // Manejar el caso en el que no hay un proceso electoral actual seleccionado
                // Puedes mostrar un mensaje de error, por ejemplo
                MessageBox.Show("No se ha seleccionado un proceso electoral.");
            }
        }

        private Canvas checkBoxCanvas = null;

        private void mostrarGrafico2(Collection<ProcesoElectoral> procesosComparativos)
        {
            grafico1 = false;
            grafico2 = true;
            grafico3 = false;

           
            // Limpiar el Canvas antes de agregar nuevos elementos
            chartCanvas.Children.Clear();

            txtTitulo.Text = "Comparación de " + procesosComparativos.FirstOrDefault()?.nombre;
            // Limpiar el Canvas de los CheckBox antes de agregar nuevos elementos
            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }

            // Vaciar la colección de partidos seleccionados
            partidosSeleccionados.Clear();

            // Crear un nuevo Canvas para los CheckBox
            checkBoxCanvas = new Canvas();
            checkBoxCanvas.Background = Brushes.LightGray;
            checkBoxCanvas.Margin = new Thickness(15);
            checkBoxCanvas.Width = 112; // Ajusta el tamaño según tus necesidades
            checkBoxCanvas.Height = procesosComparativos.Count() * 30; // Ajusta el tamaño según tus necesidades
            checkBoxCanvas.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(checkBoxCanvas, 3);
            Grid.SetRow(checkBoxCanvas, 2);
            gridPrincipal.Children.Add(checkBoxCanvas);
                    

            double fechaY = 0;

            foreach (ProcesoElectoral proceso in procesosComparativos)
            {
                CheckBox fechaCheckBox = new CheckBox();
                fechaCheckBox.Content = proceso.fecha.ToString("dd/MM/yyyy");
                fechaCheckBox.IsChecked = false;
                fechaCheckBox.Foreground = Brushes.Black;
                fechaCheckBox.FontSize = 12;

                // Establecer propiedades de estilo directamente en el código
                fechaCheckBox.BorderBrush = Brushes.Black;
                fechaCheckBox.BorderThickness = new Thickness(1);
                fechaCheckBox.Margin = new Thickness(5);

                Canvas.SetLeft(fechaCheckBox, 0);
                Canvas.SetTop(fechaCheckBox, fechaY);
                checkBoxCanvas.Children.Add(fechaCheckBox);

                fechaY += 30;

                // Manejar el evento Checked para agregar el proceso a la colección
                fechaCheckBox.Checked += (sender, e) => ManejarSeleccion(proceso, true);

                // Manejar el evento Unchecked para quitar el proceso de la colección
                fechaCheckBox.Unchecked += (sender, e) => ManejarSeleccion(proceso, false);

                // Manejar el evento KeyDown para permitir la navegación con flechas y la selección con Enter
                fechaCheckBox.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        fechaCheckBox.IsChecked = !fechaCheckBox.IsChecked;
                    }
                };
            }
        }

        // Definir una lista para los partidos seleccionados
        private List<List<Partido>> partidosSeleccionados = new List<List<Partido>>();

        private void ManejarSeleccion(ProcesoElectoral proceso, bool seleccionado)
        {
            // Eliminar los partidos relacionados con este proceso en caso de desmarcar el CheckBox
            if (!seleccionado)
            {
                LimpiarPartidos(proceso, partidosSeleccionados);
                QuitarCuadradoCheckBox(proceso.fecha.ToString("dd/MM/yyyy"));
                
            }
            else
            {
                // Agregar partidos al listado de partidos seleccionados
                foreach (Partido partido in proceso.coleccionPartidos)
                {
                    // Buscar en la lista si ya hay partidos con el mismo nombre
                    List<Partido> partidosConMismoNombre = partidosSeleccionados.FirstOrDefault(p => p.Any() && p.First().Nombre == partido.Nombre);

                    if (partidosConMismoNombre != null)
                    {
                        // Si ya hay partidos con el mismo nombre, agregar el partido a esa colección
                        partidosConMismoNombre.Add(partido);
                    }
                    else
                    {
                        // Si no hay partidos con el mismo nombre, crear una nueva colección con este partido
                        partidosSeleccionados.Add(new List<Partido> { partido });
                        

                    }
                }
                // Ordenar las sublistas en partidosSeleccionados según el número de escaños de cada partido
                partidosSeleccionados = partidosSeleccionados.OrderByDescending(lista => lista.Any() ? lista.Max(partido => partido.Escaños) : 0).ToList();
                AgregarCuadradoCheckBox(proceso.fecha.ToString("dd/MM/yyyy"));
                // Imprimir los partidos seleccionados
                ImprimirPartidosSeleccionados(partidosSeleccionados);
            }
        }

        private void AgregarCuadradoCheckBox(string fecha)
        {

            // Crear un rectángulo (o cuadrado) para agregar a la derecha del CheckBox seleccionado
            Rectangle cuadrado = new Rectangle();
            cuadrado.Width = 20; // Ajusta el tamaño según tus necesidades
            cuadrado.Height = 20;
            //Pon un margen al cuadrado
            cuadrado.Margin = new Thickness(3);
            // Establece la opacidad segun la variable flag

            cuadrado.Opacity = 1.0 / flag;
            cuadrado.Fill = Brushes.Red;
            txtTitulo.Text = flag.ToString();

            

            // Posicionar el rectángulo a la derecha del CheckBox correspondiente
            CheckBox checkBoxCorrespondiente = checkBoxCanvas.Children.OfType<CheckBox>().FirstOrDefault(cb => cb.Content.ToString() == fecha);
            if (checkBoxCorrespondiente != null)
            {
                double cuadradoLeft = Canvas.GetLeft(checkBoxCorrespondiente) + checkBoxCorrespondiente.ActualWidth + 5; // Ajusta el espaciado según tus necesidades
                double cuadradoTop = Canvas.GetTop(checkBoxCorrespondiente);
                Canvas.SetRight(cuadrado, 0);
                Canvas.SetTop(cuadrado, cuadradoTop);
                checkBoxCanvas.Children.Add(cuadrado);

                // Asociar el cuadrado al CheckBox correspondiente
                cuadrado.Tag = fecha;
            }
        }

        private void QuitarCuadradoCheckBox(string fecha)
        {
            // Buscar el cuadrado asociado al CheckBox
            Rectangle cuadradoAsociado = checkBoxCanvas.Children.OfType<Rectangle>().FirstOrDefault(c => c.Tag?.ToString() == fecha);

            if (cuadradoAsociado != null)
            {
                // Quitar el cuadrado del Canvas
                checkBoxCanvas.Children.Remove(cuadradoAsociado);
            }
        }


        private void ImprimirPartidosSeleccionados(List<List<Partido>> partidosSeleccionados)
        {
            chartCanvas.Children.Clear();

            // Verificar si hay elementos en la secuencia antes de calcular el máximo
            if (partidosSeleccionados.Any() && partidosSeleccionados.SelectMany(lista => lista).Any())
            {
                double barSpacing = 5;
                double maxEscaños = partidosSeleccionados.SelectMany(lista => lista).Max(partido => partido.Escaños);

                // Calcular el ancho de cada barra
                int numPartidos = partidosSeleccionados.SelectMany(lista => lista).Count();
                double barWidth = (chartCanvas.ActualWidth - (numPartidos - 1) * barSpacing) / (numPartidos+1);

                double inicioProporcional = barWidth * (numPartidos / (barSpacing * 5));
                double yPosition = chartCanvas.ActualHeight; // Comenzar desde la parte inferior

                double baseOpacity = 1; // Ajusta la opacidad base según tus preferencias

                
                foreach (var listaDePartidos in partidosSeleccionados)
                {
                    flag = 1.0;
                    foreach (var partido in listaDePartidos)
                    {
                        double barHeight = (partido.Escaños / maxEscaños) * (chartCanvas.ActualHeight);
                        Rectangle barra = new Rectangle();
                        barra.Width = barWidth;
                        barra.Height = barHeight;
                        barra.ToolTip = $"{partido.Nombre}: {partido.Escaños} escaños";

                        try
                        {
                            Color originalColor = (Color)ColorConverter.ConvertFromString(partido.Color);

                            // Ajusta la opacidad basada en el índice del partido

                            double adjustedOpacity = baseOpacity / flag;

                            Color adjustedColor = Color.FromArgb((byte)(originalColor.A * adjustedOpacity), originalColor.R, originalColor.G, originalColor.B);

                            // Crear una brocha con el color ajustado
                            SolidColorBrush brocha = new SolidColorBrush(adjustedColor);
                            barra.Fill = brocha;  
                            
                        }
                        catch (FormatException)
                        {
                            barra.Fill = Brushes.Gray;
                        }

                        double posicionInicio = yPosition - barHeight;
                        Canvas.SetLeft(barra, inicioProporcional);
                        Canvas.SetTop(barra, posicionInicio);
                        chartCanvas.Children.Add(barra);

                        // Agregar el nombre del partido debajo de la barra centrado
                        TextBlock nombrePartido = new TextBlock();
                        nombrePartido.Text = partido.Nombre;
                        nombrePartido.TextAlignment = TextAlignment.Center;
                        nombrePartido.FontSize = 10; // Tamaño inicial de la letra
                        nombrePartido.Width = barWidth; // Ancho igual al de la barra
                        nombrePartido.TextTrimming = TextTrimming.CharacterEllipsis; // Truncar el texto si es demasiado largo

                        // Calcular la posición vertical para que el nombre aparezca debajo de la barra
                        double yNombrePartido = chartCanvas.ActualHeight + 5;

                        // Ajusta la posición horizontal para que esté centrado debajo de la barra
                        Canvas.SetLeft(nombrePartido, inicioProporcional);
                        Canvas.SetTop(nombrePartido, yNombrePartido);
                        chartCanvas.Children.Add(nombrePartido);

                        // Incrementar la posición proporcional para la siguiente iteración
                        inicioProporcional += barWidth + barSpacing;
                        flag++;
                    }

                    
                }
                // Agregar la leyenda indicando el número de escaños
                for (int i = 7; i >= 0; i--)
                {
                    TextBlock leyenda = new TextBlock();

                    if (i == 0)
                    {
                        leyenda.Text = "0";
                        Canvas.SetLeft(leyenda, 0);
                        Canvas.SetBottom(leyenda, 0);
                    }
                    else if (i == 7)
                    {
                        leyenda.Text = maxEscaños.ToString("0");
                        Canvas.SetLeft(leyenda, 0);
                        Canvas.SetTop(leyenda, 0);
                    }
                    else
                    {
                        // Calcular la posición proporcional al número total de escaños
                        double escanosPorPaso = maxEscaños / 7.0;
                        double posicionY = (7 - i) * (chartCanvas.ActualHeight / 7.3);

                        leyenda.Text = (i * escanosPorPaso).ToString("0");
                        Canvas.SetLeft(leyenda, 0);
                        Canvas.SetTop(leyenda, posicionY);
                    }
                    chartCanvas.Children.Add(leyenda);
                }
            }
        }

        private void LimpiarPartidos(ProcesoElectoral proceso, List<List<Partido>> partidosSeleccionados)
        {
            foreach (List<Partido> listaDePartidos in partidosSeleccionados)
            {
                // Encuentra el primer partido que coincida con la colección de partidos del proceso
                Partido partidoAEliminar = listaDePartidos.FirstOrDefault(partido =>
                    proceso.coleccionPartidos.Any(p =>
                        p.Nombre == partido.Nombre &&
                        p.Escaños == partido.Escaños &&
                        p.Color == partido.Color
                    )
                );

                // Elimina solo el primer partido que coincida (si existe)
                if (partidoAEliminar != null)
                {
                    listaDePartidos.Remove(partidoAEliminar);
                }

                // Imprimir los partidos eliminados actualizados
                ImprimirPartidosSeleccionados(partidosSeleccionados);
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
                // Verificar si el proceso actual es nulo
                if (proceso != null)
                {
                    // Filtrar los procesos equivalentes por la parte de caracteres del nombre (sin números)
                    if (ObtenerParteAlfabética(proceso.nombre) == ObtenerParteAlfabética(procesoElectoralBase.nombre))
                    {
                        procesosEquivalentes.Add(proceso);
                    }
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


        private void Grafico3_Click(object sender, RoutedEventArgs e)
        {

        }

        // Funcion para mostrar el grafico 3
        private void mostrarGrafico3()
        {
            grafico1 = false;
            grafico2 = false;
            grafico3 = true;


            
        }
    }
}
