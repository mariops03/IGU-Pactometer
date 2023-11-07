using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pactometro
{
    internal class Partido
    {
        string nombre { get; set; }
        int escaños { get; set; }
        String color { get; set; }
        public Partido()
        {

        }

        public Partido(string nombre, int escaños, String color)
            {
                this.nombre = nombre;
                this.escaños = escaños;
                this.color = color;
            }

        public override string ToString()
        {
            return nombre + " " + escaños;
        }
    }

    
}
