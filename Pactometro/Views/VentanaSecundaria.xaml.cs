using System;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using Pactometro.ViewModels;


namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaSecundaria.xaml
    /// </summary>
    public partial class VentanaSecundaria : Window
    {
        private ObservableCollection<ProcesoElectoral> ColeccionElecciones;

        // Definir el evento para notificar la selección
        public event EventHandler<ProcesoElectoral> ProcesoEleccionSeleccionado;

        private VentanaSecundariaViewModel _viewModelVentanaSecundaria;

        public VentanaSecundaria(ObservableCollection<ProcesoElectoral> coleccionElecciones)
        {
            InitializeComponent();
            _viewModelVentanaSecundaria = new VentanaSecundariaViewModel(coleccionElecciones);
            this.DataContext = _viewModelVentanaSecundaria;
            ColeccionElecciones = coleccionElecciones;
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

            // Dispara el evento con el elemento seleccionado o null
            ProcesoEleccionSeleccionado?.Invoke(this, procesoElectoralSeleccionado);
        }


        private static VentanaAgregar ventanaAñadirProceso;

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia de la clase VentanaAñadirProceso, que es una ventana modal y hazte su propietario
            ventanaAñadirProceso = new VentanaAgregar(ColeccionElecciones);
            ventanaAñadirProceso.Owner = this;
            ventanaAñadirProceso.ShowDialog();
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

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (mainTable.SelectedItem != null)
            {
                // Crea una instancia de la clase VentanaModificar con el proceso electoral seleccionado
                VentanaModificar ventanaModificar = new VentanaModificar(ColeccionElecciones, mainTable.SelectedItem as ProcesoElectoral);
                ventanaModificar.Owner = this;
                ventanaModificar.ShowDialog();
                _viewModelVentanaSecundaria.OrdenarPorFecha(ColeccionElecciones);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para eliminar un proceso electoral existente
            // Asegúrate de que hay un proceso electoral seleccionado antes de continuar.
            if (mainTable.SelectedItem != null)
            {
                // Elimina de la colección el elemento seleccionado
                ColeccionElecciones.Remove(mainTable.SelectedItem as ProcesoElectoral);
                _viewModelVentanaSecundaria.OrdenarPorFecha(ColeccionElecciones);
            }
            else 
            { 
                MessageBox.Show("No hay ningún proceso electoral seleccionado");
            }
        }
    }
}
