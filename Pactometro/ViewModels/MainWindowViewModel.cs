// MainWindowViewModel.cs
using Pactometro.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Pactometro.ViewModels;

namespace Pactometro.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<ProcesoElectoral> _elecciones;

        public ObservableCollection<ProcesoElectoral> Elecciones
        {
            get => _elecciones;
            set
            {
                _elecciones = value;
                OnPropertyChanged(nameof(Elecciones));
            }
        }

        private ProcesoElectoral _eleccionSeleccionada;
        public ProcesoElectoral EleccionSeleccionada
        {
            get => _eleccionSeleccionada;
            set
            {
                _eleccionSeleccionada = value;
                OnPropertyChanged(nameof(EleccionSeleccionada));
                // Puedes añadir más lógica aquí si es necesario
            }
        }

        public MainWindowViewModel(IDatosElectorales datosElectorales)
        {
            Elecciones = datosElectorales.GenerarDatosElectorales();
            // Ordenar por fecha
            OrdenarPorFecha(Elecciones);
        }
        
    }
}
