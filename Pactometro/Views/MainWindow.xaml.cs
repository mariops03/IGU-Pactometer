using Pactometro.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
        private MainWindowViewModel _mainWindowViewModel;

        private bool grafico1 = false;
        private bool grafico2 = false;
        private bool grafico3 = false;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel(new DatosElectorales());
            DataContext = _mainWindowViewModel;
            _mainWindowViewModel.PropertyChanged += MainWindowViewModel_PropertyChanged;
            Loaded += MainWindowLoaded;
            coleccionElecciones = new ObservableCollection<ProcesoElectoral>();
            Closed += _mainWindowViewModel.MainWindow_Closed;
            btnExportar.IsEnabled = false;
            //Establece un tamaño minimo para la ventana
            MinWidth = 460;
            MinHeight = 400;
        }

        private void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Aquí maneja los cambios en las propiedades del ViewModel
            if (e.PropertyName == "EleccionSeleccionada")
            {
                // Actualiza la lógica o la interfaz de usuario según sea necesario
                procesoElectoralActual = _mainWindowViewModel.EleccionSeleccionada;
                if(procesoElectoralActual != null)
                {
                    comprobarGrafico();
                }
                else
                {
                    noHayProcesoElectoralSeleccionado();
                }
            }
            //Copmprueba si la ventana secundaria esta abierta
            if(_mainWindowViewModel.VentanaSecundaria == null)
            {
                menuGraficos.IsEnabled = false;
                // Poner todos los graficos a false
                grafico1 = false;
                grafico2 = false;
                grafico3 = false;
                noHayProcesoElectoralSeleccionado();
            }else
            {
                menuGraficos.IsEnabled = true;
            }
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            coleccionElecciones = _mainWindowViewModel.Elecciones;
            _mainWindowViewModel.AbrirVentanaSecundaria();
            procesoElectoralActual = _mainWindowViewModel.EleccionSeleccionada;
        }

        private void menuVentanaSecundaria(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.AbrirVentanaSecundaria();
        }

        //Funcion que cree un textblock en el canvas indicando que no hay ningun proceso electoral seleccionado, que por favor seleccione uno
        private void noHayProcesoElectoralSeleccionado()
        {
            btnReiniciar.Visibility = Visibility.Collapsed;
            btnPacto.Visibility = Visibility.Collapsed;

            btnExportar.IsEnabled = false;
            //Borra el canvas
            chartCanvas.Children.Clear();
            //Borra el titulo
            txtTitulo.Text = "";
            //Borra el canvas de los checkBox
            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }
           //Crea un textblock en el canvas indicando que no hay ningun proceso electoral seleccionado, que por favor seleccione uno
            TextBlock informar = new TextBlock();
            informar.Text = "POR FAVOR, SELECCIONA UN PROCESO ELECTORAL";
            informar.TextAlignment = TextAlignment.Center;
            informar.FontSize = 20;
            informar.Width = 500;
            informar.Height = 100;
            informar.FontWeight = FontWeights.Bold;
            chartCanvas.Children.Add(informar);
            //Ajusta la posicion del textblock al centro del canvas
            Canvas.SetLeft(informar, (chartCanvas.ActualWidth - informar.Width) / 2);
            Canvas.SetTop(informar, (chartCanvas.ActualHeight - informar.Height) / 2);
        }

        private void noHayGraficoSeleccionado()
        {
            btnPacto.Visibility = Visibility.Collapsed;
            btnReiniciar.Visibility = Visibility.Collapsed;

            btnExportar.IsEnabled = false;
            chartCanvas.Children.Clear();
            txtTitulo.Text = "";
            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }
            TextBlock informar = new TextBlock();
            informar.Text = "POR FAVOR, SELECCIONA UN GRAFICO";
            informar.TextAlignment = TextAlignment.Center;
            informar.FontSize = 20;
            informar.Width = 500;
            informar.Height = 100;
            informar.FontWeight = FontWeights.Bold;
            chartCanvas.Children.Add(informar);
            Canvas.SetLeft(informar, (chartCanvas.ActualWidth - informar.Width) / 2);
            Canvas.SetTop(informar, (chartCanvas.ActualHeight - informar.Height) / 2);
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
                Collection<ProcesoElectoral> procesosEquivalentes = _mainWindowViewModel.ObtenerProcesosEquivalentesPorNombre();

                // Llamar al método que crea el gráfico comparativo
                mostrarGrafico2(procesosEquivalentes);
            }
            else if (grafico3 == true)
            {
                _mainWindowViewModel.PartidosEnPrimeraBarra.Clear();
                _mainWindowViewModel.PartidosEnSegundaBarra.Clear();
                _mainWindowViewModel.RectangulosClicados.Clear();
                _mainWindowViewModel.RectangulosNoClicados.Clear();
                mostrarGrafico3();
            }
            else
            {
                noHayGraficoSeleccionado();
            }
        }

        private void menuSalir(object sender, RoutedEventArgs e)
        {
            // Preguntar al usuario si desea salir
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close(); // Cierra la ventana principal, lo que también activará el evento Closed
            }

        }

        private void Grafico1_Click(object sender, RoutedEventArgs e)
        {
            grafico1 = true;
            grafico2 = false;
            grafico3 = false;
            SizeChanged += Window_SizeChanged;
            if (procesoElectoralActual == null)
            {
                noHayProcesoElectoralSeleccionado();
            }
            else
            {
                mostrarGrafico1();
            }
        }

        private Dictionary<string, bool> estadosMarcado = new Dictionary<string, bool>();
        // Crea otro diccionario para guardar los estados de marcado antes de cambiar el gráfico
        private Dictionary<string, bool> estadosMarcado2 = new Dictionary<string, bool>();

        private void comprobarGrafico()
        {
           
                if (grafico2)
                {
                    _mainWindowViewModel.ColeccionEleccionesCheckBox.Clear();
                    GuardarEstadosMarcado();

                    procesoSeleccionado();

                    RestaurarEstadosMarcado();
                }
                else
                {
                    procesoSeleccionado();
                }
        }
            

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(grafico3 == true)
            {
                mostrarGrafico3();
            }
            else
            {
                comprobarGrafico();
            }
        }

        private void GuardarEstadosMarcado()
        {
            estadosMarcado.Clear();
            if(checkBoxCanvas != null)
            {
                // Guardar los estados de marcado de los CheckBox en el checkBoxCanvas
                foreach (CheckBox checkBox in checkBoxCanvas.Children.OfType<CheckBox>())
                {
                    estadosMarcado[checkBox.Content.ToString()] = checkBox.IsChecked ?? false;
                }
            } 
        }

        private void GuardarEstadosMarcado2()
        {
            estadosMarcado2.Clear();
            if(checkBoxCanvas != null)
            {
                // Guardar los estados de marcado de los CheckBox en el checkBoxCanvas
                foreach (CheckBox checkBox in checkBoxCanvas.Children.OfType<CheckBox>())
                {
                    estadosMarcado2[checkBox.Content.ToString()] = checkBox.IsChecked ?? false;
                }
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
            
            btnPacto.Visibility = Visibility.Collapsed;
            btnReiniciar.Visibility = Visibility.Collapsed;
            btnExportar.IsEnabled = true;
            // Eliminar el canvas de los CheckBox si existe
            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }
            //Si la opacidad de los partidos es menor que 1, se vuelve a poner a 1
            foreach (Partido partido in procesoElectoralActual.coleccionPartidos)
            {
                partido.Color = Color.FromArgb(255, partido.Color.R, partido.Color.G, partido.Color.B);
            }

            // Llamar al método que crea el gráfico utilizando el proceso electoral actual
            txtTitulo.Text = procesoElectoralActual.nombre;

            // Limpiar el Canvas antes de agregar nuevas barras y la leyenda
            chartCanvas.Children.Clear();

            double barSpacing = 5; // Espaciado entre barras

            double maxEscaños = procesoElectoralActual.coleccionPartidos.Max(partido => partido.Escaños);

            int digitos = maxEscaños.ToString().Length;
            double inicioProporcional = 15 + digitos * 5;

            int numPartidos = procesoElectoralActual.coleccionPartidos.Count;

            double espacioBarras = chartCanvas.ActualWidth - inicioProporcional;

            // Calcular el ancho de cada barra de manera que ocupen todo el Canvas
            double barWidth = (espacioBarras  - (numPartidos) * barSpacing) / (numPartidos);

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
                    barra.Fill = new SolidColorBrush(partido.Color);
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

        //Cre ael metodo para exportar el grafico
        private void Exportar_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.Exportar();
        }

        private Canvas checkBoxCanvas = null;

        private void mostrarGrafico2(Collection<ProcesoElectoral> procesosComparativos)
        {
            btnPacto.Visibility = Visibility.Collapsed;
            btnReiniciar.Visibility = Visibility.Collapsed;
            btnExportar.IsEnabled = true;
           
            // Limpiar el Canvas antes de agregar nuevos elementos
            chartCanvas.Children.Clear();           

            var nombreProceso = procesosComparativos.FirstOrDefault()?.nombre;

            var nombreSinNumeros = Regex.Replace(nombreProceso, @"[\d-]+", "");

            txtTitulo.Text = "Comparación de " + nombreSinNumeros;

            // Ordenar la colección de procesos electorales por fecha de forma que el más reciente esté primero
            procesosComparativos = new Collection<ProcesoElectoral>(procesosComparativos.OrderByDescending(p => p.fecha).ToList());

            // Limpiar el Canvas de los CheckBox antes de agregar nuevos elementos
            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }

            // Vaciar la colección de partidos seleccionados
            _mainWindowViewModel.PartidosSeleccionados.Clear();

            // Crear un nuevo Canvas para los CheckBox
            checkBoxCanvas = new Canvas();
            checkBoxCanvas.Background = Brushes.WhiteSmoke;
            checkBoxCanvas.Margin = new Thickness(15);
            checkBoxCanvas.Width = 112; // Ajusta el tamaño según tus necesidades
            checkBoxCanvas.Height = procesosComparativos.Count() * 29; // Ajusta el tamaño según tus necesidades
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

                fechaCheckBox.Checked += (sender, e) => ManejarSeleccion(proceso);
                fechaCheckBox.Unchecked += (sender, e) => ManejarSeleccion(proceso);


                fechaCheckBox.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        fechaCheckBox.IsChecked = !fechaCheckBox.IsChecked;
                    }
                };
            }
        }

        private void ManejarSeleccion(ProcesoElectoral proceso)
        {
            // Comprobar si el proceso ya está en la colección
            bool seleccionado = _mainWindowViewModel.ColeccionEleccionesCheckBox.Contains(proceso);

            if (!seleccionado)
            {
                // Si el proceso no está seleccionado, añadirlo a la colección
                _mainWindowViewModel.ColeccionEleccionesCheckBox.Add(proceso);
                // Llamadas a otros métodos como ActualizarElecciones, etc.
                _mainWindowViewModel.ActualizarElecciones();
                
            }
            else
            {
                // Si el proceso está seleccionado, eliminarlo de la colección
                _mainWindowViewModel.ColeccionEleccionesCheckBox.Remove(proceso);
                // Llamadas a otros métodos como LimpiarPartidos, etc.
                _mainWindowViewModel.LimpiarPartidos(proceso);
                _mainWindowViewModel.ActualizarElecciones();
            }
            ImprimirPartidosSeleccionados(_mainWindowViewModel.PartidosSeleccionados);
            // Llamada a ActualizarCuadrados si es necesario
            ActualizarCuadrados();
        }

        private void ActualizarCuadrados()
        {
            GuardarEstadosMarcado2();
            var cuadradosExistentes = checkBoxCanvas.Children.OfType<Rectangle>().ToList();
            foreach (var cuadrado in cuadradosExistentes)
            {
                checkBoxCanvas.Children.Remove(cuadrado);
            }
            int indice = 0;
            var checkBoxesMarcados = estadosMarcado2
                .Where(kvp => kvp.Value)
                .Select(kvp => new { Key = DateTime.ParseExact(kvp.Key, "dd/MM/yyyy", CultureInfo.InvariantCulture), Value = kvp.Value })
                .OrderByDescending(kvp => kvp.Key)
                .ToList();

            foreach (var kvp in checkBoxesMarcados)
            {
                DateTime fecha = kvp.Key;
                indice++;
                Rectangle cuadrado = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(3),
                    Opacity = 1.0 / indice,
                    Fill = Brushes.Indigo
                };

                string fechaTexto = fecha.ToString("dd/MM/yyyy");
                CheckBox checkBoxCorrespondiente = checkBoxCanvas.Children.OfType<CheckBox>().FirstOrDefault(cb => cb.Content.ToString().Equals(fechaTexto));

                if (checkBoxCorrespondiente != null)
                {
                    double cuadradoTop = Canvas.GetTop(checkBoxCorrespondiente);
                    Canvas.SetTop(cuadrado, cuadradoTop);
                    // Ajustar la posición horizontal según la disposición de tus CheckBoxes
                    // Por ejemplo, puedes usar Canvas.SetLeft si quieres alinearlos a la izquierda del CheckBox
                    Canvas.SetRight(cuadrado, 0); // Ajusta esto según sea necesario
                    checkBoxCanvas.Children.Add(cuadrado);
                }
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

                int digitos = maxEscaños.ToString().Length;
                double inicioProporcional = 15 + digitos * 5;

                int numPartidos = partidosSeleccionados.SelectMany(lista => lista).Count();


                double espacioBarras = chartCanvas.ActualWidth - inicioProporcional;

                // Calcular el ancho de cada barra de manera que ocupen todo el Canvas
                double barWidth = (espacioBarras - (numPartidos) * barSpacing) / (numPartidos);
                double acumuladorInicio = inicioProporcional;
                double yPosition = chartCanvas.ActualHeight; // Comenzar desde la parte inferior
                //Ordenar las sublistas en partidosSeleccionados según el número de escaños de cada partido
                partidosSeleccionados = partidosSeleccionados.OrderByDescending(lista => lista.Any() ? lista.Max(partido => partido.Escaños) : 0).ToList();


                foreach (var listaDePartidos in partidosSeleccionados)
                {
                    foreach (var partido in listaDePartidos)
                    {
                        double barHeight = (partido.Escaños / maxEscaños) * (chartCanvas.ActualHeight);
                        Rectangle barra = new Rectangle();
                        barra.Width = barWidth;
                        barra.Height = barHeight;
                        barra.ToolTip = $"{partido.Nombre}: {partido.Escaños} escaños";

                        try
                        {
                            SolidColorBrush brocha = new SolidColorBrush(partido.Color);
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

                        

                        // Incrementar la posición proporcional para la siguiente iteración
                        inicioProporcional += barWidth + barSpacing;
                    }
                    if (listaDePartidos != null && listaDePartidos.Any())
                    {
                        int numeroDeBarras = listaDePartidos.Count;
                        double posicionFinalGrupo = acumuladorInicio + (barWidth + barSpacing) * (numeroDeBarras - 1) + barWidth;
                        double puntoMedioGrupo = (acumuladorInicio + posicionFinalGrupo) / 2;

                        TextBlock nombrePartido = new TextBlock();
                        nombrePartido.Text = listaDePartidos.FirstOrDefault().Nombre;
                        nombrePartido.TextAlignment = TextAlignment.Center;
                        nombrePartido.FontSize = 10;
                        // Establece un ancho para el TextBlock, por ejemplo, basado en el texto
                        nombrePartido.Width = barWidth * numeroDeBarras + barSpacing * (numeroDeBarras - 1);

                        double xNombrePartido = puntoMedioGrupo - (nombrePartido.Width / 2);

                        double yNombrePartido = chartCanvas.ActualHeight + 5;
                        Canvas.SetLeft(nombrePartido, xNombrePartido);
                        Canvas.SetTop(nombrePartido, yNombrePartido);
                        chartCanvas.Children.Add(nombrePartido);

                        // Actualizar acumuladorInicio para el siguiente grupo de barras
                        acumuladorInicio = posicionFinalGrupo + barSpacing;
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

        private void Grafico2_Click(object sender, RoutedEventArgs e)
        {
            grafico1 = false;
            grafico2 = true;
            grafico3 = false;

            SizeChanged += Window_SizeChanged;

            if (procesoElectoralActual == null)
            {
                noHayProcesoElectoralSeleccionado();
            }
            else
            {
                Collection<ProcesoElectoral> procesosEquivalentes = _mainWindowViewModel.ObtenerProcesosEquivalentesPorNombre();

                mostrarGrafico2(procesosEquivalentes);
            }
        }


        private void Grafico3_Click(object sender, RoutedEventArgs e)
        {
            grafico1 = false;
            grafico2 = false;
            grafico3 = true;
            // Suscribir al evento SizeChanged de la ventana o del contenedor que contiene el Canvas
            SizeChanged += Window_SizeChanged;
            if (procesoElectoralActual == null)
            {
                noHayProcesoElectoralSeleccionado();
            }
            else
            {
                mostrarGrafico3();
            }
        }

        
        private double emptyBarYPos;
        private double yPos;

        private void Rectangle_Click(object sender, MouseButtonEventArgs e)
        {
            Rectangle clickedRect = sender as Rectangle;
            if (clickedRect != null)
            {
                bool isInFirstBar = Canvas.GetTop(clickedRect) == yPos;
                double newYPos = isInFirstBar ? emptyBarYPos : yPos;
                Canvas.SetTop(clickedRect, newYPos);

                double newXPos;
                if (isInFirstBar)
                {
                    newXPos = _mainWindowViewModel.RectangulosClicados.Sum(r => r.Width) + 15;
                    _mainWindowViewModel.RectangulosClicados.Add(clickedRect);
                    _mainWindowViewModel.RectangulosNoClicados.Remove(clickedRect);
                    _mainWindowViewModel.PartidosEnPrimeraBarra.Remove(clickedRect.Tag as Partido);
                    _mainWindowViewModel.PartidosEnSegundaBarra.Add(clickedRect.Tag as Partido);
                    ReposicionarRectangulosEnPrimeraBarra();
                }
                else
                {
                    // calcular la nueva posición X para el rectángulo teniendo en cuenta los rectangulos no clicados
                    newXPos = _mainWindowViewModel.RectangulosNoClicados.Sum(r => r.Width) + 15;
                    //añadir el rectangulo a la lista de rectangulos no clicados
                    _mainWindowViewModel.RectangulosNoClicados.Add(clickedRect);
                    _mainWindowViewModel.RectangulosClicados.Remove(clickedRect);
                    _mainWindowViewModel.PartidosEnSegundaBarra.Remove(clickedRect.Tag as Partido);
                    _mainWindowViewModel.PartidosEnPrimeraBarra.Add(clickedRect.Tag as Partido);
                    ReposicionarRectangulosEnSegundaBarra();
                }
                Canvas.SetLeft(clickedRect, newXPos);
                btnPacto.IsEnabled = _mainWindowViewModel.comprobarPacto();
            }
        }
        private void ReposicionarRectangulosEnPrimeraBarra()
        {
            double xPos = 15;
            foreach (var partido in _mainWindowViewModel.PartidosEnPrimeraBarra)
            {
                Rectangle rect = chartCanvas.Children.OfType<Rectangle>().FirstOrDefault(r => r.Tag == partido);
                if (rect != null)
                {
                    Canvas.SetLeft(rect, xPos);
                    xPos += rect.Width;
                    _mainWindowViewModel.OriginalPositions[rect] = xPos;
                }
            }
        }

        private void ReposicionarRectangulosEnSegundaBarra()
        {
            double xPos = 15;
            foreach (var partido in _mainWindowViewModel.PartidosEnSegundaBarra)
            {
                Rectangle rect = chartCanvas.Children.OfType<Rectangle>().FirstOrDefault(r => r.Tag == partido);
                if (rect != null)
                {
                    Canvas.SetLeft(rect, xPos);
                    xPos += rect.Width;
                    _mainWindowViewModel.OriginalPositions[rect] = xPos;
                }
            }
        }

        private void mostrarGrafico3()
        {
            btnPacto.Visibility = Visibility.Visible;
            btnPacto.IsEnabled = false;
            btnReiniciar.Visibility = Visibility.Visible;

            btnExportar.IsEnabled = true;
            chartCanvas.Children.Clear();

            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }

            //Si la opacidad de los partidos es menor que 1, se vuelve a poner a 1
            foreach (Partido partido in procesoElectoralActual.coleccionPartidos)
            {
                partido.Color = Color.FromArgb(255, partido.Color.R, partido.Color.G, partido.Color.B);
            }

            txtTitulo.Text = procesoElectoralActual.nombre;

            double barHeight = chartCanvas.ActualHeight / 4;
            double barGap = chartCanvas.ActualHeight / 10; 

            yPos = chartCanvas.ActualHeight / 5;
            emptyBarYPos = yPos + barHeight + barGap;

            // Create the first empty bar that represents the total number of seats
            Rectangle firstEmptyBar = new Rectangle
            {
                Height = barHeight,
                Width = chartCanvas.ActualWidth - 30,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            // Position the first empty bar on the canvas
            Canvas.SetTop(firstEmptyBar, yPos);
            Canvas.SetLeft(firstEmptyBar, 15); // Align to the left
            chartCanvas.Children.Add(firstEmptyBar);

            // Initialize the starting X position for the filled rectangles
            double xPos = 15;

            double anchoTotalBarras = firstEmptyBar.Width;

            // Create a second empty bar below the first one
            Rectangle secondEmptyBar = new Rectangle
            {
                Height = barHeight,
                Width = firstEmptyBar.Width, // Match the width of the first empty bar
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            // Position the second empty bar below the first one with a gap
            Canvas.SetTop(secondEmptyBar, yPos + barHeight + barGap);
            Canvas.SetLeft(secondEmptyBar, 15); // Align to the left

            // Add the second empty bar to the canvas
            chartCanvas.Children.Add(secondEmptyBar);


            emptyBarYPos = yPos + barHeight + barGap;

            // Draw a filled rectangle for each party, within the bounds of the first empty rectangle
            foreach (var partido in procesoElectoralActual.coleccionPartidos)
            {
                //Si el partido no esta en ninguna lista, se hace el codigo normal
                if (!_mainWindowViewModel.PartidosEnPrimeraBarra.Contains(partido) && !_mainWindowViewModel.PartidosEnSegundaBarra.Contains(partido))
                {
                    // Calculate the width of each filled rectangle based on the number of seats
                    double rectWidth = (anchoTotalBarras / procesoElectoralActual.numEscaños) * partido.Escaños;

                    // Create the filled rectangle
                    Rectangle filledBar = new Rectangle
                    {
                        Height = barHeight,
                        Width = rectWidth,
                        ToolTip = partido.Nombre + ": " + partido.Escaños + " escaños",
                        Tag = partido
                    };

                    filledBar.MouseLeftButtonDown += Rectangle_Click;

                    try
                    {
                        filledBar.Fill = new SolidColorBrush(partido.Color);
                    }
                    catch (FormatException)
                    {
                        filledBar.Fill = Brushes.Gray; // Usa gris si hay una excepción
                    }


                    // Position the filled rectangle within the first empty bar
                    Canvas.SetTop(filledBar, yPos);
                    Canvas.SetLeft(filledBar, xPos); // Align the left edge with the current x position

                    // Add the filled rectangle to the canvas
                    _mainWindowViewModel.OriginalPositions[filledBar] = xPos;

                    // Increment the X position for the next filled rectangle
                    xPos += rectWidth;

                    // Añade el partido a la lista de partidos de la primera barra
                    _mainWindowViewModel.PartidosEnPrimeraBarra.Add(partido);
                    // Añade el rectangulo a la lista de rectangulos no clicados
                    _mainWindowViewModel.RectangulosNoClicados.Add(filledBar);
                }
            }

            double xPosNoClicados = 15;
            foreach (var rectangulo in _mainWindowViewModel.RectangulosNoClicados)
            {
                rectangulo.Height = barHeight;
                rectangulo.Width = (anchoTotalBarras / procesoElectoralActual.numEscaños) * (rectangulo.Tag as Partido).Escaños;
                Canvas.SetTop(rectangulo, yPos);
                Canvas.SetLeft(rectangulo, xPosNoClicados);
                xPosNoClicados += rectangulo.Width;
                chartCanvas.Children.Add(rectangulo);
            }

            // Añade los rectangulos clicados a la segunda barra
            double xPosClicados = 15;
            foreach (var rectangulo in _mainWindowViewModel.RectangulosClicados)
            {
                rectangulo.Height = barHeight;
                rectangulo.Width = (anchoTotalBarras / procesoElectoralActual.numEscaños) * (rectangulo.Tag as Partido).Escaños;
                Canvas.SetTop(rectangulo, emptyBarYPos);
                Canvas.SetLeft(rectangulo, xPosClicados);
                xPosClicados += rectangulo.Width;
                chartCanvas.Children.Add(rectangulo);
            }

            // Calcular la posición X de la barra de mayoría absoluta
            int mayoriaAbsoluta = procesoElectoralActual.mayoriaAbsoluta;
            double anchoTotalBarra = firstEmptyBar.Width;
            double posicionBarraMayoria = ((anchoTotalBarra / procesoElectoralActual.numEscaños) * mayoriaAbsoluta) + 15;

            // Crear la barra de mayoría absoluta
            Rectangle barraMayoriaAbsoluta = new Rectangle
            {
                Height = barHeight,
                Width = 2, // Un ancho pequeño para que sea una línea 
                Fill = Brushes.Red // Color rojo para destacar
            };

            //Ponle contorno a la barra de mayoria absoluta
            barraMayoriaAbsoluta.Stroke = Brushes.Black;
            barraMayoriaAbsoluta.StrokeThickness = 0.4;

            // Posicionar la barra de mayoría absoluta en la barra inferior
            Canvas.SetTop(barraMayoriaAbsoluta, emptyBarYPos);
            Canvas.SetLeft(barraMayoriaAbsoluta, posicionBarraMayoria);

            // Añadir la barra de mayoría absoluta al canvas
            chartCanvas.Children.Add(barraMayoriaAbsoluta);

        }

        //Crea un evento para reiniciar el grafico
        private void Reiniciar_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.reiniciarPactometro();
            mostrarGrafico3();
        }

        //Crea un evento para completar un pacto
        private void Pacto_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.pactoCompletado();
        }
    }
}
