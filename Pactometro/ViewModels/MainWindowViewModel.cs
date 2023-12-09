using Pactometro.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pactometro.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        // Colección de elecciones seleccionadas por CheckBox
        private ObservableCollection<ProcesoElectoral> _coleccionEleccionesCheckBox = new ObservableCollection<ProcesoElectoral>();

        private Dictionary<Rectangle, double> originalPositions = new Dictionary<Rectangle, double>();
        private List<Partido> partidosEnPrimeraBarra = new List<Partido>();
        private List<Partido> partidosEnSegundaBarra = new List<Partido>();
        private List<Rectangle> rectangulosClicados = new List<Rectangle>();
        private List<Rectangle> rectangulosNoClicados = new List<Rectangle>();


        public MainWindowViewModel(IDatosElectorales datosElectorales)
        {
            Elecciones = datosElectorales.GenerarDatosElectorales(); // Usar la propiedad heredada
        }
        public MainWindowViewModel()
        {
        }

        private VentanaSecundaria _ventanaSecundaria;

        public VentanaSecundaria VentanaSecundaria
        {
            get => _ventanaSecundaria;
            set
            {
                if (_ventanaSecundaria != value)
                {
                    _ventanaSecundaria = value;
                    OnPropertyChanged(nameof(VentanaSecundaria));
                }
            }
        }


        public void AbrirVentanaSecundaria()
        {
            if (VentanaSecundaria == null)
            {
                VentanaSecundaria = new VentanaSecundaria(Elecciones);

                // Así es como podrías acceder directamente a la instancia de MainWindow en el ViewModel
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                double nuevaPosX;
                double nuevaPosY;
                double distanciaEntreVentanas = 10; // Puedes ajustar esto a tu preferencia
                if (mainWindow != null)
                {
                    nuevaPosX = mainWindow.Left + mainWindow.Width + distanciaEntreVentanas;
                    nuevaPosY = mainWindow.Top;

                    VentanaSecundaria.Left = nuevaPosX;
                    VentanaSecundaria.Top = nuevaPosY;

                    VentanaSecundaria.ProcesoEleccionSeleccionado += VentanaSecundaria_ProcesoEleccionSeleccionado;
                    VentanaSecundaria.Closed += VentanaSecundaria_Closed;
                    VentanaSecundaria.Show();
                }                
            }
            else
            {
                VentanaSecundaria.Activate();
            }
        }

        private void VentanaSecundaria_Closed(object sender, EventArgs e)
        {
            // Manejar el evento Closed de la ventana secundaria si es necesario
            VentanaSecundaria = null; // Liberar la referencia a la ventana secundaria
        }

        public void MainWindow_Closed(object sender, EventArgs e)
        {
            // Preguntar al usuario si desea salir
            Application.Current.Shutdown(); // Cierra la aplicación cuando la ventana principal se cierra
        }

        private void VentanaSecundaria_ProcesoEleccionSeleccionado(object sender, ProcesoElectoral procesoElectoral)
        {
            // Manejar el evento ProcesoEleccionSeleccionado de la ventana secundaria si es necesario
            EleccionSeleccionada = procesoElectoral;
        }

        public void Exportar()
        {
            VentanaExportar ventanaExportar = new VentanaExportar();
            // Puedes establecer el ViewModel como DataContext de la ventana si es necesario
            ventanaExportar.Owner = Application.Current.MainWindow;
            ventanaExportar.ShowDialog();
        }


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

        // Lista de partidos en la primera barra
        public List<Partido> PartidosEnPrimeraBarra
        {
            get => partidosEnPrimeraBarra;
            set
            {
                if (partidosEnPrimeraBarra != value)
                {
                    partidosEnPrimeraBarra = value;
                    OnPropertyChanged(nameof(PartidosEnPrimeraBarra));
                }
            }
        }

        // Lista de partidos en la segunda barra

        public List<Partido> PartidosEnSegundaBarra
        {
            get => partidosEnSegundaBarra;
            set
            {
                if (partidosEnSegundaBarra != value)
                {
                    partidosEnSegundaBarra = value;
                    OnPropertyChanged(nameof(PartidosEnSegundaBarra));
                }
            }
        }

        // Lista de rectángulos clicados

        public List<Rectangle> RectangulosClicados
        {
            get => rectangulosClicados;
            set
            {
                if (rectangulosClicados != value)
                {
                    rectangulosClicados = value;
                    OnPropertyChanged(nameof(rectangulosClicados));
                }
            }
        }

        // Lista de rectángulos no clicados

        public List<Rectangle> RectangulosNoClicados
        {
            get => rectangulosNoClicados;
            set
            {
                if (rectangulosNoClicados != value)
                {
                    rectangulosNoClicados = value;
                    OnPropertyChanged(nameof(RectangulosNoClicados));
                }
            }
        }

        // Lista de posiciones originales de los rectángulos

        public Dictionary<Rectangle, double> OriginalPositions
        {
            get => originalPositions;
            set
            {
                if (originalPositions != value)
                {
                    originalPositions = value;
                    OnPropertyChanged(nameof(OriginalPositions));
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
        public Collection<ProcesoElectoral> ObtenerProcesosEquivalentesPorNombre()
        {
            ProcesoElectoral procesoElectoralBase = EleccionSeleccionada;
            Collection<ProcesoElectoral> procesosEquivalentes = new Collection<ProcesoElectoral>();

            //Restablecer la opacidad de los colores de los partidos
            foreach (ProcesoElectoral proceso in Elecciones)
            {
                foreach (Partido partido in proceso.coleccionPartidos)
                {
                    partido.Color = Color.FromArgb(255, partido.Color.R, partido.Color.G, partido.Color.B);
                }
            }

            foreach (ProcesoElectoral proceso in Elecciones)
            {
                if (proceso != null &&  ObtenerParteAlfabetica(proceso.nombre) == ObtenerParteAlfabetica(procesoElectoralBase.nombre))
                {
                    procesosEquivalentes.Add(proceso);
                }
            }
            return procesosEquivalentes;
        }

        public void reiniciarPactometro()
        {
            PartidosEnPrimeraBarra.Clear();
            PartidosEnSegundaBarra.Clear();
            RectangulosClicados.Clear();
            RectangulosNoClicados.Clear();
        }

        public bool comprobarPacto()
        {
            return PartidosEnPrimeraBarra.Sum(partido => partido.Escaños) < EleccionSeleccionada.mayoriaAbsoluta;
        }

        public void pactoCompletado()
        {
            if (PartidosEnSegundaBarra.Any())
            {
                int numeroDeEscaños = PartidosEnSegundaBarra.Sum(partido => partido.Escaños);
                string mensaje = "     ¡SE HA PRODUCIDO UN PACTO!\n           TOTAL DE ESCAÑOS: " + numeroDeEscaños + "\n\n     Partidos:\n";
                foreach (var partido in PartidosEnSegundaBarra)
                {
                    if (partido != null)
                    {
                        mensaje += "     - " + partido.Nombre + "\n";
                    }
                }
                MessageBox.Show(mensaje);
            }
            else
            {
                MessageBox.Show("No hay partidos en la segunda barra.");
            }
        }
    }
}
