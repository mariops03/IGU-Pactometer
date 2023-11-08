using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pactometro
{
    internal class ProcesoElectoral
    {
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public int numEscaños { get; set; }
        public ObservableCollection<Partido> coleccionPartidos = new ObservableCollection<Partido>();
        public int mayoriaAbsoluta { get; set; }

        public ProcesoElectoral()
        {

        }
        public ProcesoElectoral(string nombre, DateTime fecha, int numEscaños, int mayoriaAbsoluta)
        {
            this.nombre = nombre;
            this.fecha = fecha;
            this.numEscaños = numEscaños;
            this.mayoriaAbsoluta = mayoriaAbsoluta;
        }

        public override string ToString()
        {
            return nombre + " " + fecha + "" + numEscaños + "" + mayoriaAbsoluta;
        }
    }
}
