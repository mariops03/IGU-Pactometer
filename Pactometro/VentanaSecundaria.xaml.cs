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
            añadirDatos(ColeccionElecciones);
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

        private void añadirDatos(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {

            //si el numero de colecciones es menor que 4, añade 4 colecciones
            if (coleccionElecciones.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    coleccionElecciones.Add(new ProcesoElectoral());
                }
            }


            // ELECCIONES GENERALES 23/07/2023
            for (int i = 0; i < 11; i++)
            {
                coleccionElecciones[0].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[0].nombre = "Elecciones Generales 23-07-2023";
            coleccionElecciones[0].fecha = new DateTime(2023, 7, 23);
            coleccionElecciones[0].numEscaños = 350;
            coleccionElecciones[0].mayoriaAbsoluta = 176;


            // ELECCIONES GENERALES 23/07/2023
            coleccionElecciones[0].coleccionPartidos[0].Nombre = "PP";
            coleccionElecciones[0].coleccionPartidos[0].Escaños = 136;
            coleccionElecciones[0].coleccionPartidos[0].Color = Colors.Blue;

            coleccionElecciones[0].coleccionPartidos[1].Nombre = "PSOE";
            coleccionElecciones[0].coleccionPartidos[1].Escaños = 122;
            coleccionElecciones[0].coleccionPartidos[1].Color = Colors.Red;

            coleccionElecciones[0].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[0].coleccionPartidos[2].Escaños = 33;
            coleccionElecciones[0].coleccionPartidos[2].Color = Colors.Green;

            coleccionElecciones[0].coleccionPartidos[3].Nombre = "Sumar";
            coleccionElecciones[0].coleccionPartidos[3].Escaños = 31;
            coleccionElecciones[0].coleccionPartidos[3].Color = Colors.Purple;

            coleccionElecciones[0].coleccionPartidos[4].Nombre = "ERC";
            coleccionElecciones[0].coleccionPartidos[4].Escaños = 7;
            coleccionElecciones[0].coleccionPartidos[4].Color = Colors.Yellow;

            coleccionElecciones[0].coleccionPartidos[5].Nombre = "JUNTS";
            coleccionElecciones[0].coleccionPartidos[5].Escaños = 7;
            coleccionElecciones[0].coleccionPartidos[5].Color = Colors.LightGreen;

            coleccionElecciones[0].coleccionPartidos[6].Nombre = "EH_BILDU";
            coleccionElecciones[0].coleccionPartidos[6].Escaños = 6;
            coleccionElecciones[0].coleccionPartidos[6].Color = Colors.LightBlue;

            coleccionElecciones[0].coleccionPartidos[7].Nombre = "PNV";
            coleccionElecciones[0].coleccionPartidos[7].Escaños = 5;
            coleccionElecciones[0].coleccionPartidos[7].Color = Colors.DarkGreen;

            coleccionElecciones[0].coleccionPartidos[8].Nombre = "BNG";
            coleccionElecciones[0].coleccionPartidos[8].Escaños = 1;
            coleccionElecciones[0].coleccionPartidos[8].Color = Colors.MediumPurple;

            coleccionElecciones[0].coleccionPartidos[9].Nombre = "CCA";
            coleccionElecciones[0].coleccionPartidos[9].Escaños = 1;
            coleccionElecciones[0].coleccionPartidos[9].Color = Colors.LightCoral;

            coleccionElecciones[0].coleccionPartidos[10].Nombre = "UPN";
            coleccionElecciones[0].coleccionPartidos[10].Escaños = 1;
            coleccionElecciones[0].coleccionPartidos[10].Color = Colors.LightYellow;


            // ELECCIONES GENERALES 10/11/2019
            for (int i = 0; i < 14; i++)
            {
                coleccionElecciones[1].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[1].nombre = "Elecciones Generales 10-11-2019";
            coleccionElecciones[1].fecha = new DateTime(2019, 11, 10);
            coleccionElecciones[1].numEscaños = 350;
            coleccionElecciones[1].mayoriaAbsoluta = 176;

            coleccionElecciones[1].coleccionPartidos[0].Nombre = "PSOE";
            coleccionElecciones[1].coleccionPartidos[0].Escaños = 120;
            coleccionElecciones[1].coleccionPartidos[0].Color = Colors.Red;

            coleccionElecciones[1].coleccionPartidos[1].Nombre = "PP";
            coleccionElecciones[1].coleccionPartidos[1].Escaños = 89;
            coleccionElecciones[1].coleccionPartidos[1].Color = Colors.Blue;

            coleccionElecciones[1].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[1].coleccionPartidos[2].Escaños = 52;
            coleccionElecciones[1].coleccionPartidos[2].Color = Colors.Green;

            coleccionElecciones[1].coleccionPartidos[3].Nombre = "PODEMOS";
            coleccionElecciones[1].coleccionPartidos[3].Escaños = 35;
            coleccionElecciones[1].coleccionPartidos[3].Color = Colors.Purple;

            coleccionElecciones[1].coleccionPartidos[4].Nombre = "ERC";
            coleccionElecciones[1].coleccionPartidos[4].Escaños = 13;
            coleccionElecciones[1].coleccionPartidos[4].Color = Colors.Yellow;

            coleccionElecciones[1].coleccionPartidos[5].Nombre = "CS";
            coleccionElecciones[1].coleccionPartidos[5].Escaños = 10;
            coleccionElecciones[1].coleccionPartidos[5].Color = Colors.Orange;

            coleccionElecciones[1].coleccionPartidos[6].Nombre = "JUNTS";
            coleccionElecciones[1].coleccionPartidos[6].Escaños = 8;
            coleccionElecciones[1].coleccionPartidos[6].Color = Colors.LightGreen;

            coleccionElecciones[1].coleccionPartidos[7].Nombre = "EAJ_PNV";
            coleccionElecciones[1].coleccionPartidos[7].Escaños = 6;
            coleccionElecciones[1].coleccionPartidos[7].Color = Colors.DarkGreen;

            coleccionElecciones[1].coleccionPartidos[8].Nombre = "EH_BILDU";
            coleccionElecciones[1].coleccionPartidos[8].Escaños = 5;
            coleccionElecciones[1].coleccionPartidos[8].Color = Colors.LightBlue;

            coleccionElecciones[1].coleccionPartidos[9].Nombre = "MASPAIS";
            coleccionElecciones[1].coleccionPartidos[9].Escaños = 3;
            coleccionElecciones[1].coleccionPartidos[9].Color = Colors.Magenta;

            coleccionElecciones[1].coleccionPartidos[10].Nombre = "CUP_PR";
            coleccionElecciones[1].coleccionPartidos[10].Escaños = 2;
            coleccionElecciones[1].coleccionPartidos[10].Color = Colors.MediumPurple; // lightPurple

            coleccionElecciones[1].coleccionPartidos[11].Nombre = "CCA";
            coleccionElecciones[1].coleccionPartidos[11].Escaños = 2;
            coleccionElecciones[1].coleccionPartidos[11].Color = Colors.LightCoral; // lightRed

            coleccionElecciones[1].coleccionPartidos[12].Nombre = "BNG";
            coleccionElecciones[1].coleccionPartidos[12].Escaños = 1;
            coleccionElecciones[1].coleccionPartidos[12].Color = Colors.OrangeRed; 

            coleccionElecciones[1].coleccionPartidos[13].Nombre = "OTROS";
            coleccionElecciones[1].coleccionPartidos[13].Escaños = 4;
            coleccionElecciones[1].coleccionPartidos[13].Color = Colors.Gray;

            // ELECCIONES AUTONÓMICAS CyL 14/2/2022
            for (int i = 0; i < 8; i++)
            {
                coleccionElecciones[2].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[2].nombre = "Elecciones Autonómicas CyL 14-2-2022";
            coleccionElecciones[2].fecha = new DateTime(2022, 2, 14);
            coleccionElecciones[2].numEscaños = 81;
            coleccionElecciones[2].mayoriaAbsoluta = 41;

            // Continuación de ELECCIONES AUTONÓMICAS CyL 14/2/2022
            coleccionElecciones[2].coleccionPartidos[0].Nombre = "PP";
            coleccionElecciones[2].coleccionPartidos[0].Escaños = 31;
            coleccionElecciones[2].coleccionPartidos[0].Color = Colors.Blue;

            coleccionElecciones[2].coleccionPartidos[1].Nombre = "PSOE";
            coleccionElecciones[2].coleccionPartidos[1].Escaños = 28;
            coleccionElecciones[2].coleccionPartidos[1].Color = Colors.Red;

            coleccionElecciones[2].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[2].coleccionPartidos[2].Escaños = 13;
            coleccionElecciones[2].coleccionPartidos[2].Color = Colors.Green;

            coleccionElecciones[2].coleccionPartidos[3].Nombre = "UPL";
            coleccionElecciones[2].coleccionPartidos[3].Escaños = 3;
            coleccionElecciones[2].coleccionPartidos[3].Color = Colors.MediumPurple; // lightPurple

            coleccionElecciones[2].coleccionPartidos[4].Nombre = "SY";
            coleccionElecciones[2].coleccionPartidos[4].Escaños = 3;
            coleccionElecciones[2].coleccionPartidos[4].Color = Colors.LightBlue;

            coleccionElecciones[2].coleccionPartidos[5].Nombre = "PODEMOS";
            coleccionElecciones[2].coleccionPartidos[5].Escaños = 1;
            coleccionElecciones[2].coleccionPartidos[5].Color = Colors.Purple;

            coleccionElecciones[2].coleccionPartidos[6].Nombre = "CS";
            coleccionElecciones[2].coleccionPartidos[6].Escaños = 1;
            coleccionElecciones[2].coleccionPartidos[6].Color = Colors.Orange;

            coleccionElecciones[2].coleccionPartidos[7].Nombre = "XAV";
            coleccionElecciones[2].coleccionPartidos[7].Escaños = 1;
            coleccionElecciones[2].coleccionPartidos[7].Color = Color.FromRgb(144, 238, 144); // lightGreen

            // ELECCIONES AUTONÓMICAS CyL 26/5/2019
            for (int i = 0; i < 7; i++)
            {
                coleccionElecciones[3].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[3].nombre = "Elecciones Autonómicas CyL 26-5-2019";
            coleccionElecciones[3].fecha = new DateTime(2019, 5, 26);
            coleccionElecciones[3].numEscaños = 81;
            coleccionElecciones[3].mayoriaAbsoluta = 41;

            coleccionElecciones[3].coleccionPartidos[0].Nombre = "PSOE";
            coleccionElecciones[3].coleccionPartidos[0].Escaños = 35;
            coleccionElecciones[3].coleccionPartidos[0].Color = Colors.Red;

            coleccionElecciones[3].coleccionPartidos[1].Nombre = "PP";
            coleccionElecciones[3].coleccionPartidos[1].Escaños = 29;
            coleccionElecciones[3].coleccionPartidos[1].Color = Colors.Blue;

            coleccionElecciones[3].coleccionPartidos[2].Nombre = "CS";
            coleccionElecciones[3].coleccionPartidos[2].Escaños = 12;
            coleccionElecciones[3].coleccionPartidos[2].Color = Colors.Orange;

            coleccionElecciones[3].coleccionPartidos[3].Nombre = "PODEMOS";
            coleccionElecciones[3].coleccionPartidos[3].Escaños = 2;
            coleccionElecciones[3].coleccionPartidos[3].Color = Colors.Purple;

            coleccionElecciones[3].coleccionPartidos[4].Nombre = "VOX";
            coleccionElecciones[3].coleccionPartidos[4].Escaños = 1;
            coleccionElecciones[3].coleccionPartidos[4].Color = Colors.Green;

            coleccionElecciones[3].coleccionPartidos[5].Nombre = "UPL";
            coleccionElecciones[3].coleccionPartidos[5].Escaños = 1;
            coleccionElecciones[3].coleccionPartidos[5].Color = Colors.MediumPurple; // lightPurple

            coleccionElecciones[3].coleccionPartidos[6].Nombre = "XAV";
            coleccionElecciones[3].coleccionPartidos[6].Escaños = 1;
            coleccionElecciones[3].coleccionPartidos[6].Color = Color.FromRgb(144, 238, 144); // lightGreen

            // ELECCIONES GENERALES 30-11-2023
            for (int i = 0; i < 7; i++)
            {
                coleccionElecciones[4].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[4].nombre = "Elecciones Generales 30-11-2023";
            coleccionElecciones[4].fecha = new DateTime(2023, 11, 30);
            coleccionElecciones[4].numEscaños = 350;
            coleccionElecciones[4].mayoriaAbsoluta = 176;

            coleccionElecciones[4].coleccionPartidos[0].Nombre = "PP";
            coleccionElecciones[4].coleccionPartidos[0].Escaños = 166;
            coleccionElecciones[4].coleccionPartidos[0].Color = Colors.Blue;

            coleccionElecciones[4].coleccionPartidos[1].Nombre = "PSOE";
            coleccionElecciones[4].coleccionPartidos[1].Escaños = 100;
            coleccionElecciones[4].coleccionPartidos[1].Color = Colors.Red;

            coleccionElecciones[4].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[4].coleccionPartidos[2].Escaños = 40;
            coleccionElecciones[4].coleccionPartidos[2].Color = Colors.Green;

            coleccionElecciones[4].coleccionPartidos[3].Nombre = "PODEMOS";
            coleccionElecciones[4].coleccionPartidos[3].Escaños = 20;
            coleccionElecciones[4].coleccionPartidos[3].Color = Colors.Purple;

            coleccionElecciones[4].coleccionPartidos[4].Nombre = "ERC";
            coleccionElecciones[4].coleccionPartidos[4].Escaños = 10;
            coleccionElecciones[4].coleccionPartidos[4].Color = Colors.Yellow;

            coleccionElecciones[4].coleccionPartidos[5].Nombre = "JUNTS";
            coleccionElecciones[4].coleccionPartidos[5].Escaños = 10;
            coleccionElecciones[4].coleccionPartidos[5].Color = Color.FromRgb(144, 238, 144); // lightGreen

            coleccionElecciones[4].coleccionPartidos[6].Nombre = "EH_BILDU";
            coleccionElecciones[4].coleccionPartidos[6].Escaños = 10;
            coleccionElecciones[4].coleccionPartidos[6].Color = Colors.LightBlue;

            // Continúa añadiendo más partidos y elecciones si es necesario

            mainTable.ItemsSource = coleccionElecciones;
        
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
            }
            else 
            { 
                MessageBox.Show("No hay ningún proceso electoral seleccionado");
            }
        }

    }
}
