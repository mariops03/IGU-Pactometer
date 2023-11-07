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
        }

        private void añadirPatidos()
        {
            Collection<ProcesoElectoral> coleccionProcesos = new Collection<ProcesoElectoral>();
            for (int i = 0; i < 5; i++)
            {
                coleccionProcesos.Add(new ProcesoElectoral());
            }

            for(int i = 0; i < 7; i++)
            {
                coleccionProcesos[0].coleccionPartidos.Add(new Partido());
            }

            // ELECCIONES GENERALES 23/07/2023
            coleccionProcesos[0].nombre = "Elecciones Generales 23-07-2023";
            coleccionProcesos[0].fecha = new DateTime(2023, 07, 23);
            coleccionProcesos[0].numEscaños = 350;
            coleccionProcesos[0].mayoriaAbsoluta = 176;

            coleccionProcesos[0].coleccionPartidos[0].nombre = "PP";
            coleccionProcesos[0].coleccionPartidos[0].escaños = 136;
            coleccionProcesos[0].coleccionPartidos[0].color = "Blue";

            coleccionProcesos[0].coleccionPartidos[1].nombre = "PSOE";
            coleccionProcesos[0].coleccionPartidos[1].escaños = 122;
            coleccionProcesos[0].coleccionPartidos[1].color = "Red";

            coleccionProcesos[0].coleccionPartidos[2].nombre = "VOX";
            coleccionProcesos[0].coleccionPartidos[2].escaños = 33;
            coleccionProcesos[0].coleccionPartidos[2].color = "Green";

            coleccionProcesos[0].coleccionPartidos[3].nombre = "Sumar";
            coleccionProcesos[0].coleccionPartidos[3].escaños = 31;
            coleccionProcesos[0].coleccionPartidos[3].color = "Purple";

            coleccionProcesos[0].coleccionPartidos[4].nombre = "ERC";
            coleccionProcesos[0].coleccionPartidos[4].escaños = 7;
            coleccionProcesos[0].coleccionPartidos[4].color = "Yellow";

            coleccionProcesos[0].coleccionPartidos[5].nombre = "JUNTS";
            coleccionProcesos[0].coleccionPartidos[5].escaños = 7;
            coleccionProcesos[0].coleccionPartidos[5].color = "lightGreen";

            coleccionProcesos[0].coleccionPartidos[6].nombre = "EH Bildu";
            coleccionProcesos[0].coleccionPartidos[6].escaños = 6;
            coleccionProcesos[0].coleccionPartidos[6].color = "lightBlue";

            coleccionProcesos[0].coleccionPartidos[7].nombre = "PNV";
            coleccionProcesos[0].coleccionPartidos[7].escaños = 5;
            coleccionProcesos[0].coleccionPartidos[7].color = "darkGreen";

            coleccionProcesos[0].coleccionPartidos[8].nombre = "BNG";
            coleccionProcesos[0].coleccionPartidos[8].escaños = 1;
            coleccionProcesos[0].coleccionPartidos[8].color = "lightPurple";

            coleccionProcesos[0].coleccionPartidos[9].nombre = "CCA";
            coleccionProcesos[0].coleccionPartidos[9].escaños = 1;
            coleccionProcesos[0].coleccionPartidos[9].color = "lightRed";

            coleccionProcesos[0].coleccionPartidos[10].nombre = "UPN";
            coleccionProcesos[0].coleccionPartidos[10].escaños = 1;
            coleccionProcesos[0].coleccionPartidos[10].color = "lightYellow";

            // ELECCIONES GENERALES 10/11/2019
            coleccionProcesos[1].nombre = "Elecciones Generales 10-11-2019";
            coleccionProcesos[1].fecha = new DateTime(2019, 11, 10);
            coleccionProcesos[1].numEscaños = 350;
            coleccionProcesos[1].mayoriaAbsoluta = 176;

            coleccionProcesos[1].coleccionPartidos[0].nombre = "PSOE";
            coleccionProcesos[1].coleccionPartidos[0].escaños = 120;
            coleccionProcesos[1].coleccionPartidos[0].color = "Red";

            coleccionProcesos[1].coleccionPartidos[1].nombre = "PP";
            coleccionProcesos[1].coleccionPartidos[1].escaños = 89;
            coleccionProcesos[1].coleccionPartidos[1].color = "Blue";

            coleccionProcesos[1].coleccionPartidos[2].nombre = "VOX";
            coleccionProcesos[1].coleccionPartidos[2].escaños = 52;
            coleccionProcesos[1].coleccionPartidos[2].color = "Green";

            coleccionProcesos[1].coleccionPartidos[3].nombre = "PODEMOS";
            coleccionProcesos[1].coleccionPartidos[3].escaños = 35;
            coleccionProcesos[1].coleccionPartidos[3].color = "Purple";

            coleccionProcesos[1].coleccionPartidos[4].nombre = "ERC";
            coleccionProcesos[1].coleccionPartidos[4].escaños = 13;
            coleccionProcesos[1].coleccionPartidos[4].color = "Yellow";

            coleccionProcesos[1].coleccionPartidos[5].nombre = "CS";
            coleccionProcesos[1].coleccionPartidos[5].escaños = 10;
            coleccionProcesos[1].coleccionPartidos[5].color = "Orange";

            coleccionProcesos[1].coleccionPartidos[6].nombre = "JUNTS";
            coleccionProcesos[1].coleccionPartidos[6].escaños = 8;
            coleccionProcesos[1].coleccionPartidos[6].color = "lightGreen";

            coleccionProcesos[1].coleccionPartidos[7].nombre = "EAJ_PNV";
            coleccionProcesos[1].coleccionPartidos[7].escaños = 6;
            coleccionProcesos[1].coleccionPartidos[7].color = "darkGreen";

            coleccionProcesos[1].coleccionPartidos[8].nombre = "EH_BILDU";
            coleccionProcesos[1].coleccionPartidos[8].escaños = 5;
            coleccionProcesos[1].coleccionPartidos[8].color = "lightBlue";

            coleccionProcesos[1].coleccionPartidos[9].nombre = "MASPAIS";
            coleccionProcesos[1].coleccionPartidos[9].escaños = 3;
            coleccionProcesos[1].coleccionPartidos[9].color = "Magenta";

            coleccionProcesos[1].coleccionPartidos[10].nombre = "CUP_PR";
            coleccionProcesos[1].coleccionPartidos[10].escaños = 2;
            coleccionProcesos[1].coleccionPartidos[10].color = "lightPurple";

            coleccionProcesos[1].coleccionPartidos[11].nombre = "CCA";
            coleccionProcesos[1].coleccionPartidos[11].escaños = 2;
            coleccionProcesos[1].coleccionPartidos[11].color = "lightRed";

            coleccionProcesos[1].coleccionPartidos[12].nombre = "BNG";
            coleccionProcesos[1].coleccionPartidos[12].escaños = 1;
            coleccionProcesos[1].coleccionPartidos[12].color = "lightOrange";

            coleccionProcesos[1].coleccionPartidos[13].nombre = "OTROS";
            coleccionProcesos[1].coleccionPartidos[13].escaños = 4;
            coleccionProcesos[1].coleccionPartidos[13].color = "Gray";

            // ELECCIONES AUTONÓMICAS CyL 14/2/2022
            coleccionProcesos[2].nombre = "Elecciones Autonómicas CyL 14-2-2022";
            coleccionProcesos[2].fecha = new DateTime(2022, 2, 14);
            coleccionProcesos[2].numEscaños = 81;
            coleccionProcesos[2].mayoriaAbsoluta = 41;

            coleccionProcesos[2].coleccionPartidos[0].nombre = "PP";
            coleccionProcesos[2].coleccionPartidos[0].escaños = 31;
            coleccionProcesos[2].coleccionPartidos[0].color = "Blue";

            coleccionProcesos[2].coleccionPartidos[1].nombre = "PSOE";
            coleccionProcesos[2].coleccionPartidos[1].escaños = 28;
            coleccionProcesos[2].coleccionPartidos[1].color = "Red";

            coleccionProcesos[2].coleccionPartidos[2].nombre = "VOX";
            coleccionProcesos[2].coleccionPartidos[2].escaños = 13;
            coleccionProcesos[2].coleccionPartidos[2].color = "Green";

            coleccionProcesos[2].coleccionPartidos[3].nombre = "UPL";
            coleccionProcesos[2].coleccionPartidos[3].escaños = 3;
            coleccionProcesos[2].coleccionPartidos[3].color = "lightPurple";

            coleccionProcesos[2].coleccionPartidos[4].nombre = "SY";
            coleccionProcesos[2].coleccionPartidos[4].escaños = 3;
            coleccionProcesos[2].coleccionPartidos[4].color = "lightBlue";

            coleccionProcesos[2].coleccionPartidos[5].nombre = "PODEMOS";
            coleccionProcesos[2].coleccionPartidos[5].escaños = 1;
            coleccionProcesos[2].coleccionPartidos[5].color = "Purple";

            coleccionProcesos[2].coleccionPartidos[6].nombre = "CS";
            coleccionProcesos[2].coleccionPartidos[6].escaños = 1;
            coleccionProcesos[2].coleccionPartidos[6].color = "Orange";

            coleccionProcesos[2].coleccionPartidos[7].nombre = "XAV";
            coleccionProcesos[2].coleccionPartidos[7].escaños = 1;
            coleccionProcesos[2].coleccionPartidos[7].color = "lightGreen";

            // ELECCIONES AUTONÓMICAS CyL 26/5/2019
            coleccionProcesos[3].nombre = "Elecciones Autonómicas CyL 26-5-2019";
            coleccionProcesos[3].fecha = new DateTime(2019, 5, 26);
            coleccionProcesos[3].numEscaños = 81;
            coleccionProcesos[3].mayoriaAbsoluta = 41;

            coleccionProcesos[3].coleccionPartidos[0].nombre = "PSOE";
            coleccionProcesos[3].coleccionPartidos[0].escaños = 35;
            coleccionProcesos[3].coleccionPartidos[0].color = "Red";

            coleccionProcesos[3].coleccionPartidos[1].nombre = "PP";
            coleccionProcesos[3].coleccionPartidos[1].escaños = 29;
            coleccionProcesos[3].coleccionPartidos[1].color = "Blue";

            coleccionProcesos[3].coleccionPartidos[2].nombre = "CS";
            coleccionProcesos[3].coleccionPartidos[2].escaños = 12;
            coleccionProcesos[3].coleccionPartidos[2].color = "Orange";

            coleccionProcesos[3].coleccionPartidos[3].nombre = "PODEMOS";
            coleccionProcesos[3].coleccionPartidos[3].escaños = 2;
            coleccionProcesos[3].coleccionPartidos[3].color = "Purple";

            coleccionProcesos[3].coleccionPartidos[4].nombre = "VOX";
            coleccionProcesos[3].coleccionPartidos[4].escaños = 1;
            coleccionProcesos[3].coleccionPartidos[4].color = "Green";

            coleccionProcesos[3].coleccionPartidos[5].nombre = "UPL";
            coleccionProcesos[3].coleccionPartidos[5].escaños = 1;
            coleccionProcesos[3].coleccionPartidos[5].color = "lightPurple";

            coleccionProcesos[3].coleccionPartidos[6].nombre = "XAV";
            coleccionProcesos[3].coleccionPartidos[6].escaños = 1;
            coleccionProcesos[3].coleccionPartidos[6].color = "lightGreen";

        }
    }
}
