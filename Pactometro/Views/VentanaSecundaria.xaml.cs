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
using System.Windows.Input;

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
            _viewModelVentanaSecundaria.OrdenarPorFechaDescendente();
            _viewModelVentanaSecundaria.OrdenarPorEscañosDescendente();
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
                else if(header == "NUMERO DE ESCAÑOS")
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
            // Crear una nueva instancia de ProcesoElectoral para añadir
            ProcesoElectoral nuevoProceso = new ProcesoElectoral();

            // Crear la ventana en modo Agregar, pasando el nuevo proceso electoral
            VentanaAñadirModificar ventanaAñadirProceso = new VentanaAñadirModificar(ModoVentana.Agregar, ColeccionElecciones, nuevoProceso);
            ventanaAñadirProceso.Owner = this;
            ventanaAñadirProceso.ShowDialog();

            // Actualizar la colección de procesos electorales
            _viewModelVentanaSecundaria.OrdenarPorFechaDescendente();
            mainTable.InvalidateVisual();
        }


        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (mainTable.SelectedItem != null)
            {
                // Obtener el proceso electoral seleccionado para modificar
                ProcesoElectoral procesoAEditar = mainTable.SelectedItem as ProcesoElectoral;

                // Crear la ventana en modo Modificar, pasando el proceso existente
                VentanaAñadirModificar ventanaModificar = new VentanaAñadirModificar(ModoVentana.Modificar, ColeccionElecciones, procesoAEditar);
                ventanaModificar.Owner = this;
                ventanaModificar.ShowDialog();

                // Actualizar la colección de procesos electorales
                _viewModelVentanaSecundaria.OrdenarPorFechaDescendente();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (mainTable.SelectedItem != null)
            {
                // Elimina de la colección el elemento seleccionado
                ColeccionElecciones.Remove(mainTable.SelectedItem as ProcesoElectoral);
            }
            else 
            { 
                MessageBox.Show("No hay ningún proceso electoral seleccionado");
            }
        }
    }
}
