using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logicgame
{
    public partial class Minigame : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private Point upleft = new Point(40, 40);
        private Point downright = new Point(900, 730);
        private Point minmaxul = new Point(40, 40);
        private Point minmaxdr = new Point(900, 730);
        private Bitmap item = new Bitmap(@".\item.png");
        private Bitmap minxmaxpng = new Bitmap(@".\min.png");
        private static PointF rloc = new PointF(400, 300);
        private Point b1 = new Point(600, 150);
        private static Random rnd = new Random();

        public Minigame()
        {
            InitializeComponent();
            pictureBox1.Image = item;
        }

        private void startbtn_Click(object sender, EventArgs e)
        {
            fgg();
        }

        private Bitmap Game()
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
            Bitmap CroppedImage = screenshot.Clone(new System.Drawing.Rectangle(upleft.X,upleft.Y,downright.X,downright.Y), screenshot.PixelFormat);
            screenshot.Dispose();
            screenGraph.Dispose();
            return CroppedImage;
        }

        private Bitmap minmax()
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
            Bitmap CroppedImage = screenshot.Clone(new System.Drawing.Rectangle(minmaxul.X, minmaxul.Y, minmaxdr.X, minmaxdr.Y), screenshot.PixelFormat);
            screenshot.Dispose();
            screenGraph.Dispose();
            return CroppedImage;
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
            for (int i = 0; i < length - firstStart; ++i)
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

        private void fgg()
        {
            Point k;
            Point m;
            Bitmap bm;
            Point o;

            do
            {
                int num = 0;
                GC.Collect();
                moveclick(rloc);
                System.Threading.Thread.Sleep(rnd.Next(400, 700));
                bm = Game();
                //k = (Point)Find(bm, item);
                bool l = FindBitmap(bm, item,out k);
                bm.Dispose();
                if (k != new Point(0,0))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        o = new Point((b1.X+rnd.Next(-10,10)), (b1.Y + num+rnd.Next(-5,5)));
                        moveclick(o);
                        num += 30;
                        bool l = FindBitmap(bm, minxmaxpng, out m);
                        if (m != new Point(0,0))
                        {
                            //max loc
                             moveclick(o);
                             System.Threading.Thread.Sleep(rnd.Next(300, 500));

                        }
                        System.Threading.Thread.Sleep(rnd.Next(500, 1200));
                    }
                    System.Threading.Thread.Sleep(rnd.Next(700, 1200));
                }
                System.Threading.Thread.Sleep(rnd.Next(2000, 2400));
            } while (true);
        }
        private static void moveclick(PointF p)
        {
            SampleMouseMove.MoveMouse((int)p.X + (rnd.Next(-1, 1)), (int)p.Y + (rnd.Next(-1, 1)), 4, 4);
            System.Threading.Thread.Sleep(rnd.Next(300, 600));
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        private static bool IsInnerImage(Bitmap searchBitmap, Bitmap withinBitmap, int left, int top)
        {
            for (int y = top; y < top + withinBitmap.Height; y++)
            {
                for (int x = left; x < left + withinBitmap.Width; x++)
                {
                    if (searchBitmap.GetPixel(x, y) != withinBitmap.GetPixel(x - left, y - top))
                        return false;
                }
            }

            return true;
        }

        private static bool FindBitmap(Bitmap searchBitmap, Bitmap withinBitmap, out Point point)
        {
            Color innerTopLeft = withinBitmap.GetPixel(0, 0);

            for (int y = 0; y < searchBitmap.Height - withinBitmap.Height; y++)
            {
                for (int x = 0; x < searchBitmap.Width - withinBitmap.Width; x++)
                {
                    Color clr = searchBitmap.GetPixel(x, y);
                    if (innerTopLeft == clr && IsInnerImage(searchBitmap, withinBitmap, x, y))
                    {
                        point = new Point(x, y); // Top left corner of the inner bitmap in searchBitmap - coordinates
                        return true;
                    }
                }
            }

            point = Point.Empty;
            return false;
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
