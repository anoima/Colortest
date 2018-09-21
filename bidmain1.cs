using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Mainform : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;

        public Point marketlocul;
        Point marketlocdr;
        Point notificationlocul;
        Point notificationlocdr;
        int[,] marketloc = new int[10, 2];
        int[,] notificationloc = new int[5, 2];
        int marketnum = 0;
        int notificationnum = 0;
        Random rnd = new Random();
        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        public static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        //<Summary>
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        //</Summary>

        Boolean market = true;
        Boolean notification = true;
        Bitmap itempicture;
        Bitmap marketscreen;
        Bitmap notificatinoscreen;
        int rndmin = 1000;
        int rndmax = 1200;

        public Mainform()
        {
            InitializeComponent();
            stopbtn.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getoptions();
            //List<string> items = new List<string>();
            //items.Add("Crescent Guardian+2");
            //items.Add("Crescent Guardian+4");
            //items.Add("Crescent Guardian+8");
            //items.Add("Crescent of+2");
            //items.Add("Cron stone+2");
            //items.Add("caphreas of+2");
            AutoCompleteStringCollection sourceName = new AutoCompleteStringCollection();
            string[] lines = System.IO.File.ReadAllLines(@"U:\idname.txt");
            foreach (string name in lines)
            {
                sourceName.Add(name.Substring(1,name.Length-1));
            }

            itemnamebox.AutoCompleteCustomSource = sourceName;
            itemnamebox.AutoCompleteMode = AutoCompleteMode.Suggest;
            itemnamebox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
                startbtn.Visible = false;
                stopbtn.Visible = true;
                taskroute();
        }

        private void stopbtn_Click(object sender, EventArgs e)
        {
            startbtn.Visible = true;
            stopbtn.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private Bitmap GetMarket()
        {
            marketlocul = new Point(marketloc[0, 0], marketloc[0, 1]);
            marketlocdr = new Point(marketloc[1, 0] - marketloc[0, 0], marketloc[1, 1] - marketloc[0, 1]);

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
            Bitmap CroppedImage = screenshot.Clone(new System.Drawing.Rectangle(marketlocul.X, marketlocul.Y, marketlocdr.X, marketlocdr.Y), screenshot.PixelFormat);
            screenshot.Dispose();
            screenGraph.Dispose();
            return CroppedImage;
        }

        private Bitmap GetMarketItem(Point marketlocul, Point marketlocldr)
        {
            //marketlocul = new Point(marketloc[0, 0], marketloc[0, 1]);
            // marketlocdr = new Point(marketloc[1, 0] - marketloc[0, 0], marketloc[1, 1] - marketloc[0, 1]);

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
            Bitmap CroppedImage = screenshot.Clone(new System.Drawing.Rectangle(marketlocul.X, marketlocul.Y, marketlocdr.X, marketlocdr.Y), screenshot.PixelFormat);
            screenshot.Dispose();
            screenGraph.Dispose();
            return CroppedImage;
        }

        private Bitmap GetNotification()
        {
            notificationlocul = new Point(notificationloc[0, 0], notificationloc[0, 1]);
            notificationlocdr = new Point(notificationloc[1, 0] - notificationloc[0, 0], notificationloc[1, 1] - notificationloc[0, 1]);

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
            Bitmap CroppedImage = screenshot.Clone(new System.Drawing.Rectangle(notificationlocul.X, notificationlocul.Y, notificationlocdr.X, notificationlocdr.Y), screenshot.PixelFormat);

            return CroppedImage;
        }

        private void itempic_Click(object sender, EventArgs e)
        {
            itempic.Image = Clipboard.GetImage();
        }

        private void itempic_DoubleClick(object sender, EventArgs e)
        {
            Mainform.KeyDown(Keys.LShiftKey);
            Mainform.KeyDown(Keys.LWin);
            Mainform.KeyDown(Keys.S);
            Mainform.KeyUp(Keys.LWin);
            Mainform.KeyUp(Keys.LShiftKey);
            Mainform.KeyUp(Keys.S);
        }

        public Point Find(Bitmap haystack, Bitmap needle)
        {
            if (null == haystack || null == needle)
            {
                return new Point(0, 0);
            }
            if (haystack.Width < needle.Width || haystack.Height < needle.Height)
            {
                return new Point(0, 0);
            }

            var haystackArray = GetPixelArray(haystack);
            var needleArray = GetPixelArray(needle);

            foreach (var firstLineMatchPoint in FindMatch(haystackArray.Take(haystack.Height - needle.Height), needleArray[0]))
            {
                if (IsNeedlePresentAtLocation(haystackArray, needleArray, firstLineMatchPoint, 1))
                {
                    return firstLineMatchPoint;
                }
            }

            return new Point(0, 0);
        }

        private int[][] GetPixelArray(Bitmap bitmap)
        {
            var result = new int[bitmap.Height][];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                result[y] = new int[bitmap.Width];
                Marshal.Copy(bitmapData.Scan0 + y * bitmapData.Stride, result[y], 0, result[y].Length);
            }

            bitmap.UnlockBits(bitmapData);

            return result;
        }

        private IEnumerable<Point> FindMatch(IEnumerable<int[]> haystackLines, int[] needleLine)
        {
            var y = 0;
            foreach (var haystackLine in haystackLines)
            {
                for (int x = 0, n = haystackLine.Length - needleLine.Length; x < n; ++x)
                {
                    if (ContainSameElements(haystackLine, x, needleLine, 0, needleLine.Length))
                    {
                        yield return new Point(x, y);
                    }
                }
                y += 1;
            }
        }

        private bool ContainSameElements(int[] first, int firstStart, int[] second, int secondStart, int length)
        {
            for (int i = 0; i < length; ++i)
            {
                if (first[i + firstStart] != second[i + secondStart])
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNeedlePresentAtLocation(int[][] haystack, int[][] needle, Point point, int alreadyVerified)
        {
            //we already know that "alreadyVerified" lines already match, so skip them
            for (int y = alreadyVerified; y < needle.Length; ++y)
            {
                if (!ContainSameElements(haystack[y + point.Y], point.X, needle[y], 0, needle.Length))
                {
                    return false;
                }
            }
            return true;
        }

        private static Bitmap ConvertToFormat(Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }
        private void taskroute()
        {
            itempicture = new Bitmap(itempic.Image);
            notificatinoscreen = GetNotification();
            marketscreen = GetMarket();
            //for (int i = 0; i < notificationloc.Length; i++)
            //{
            //    MessageBox.Show("X: " + notificationloc[i, 0] + " Y: " + notificationloc[i, 1]);
            //}
            if (Find(notificatinoscreen, itempicture) != null)
            {

                System.Threading.Thread.Sleep(rnd.Next(rndmin, rndmax));
                Point market = (Point)Find(marketscreen, itempicture);
                market = (Point)Find(marketscreen, itempicture);
                SampleMouseMove.MoveMouse(market.X + marketloc[0, 0] + 100, market.Y + marketloc[0, 1] + 30, 2, 2);
                do
                {
                    GC.Collect();
                    marketscreen.Dispose();
                    marketscreen = GetMarket();

                    market = (Point)Find(marketscreen, itempicture);
                    if (market != new Point(0, 0))
                    {
                        SampleMouseMove.MoveMouse(market.X + marketloc[0, 0] + 100, market.Y + marketloc[0, 1] + 30, 2, 2);
                    }
                } while (market != new Point(0, 0));
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Optionsform o = new Optionsform();
            o.ShowDialog();
        }

        public void getoptions()
        {
            string[] lines = System.IO.File.ReadAllLines(@".\Options.txt");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                switch (line[0])
                {
                    case '1': rndmin = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '2': rndmax = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '3': marketloc[0, 0] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '4': marketloc[0, 1] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '5': marketloc[1, 0] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '6': marketloc[1, 1] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '7': notificationloc[0, 0] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '8': notificationloc[0, 1] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case '9': notificationloc[1, 0] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                    case 'a': notificationloc[1, 1] = Convert.ToInt32(line.Substring(3, line.Length - 3)); break;
                }
            }
        }

    }

    static class SampleMouseMove
    {

        static Random random = new Random();
        static int mouseSpeed = 28;


        public static void MoveMouse(int x, int y, int rx, int ry)
        {
            Point c = new Point();
            GetCursorPos(out c);

            x += random.Next(rx);
            y += random.Next(ry);

            double randomSpeed = Math.Max((random.Next(mouseSpeed) / 2.0 + mouseSpeed) / 10.0, 0.1);

            WindMouse(c.X, c.Y, x, y, 9.0, 3.0, 10.0 / randomSpeed,
                15.0 / randomSpeed, 10.0 * randomSpeed, 10.0 * randomSpeed);
        }

        static void WindMouse(double xs, double ys, double xe, double ye,
            double gravity, double wind, double minWait, double maxWait,
            double maxStep, double targetArea)
        {

            double dist, windX = 0, windY = 0, veloX = 0, veloY = 0, randomDist, veloMag, step;
            int oldX, oldY, newX = (int)Math.Round(xs), newY = (int)Math.Round(ys);

            double waitDiff = maxWait - minWait;
            double sqrt2 = Math.Sqrt(2.0);
            double sqrt3 = Math.Sqrt(3.0);
            double sqrt5 = Math.Sqrt(5.0);

            dist = Hypot(xe - xs, ye - ys);

            while (dist > 1.0)
            {

                wind = Math.Min(wind, dist);

                if (dist >= targetArea)
                {
                    int w = random.Next((int)Math.Round(wind) * 2 + 1);
                    windX = windX / sqrt3 + (w - wind) / sqrt5;
                    windY = windY / sqrt3 + (w - wind) / sqrt5;
                }
                else
                {
                    windX = windX / sqrt2;
                    windY = windY / sqrt2;
                    if (maxStep < 3)
                        maxStep = random.Next(3) + 3.0;
                    else
                        maxStep = maxStep / sqrt5;
                }

                veloX += windX;
                veloY += windY;
                veloX = veloX + gravity * (xe - xs) / dist;
                veloY = veloY + gravity * (ye - ys) / dist;

                if (Hypot(veloX, veloY) > maxStep)
                {
                    randomDist = maxStep / 2.0 + random.Next((int)Math.Round(maxStep) / 2);
                    veloMag = Hypot(veloX, veloY);
                    veloX = (veloX / veloMag) * randomDist;
                    veloY = (veloY / veloMag) * randomDist;
                }

                oldX = (int)Math.Round(xs);
                oldY = (int)Math.Round(ys);
                xs += veloX;
                ys += veloY;
                dist = Hypot(xe - xs, ye - ys);
                newX = (int)Math.Round(xs);
                newY = (int)Math.Round(ys);

                if (oldX != newX || oldY != newY)
                    SetCursorPos(newX, newY);

                step = Hypot(xs - oldX, ys - oldY);
                int wait = (int)Math.Round(waitDiff * (step / maxStep) + minWait);
                Thread.Sleep(wait);
            }

            int endX = (int)Math.Round(xe);
            int endY = (int)Math.Round(ye);
            if (endX != newX || endY != newY)
                SetCursorPos(endX, endY);
        }

        static double Hypot(double dx, double dy)
        {
            return Math.Sqrt(dx * dx + dy * dy);
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point p);
    }
}
