﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

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
                // Hacer que la ventana secundaria sea propiedad de la ventana principal
                ventanaSecundaria.Owner = this;

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
            if(procesoElectoral == null)
            {
                noHayProcesoElectoralSeleccionado();
                procesoElectoralActual = null;
            }
            else
            {
                procesoElectoralActual = procesoElectoral;

                comprobarGrafico();
            }
        }

        //Funcion que cree un textblock en el canvas indicando que no hay ningun proceso electoral seleccionado, que por favor seleccione uno
        private void noHayProcesoElectoralSeleccionado()
        {
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
            chartCanvas.Children.Add(informar);
            //Ajusta la posicion del textblock al centro del canvas
            Canvas.SetLeft(informar, (chartCanvas.ActualWidth - informar.Width) / 2);
            Canvas.SetTop(informar, (chartCanvas.ActualHeight - informar.Height) / 2);
        }

        private void noHayGraficoSeleccionado()
        {
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
                Collection<ProcesoElectoral> procesosEquivalentes = ObtenerProcesosEquivalentesPorNombre(procesoElectoralActual);

                // Llamar al método que crea el gráfico comparativo
                mostrarGrafico2(procesosEquivalentes);
            }
            else if (grafico3 == true)
            {
                partidosEnPrimeraBarra.Clear();
                partidosEnSegundaBarra.Clear();
                clickedRectangles.Clear();
                rectangulosNoClicados.Clear();
                mostrarGrafico3();
            }
            else
            {
                noHayGraficoSeleccionado();
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
                    coleccionEleccionesCheckBox.Clear();
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
            comprobarGrafico();
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

            double inicioProporcional = barWidth * (numPartidos / (barSpacing * 5));

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
            //Poner visible el boton de exportar
            exportar.Visibility = Visibility.Visible;
        }

        //Cre ael metodo para exportar el grafico
        private void Exportar_Click(object sender, RoutedEventArgs e)
        {
            // Abrir la ventana de exportación
            VentanaExportar ventanaExportar = new VentanaExportar();
            ventanaExportar.Owner = this; // Establecer la ventana principal como propietaria de la ventana de exportación
            ventanaExportar.ShowDialog(); // Mostrar la ventana de exportación como modal
        }

        private Canvas checkBoxCanvas = null;

        private void mostrarGrafico2(Collection<ProcesoElectoral> procesosComparativos)
        {
           
            // Limpiar el Canvas antes de agregar nuevos elementos
            chartCanvas.Children.Clear();


            //Hacer visible el boton de exportar
            

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
            partidosSeleccionados.Clear();

            // Crear un nuevo Canvas para los CheckBox
            checkBoxCanvas = new Canvas();
            checkBoxCanvas.Background = Brushes.LightGray;
            checkBoxCanvas.Margin = new Thickness(15);
            checkBoxCanvas.Width = 112; // Ajusta el tamaño según tus necesidades
            checkBoxCanvas.Height = procesosComparativos.Count() * 29; // Ajusta el tamaño según tus necesidades
            checkBoxCanvas.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(checkBoxCanvas, 3);
            Grid.SetRow(checkBoxCanvas, 2);
            gridPrincipal.Children.Add(checkBoxCanvas);

            exportar.Visibility = Visibility.Visible;


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
        //Crea una coleccion de elecciones para guardar los procesos electorales de los checkBox
        private Collection<ProcesoElectoral> coleccionEleccionesCheckBox = new Collection<ProcesoElectoral>();

        private void ManejarSeleccion(ProcesoElectoral proceso, bool seleccionado)
        {
            // Eliminar los partidos relacionados con este proceso en caso de desmarcar el CheckBox
            if (!seleccionado)
            {
                // Eliminar el proceso de la colección de elecciones en orden de fecha
                coleccionEleccionesCheckBox.Remove(proceso);
                coleccionEleccionesCheckBox = new Collection<ProcesoElectoral>(coleccionEleccionesCheckBox.OrderByDescending(p => p.fecha).ToList());
                LimpiarPartidos(proceso, partidosSeleccionados);
                //QuitarCuadradoCheckBox(proceso.fecha.ToString("dd/MM/yyyy"));
            }
            else
            {
                //Eliminar partidos seleccionados
                partidosSeleccionados.Clear();
                //Agregar el proceso a la coleccion de elecciones en orden de fecha
                coleccionEleccionesCheckBox.Add(proceso);
                // Ordenar la colección de elecciones por fecha de forma inversa
                coleccionEleccionesCheckBox = new Collection<ProcesoElectoral>(coleccionEleccionesCheckBox.OrderByDescending(p => p.fecha).ToList());
                
                // Recorrer la coleccion de procesos electorales de los checkBox

                foreach (ProcesoElectoral procesoCheckBox in coleccionEleccionesCheckBox)
                {
                    foreach (Partido partido in procesoCheckBox.coleccionPartidos)
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
                }
                // Ordenar las sublistas en partidosSeleccionados según el número de escaños de cada partido
                partidosSeleccionados = partidosSeleccionados.OrderByDescending(lista => lista.Any() ? lista.Max(partido => partido.Escaños) : 0).ToList();
                
                ImprimirPartidosSeleccionados(partidosSeleccionados);

            }
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
                    Fill = Brushes.Blue
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

                // Calcular el ancho de cada barra
                int numPartidos = partidosSeleccionados.SelectMany(lista => lista).Count();
                double barWidth = (chartCanvas.ActualWidth - (numPartidos - 1) * barSpacing) / (numPartidos + 1);
                double inicioProporcional = barWidth * (numPartidos / (barSpacing * 5));
                double acumuladorInicio = inicioProporcional;
                double yPosition = chartCanvas.ActualHeight; // Comenzar desde la parte inferior

                double baseOpacity = 1; // Ajusta la opacidad base según tus preferencias
                //Ordenar las sublistas en partidosSeleccionados según el número de escaños de cada partido
                partidosSeleccionados = partidosSeleccionados.OrderByDescending(lista => lista.Any() ? lista.Max(partido => partido.Escaños) : 0).ToList();

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
                            // Asumiendo que partido.Color es un System.Windows.Media.Color
                            Color originalColor = partido.Color;

                            // Ajusta la opacidad basada en el índice del partido
                            double adjustedOpacity = baseOpacity / flag;

                            // Ajustar el alfa (opacidad) del color
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

                        

                        // Incrementar la posición proporcional para la siguiente iteración
                        inicioProporcional += barWidth + barSpacing;
                        flag++;
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

                    // Agregar el nombre del partido debajo de la barra centrado


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
            grafico1 = false;
            grafico2 = true;
            grafico3 = false;

            SizeChanged += Window_SizeChanged;
            // Verificar si hay un proceso electoral actual antes de crear el gráfico
            if (procesoElectoralActual == null)
            {
                noHayProcesoElectoralSeleccionado();
            }
            else
            {
                // Obtener otros procesos electorales equivalentes para la comparación
                Collection<ProcesoElectoral> procesosEquivalentes = ObtenerProcesosEquivalentesPorNombre(procesoElectoralActual);

                // Llamar al método que crea el gráfico comparativo
                mostrarGrafico2(procesosEquivalentes);
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
            // Verificar si el nombre es null
            if (nombre == null)
            {
                throw new ArgumentNullException(nameof(nombre));
            }

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

        // Additional fields to keep track of the clicked rectangles and their order
        private Dictionary<Rectangle, double> originalPositions = new Dictionary<Rectangle, double>();
        private List<Partido> partidosEnPrimeraBarra = new List<Partido>();
        private List<Partido> partidosEnSegundaBarra = new List<Partido>();
        private List<Rectangle> clickedRectangles = new List<Rectangle>();
        private List<Rectangle> rectangulosNoClicados = new List<Rectangle>();
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
                    newXPos = clickedRectangles.Sum(r => r.Width) + 15;
                    clickedRectangles.Add(clickedRect);
                    rectangulosNoClicados.Remove(clickedRect);
                    partidosEnPrimeraBarra.Remove(clickedRect.Tag as Partido);
                    partidosEnSegundaBarra.Add(clickedRect.Tag as Partido);
                    ReposicionarRectangulosEnPrimeraBarra();
                }
                else
                {
                    // calcular la nueva posición X para el rectángulo teniendo en cuenta los rectangulos no clicados
                    newXPos = rectangulosNoClicados.Sum(r => r.Width) + 15;
                    //añadir el rectangulo a la lista de rectangulos no clicados
                    rectangulosNoClicados.Add(clickedRect);
                    clickedRectangles.Remove(clickedRect);
                    partidosEnSegundaBarra.Remove(clickedRect.Tag as Partido);
                    partidosEnPrimeraBarra.Add(clickedRect.Tag as Partido);
                }
                Canvas.SetLeft(clickedRect, newXPos);
                VerificarPactoHabilitado();
            }
        }

        private void ReposicionarRectangulosEnPrimeraBarra()
        {
            double xPos = 15;
            foreach (var partido in partidosEnPrimeraBarra)
            {
                Rectangle rect = chartCanvas.Children
                    .OfType<Rectangle>()
                    .FirstOrDefault(r => r.Tag == partido);
                if (rect != null)
                {
                    Canvas.SetLeft(rect, xPos);
                    xPos += rect.Width;
                    originalPositions[rect] = xPos;
                }
            }
        }

        private void VerificarPactoHabilitado()
        {
            pacto.IsEnabled = partidosEnPrimeraBarra.Sum(partido => partido.Escaños) < procesoElectoralActual.mayoriaAbsoluta;
        }

        private Button pacto;
        private void mostrarGrafico3()
        {
            chartCanvas.Children.Clear();

            if (checkBoxCanvas != null)
            {
                gridPrincipal.Children.Remove(checkBoxCanvas);
            }

            txtTitulo.Text = procesoElectoralActual.nombre;

            double barHeight = chartCanvas.ActualHeight / 4; // height of each bar
            double barGap = chartCanvas.ActualHeight / 10; // gap between bars

            yPos = chartCanvas.ActualHeight / 6;
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

            // Calcular la posición X de la barra de mayoría absoluta
            int mayoriaAbsoluta = procesoElectoralActual.mayoriaAbsoluta;
            double anchoTotalBarra = firstEmptyBar.Width;
            double posicionBarraMayoria = ((anchoTotalBarra / procesoElectoralActual.numEscaños) * mayoriaAbsoluta) + 15;

            // Crear la barra de mayoría absoluta
            Rectangle barraMayoriaAbsoluta = new Rectangle
            {
                Height = barHeight,
                Width = 2, // Un ancho pequeño para que sea una línea delgada
                Fill = Brushes.Red // Color rojo para destacar
            };

            // Posicionar la barra de mayoría absoluta en la barra inferior
            Canvas.SetTop(barraMayoriaAbsoluta, emptyBarYPos);
            Canvas.SetLeft(barraMayoriaAbsoluta, posicionBarraMayoria);

            // Añadir la barra de mayoría absoluta al canvas
            chartCanvas.Children.Add(barraMayoriaAbsoluta);

            emptyBarYPos = yPos + barHeight + barGap;

            // Draw a filled rectangle for each party, within the bounds of the first empty rectangle
            foreach (var partido in procesoElectoralActual.coleccionPartidos)
            {
                //Si el partido no esta en ninguna lista, se hace el codigo normal
                if (!partidosEnPrimeraBarra.Contains(partido) && !partidosEnSegundaBarra.Contains(partido))
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
                    //chartCanvas.Children.Add(filledBar);
                    originalPositions[filledBar] = xPos;

                    // Increment the X position for the next filled rectangle
                    xPos += rectWidth;

                    // Añade el partido a la lista de partidos de la primera barra
                    partidosEnPrimeraBarra.Add(partido);
                    // Añade el rectangulo a la lista de rectangulos no clicados
                    rectangulosNoClicados.Add(filledBar);
                }
            }

            double xPosNoClicados = 15;
            foreach (var rectangulo in rectangulosNoClicados)
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
            foreach (var rectangulo in clickedRectangles)
            {
                rectangulo.Height = barHeight;
                rectangulo.Width = (anchoTotalBarras / procesoElectoralActual.numEscaños) * (rectangulo.Tag as Partido).Escaños;
                Canvas.SetTop(rectangulo, emptyBarYPos);
                Canvas.SetLeft(rectangulo, xPosClicados);
                xPosClicados += rectangulo.Width;
                chartCanvas.Children.Add(rectangulo);
            }


            //Añade un boton para reiniciar el grafico
            Button reiniciar = new Button();
            reiniciar.Content = "Reiniciar";
            reiniciar.Width = 100;
            reiniciar.Height = 30;
            reiniciar.Click += Reiniciar_Click;

            Canvas.SetBottom(reiniciar, 10);
            Canvas.SetLeft(reiniciar, 50);
            chartCanvas.Children.Add(reiniciar);
 
            //Añade un boton para completar un pacto
            pacto = new Button();
            pacto.Content = "Pacto";
            pacto.Width = 100;
            pacto.Height = 30;
            pacto.Click += Pacto_Click;
            pacto.IsEnabled = false;
            Canvas.SetBottom(pacto, 10);
            Canvas.SetRight(pacto, 50);
            chartCanvas.Children.Add(pacto);
            //Desactivar el boton de pacto si la suma de los escaños de los partidos de la segunda barra es menor que la mayoria absoluta
            
        }

        //Crea un evento para reiniciar el grafico
        private void Reiniciar_Click(object sender, RoutedEventArgs e)
        {
            partidosEnPrimeraBarra.Clear();
            partidosEnSegundaBarra.Clear();
            clickedRectangles.Clear();
            rectangulosNoClicados.Clear();
            mostrarGrafico3();
        }

        //Crea un evento para completar un pacto
        private void Pacto_Click(object sender, RoutedEventArgs e)
        {
            if (partidosEnSegundaBarra.Any())
            {
                int numeroDeEscaños = partidosEnSegundaBarra.Sum(partido => partido.Escaños);
                string mensaje = "     ¡SE HA PRODUCIDO UN PACTO!\n           TOTAL DE ESCAÑOS: " + numeroDeEscaños + "\n\n     Partidos:\n";
                foreach (var partido in partidosEnSegundaBarra)
                {
                    if (partido != null)
                    {
                        mensaje += "     - " + partido.Nombre + "\n";
                    }
                }
                MessageBox.Show(mensaje);
            }
            else
            {
                MessageBox.Show("No hay partidos en la segunda barra.");
            }
        }


    }
}
