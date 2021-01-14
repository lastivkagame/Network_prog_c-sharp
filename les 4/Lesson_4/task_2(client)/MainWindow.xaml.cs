using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace task_2_client_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int port = 2020;
        const int maxlent = 8000;
        public MainWindow()
        {
            InitializeComponent();

            //Console.Title = "Client";
            //var path = "winter2.jpg";

            //var aTimer = new System.Timers.Timer();
            //aTimer.Interval = 5000;
            //aTimer.Elapsed += OnTimedEvent;

            

            var path = "screen3.png";
            System.Drawing.Image im = ScreenCapture.CaptureDesktop();
            byte[] image_data = ImageToByteArray(im);
            var client = new UdpClient();
            SendHeaders(path, client, image_data);
            client.Close();

            //Console.ReadLine();


            // ScreenCapture sc = new ScreenCapture();
            // // capture entire screen, and save it to a file
            // System.Drawing.Image img = sc.C
            // // display image in a Picture control named imageDisplay
            //// this.imageDisplay.Image = img;
            // // capture this window, and save it
            // sc.CaptureWindowToFile(this.Handle, "C:\\temp2.gif", ImageFormat.Gif);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            
            //throw new NotImplementedException();
        }

        private static void SendHeaders(string path, UdpClient client, byte[] image_data)
        {
            var filename = System.IO.Path.GetFileName(path);
            //byte [] datatitle = 
            client.Send(Encoding.UTF8.GetBytes(filename), filename.Length, "127.0.0.1", port);
            SendBody(path, client, image_data);
            //throw new NotImplementedException();
        }

        private static void SendBody(string path, UdpClient client, byte[] image_data)
        {
            //var data = File.ReadAllBytes(path);

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

            client.Send(Encoding.UTF8.GetBytes(count.ToString()), count.ToString().Length, "127.0.0.1", port);

            for (int i = 0; i < count; i++)
            {
                client.Send(bigdata[i], bigdata[i].Length, "127.0.0.1", port);
            }

            //throw new NotImplementedException();
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                //imageIn.Save(ms, imageIn.RawFormat);
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

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

        //public enum enmScreenCaptureMode
        //{
        //    Screen,
        //    Window
        //}

        //class ScreenCapturer
        //{
        //    [DllImport("user32.dll")]
        //    private static extern IntPtr GetForegroundWindow();

        //    [DllImport("user32.dll")]
        //    private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        //    [StructLayout(LayoutKind.Sequential)]
        //    private struct Rect
        //    {
        //        public int Left;
        //        public int Top;
        //        public int Right;
        //        public int Bottom;
        //    }

        //    //public Bitmap Capture(enmScreenCaptureMode screenCaptureMode = enmScreenCaptureMode.Window)
        //    //{
        //    //    System.Drawing.Rectangle bounds;

        //    //    if (screenCaptureMode == enmScreenCaptureMode.Screen)
        //    //    {
        //    //        bounds = Screen.GetBounds(Point.Empty);
        //    //        CursorPosition = Cursor.Position;
        //    //    }
        //    //    else
        //    //    {
        //    //        var foregroundWindowsHandle = GetForegroundWindow();
        //    //        var rect = new Rect();
        //    //        GetWindowRect(foregroundWindowsHandle, ref rect);
        //    //        bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        //    //        CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
        //    //    }

        //    //    var result = new Bitmap(bounds.Width, bounds.Height);

        //    //    using (var g = Graphics.FromImage(result))
        //    //    {
        //    //        g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        //    //    }

        //    //    return result;
        //    //}

        //    public System.Drawing.Point CursorPosition
        //    {
        //        get;
        //        protected set;
        //    }
        //}
    }
}
