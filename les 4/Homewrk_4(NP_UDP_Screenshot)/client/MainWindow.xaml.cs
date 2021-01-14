using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int count = 0;
        public const int port = 2020;
        public const int maxlent = 8000; // к-сть яку хочемо надіслати
        public System.Threading.Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            #region Timer 1 minute
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            timer = new System.Threading.Timer((e) =>
            {
                TimerTick();
            }, null, startTimeSpan, periodTimeSpan);
            #endregion

        }

        private void TimerTick()
        {
            MessageBox.Show((count++) + " minutes", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            var path = "screen" + count + ".png";
            System.Drawing.Image im = ScreenCapture.CaptureDesktop(); // робимо скрін
            byte[] image_data = ImageToByteArray(im);

            // надсилаємо
            var client = new UdpClient();
            SendHeaders(path, client, image_data);
            client.Close();
        }

        private static void SendHeaders(string path, UdpClient client, byte[] image_data)
        {
            var filename = System.IO.Path.GetFileName(path);
            // тут тільки назву (номер) скріна
            client.Send(Encoding.UTF8.GetBytes(filename), filename.Length, "127.0.0.1", port);
            SendBody(path, client, image_data);
        }

        private static void SendBody(string path, UdpClient client, byte[] image_data)
        {
            // розбивка на кусочки 
            var count = (int)image_data.Length / maxlent;
            int start = 0;
            int ends = maxlent;

            byte[][] bigdata = new byte[count][];
            List<byte[]> bigdatalist = new List<byte[]>();

            for (int i = 0; i < count; i++)
            {
                bigdata[i] = new byte[maxlent];

                //Array.Copy(image_data, start, bigdata[i], ends, image_data.Length - count * maxlent);
                var size = ends - start;
                //byte[] temp = new byte[size];
                //Array.Copy(image_data, start, temp, 0, size);
                Array.Copy(image_data, start, bigdata[i], 0, size);

                start = ends;
                ends += maxlent;
            }

            //надсилається к-сть кусочків які там треба буде прийняти
            client.Send(Encoding.UTF8.GetBytes(count.ToString()), count.ToString().Length, "127.0.0.1", port);

            // і надсилаємо 
            for (int i = 0; i < count; i++)
            {
                client.Send(bigdata[i], bigdata[i].Length, "127.0.0.1", port);
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///  клас що робить скіни силка: https://stackoverflow.com/questions/1163761/capture-screenshot-of-active-window
        /// </summary>
        public class ScreenCapture
        {
            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetDesktopWindow();

            [StructLayout(LayoutKind.Sequential)]
            private struct Rect
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            [DllImport("user32.dll")]
            private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

            public static System.Drawing.Image CaptureDesktop()
            {
                return CaptureWindow(GetDesktopWindow());
            }

            public static Bitmap CaptureActiveWindow()
            {
                return CaptureWindow(GetForegroundWindow());
            }

            public static Bitmap CaptureWindow(IntPtr handle)
            {
                var rect = new Rect();
                GetWindowRect(handle, ref rect);
                var bounds = new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                var result = new Bitmap(bounds.Width, bounds.Height);

                using (var graphics = Graphics.FromImage(result))
                {
                    graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
                }

                return result;
            }
        }

    }
}
