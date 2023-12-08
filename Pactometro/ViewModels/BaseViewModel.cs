using System.Collections.ObjectModel;
using System.ComponentModel;
using Pactometro.Model; // Asegúrate de que este espacio de nombres contenga tus clases de modelo
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System;

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

        public string ObtenerParteAlfabetica(string nombre)
        {
            // Verificar si el nombre es null
            if (nombre == null)
            {
                throw new ArgumentNullException(nameof(nombre));
            }

            // Buscar la posición del último espacio en blanco
            int indiceUltimoEspacio = nombre.LastIndexOf(' ');

            // Verificar si se encontró un espacio en blanco
            if (indiceUltimoEspacio >= 0)
            {
                // Obtener la parte alfabética antes del último espacio en blanco
                return nombre.Substring(0, indiceUltimoEspacio);
            }

            // En caso de que no haya espacio en blanco, devolver el nombre original
            return nombre;
        }
    }
}
