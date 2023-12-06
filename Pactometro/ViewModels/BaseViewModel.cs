using System.Collections.ObjectModel;
using System.ComponentModel;
using Pactometro.Model; // Asegúrate de que este espacio de nombres contenga tus clases de modelo
using System.Linq;

namespace Pactometro.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ProcesoElectoral> _elecciones;
        private ProcesoElectoral _eleccionSeleccionada;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                }
            }
        }

        // Método para ordenar por fecha
        public void OrdenarPorFecha(ObservableCollection<ProcesoElectoral> coleccion)
        {
            var ordenada = coleccion.OrderByDescending(e => e.fecha).ToList();
            coleccion.Clear();
            foreach (var item in ordenada)
            {
                coleccion.Add(item);
            }
        }


        // Método para ordenar por nombre
        public void OrdenarPorNombre(ObservableCollection<ProcesoElectoral> coleccion)
        {
            var ordenada = coleccion.OrderBy(e => e.nombre).ToList();
            coleccion.Clear();
            foreach (var item in ordenada)
            {
                coleccion.Add(item);
            }
        }
    }
}
