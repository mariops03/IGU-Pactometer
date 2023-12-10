using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Pactometro.ViewModels;
using System.Windows.Input;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        // Definir el evento para notificar la selección
        public event EventHandler<ProcesoElectoral> ProcesoEleccionSeleccionado;

        private VentanaSecundariaViewModel _viewModelVentanaSecundaria;

        public VentanaSecundaria(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            _viewModelVentanaSecundaria = new VentanaSecundariaViewModel(coleccionElecciones);
            this.DataContext = _viewModelVentanaSecundaria;
            _viewModelVentanaSecundaria.OrdenarPorFechaDescendente();
        }

        private void mainTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtiene el elemento seleccionado, que puede ser null si no hay selección
            ProcesoElectoral procesoElectoralSeleccionado = mainTable.SelectedItem as ProcesoElectoral;

            if (procesoElectoralSeleccionado != null)
            {
                // Actualiza secondaryTable si hay una selección
                secondaryTable.ItemsSource = procesoElectoralSeleccionado.coleccionPartidos;
                if (secondaryTable.Items.Count > 0)
                {
                    secondaryTable.ScrollIntoView(secondaryTable.Items[0]);
                }
                btnModificar.Visibility = Visibility.Visible;
                btnEliminar.Visibility = Visibility.Visible;
            }
            else
            {
                // Limpia secondaryTable si no hay selección
                secondaryTable.ItemsSource = null;
                btnModificar.Visibility = Visibility.Collapsed;
                btnEliminar.Visibility = Visibility.Collapsed;
            }

            //Actualiza el valor de la propiedad ProcesoEleccionSeleccionado en el ViewModel
            _viewModelVentanaSecundaria.EleccionSeleccionada = procesoElectoralSeleccionado;

            // Dispara el evento con el elemento seleccionado o null
            ProcesoEleccionSeleccionado?.Invoke(this, procesoElectoralSeleccionado);
        }

        private void Header_Click(object sender, MouseButtonEventArgs e)
        {
            var headerTextBlock = sender as TextBlock;
            if (headerTextBlock != null)
            {
                var header = headerTextBlock.Text;
                if(header == "ELECCIÓN")
                {
                    _viewModelVentanaSecundaria.OrdenarPorNombre();
                }
                else if(header == "FECHA")
                {
                    _viewModelVentanaSecundaria.OrdenarPorFecha();
                }
                else if(header == "NUMERO DE ESCAÑOS" || header == "MAYORÍA ABSOLUTA")
                {
                    _viewModelVentanaSecundaria.OrdenarPorEscaños();
                }else if(header == "PARTIDO")
                {
                    _viewModelVentanaSecundaria.OrdenarPorNombrePartido();
                }else if(header == "ESCAÑOS")
                {
                    _viewModelVentanaSecundaria.OrdenarPorEscañosPartido();
                }

            }
        }

        private void btnImportarCSV_Click(object sender, RoutedEventArgs e)
        {
            _viewModelVentanaSecundaria.ImportarDatosCSV();
        }

        private void btnExportarCSV_Click(object sender, RoutedEventArgs e)
        {
            _viewModelVentanaSecundaria.ExportarDatosCSV();
        }

        private void btnImportarJSON_Click(object sender, RoutedEventArgs e)
        {
            _viewModelVentanaSecundaria.ImportarDatosJSON();
        }

        private void btnExportarJSON_Click(object sender, RoutedEventArgs e)
        {
            _viewModelVentanaSecundaria.ExportarDatosJson();
        }


        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            _viewModelVentanaSecundaria.AñadirProcesoElectoral();
        }


        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            _viewModelVentanaSecundaria.ModificarProcesoElectoral();
        }


        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar el proceso electoral seleccionado?", "Eliminar proceso electoral", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _viewModelVentanaSecundaria.eliminarProcesoElectoral();
            }
        }
    }
}
