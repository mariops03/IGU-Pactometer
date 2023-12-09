using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Imaging;
using Microsoft.Win32;
using Pactometro.ViewModels;

namespace Pactometro
{
    public partial class VentanaExportar : Window
    {
        Window mainWindow;
        private VentanaExportarViewModel viewModel;

        public VentanaExportar()
        {
            InitializeComponent();
            mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow == null)
            {
                MessageBox.Show("Error: Ventana principal no disponible.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            viewModel = new VentanaExportarViewModel();
            this.DataContext = viewModel;

            // Establecer PNG como opción predeterminada
            formatComboBox.SelectedIndex = 0;

            ToggleQualityOptionsVisibility(false);

            this.Height = 205;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedFormat = ((ComboBoxItem)formatComboBox.SelectedItem).Content.ToString();
                if (string.IsNullOrEmpty(selectedFormat))
                {
                    MessageBox.Show("Por favor, selecciona un formato de imagen.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Oculta el menú de la ventana principal
                var menu = (Menu)mainWindow.FindName("menu");
                if (menu != null) menu.Visibility = Visibility.Hidden;

                int flag = 0;
                //Ocultar el botón de exportar de la ventana principal
                var btnPacto = (Button)mainWindow.FindName("btnPacto");
                if (btnPacto != null)
                {
                    if (btnPacto.Visibility == Visibility.Visible) btnPacto.Visibility = Visibility.Collapsed;
                    else flag = 1;

                }

                var btnReiniciar = (Button)mainWindow.FindName("btnReiniciar");
                if (btnReiniciar != null) btnReiniciar.Visibility = Visibility.Collapsed;


                int quality = GetQualityFromRadioButtons();
                BitmapEncoder encoder = viewModel.GetEncoder(selectedFormat, quality);
                RenderTargetBitmap capturedImage = viewModel.CaptureContent();


                // Muestra el menú de la ventana principal
                if (menu != null) menu.Visibility = Visibility.Visible;

                if (flag == 0)
                {
                    if (btnPacto != null) btnPacto.Visibility = Visibility.Visible;
                    if (btnReiniciar != null) btnReiniciar.Visibility = Visibility.Visible;
                }

                if (capturedImage == null)
                {
                    MessageBox.Show("Error al capturar la imagen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                encoder.Frames.Add(BitmapFrame.Create(capturedImage));
                viewModel.SaveImage(encoder, selectedFormat);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetQualityFromRadioButtons()
        {
            if (radioButtonLow.IsChecked == true) return 33;
            if (radioButtonMedium.IsChecked == true) return 66;
            return 100; // High quality or default
        }

        private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFormat = ((ComboBoxItem)formatComboBox.SelectedItem)?.Content.ToString();
            bool isJpgSelected = selectedFormat == "JPG";

            ToggleQualityOptionsVisibility(isJpgSelected);

            // Ajusta el tamaño de la ventana
            this.Height = isJpgSelected ? 300 : 205; // Ejemplo de tamaños, ajusta según tus necesidades
        }


        private void ToggleQualityOptionsVisibility(bool isVisible)
        {
            opcionesCalidad.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            int flag = 0;
            //Ocultar el botón de exportar de la ventana principal
            var btnPacto = (Button)mainWindow.FindName("btnPacto");
            if (btnPacto != null)
            {
                if(btnPacto.Visibility == Visibility.Visible) btnPacto.Visibility = Visibility.Collapsed;
                else flag = 1;
                    
            }
            
            var btnReiniciar = (Button)mainWindow.FindName("btnReiniciar");
            if (btnReiniciar != null) btnReiniciar.Visibility = Visibility.Collapsed;

            var menu = (Menu)mainWindow.FindName("menu");
            if (menu != null) menu.Visibility = Visibility.Collapsed;


            RenderTargetBitmap capturedImage = viewModel.CaptureContent();
            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(capturedImage.PixelWidth, capturedImage.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                BitmapData data = bmp.LockBits(
                    new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                capturedImage.CopyPixels(
                    Int32Rect.Empty,
                    data.Scan0,
                    data.Height * data.Stride,
                    data.Stride);

                bmp.UnlockBits(data);

                using (System.Drawing.Bitmap clipboardBmp = new System.Drawing.Bitmap(bmp))
                {
                    Clipboard.SetImage(viewModel.ConvertBitmapToBitmapSource(clipboardBmp));
                }
            }
            if (menu != null) menu.Visibility = Visibility.Visible;
            if(flag == 0)
            {
                if (btnPacto != null) btnPacto.Visibility = Visibility.Visible;
                if (btnReiniciar != null) btnReiniciar.Visibility = Visibility.Visible;
            }
        }
    }
}
