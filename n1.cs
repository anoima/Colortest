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
using AForge.Imaging;
using System.Threading;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        int screenwidth = Screen.PrimaryScreen.Bounds.Width;
        int screenheight = Screen.PrimaryScreen.Bounds.Height;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;

        Point marketlocul;
        Point marketlocdr;
        Point notificationlocul;
        Point notificationlocdr;
        int[,] marketloc = new int[5,2];
        int[,] notificationloc = new int[3, 2];
        int marketnum = 0;
        int notificationnum = 0;

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

        Boolean market=true;
        Boolean notification = true;
        Bitmap itempicture;
        Bitmap marketscreen;

        public Form1()
        {
            InitializeComponent();
            stopbtn.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
                startbtn.Visible = false;
                stopbtn.Visible = true;
                itempicture=new Bitmap(itempic.Image);
                marketscreen = GetMarket();
                Find(marketscreen, itempicture);
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
            notificationlocul = new Point(notificationloc[0, 0], notificationloc[0, 1]);
            notificationlocdr = new Point(notificationloc[1, 0] - notificationloc[0, 0], notificationloc[1, 1] - notificationloc[0, 1]);

            marketlocul=new Point(marketloc[0,0],marketloc[0,1]);
            marketlocdr = new Point(marketloc[1, 0]-marketloc[0,0], marketloc[1, 1]-marketloc[0,1]);

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

            return CroppedImage;
        }

        private void itempic_Click(object sender, EventArgs e)
        {
            itempic.Image = Clipboard.GetImage();
        }

        private void itempic_DoubleClick(object sender, EventArgs e)
        {
            Form1.KeyDown(Keys.LShiftKey);
            Form1.KeyDown(Keys.LWin);
            Form1.KeyDown(Keys.S);
            Form1.KeyUp(Keys.LWin);
            Form1.KeyUp(Keys.LShiftKey);
            Form1.KeyUp(Keys.S);
        }

        //private void getpic(){
        //    System.Drawing.Bitmap sourceImage = ConvertToFormat(marketscreen,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //    System.Drawing.Bitmap template = ConvertToFormat(itempicture, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //    // create template matching algorithm's instance
        //    // (set similarity threshold to 92.1%)

        //   ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.921f);
        //        // find all matchings with specified above similarity

        //        TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template);
        //        // highlight found matchings

        //   BitmapData data = sourceImage.LockBits(
        //        new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
        //        ImageLockMode.ReadWrite, sourceImage.PixelFormat);
        //    foreach (TemplateMatch m in matchings)
        //    {

        //            Drawing.Rectangle(data, m.Rectangle, Color.White);

        //        MessageBox.Show(m.Rectangle.Location.ToString());
        //        // do something else with matching
        //    }
        //    sourceImage.UnlockBits(data);
        //}

        public Point? Find(Bitmap haystack, Bitmap needle)
        {
            if (null == haystack || null == needle)
            {
                return null;
            }
            if (haystack.Width < needle.Width || haystack.Height < needle.Height)
            {
                return null;
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

            return null;
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

        private static Bitmap ConvertToFormat( Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }

        private void Eventmarket(object sender, EventArgs e)
        {
            marketloc[marketnum, 0] = Cursor.Position.X;
            marketloc[marketnum, 1] = Cursor.Position.Y;
            marketnum++;
        }

        private void Eventnotification(object sender, EventArgs e)
        {
            notificationloc[notificationnum, 0] = Cursor.Position.X;
            notificationloc[notificationnum, 1] = Cursor.Position.Y;
            notificationnum++;
        }

        private void marketlocbtn_Click(object sender, EventArgs e)
        {
            if (market)
            {
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(Eventmarket);
                market = false;
            }
            else
            {
                MouseHook.stop();
            }
               
         }

        private void notificationlocbtn_Click(object sender, EventArgs e)
        {
            if (notification)
            {
                MouseHook.Start();
                MouseHook.MouseAction += new EventHandler(Eventnotification);
                notification = false;
            }
            else
            {
                MouseHook.stop();
            }
        }
           
        
    }
     public static class MouseHook
    {
        public static event EventHandler MouseAction = delegate { };

        public static void Start()
        {
                _hookID = SetHook(_proc);
        }

        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_MOUSE_LL, proc,
                      GetModuleHandle(curModule.ModuleName), 0);
                }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
                if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
                {       
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    MouseAction(null, new EventArgs());
                }

                    return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }

     static class SampleMouseMove
     {

         static Random random = new Random();
         static int mouseSpeed = 35;


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
