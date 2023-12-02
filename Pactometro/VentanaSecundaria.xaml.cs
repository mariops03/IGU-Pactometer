﻿using System;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;


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
            añadirDatos(coleccionElecciones);
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

            coleccionElecciones[0].coleccionPartidos[6].Nombre = "EH Bildu";
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

            coleccionElecciones[4].coleccionPartidos[6].Nombre = "EH Bildu";
            coleccionElecciones[4].coleccionPartidos[6].Escaños = 10;
            coleccionElecciones[4].coleccionPartidos[6].Color = Colors.LightBlue;

            // Continúa añadiendo más partidos y elecciones si es necesario

            mainTable.ItemsSource = coleccionElecciones;
        
    }

    private static VentanaAgregar ventanaAñadirProceso;

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            // Verifica si ya hay una instancia abierta
            if (ventanaAñadirProceso == null || !ventanaAñadirProceso.IsVisible)
            {
                ventanaAñadirProceso = new VentanaAgregar(ColeccionElecciones);
                ventanaAñadirProceso.Closed += VentanaAñadirProceso_Closed;
                ventanaAñadirProceso.ShowDialog();
            }
            else
            {
                // La ventana ya está abierta, puedes llevarla al frente si es necesario
                ventanaAñadirProceso.Activate();
            }
        }

        private void VentanaAñadirProceso_Closed(object sender, EventArgs e)
        {
            // Manejar el evento de cierre, por ejemplo, establecer la referencia a null
            ventanaAñadirProceso = null;
        }

        private void ExportarDatos()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo JSON (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                string json = JsonConvert.SerializeObject(ColeccionElecciones);
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private void ImportarDatos()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivo JSON (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(openFileDialog.FileName);
                ObservableCollection<ProcesoElectoral> datosImportados = JsonConvert.DeserializeObject<ObservableCollection<ProcesoElectoral>>(json);

                // Aquí debes decidir si reemplazar los datos existentes o agregarlos a la colección existente
                ColeccionElecciones.Clear();
                foreach (var item in datosImportados)
                {
                    ColeccionElecciones.Add(item);
                }
            }
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para importar los datos
            // Podría ser abrir un cuadro de diálogo para seleccionar un archivo, etc.
        }

        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para exportar los datos
            // Podría ser abrir un cuadro de diálogo para guardar un archivo, etc.
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (mainTable.SelectedItem != null)
            {
                // Crea una instancia de 
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
