using System;
using System.Windows.Media;

namespace Pactometro
{
    public class Partido
    {
        public string Nombre { get; set; }
        public int Escaños { get; set; }
        public Color Color { get; set; }

        public Partido()
        {

        }

        public Partido(string nombre, int escaños, Color color)
        {
            this.Nombre = nombre;
            this.Escaños = escaños;
            this.Color = color;
        }

        public override string ToString()
        {
            return Nombre + " " + Escaños;
        }
    }
}
