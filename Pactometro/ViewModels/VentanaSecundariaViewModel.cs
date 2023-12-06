using System.Collections.ObjectModel;
using Pactometro; // Asegúrate de que este espacio de nombres contenga tus clases de modelo

namespace Pactometro.ViewModels
{
    public class VentanaSecundariaViewModel : BaseViewModel
    {
        private ObservableCollection<ProcesoElectoral> _elecciones;
        private ProcesoElectoral _eleccionSeleccionada;

        public ObservableCollection<ProcesoElectoral> Elecciones
        {
            get => _elecciones;
            set
            {
                _elecciones = value;
                OnPropertyChanged(nameof(Elecciones));
            }
        }

        public ProcesoElectoral EleccionSeleccionada
        {
            get => _eleccionSeleccionada;
            set
            {
                if (_eleccionSeleccionada != value)
                {
                    _eleccionSeleccionada = value;
                    OnPropertyChanged(nameof(EleccionSeleccionada));
                    // Notificar que la lista de Partidos debe actualizarse
                    OnPropertyChanged(nameof(Partidos));
                }
            }
        }

        public ObservableCollection<Partido> Partidos => EleccionSeleccionada?.coleccionPartidos;

        public VentanaSecundariaViewModel(ObservableCollection<ProcesoElectoral> elecciones)
        {
            _elecciones = elecciones;
        }
    }
}
