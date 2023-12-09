using System.Collections.ObjectModel;

namespace Pactometro.Model
{
    public interface IDatosElectorales
    {
        ObservableCollection<ProcesoElectoral> GenerarDatosElectorales();
    }
}
