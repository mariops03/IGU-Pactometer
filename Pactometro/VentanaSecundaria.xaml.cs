using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;

        // Definir el evento para notificar la selección
        public event EventHandler<string> ProcesoEleccionSeleccionado;

        public VentanaSecundaria(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            ColeccionElecciones = coleccionElecciones;
            añadirDatos(coleccionElecciones);
        }

        private void mainTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTable.SelectedItem != null)
            {
                var selectedData = mainTable.SelectedItem as ProcesoElectoral;

                if (selectedData != null)
                {
                    // Asegúrate de que coleccionPartidos es una propiedad en ProcesoElectoral
                    secondaryTable.ItemsSource = selectedData.coleccionPartidos;

                    if (secondaryTable.Items.Count > 0)
                    {
                        secondaryTable.ScrollIntoView(secondaryTable.Items[0]);
                    }

                    // Disparar el evento para notificar a la MainWindow sobre la selección
                    ProcesoEleccionSeleccionado?.Invoke(this, selectedData.nombre);
                }
            }
        }

        private void añadirDatos(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            //si el numero de colecciones es menor que 4, añade 4 colecciones
            if (coleccionElecciones.Count == 0)
            {
                for (int i = 0; i < 4; i++)
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
            coleccionElecciones[0].fecha = new DateTime(2023, 07, 23).Date;
            coleccionElecciones[0].numEscaños = 350;
            coleccionElecciones[0].mayoriaAbsoluta = 176;

            coleccionElecciones[0].coleccionPartidos[0].Nombre = "PP";
            coleccionElecciones[0].coleccionPartidos[0].Escaños = 136;
            coleccionElecciones[0].coleccionPartidos[0].Color = "Blue";

            coleccionElecciones[0].coleccionPartidos[1].Nombre = "PSOE";
            coleccionElecciones[0].coleccionPartidos[1].Escaños = 122;
            coleccionElecciones[0].coleccionPartidos[1].Color = "Red";

            coleccionElecciones[0].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[0].coleccionPartidos[2].Escaños = 33;
            coleccionElecciones[0].coleccionPartidos[2].Color = "Green";

            coleccionElecciones[0].coleccionPartidos[3].Nombre = "Sumar";
            coleccionElecciones[0].coleccionPartidos[3].Escaños = 31;
            coleccionElecciones[0].coleccionPartidos[3].Color = "Purple";

            coleccionElecciones[0].coleccionPartidos[4].Nombre = "ERC";
            coleccionElecciones[0].coleccionPartidos[4].Escaños = 7;
            coleccionElecciones[0].coleccionPartidos[4].Color = "Yellow";

            coleccionElecciones[0].coleccionPartidos[5].Nombre = "JUNTS";
            coleccionElecciones[0].coleccionPartidos[5].Escaños = 7;
            coleccionElecciones[0].coleccionPartidos[5].Color = "lightGreen";

            coleccionElecciones[0].coleccionPartidos[6].Nombre = "EH Bildu";
            coleccionElecciones[0].coleccionPartidos[6].Escaños = 6;
            coleccionElecciones[0].coleccionPartidos[6].Color = "lightBlue";

            coleccionElecciones[0].coleccionPartidos[7].Nombre = "PNV";
            coleccionElecciones[0].coleccionPartidos[7].Escaños = 5;
            coleccionElecciones[0].coleccionPartidos[7].Color = "darkGreen";

            coleccionElecciones[0].coleccionPartidos[8].Nombre = "BNG";
            coleccionElecciones[0].coleccionPartidos[8].Escaños = 1;
            coleccionElecciones[0].coleccionPartidos[8].Color = "lightPurple";

            coleccionElecciones[0].coleccionPartidos[9].Nombre = "CCA";
            coleccionElecciones[0].coleccionPartidos[9].Escaños = 1;
            coleccionElecciones[0].coleccionPartidos[9].Color = "lightRed";

            coleccionElecciones[0].coleccionPartidos[10].Nombre = "UPN";
            coleccionElecciones[0].coleccionPartidos[10].Escaños = 1;
            coleccionElecciones[0].coleccionPartidos[10].Color = "lightYellow";

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
            coleccionElecciones[1].coleccionPartidos[0].Color = "Red";

            coleccionElecciones[1].coleccionPartidos[1].Nombre = "PP";
            coleccionElecciones[1].coleccionPartidos[1].Escaños = 89;
            coleccionElecciones[1].coleccionPartidos[1].Color = "Blue";

            coleccionElecciones[1].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[1].coleccionPartidos[2].Escaños = 52;
            coleccionElecciones[1].coleccionPartidos[2].Color = "Green";

            coleccionElecciones[1].coleccionPartidos[3].Nombre = "PODEMOS";
            coleccionElecciones[1].coleccionPartidos[3].Escaños = 35;
            coleccionElecciones[1].coleccionPartidos[3].Color = "Purple";

            coleccionElecciones[1].coleccionPartidos[4].Nombre = "ERC";
            coleccionElecciones[1].coleccionPartidos[4].Escaños = 13;
            coleccionElecciones[1].coleccionPartidos[4].Color = "Yellow";

            coleccionElecciones[1].coleccionPartidos[5].Nombre = "CS";
            coleccionElecciones[1].coleccionPartidos[5].Escaños = 10;
            coleccionElecciones[1].coleccionPartidos[5].Color = "Orange";

            coleccionElecciones[1].coleccionPartidos[6].Nombre = "JUNTS";
            coleccionElecciones[1].coleccionPartidos[6].Escaños = 8;
            coleccionElecciones[1].coleccionPartidos[6].Color = "lightGreen";

            coleccionElecciones[1].coleccionPartidos[7].Nombre = "EAJ_PNV";
            coleccionElecciones[1].coleccionPartidos[7].Escaños = 6;
            coleccionElecciones[1].coleccionPartidos[7].Color = "darkGreen";

            coleccionElecciones[1].coleccionPartidos[8].Nombre = "EH_BILDU";
            coleccionElecciones[1].coleccionPartidos[8].Escaños = 5;
            coleccionElecciones[1].coleccionPartidos[8].Color = "lightBlue";

            coleccionElecciones[1].coleccionPartidos[9].Nombre = "MASPAIS";
            coleccionElecciones[1].coleccionPartidos[9].Escaños = 3;
            coleccionElecciones[1].coleccionPartidos[9].Color = "Magenta";

            coleccionElecciones[1].coleccionPartidos[10].Nombre = "CUP_PR";
            coleccionElecciones[1].coleccionPartidos[10].Escaños = 2;
            coleccionElecciones[1].coleccionPartidos[10].Color = "lightPurple";

            coleccionElecciones[1].coleccionPartidos[11].Nombre = "CCA";
            coleccionElecciones[1].coleccionPartidos[11].Escaños = 2;
            coleccionElecciones[1].coleccionPartidos[11].Color = "lightRed";

            coleccionElecciones[1].coleccionPartidos[12].Nombre = "BNG";
            coleccionElecciones[1].coleccionPartidos[12].Escaños = 1;
            coleccionElecciones[1].coleccionPartidos[12].Color = "lightOrange";

            coleccionElecciones[1].coleccionPartidos[13].Nombre = "OTROS";
            coleccionElecciones[1].coleccionPartidos[13].Escaños = 4;
            coleccionElecciones[1].coleccionPartidos[13].Color = "Gray";

            // ELECCIONES AUTONÓMICAS CyL 14/2/2022
            for (int i = 0; i < 8; i++)
            {
                coleccionElecciones[2].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[2].nombre = "Elecciones Autonómicas CyL 14-2-2022";
            coleccionElecciones[2].fecha = new DateTime(2022, 2, 14);
            coleccionElecciones[2].numEscaños = 81;
            coleccionElecciones[2].mayoriaAbsoluta = 41;

            coleccionElecciones[2].coleccionPartidos[0].Nombre = "PP";
            coleccionElecciones[2].coleccionPartidos[0].Escaños = 31;
            coleccionElecciones[2].coleccionPartidos[0].Color = "Blue";

            coleccionElecciones[2].coleccionPartidos[1].Nombre = "PSOE";
            coleccionElecciones[2].coleccionPartidos[1].Escaños = 28;
            coleccionElecciones[2].coleccionPartidos[1].Color = "Red";

            coleccionElecciones[2].coleccionPartidos[2].Nombre = "VOX";
            coleccionElecciones[2].coleccionPartidos[2].Escaños = 13;
            coleccionElecciones[2].coleccionPartidos[2].Color = "Green";

            coleccionElecciones[2].coleccionPartidos[3].Nombre = "UPL";
            coleccionElecciones[2].coleccionPartidos[3].Escaños = 3;
            coleccionElecciones[2].coleccionPartidos[3].Color = "lightPurple";

            coleccionElecciones[2].coleccionPartidos[4].Nombre = "SY";
            coleccionElecciones[2].coleccionPartidos[4].Escaños = 3;
            coleccionElecciones[2].coleccionPartidos[4].Color = "lightBlue";

            coleccionElecciones[2].coleccionPartidos[5].Nombre = "PODEMOS";
            coleccionElecciones[2].coleccionPartidos[5].Escaños = 1;
            coleccionElecciones[2].coleccionPartidos[5].Color = "Purple";

            coleccionElecciones[2].coleccionPartidos[6].Nombre = "CS";
            coleccionElecciones[2].coleccionPartidos[6].Escaños = 1;
            coleccionElecciones[2].coleccionPartidos[6].Color = "Orange";

            coleccionElecciones[2].coleccionPartidos[7].Nombre = "XAV";
            coleccionElecciones[2].coleccionPartidos[7].Escaños = 1;
            coleccionElecciones[2].coleccionPartidos[7].Color = "lightGreen";

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
            coleccionElecciones[3].coleccionPartidos[0].Color = "Red";

            coleccionElecciones[3].coleccionPartidos[1].Nombre = "PP";
            coleccionElecciones[3].coleccionPartidos[1].Escaños = 29;
            coleccionElecciones[3].coleccionPartidos[1].Color = "Blue";

            coleccionElecciones[3].coleccionPartidos[2].Nombre = "CS";
            coleccionElecciones[3].coleccionPartidos[2].Escaños = 12;
            coleccionElecciones[3].coleccionPartidos[2].Color = "Orange";

            coleccionElecciones[3].coleccionPartidos[3].Nombre = "PODEMOS";
            coleccionElecciones[3].coleccionPartidos[3].Escaños = 2;
            coleccionElecciones[3].coleccionPartidos[3].Color = "Purple";

            coleccionElecciones[3].coleccionPartidos[4].Nombre = "VOX";
            coleccionElecciones[3].coleccionPartidos[4].Escaños = 1;
            coleccionElecciones[3].coleccionPartidos[4].Color = "Green";

            coleccionElecciones[3].coleccionPartidos[5].Nombre = "UPL";
            coleccionElecciones[3].coleccionPartidos[5].Escaños = 1;
            coleccionElecciones[3].coleccionPartidos[5].Color = "lightPurple";

            coleccionElecciones[3].coleccionPartidos[6].Nombre = "XAV";
            coleccionElecciones[3].coleccionPartidos[6].Escaños = 1;
            coleccionElecciones[3].coleccionPartidos[6].Color = "lightGreen";

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
    }
}
