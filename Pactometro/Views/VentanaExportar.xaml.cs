using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace Pactometro
{
    public partial class VentanaExportar : Window
    {
        Window mainWindow;

        public VentanaExportar()
        {
            InitializeComponent();
            mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow == null)
            {
                MessageBox.Show("Error: Ventana principal no disponible.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            // Establecer PNG como opción predeterminada
            formatComboBox.SelectedIndex = 0; // Suponiendo que PNG es el primer ítem en tu ComboBox

            // Ocultar opciones de calidad inicialmente
            ToggleQualityOptionsVisibility(false);


            // Ajustar el tamaño de la ventana para PNG
            this.Height = 205; // Ajusta este valor según tus necesidades
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
                BitmapEncoder encoder = GetEncoder(selectedFormat, quality);
                RenderTargetBitmap capturedImage = CaptureContent();


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
                SaveImage(encoder, selectedFormat);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se produjo un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private BitmapEncoder GetEncoder(string format, int quality)
        {
            switch (format)
            {
                case "JPG":
                    JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                    jpegEncoder.QualityLevel = quality;
                    return jpegEncoder;
                case "PNG":
                    return new PngBitmapEncoder();
                default:
                    throw new InvalidOperationException("Formato no soportado");
            }
        }


        private int GetQualityFromRadioButtons()
        {
            if (radioButtonLow.IsChecked == true) return 30;
            if (radioButtonMedium.IsChecked == true) return 60;
            return 100; // High quality or default
        }

        private RenderTargetBitmap CaptureContent()
        {
            if (mainWindow == null)
            {
                MessageBox.Show("La ventana principal no está disponible para la captura.");
                return null;
            }

            int width = (int)mainWindow.ActualWidth - 15;
            int height = (int)mainWindow.ActualHeight - 37;

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width,
                height,
                96, // dpiX
                96, // dpiY
                PixelFormats.Pbgra32);

            rtb.Render(mainWindow);

            return rtb;
        }


        private void SaveImage(BitmapEncoder encoder, string format)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = $"Image files (*.{format.ToLower()})|*.{format.ToLower()}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    // Guarda la imagen en el archivo
                    encoder.Save(fileStream);
                }
            }
        }


        private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFormat = ((ComboBoxItem)formatComboBox.SelectedItem)?.Content.ToString();
            bool isJpgSelected = selectedFormat == "JPG";

            ToggleQualityOptionsVisibility(isJpgSelected);

            // Ajusta el tamaño de la ventana
            this.Height = isJpgSelected ? 355 : 205; // Ejemplo de tamaños, ajusta según tus necesidades
        }


        private void ToggleQualityOptionsVisibility(bool isVisible)
        {
            qualityOptionsPanel.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
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


            RenderTargetBitmap capturedImage = CaptureContent();
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
                    Clipboard.SetImage(ConvertBitmapToBitmapSource(clipboardBmp));
                }
            }

            if (menu != null) menu.Visibility = Visibility.Visible;
            if(flag == 0)
            {
                if (btnPacto != null) btnPacto.Visibility = Visibility.Visible;
                if (btnReiniciar != null) btnReiniciar.Visibility = Visibility.Visible;
            }
            
        }
        private BitmapSource ConvertBitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, bitmap.HorizontalResolution, bitmap.VerticalResolution, PixelFormats.Bgra32, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

    }
}
