using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pactometro
{
    /// <summary>
    /// Lógica de interacción para VentanaExportar.xaml
    /// </summary>
    public partial class VentanaExportar : Window
    {
        Window mainWindow;
        public string SelectedFormat { get; private set; }
        public int Quality { get; private set; }
        public VentanaExportar()
        {
            InitializeComponent();
            mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow == null)
            {
                // Manejar el caso de que mainWindow sea nula
            }
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el formato seleccionado
           /* if(SelectedFormat == null) { 
                // Mostrar un mensaje de error si no se ha seleccionado un formato
                MessageBox.Show("No se ha seleccionado un formato de imagen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/
            SelectedFormat = ((ComboBoxItem)formatComboBox.SelectedItem).Content.ToString();
            // Obtener la calidad para JPEG
            Quality = (int)qualitySlider.Value;


            // Seleccionar el codificador basado en el formato elegido
            BitmapEncoder encoder;
            switch (SelectedFormat)
            {
                case "JPEG":
                    JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                    jpegEncoder.QualityLevel = Quality;
                    encoder = jpegEncoder;
                    break;
                case "PNG":
                    encoder = new PngBitmapEncoder();
                    break;
                case "BMP":
                    encoder = new BmpBitmapEncoder();
                    break;
                case "GIF":
                    encoder = new GifBitmapEncoder();
                    break;
                case "TIFF":
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new InvalidOperationException("Formato no soportado");
            }

            RenderTargetBitmap capturedImage = CaptureContent();

            // Añadir la imagen capturada al codificador
            encoder.Frames.Add(BitmapFrame.Create(capturedImage));

            // Guardar la imagen
            SaveImage(encoder);

            // Cierra la ventana y retorna a la ventana principal
            this.DialogResult = true;
            this.Close();
        }

        private RenderTargetBitmap CaptureContent()
        {
            // Verificar que mainWindow no es null
            if (mainWindow != null)
            {
                // Ocultar elementos en mainWindow si es necesario
                // Por ejemplo, si quieres ocultar un botón de exportación en mainWindow
                // var exportarButton = (Button)mainWindow.FindName("exportar");
                // if (exportarButton != null) exportarButton.Visibility = Visibility.Hidden;

                var exportarButton = (Button)mainWindow.FindName("exportar");
                if (exportarButton != null) exportarButton.Visibility = Visibility.Hidden;

                // Ocultar el menu de la ventana principal
                var menu = (Menu)mainWindow.FindName("menu");
                if (menu != null) menu.Visibility = Visibility.Hidden;


                // Captura de la ventana completa
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)mainWindow.ActualWidth, (int)mainWindow.ActualHeight,
                    96d, 96d, System.Windows.Media.PixelFormats.Default);
                rtb.Render(mainWindow);

                //Volver a mostrar los elementos ocultos
                if (exportarButton != null) exportarButton.Visibility = Visibility.Visible;
                if (menu != null) menu.Visibility = Visibility.Visible;

                return rtb;
            }
            else
            {
                throw new InvalidOperationException("La ventana principal no está disponible.");
            }
        }

        private void SaveImage(BitmapEncoder encoder)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Image files|*.png;*.jpeg;*.bmp;*.gif;*.tiff";
            if (dlg.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
