using System.Collections.ObjectModel;
using System.ComponentModel;
using Pactometro.Model; // Asegúrate de que este espacio de nombres contenga tus clases de modelo
using System.Linq;
using System.Collections.Generic;
using System.Windows;

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
        public void OrdenarPorFecha()
        {
            List<ProcesoElectoral> ordenada;

            if (Elecciones.SequenceEqual(Elecciones.OrderBy(e => e.fecha)))
            {
                ordenada = Elecciones.OrderByDescending(e => e.fecha).ToList();
            }
            else
            {
                ordenada = Elecciones.OrderBy(e => e.fecha).ToList();
            }

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }

        // Método para ordenar por nombre
        public void OrdenarPorNombre()
        {
            List<ProcesoElectoral> ordenada;

            if (Elecciones.SequenceEqual(Elecciones.OrderBy(e => e.nombre)))
            {
                ordenada = Elecciones.OrderByDescending(e => e.nombre).ToList();
            }
            else
            {
                ordenada = Elecciones.OrderBy(e => e.nombre).ToList();
            }

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }

        // Método para ordenar por número de escaños
        public void OrdenarPorEscaños()
        {
            List<ProcesoElectoral> ordenada;

            if (Elecciones.SequenceEqual(Elecciones.OrderBy(e => e.numEscaños)))
            {
                ordenada = Elecciones.OrderByDescending(e => e.numEscaños).ToList();
            }
            else
            {
                ordenada = Elecciones.OrderBy(e => e.numEscaños).ToList();
            }

            Elecciones.Clear();
            foreach (var item in ordenada)
            {
                Elecciones.Add(item);
            }
        }

        // Método para ordenar por nombre del partido
        public void OrdenarPorNombrePartido()
        {
            if (EleccionSeleccionada == null || EleccionSeleccionada.coleccionPartidos == null)
                return;

            var coleccion = EleccionSeleccionada.coleccionPartidos;
            List<Partido> ordenada;

            if (coleccion.SequenceEqual(coleccion.OrderBy(p => p.Nombre)))
            {
                ordenada = coleccion.OrderByDescending(p => p.Nombre).ToList();
            }
            else
            {
                ordenada = coleccion.OrderBy(p => p.Nombre).ToList();
            }

            coleccion.Clear();
            foreach (var partido in ordenada)
            {
                coleccion.Add(partido);
            }
        }

        // Método para ordenar por escaños del partido
        public void OrdenarPorEscañosPartido()
        {
            if (EleccionSeleccionada == null || EleccionSeleccionada.coleccionPartidos == null)
                return;

            var coleccion = EleccionSeleccionada.coleccionPartidos;
            List<Partido> ordenada;

            if (coleccion.SequenceEqual(coleccion.OrderBy(p => p.Escaños)))
            {
                ordenada = coleccion.OrderByDescending(p => p.Escaños).ToList();
            }
            else
            {
                ordenada = coleccion.OrderBy(p => p.Escaños).ToList();
            }

            coleccion.Clear();
            foreach (var partido in ordenada)
            {
                coleccion.Add(partido);
            }
        }
    }
}
