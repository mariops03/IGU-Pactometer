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
    /// Lógica de interacción para VentanaAgregar.xaml
    /// </summary>
    /// 
    
    public partial class VentanaAgregar : Window
    {
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;
        public VentanaAgregar(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
        }
    }
}
