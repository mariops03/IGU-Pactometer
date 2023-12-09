using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Drawing.Imaging;

namespace Pactometro.ViewModels
{
    internal class VentanaExportarViewModel
    {
        public VentanaExportarViewModel()
        {
        }

        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public BitmapEncoder GetEncoder(string format, int quality)
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

        public RenderTargetBitmap CaptureContent()
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

        public void SaveImage(BitmapEncoder encoder, string format)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = $"Image files (*.{format.ToLower()})|*.{format.ToLower()}",
                // Poner el nombre predeterminado del archivo con la fecha en formato dd-MM-yyyy y la hora en formato HH-mm-ss
                FileName = $"Pactometro {DateTime.Now.ToString("dd-MM-yyyy HHmmss")}.{format.ToLower()}"
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

        public BitmapSource ConvertBitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, bitmap.HorizontalResolution, bitmap.VerticalResolution, PixelFormats.Bgra32, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}
