using System;
using System.Collections.ObjectModel;

namespace Pactometro
{
    public class ProcesoElectoral
    {
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public int numEscaños { get; set; }
        public ObservableCollection<Partido> coleccionPartidos { get; set; }
        public int mayoriaAbsoluta { get; set; }

        public ProcesoElectoral()
        {
            coleccionPartidos = new ObservableCollection<Partido>();
        }

        public ProcesoElectoral(string nombre, DateTime fecha, int numEscaños, int mayoriaAbsoluta)
        {
            this.nombre = nombre;
            this.fecha = fecha;
            this.numEscaños = numEscaños;
            this.mayoriaAbsoluta = mayoriaAbsoluta;
            coleccionPartidos = new ObservableCollection<Partido>();
        }

        public override string ToString()
        {
            return $"{nombre} {fecha} {numEscaños} {mayoriaAbsoluta}";
        }
    }
}