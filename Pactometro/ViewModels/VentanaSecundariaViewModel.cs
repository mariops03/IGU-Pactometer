using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Linq;
using System.IO;
using System.Windows.Media;
using System.Collections.Generic;

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
                FileName = $"DatosExportados {DateTime.Now.ToString("dd-MM-yyyy HHmmss")}.csv"
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
            OrdenarPorFecha();
        }


        public void ExportarDatosJson()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo JSON (*.json)|*.json",
                // Crea un file name con la fecha actual
                FileName = $"DatosExportados {DateTime.Now.ToString("dd-MM-yyyy HHmmss")}.json"
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
            OrdenarPorFecha();
        }

        public void AñadirProcesoElectoral()
        {
            // Crear una nueva instancia de ProcesoElectoral para añadir
            ProcesoElectoral nuevoProceso = new ProcesoElectoral();

            // Crear la ventana en modo Agregar, pasando el nuevo proceso electoral
            VentanaAñadirModificar ventanaAñadirProceso = new VentanaAñadirModificar(ModoVentana.Agregar, Elecciones, nuevoProceso);
            ventanaAñadirProceso.ShowDialog();

            // Actualizar la colección de procesos electorales
            OrdenarPorFechaDescendente();
        }

        public void ModificarProcesoElectoral()
        {
            if (EleccionSeleccionada == null)
            {
                MessageBox.Show("No hay ningún proceso electoral seleccionado");
                return;
            }

            // Crear la ventana en modo Modificar, pasando el proceso existente
            VentanaAñadirModificar ventanaModificar = new VentanaAñadirModificar(ModoVentana.Modificar, Elecciones, EleccionSeleccionada);
            ventanaModificar.Owner = Application.Current.MainWindow;
            ventanaModificar.ShowDialog();

            // Actualizar la colección de procesos electorales
            OrdenarPorFechaDescendente();
        }

        public void eliminarProcesoElectoral()
        {
            if (EleccionSeleccionada == null)
            {
                MessageBox.Show("No hay ningún proceso electoral seleccionado");
                return;
            }

            // Elimina de la colección el elemento seleccionado
            Elecciones.Remove(EleccionSeleccionada);
        }

        public void OrdenarPorFecha()
        {
            List<ProcesoElectoral> ordenada;

            if (Elecciones.SequenceEqual(Elecciones.OrderBy(e => e.fecha)))
            {
                ordenada = Elecciones.OrderByDescending(e => e.fecha).ToList();
            }
            else
            {
                ordenada = Elecciones.OrderBy(e => e.fecha).ToList();
            }

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }

        // Método para ordenar por nombre
        public void OrdenarPorNombre()
        {
            List<ProcesoElectoral> ordenada;

            if (Elecciones.SequenceEqual(Elecciones.OrderBy(e => e.nombre)))
            {
                ordenada = Elecciones.OrderByDescending(e => e.nombre).ToList();
            }
            else
            {
                ordenada = Elecciones.OrderBy(e => e.nombre).ToList();
            }

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }

        // Método para ordenar por número de escaños
        public void OrdenarPorEscaños()
        {
            List<ProcesoElectoral> ordenada;

            if (Elecciones.SequenceEqual(Elecciones.OrderBy(e => e.numEscaños)))
            {
                ordenada = Elecciones.OrderByDescending(e => e.numEscaños).ToList();
            }
            else
            {
                ordenada = Elecciones.OrderBy(e => e.numEscaños).ToList();
            }

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }

        // Método para ordenar por nombre del partido
        public void OrdenarPorNombrePartido()
        {
            if (EleccionSeleccionada == null || EleccionSeleccionada.coleccionPartidos == null)
                return;

            var coleccion = EleccionSeleccionada.coleccionPartidos;
            List<Partido> ordenada;

            if (coleccion.SequenceEqual(coleccion.OrderBy(p => p.Nombre)))
            {
                ordenada = coleccion.OrderByDescending(p => p.Nombre).ToList();
            }
            else
            {
                ordenada = coleccion.OrderBy(p => p.Nombre).ToList();
            }

            coleccion.Clear();
            foreach (var partido in ordenada)
            {
                coleccion.Add(partido);
            }
        }

        // Método para ordenar por escaños del partido
        public void OrdenarPorEscañosPartido()
        {
            if (EleccionSeleccionada == null || EleccionSeleccionada.coleccionPartidos == null)
                return;

            var coleccion = EleccionSeleccionada.coleccionPartidos;
            List<Partido> ordenada;

            if (coleccion.SequenceEqual(coleccion.OrderBy(p => p.Escaños)))
            {
                ordenada = coleccion.OrderByDescending(p => p.Escaños).ToList();
            }
            else
            {
                ordenada = coleccion.OrderBy(p => p.Escaños).ToList();
            }

            coleccion.Clear();
            foreach (var partido in ordenada)
            {
                coleccion.Add(partido);
            }
        }

        //Funcion para ordenar los partidos por escaños de forma descendente, pero esta funcion recibe una coleccion de partidos como parametro
        public void OrdenarPorEscañosPartidoDescendente(ObservableCollection<Partido> coleccion)
        {
            List<Partido> ordenada;

            ordenada = coleccion.OrderByDescending(p => p.Escaños).ToList();

            coleccion.Clear();
            foreach (var partido in ordenada)
            {
                coleccion.Add(partido);
            }
        }

        //Funcion para ordenar las elecciones por fecha de forma descendente
        public void OrdenarPorFechaDescendente()
        {
            List<ProcesoElectoral> ordenada;

            ordenada = Elecciones.OrderByDescending(e => e.fecha).ToList();

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }
    }
}
