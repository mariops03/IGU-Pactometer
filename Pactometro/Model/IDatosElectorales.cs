using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pactometro.Model
{
    public interface IDatosElectorales
    {
        ObservableCollection<ProcesoElectoral> GenerarDatosElectorales();
    }
}
