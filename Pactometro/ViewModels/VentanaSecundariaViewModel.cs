using System.Collections.ObjectModel;
using System.ComponentModel;
using Pactometro;

namespace Pactometro.ViewModels
{
    public class VentanaSecundariaViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        public VentanaSecundariaViewModel(ObservableCollection<ProcesoElectoral> elecciones)
        {
            _elecciones = elecciones;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
