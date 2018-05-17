using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        private static bool switcher = true;

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            switcher=true;
            Color cl = Color.FromArgb(31,255,201,14);
            Color numcolor = Color.FromArgb(238, 237, 28, 36);
            Bitmap screenCapture = GetScreen();
            Point Colorpos = GetColorPos(cl, screenCapture);
            Console.WriteLine(Colorpos);
            do
            {
                GC.Collect();
                screenCapture.Dispose();
                screenCapture = GetScreen();
                if (screenCapture.GetPixel(Colorpos.X, Colorpos.Y) != cl)
                {
                    Colorpos = GetColorPos(cl, screenCapture);
                }
                if (!getnum(Colorpos, numcolor, screenCapture))
                {
                    Clickloc(Colorpos);
                }
                else
                {
                    switcher = false;
                    //break;
                }
            } while (switcher);
            sw.Stop();
            TimeSpan time = sw.Elapsed;
            Console.WriteLine("Successfully.\n Time : {0}.", time.Seconds);
            MessageBox.Show("Successfully.\nTime : " + time.Seconds+" second", "Successfull", MessageBoxButtons.OK);

        }

        private Bitmap GetScreen()
        {
            Bitmap screenshot = new Bitmap(SystemInformation.VirtualScreen.Width,
                               SystemInformation.VirtualScreen.Height,
                               PixelFormat.Format32bppArgb);
            Graphics screenGraph = Graphics.FromImage(screenshot);
            screenGraph.CopyFromScreen(SystemInformation.VirtualScreen.X,
                                       SystemInformation.VirtualScreen.Y,
                                       0,
                                       0,
                                       SystemInformation.VirtualScreen.Size,
                                       CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        private Point GetColorPos(Color color, Bitmap screenCapture)
        {
            Color c;
            for (int x = 0; x < screenCapture.Width; x++) //screenCapture.Width
            {
                for (int y = 0; y < screenCapture.Height; y++)//screenCapture.Height
                {
                    c = screenCapture.GetPixel(x, y);//GetColorAt(new Point(x, y), screenCapture);
				if(c.R == color.R && c.G == color.G && c.B == color.B)
				{
                    return new Point(x, y);
				}
                }
            }
            return new Point(0,0);
        }

        public void Clickloc(Point location)
        {
        System.Windows.Forms.Cursor.Position = new Point(location.X-55,location.Y+50) ;
        MouseClick();
        }

        public bool getnum(Point location, Color color,Bitmap screen)
         {
             Color c1 = screen.GetPixel(location.X - 10, location.Y - 96);
             Color c2 = screen.GetPixel(location.X - 89, location.Y - 76);
             Color c3=screen.GetPixel(location.X - 89, location.Y - 76);
             if (c1.R == color.R && c1.G == color.G && c1.B == color.B)
             {
                 return true;
             }
             else
             {
                 return false;
             }
         }
         public void MouseClick()
         {
             int x = 100 * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
             int y = 100 * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
             mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
             mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
         }

         private void Form1_Load(object sender, EventArgs e)
         {

         }
    }
}
