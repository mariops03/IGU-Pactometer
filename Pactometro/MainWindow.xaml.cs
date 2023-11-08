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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            VentanaSecundaria ventanaSecundaria = new VentanaSecundaria();

            ventanaSecundaria.mainTable.ItemsSource = coleccionElecciones;

            // Calcular la posición de la ventana secundaria en relación con la ventana principal
            double distanciaEntreVentanas = 10; // Puedes ajustar esto a tu preferencia
            double nuevaPosX = Left + Width + distanciaEntreVentanas;
            double nuevaPosY = Top;

            // Establecer la posición de la ventana secundaria
            ventanaSecundaria.Left = nuevaPosX;
            ventanaSecundaria.Top = nuevaPosY;

            // Mostrar la ventana secundaria
            ventanaSecundaria.Show();
        }

        private void añadirPatidos()
        {
            ObservableCollection<ProcesoElectoral> coleccionElecciones = new ObservableCollection<ProcesoElectoral>();
            for (int i = 0; i < 5; i++)
            {
                coleccionElecciones.Add(new ProcesoElectoral());
            }

            // ELECCIONES GENERALES 23/07/2023
            for(int i = 0; i < 11; i++)
            {
                coleccionElecciones[0].coleccionPartidos.Add(new Partido());
            }

            
            coleccionElecciones[0].nombre = "Elecciones Generales 23-07-2023";
            coleccionElecciones[0].fecha = new DateTime(2023, 07, 23);
            coleccionElecciones[0].numEscaños = 350;
            coleccionElecciones[0].mayoriaAbsoluta = 176;

            coleccionElecciones[0].coleccionPartidos[0].nombre = "PP";
            coleccionElecciones[0].coleccionPartidos[0].escaños = 136;
            coleccionElecciones[0].coleccionPartidos[0].color = "Blue";

            coleccionElecciones[0].coleccionPartidos[1].nombre = "PSOE";
            coleccionElecciones[0].coleccionPartidos[1].escaños = 122;
            coleccionElecciones[0].coleccionPartidos[1].color = "Red";

            coleccionElecciones[0].coleccionPartidos[2].nombre = "VOX";
            coleccionElecciones[0].coleccionPartidos[2].escaños = 33;
            coleccionElecciones[0].coleccionPartidos[2].color = "Green";

            coleccionElecciones[0].coleccionPartidos[3].nombre = "Sumar";
            coleccionElecciones[0].coleccionPartidos[3].escaños = 31;
            coleccionElecciones[0].coleccionPartidos[3].color = "Purple";

            coleccionElecciones[0].coleccionPartidos[4].nombre = "ERC";
            coleccionElecciones[0].coleccionPartidos[4].escaños = 7;
            coleccionElecciones[0].coleccionPartidos[4].color = "Yellow";

            coleccionElecciones[0].coleccionPartidos[5].nombre = "JUNTS";
            coleccionElecciones[0].coleccionPartidos[5].escaños = 7;
            coleccionElecciones[0].coleccionPartidos[5].color = "lightGreen";

            coleccionElecciones[0].coleccionPartidos[6].nombre = "EH Bildu";
            coleccionElecciones[0].coleccionPartidos[6].escaños = 6;
            coleccionElecciones[0].coleccionPartidos[6].color = "lightBlue";

            coleccionElecciones[0].coleccionPartidos[7].nombre = "PNV";
            coleccionElecciones[0].coleccionPartidos[7].escaños = 5;
            coleccionElecciones[0].coleccionPartidos[7].color = "darkGreen";

            coleccionElecciones[0].coleccionPartidos[8].nombre = "BNG";
            coleccionElecciones[0].coleccionPartidos[8].escaños = 1;
            coleccionElecciones[0].coleccionPartidos[8].color = "lightPurple";

            coleccionElecciones[0].coleccionPartidos[9].nombre = "CCA";
            coleccionElecciones[0].coleccionPartidos[9].escaños = 1;
            coleccionElecciones[0].coleccionPartidos[9].color = "lightRed";

            coleccionElecciones[0].coleccionPartidos[10].nombre = "UPN";
            coleccionElecciones[0].coleccionPartidos[10].escaños = 1;
            coleccionElecciones[0].coleccionPartidos[10].color = "lightYellow";

            // ELECCIONES GENERALES 10/11/2019
            for (int i = 0; i < 14; i++)
            {
                coleccionElecciones[1].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[1].nombre = "Elecciones Generales 10-11-2019";
            coleccionElecciones[1].fecha = new DateTime(2019, 11, 10);
            coleccionElecciones[1].numEscaños = 350;
            coleccionElecciones[1].mayoriaAbsoluta = 176;

            coleccionElecciones[1].coleccionPartidos[0].nombre = "PSOE";
            coleccionElecciones[1].coleccionPartidos[0].escaños = 120;
            coleccionElecciones[1].coleccionPartidos[0].color = "Red";

            coleccionElecciones[1].coleccionPartidos[1].nombre = "PP";
            coleccionElecciones[1].coleccionPartidos[1].escaños = 89;
            coleccionElecciones[1].coleccionPartidos[1].color = "Blue";

            coleccionElecciones[1].coleccionPartidos[2].nombre = "VOX";
            coleccionElecciones[1].coleccionPartidos[2].escaños = 52;
            coleccionElecciones[1].coleccionPartidos[2].color = "Green";

            coleccionElecciones[1].coleccionPartidos[3].nombre = "PODEMOS";
            coleccionElecciones[1].coleccionPartidos[3].escaños = 35;
            coleccionElecciones[1].coleccionPartidos[3].color = "Purple";

            coleccionElecciones[1].coleccionPartidos[4].nombre = "ERC";
            coleccionElecciones[1].coleccionPartidos[4].escaños = 13;
            coleccionElecciones[1].coleccionPartidos[4].color = "Yellow";

            coleccionElecciones[1].coleccionPartidos[5].nombre = "CS";
            coleccionElecciones[1].coleccionPartidos[5].escaños = 10;
            coleccionElecciones[1].coleccionPartidos[5].color = "Orange";

            coleccionElecciones[1].coleccionPartidos[6].nombre = "JUNTS";
            coleccionElecciones[1].coleccionPartidos[6].escaños = 8;
            coleccionElecciones[1].coleccionPartidos[6].color = "lightGreen";

            coleccionElecciones[1].coleccionPartidos[7].nombre = "EAJ_PNV";
            coleccionElecciones[1].coleccionPartidos[7].escaños = 6;
            coleccionElecciones[1].coleccionPartidos[7].color = "darkGreen";

            coleccionElecciones[1].coleccionPartidos[8].nombre = "EH_BILDU";
            coleccionElecciones[1].coleccionPartidos[8].escaños = 5;
            coleccionElecciones[1].coleccionPartidos[8].color = "lightBlue";

            coleccionElecciones[1].coleccionPartidos[9].nombre = "MASPAIS";
            coleccionElecciones[1].coleccionPartidos[9].escaños = 3;
            coleccionElecciones[1].coleccionPartidos[9].color = "Magenta";

            coleccionElecciones[1].coleccionPartidos[10].nombre = "CUP_PR";
            coleccionElecciones[1].coleccionPartidos[10].escaños = 2;
            coleccionElecciones[1].coleccionPartidos[10].color = "lightPurple";

            coleccionElecciones[1].coleccionPartidos[11].nombre = "CCA";
            coleccionElecciones[1].coleccionPartidos[11].escaños = 2;
            coleccionElecciones[1].coleccionPartidos[11].color = "lightRed";

            coleccionElecciones[1].coleccionPartidos[12].nombre = "BNG";
            coleccionElecciones[1].coleccionPartidos[12].escaños = 1;
            coleccionElecciones[1].coleccionPartidos[12].color = "lightOrange";

            coleccionElecciones[1].coleccionPartidos[13].nombre = "OTROS";
            coleccionElecciones[1].coleccionPartidos[13].escaños = 4;
            coleccionElecciones[1].coleccionPartidos[13].color = "Gray";

            // ELECCIONES AUTONÓMICAS CyL 14/2/2022
            for (int i = 0; i < 8; i++)
            {
                coleccionElecciones[2].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[2].nombre = "Elecciones Autonómicas CyL 14-2-2022";
            coleccionElecciones[2].fecha = new DateTime(2022, 2, 14);
            coleccionElecciones[2].numEscaños = 81;
            coleccionElecciones[2].mayoriaAbsoluta = 41;

            coleccionElecciones[2].coleccionPartidos[0].nombre = "PP";
            coleccionElecciones[2].coleccionPartidos[0].escaños = 31;
            coleccionElecciones[2].coleccionPartidos[0].color = "Blue";

            coleccionElecciones[2].coleccionPartidos[1].nombre = "PSOE";
            coleccionElecciones[2].coleccionPartidos[1].escaños = 28;
            coleccionElecciones[2].coleccionPartidos[1].color = "Red";

            coleccionElecciones[2].coleccionPartidos[2].nombre = "VOX";
            coleccionElecciones[2].coleccionPartidos[2].escaños = 13;
            coleccionElecciones[2].coleccionPartidos[2].color = "Green";

            coleccionElecciones[2].coleccionPartidos[3].nombre = "UPL";
            coleccionElecciones[2].coleccionPartidos[3].escaños = 3;
            coleccionElecciones[2].coleccionPartidos[3].color = "lightPurple";

            coleccionElecciones[2].coleccionPartidos[4].nombre = "SY";
            coleccionElecciones[2].coleccionPartidos[4].escaños = 3;
            coleccionElecciones[2].coleccionPartidos[4].color = "lightBlue";

            coleccionElecciones[2].coleccionPartidos[5].nombre = "PODEMOS";
            coleccionElecciones[2].coleccionPartidos[5].escaños = 1;
            coleccionElecciones[2].coleccionPartidos[5].color = "Purple";

            coleccionElecciones[2].coleccionPartidos[6].nombre = "CS";
            coleccionElecciones[2].coleccionPartidos[6].escaños = 1;
            coleccionElecciones[2].coleccionPartidos[6].color = "Orange";

            coleccionElecciones[2].coleccionPartidos[7].nombre = "XAV";
            coleccionElecciones[2].coleccionPartidos[7].escaños = 1;
            coleccionElecciones[2].coleccionPartidos[7].color = "lightGreen";

            // ELECCIONES AUTONÓMICAS CyL 26/5/2019

            for (int i = 0; i < 7; i++)
            {
                coleccionElecciones[3].coleccionPartidos.Add(new Partido());
            }

            coleccionElecciones[3].nombre = "Elecciones Autonómicas CyL 26-5-2019";
            coleccionElecciones[3].fecha = new DateTime(2019, 5, 26);
            coleccionElecciones[3].numEscaños = 81;
            coleccionElecciones[3].mayoriaAbsoluta = 41;

            coleccionElecciones[3].coleccionPartidos[0].nombre = "PSOE";
            coleccionElecciones[3].coleccionPartidos[0].escaños = 35;
            coleccionElecciones[3].coleccionPartidos[0].color = "Red";

            coleccionElecciones[3].coleccionPartidos[1].nombre = "PP";
            coleccionElecciones[3].coleccionPartidos[1].escaños = 29;
            coleccionElecciones[3].coleccionPartidos[1].color = "Blue";

            coleccionElecciones[3].coleccionPartidos[2].nombre = "CS";
            coleccionElecciones[3].coleccionPartidos[2].escaños = 12;
            coleccionElecciones[3].coleccionPartidos[2].color = "Orange";

            coleccionElecciones[3].coleccionPartidos[3].nombre = "PODEMOS";
            coleccionElecciones[3].coleccionPartidos[3].escaños = 2;
            coleccionElecciones[3].coleccionPartidos[3].color = "Purple";

            coleccionElecciones[3].coleccionPartidos[4].nombre = "VOX";
            coleccionElecciones[3].coleccionPartidos[4].escaños = 1;
            coleccionElecciones[3].coleccionPartidos[4].color = "Green";

            coleccionElecciones[3].coleccionPartidos[5].nombre = "UPL";
            coleccionElecciones[3].coleccionPartidos[5].escaños = 1;
            coleccionElecciones[3].coleccionPartidos[5].color = "lightPurple";

            coleccionElecciones[3].coleccionPartidos[6].nombre = "XAV";
            coleccionElecciones[3].coleccionPartidos[6].escaños = 1;
            coleccionElecciones[3].coleccionPartidos[6].color = "lightGreen";

        }
    }
}
