using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Windows;
using Pactometro; // Asegúrate de que este espacio de nombres contenga tus clases de modelo
using System.Linq;
using System.IO;
using System.Windows.Media;

namespace Pactometro.ViewModels
{
    public class VentanaSecundariaViewModel : BaseViewModel
    {
        public ObservableCollection<Partido> Partidos => EleccionSeleccionada?.coleccionPartidos;

        public VentanaSecundariaViewModel(ObservableCollection<ProcesoElectoral> elecciones)
        {
            Elecciones = elecciones; // Usar la propiedad heredada
        }

        public void ExportarDatosCSV()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo CSV (*.csv)|*.csv",
                //Crea un file name con la fecha actual
                FileName = $"DatosExportados_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StringBuilder csvContent = new StringBuilder();
                    // Encabezados del CSV
                    csvContent.AppendLine("Nombre,Fecha,NumEscaños,MayoriaAbsoluta,Partidos");

                    foreach (var proceso in Elecciones)
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



        public void ImportarDatosCSV()
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

                    Elecciones.Clear();
                    foreach (var item in datosImportados)
                    {
                        Elecciones.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al importar el archivo CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            OrdenarPorFecha(Elecciones);
        }


        public void ExportarDatosJson()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo JSON (*.json)|*.json",
                // Crea un file name con la fecha actual
                FileName = $"DatosExportados_{DateTime.Now.ToString("yyyyMMddHHmmss")}.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(Elecciones, Formatting.Indented);
                    File.WriteAllText(saveFileDialog.FileName, json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar el archivo JSON: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void ImportarDatosJSON()
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
                        Elecciones.Clear();
                        foreach (var proceso in datosImportados)
                        {
                            Elecciones.Add(proceso);
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
            OrdenarPorFecha(Elecciones);
        }
    }
}
