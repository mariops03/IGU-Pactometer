using System;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using Pactometro.ViewModels;


namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;

        // Definir el evento para notificar la selección
        public event EventHandler<ProcesoElectoral> ProcesoEleccionSeleccionado;
        ProcesoElectoral procesoElectoralSeleccionado;

        public VentanaSecundaria(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            ColeccionElecciones = coleccionElecciones;
            this.DataContext = new VentanaSecundariaViewModel(ColeccionElecciones);
        }

        private void mainTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtiene el elemento seleccionado, que puede ser null si no hay selección
            ProcesoElectoral procesoElectoralSeleccionado = mainTable.SelectedItem as ProcesoElectoral;

            if (procesoElectoralSeleccionado != null)
            {
                // Actualiza secondaryTable si hay una selección
                secondaryTable.ItemsSource = procesoElectoralSeleccionado.coleccionPartidos;
                if (secondaryTable.Items.Count > 0)
                {
                    secondaryTable.ScrollIntoView(secondaryTable.Items[0]);
                }
                btnModificar.Visibility = Visibility.Visible;
                btnEliminar.Visibility = Visibility.Visible;
            }
            else
            {
                // Limpia secondaryTable si no hay selección
                secondaryTable.ItemsSource = null;
                btnModificar.Visibility = Visibility.Collapsed;
                btnEliminar.Visibility = Visibility.Collapsed;
            }

            // Dispara el evento con el elemento seleccionado o null
            ProcesoEleccionSeleccionado?.Invoke(this, procesoElectoralSeleccionado);
        }


        private static VentanaAgregar ventanaAñadirProceso;

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia de la clase VentanaAñadirProceso, que es una ventana modal y hazte su propietario
            ventanaAñadirProceso = new VentanaAgregar(ColeccionElecciones);
            ventanaAñadirProceso.Owner = this;
            ventanaAñadirProceso.ShowDialog();
        }

        private void VentanaAñadirProceso_Closed(object sender, EventArgs e)
        {
            // Manejar el evento de cierre, por ejemplo, establecer la referencia a null
            ventanaAñadirProceso = null;
        }

        private void ExportarDatosCSV()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                FileName = "DatosExportados.csv" // Nombre de archivo predeterminado para CSV
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StringBuilder csvContent = new StringBuilder();
                    // Encabezados del CSV
                    csvContent.AppendLine("Nombre,Fecha,NumEscaños,MayoriaAbsoluta,Partidos");

                    foreach (var proceso in ColeccionElecciones)
                    {
                        // Convertir la colección de partidos a una cadena de texto incluyendo el color
                        var partidosString = string.Join("; ", proceso.coleccionPartidos.Select(p =>
                            $"{p.Nombre} ({p.Escaños} escaños) - {p.Color.ToString()}"));

                        // Añadir una línea al CSV para cada ProcesoElectoral
                        string fechaFormateada = proceso.fecha.ToString("yyyy-MM-dd"); // Formato de fecha ISO 8601
                        csvContent.AppendLine($"\"{proceso.nombre}\",\"{fechaFormateada}\",{proceso.numEscaños},{proceso.mayoriaAbsoluta},\"{partidosString}\"");
                    }

                    // Escribir el contenido CSV en el archivo
                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el archivo CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void ImportarDatosCSV()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var datosImportados = new ObservableCollection<ProcesoElectoral>();
                    var lines = File.ReadAllLines(openFileDialog.FileName);

                    foreach (var line in lines.Skip(1)) // Se omite el encabezado
                    {
                        var columns = line.Split(new[] { ',' }, 5); // Divide solo en las primeras 5 columnas, la última contiene los partidos

                        var proceso = new ProcesoElectoral
                        {
                            nombre = columns[0].Trim('"'),
                            fecha = DateTime.ParseExact(columns[1].Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            numEscaños = int.Parse(columns[2]),
                            mayoriaAbsoluta = int.Parse(columns[3]),
                            coleccionPartidos = new ObservableCollection<Partido>()
                        };

                        var partidosInfo = columns[4].Trim('"').Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var partidoInfo in partidosInfo)
                        {
                            // Dividimos la información del partido en los componentes nombre, escaños y color
                            var partidoDetails = partidoInfo.Trim().Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                            var nombreYEscaños = partidoDetails[0].Split(new[] { " (" }, StringSplitOptions.RemoveEmptyEntries);
                            var nombrePartido = nombreYEscaños[0].Trim();
                            var escañosPartido = int.Parse(nombreYEscaños[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0]);

                            // Parseamos el color en formato hexadecimal
                            var colorPartido = (Color)ColorConverter.ConvertFromString(partidoDetails[1].Trim());

                            proceso.coleccionPartidos.Add(new Partido
                            {
                                Nombre = nombrePartido,
                                Escaños = escañosPartido,
                                Color = colorPartido
                            });
                        }

                        datosImportados.Add(proceso);
                    }

                    ColeccionElecciones.Clear();
                    foreach (var item in datosImportados)
                    {
                        ColeccionElecciones.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al importar el archivo CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            OrdenarColeccionPorFecha();
        }




        private void ExportarDatosJson()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo JSON (*.json)|*.json",
                FileName = "DatosExportados.json" // Nombre de archivo predeterminado para JSON
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(ColeccionElecciones, Formatting.Indented);
                    File.WriteAllText(saveFileDialog.FileName, json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el archivo JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ImportarDatosJSON()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivo JSON (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Leer el contenido del archivo
                    string json = File.ReadAllText(openFileDialog.FileName);

                    // Deserializar el JSON a la colección
                    var datosImportados = JsonConvert.DeserializeObject<ObservableCollection<ProcesoElectoral>>(json);

                    if (datosImportados != null)
                    {
                        ColeccionElecciones.Clear();
                        foreach (var proceso in datosImportados)
                        {
                            ColeccionElecciones.Add(proceso);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El archivo JSON no contiene datos válidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al importar de JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            OrdenarColeccionPorFecha();
        }


        private void btnImportarCSV_Click(object sender, RoutedEventArgs e)
        {
            ImportarDatosCSV();
        }

        private void btnExportarCSV_Click(object sender, RoutedEventArgs e)
        {
            ExportarDatosCSV();
        }

        private void btnImportarJSON_Click(object sender, RoutedEventArgs e)
        {
            ImportarDatosJSON();
        }

        private void btnExportarJSON_Click(object sender, RoutedEventArgs e)
        {
            ExportarDatosJson();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (mainTable.SelectedItem != null)
            {
                // Crea una instancia de la clase VentanaModificar con el proceso electoral seleccionado
                VentanaModificar ventanaModificar = new VentanaModificar(ColeccionElecciones, mainTable.SelectedItem as ProcesoElectoral);
                ventanaModificar.Owner = this;
                ventanaModificar.ShowDialog();
                OrdenarColeccionPorFecha();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para eliminar un proceso electoral existente
            // Asegúrate de que hay un proceso electoral seleccionado antes de continuar.
            if (mainTable.SelectedItem != null)
            {
                // Elimina de la colección el elemento seleccionado
                ColeccionElecciones.Remove(mainTable.SelectedItem as ProcesoElectoral);
                OrdenarColeccionPorFecha();
            }
            else 
            { 
                MessageBox.Show("No hay ningún proceso electoral seleccionado");
            }
        }

        public void OrdenarColeccionPorFecha()
        {
            var itemsOrdenados = ColeccionElecciones.OrderByDescending(proceso => proceso.fecha).ToList();

            ColeccionElecciones.Clear();

            foreach (var item in itemsOrdenados)
            {
                ColeccionElecciones.Add(item);
            }
        }


    }
}
