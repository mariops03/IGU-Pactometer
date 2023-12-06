using System.Collections.ObjectModel;
using Pactometro; // Asegúrate de que este espacio de nombres contenga tus clases de modelo

namespace Pactometro.ViewModels
{
    public class VentanaSecundariaViewModel : BaseViewModel
    {
        public ObservableCollection<Partido> Partidos => EleccionSeleccionada?.coleccionPartidos;

        public VentanaSecundariaViewModel(ObservableCollection<ProcesoElectoral> elecciones)
        {
            Elecciones = elecciones; // Usar la propiedad heredada
        }
    }
}
