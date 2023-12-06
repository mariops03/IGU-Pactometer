// MainWindowViewModel.cs
using Pactometro.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pactometro.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
