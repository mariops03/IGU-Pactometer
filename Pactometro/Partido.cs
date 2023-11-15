using System;

namespace Pactometro
{
    public class Partido
    {
        public string Nombre { get; set; }
        public int Escaños { get; set; }
        public string Color { get; set; }

        public Partido()
        {
            // Inicialización explícita para el constructor por defecto
            this.Nombre = string.Empty;
            this.Escaños = 0;
            this.Color = string.Empty;
        }

        public Partido(string nombre, int escaños, string color)
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
