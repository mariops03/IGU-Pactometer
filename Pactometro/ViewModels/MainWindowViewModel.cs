using Pactometro.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace Pactometro.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Colección de elecciones seleccionadas por CheckBox
        private ObservableCollection<ProcesoElectoral> _coleccionEleccionesCheckBox = new ObservableCollection<ProcesoElectoral>();
        public ObservableCollection<ProcesoElectoral> ColeccionEleccionesCheckBox
        {
            get => _coleccionEleccionesCheckBox;
            set
            {
                if (_coleccionEleccionesCheckBox != value)
                {
                    _coleccionEleccionesCheckBox = value;
                    OnPropertyChanged(nameof(ColeccionEleccionesCheckBox));
                }
            }
        }

        // Lista de listas de partidos seleccionados
        private List<List<Partido>> _partidosSeleccionados = new List<List<Partido>>();
        public List<List<Partido>> PartidosSeleccionados
        {
            get => _partidosSeleccionados;
            set
            {
                if (_partidosSeleccionados != value)
                {
                    _partidosSeleccionados = value;
                    OnPropertyChanged(nameof(PartidosSeleccionados));
                }
            }
        }

        public MainWindowViewModel(IDatosElectorales datosElectorales)
        {
            Elecciones = datosElectorales.GenerarDatosElectorales(); // Usar la propiedad heredada
            OrdenarPorFecha(Elecciones); // Usar el método heredado para ordenar
        }

        public void ActualizarElecciones()
        {
            _coleccionEleccionesCheckBox = new ObservableCollection<ProcesoElectoral>(_coleccionEleccionesCheckBox.OrderByDescending(p => p.fecha).ToList());

            double opacidad = 1.0;
            double i = 1.0;

            if (_coleccionEleccionesCheckBox.Any())
            {
                _partidosSeleccionados.Clear();
                foreach (ProcesoElectoral procesoCheckBox in _coleccionEleccionesCheckBox)
                {
                    foreach (Partido partido in procesoCheckBox.coleccionPartidos)
                    {
                        partido.Color = Color.FromArgb((byte)(255 * opacidad), partido.Color.R, partido.Color.G, partido.Color.B);

                        List<Partido> partidosConMismoNombre = _partidosSeleccionados.FirstOrDefault(p => p.Any() && p.First().Nombre == partido.Nombre);

                        if (partidosConMismoNombre != null)
                        {
                            partidosConMismoNombre.Add(partido);
                        }
                        else
                        {
                            _partidosSeleccionados.Add(new List<Partido> { partido });
                        }
                    }
                    i++;
                    opacidad = 1.0 / i;
                }
                _partidosSeleccionados = _partidosSeleccionados.OrderByDescending(lista => lista.Any() ? lista.Max(partido => partido.Escaños) : 0).ToList();
                OnPropertyChanged(nameof(PartidosSeleccionados)); // Notificar a la vista sobre el cambio en la lista
            }
        }

        public void LimpiarPartidos(ProcesoElectoral proceso)
        {
            foreach (List<Partido> listaDePartidos in PartidosSeleccionados.ToList())
            {
                Partido partidoAEliminar = listaDePartidos.FirstOrDefault(partido =>
                    proceso.coleccionPartidos.Any(p =>
                        p.Nombre == partido.Nombre &&
                        p.Escaños == partido.Escaños &&
                        p.Color == partido.Color
                    )
                );

                if (partidoAEliminar != null)
                {
                    listaDePartidos.Remove(partidoAEliminar);
                }
            }
            OnPropertyChanged(nameof(PartidosSeleccionados));
        }

        // Método para obtener procesos electorales equivalentes por nombre
        public Collection<ProcesoElectoral> ObtenerProcesosEquivalentesPorNombre(ProcesoElectoral procesoElectoralBase)
        {
            Collection<ProcesoElectoral> procesosEquivalentes = new Collection<ProcesoElectoral>();

            foreach (ProcesoElectoral proceso in Elecciones)
            {
                if (proceso != null && ObtenerParteAlfabética(proceso.nombre) == ObtenerParteAlfabética(procesoElectoralBase.nombre))
                {
                    procesosEquivalentes.Add(proceso);
                }
            }
            return procesosEquivalentes;
        }



        // Método auxiliar para obtener la parte alfabética del nombre
        private string ObtenerParteAlfabética(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentNullException(nameof(nombre));
            }

            int indiceUltimoEspacio = nombre.LastIndexOf(' ');
            return indiceUltimoEspacio >= 0 ? nombre.Substring(0, indiceUltimoEspacio) : nombre;
        }
    }
}
