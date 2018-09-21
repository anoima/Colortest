using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Optionsform : Form
    {
        public int rndmin;
        public int rndmax;
        int[,] marketloc = new int[10, 2];
        int[,] notificationloc = new int[5, 2];
        int marketnum = 0;
        int notificationnum = 0;
        Boolean market = true;
        Boolean notification = true;
        public Optionsform()
        {

            InitializeComponent();
            getoptions();
            addvalues();
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

        private void setmarketbtn_Click(object sender, EventArgs e)
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

        private void setnotificationbtn_Click(object sender, EventArgs e)
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

        public void setoptions()
        {
            StringBuilder newFile = new StringBuilder();

            string temp = "";

            string[] file = File.ReadAllLines(@".\Options.txt");

            foreach (string line in file)
            {
                switch (line[0])
                {
                    case '1':
                        {
                            temp = line.Replace(line, "1   " + getnum(notwaitmin.Text.ToString()));
                            newFile.Append(temp + "\r\n");
                            newFile.Append(line + "\r\n");
                        } break;
                    case '2':
                        {
                            temp = line.Replace(line, "1   " + getnum(notwaitmax.Text.ToString()));
                            newFile.Append(temp + "\r\n");
                            newFile.Append(line + "\r\n");
                        } break;
                }
                      
            }
          File.WriteAllText(@".\Options.txt", newFile.ToString());
        }

        public int getnum(string numstring) {
            string s=numstring.Replace("s", "-");
            string ss=s.Replace("m", "-");
            string[] nn=ss.Split('-');
            int k = 0;
            if (nn.Length==3)
            {
                k = (Convert.ToInt32(nn[0]) * 60000) + (Convert.ToInt32(nn[1])*1000);
            }
            else
            {
                k = (Convert.ToInt32(nn[0]) * 1000);
            }
           
            return k;
        }


        public void addvalues()
        {
            Object[] row;
            dataGridView1.ColumnCount = 2;
            row = new Object[] { "After Notification Min", rndmin / 1000 + " Sec" };
            dataGridView1.Rows.Add(row);
            row = new Object[] { "After Notification Max", rndmax / 1000 + " Sec" };
            dataGridView1.Rows.Add(row);
            row = new Object[] { "Market Point up-left", marketloc[0, 0] + "." + marketloc[0, 1] };
            dataGridView1.Rows.Add(row);
            row = new Object[] { "Market Point down-right", marketloc[1, 0] + "." + marketloc[1, 1] };
            dataGridView1.Rows.Add(row);
            row = new Object[] { "Notification Point up-left", notificationloc[0, 0] + "." + notificationloc[0, 1] };
            dataGridView1.Rows.Add(row);
            row = new Object[] { "Notification Point down-right", notificationloc[1, 0] + "." + notificationloc[1, 1] };
            dataGridView1.Rows.Add(row);
            for (int i = 2; i <= 130; i += 2)
            {
                if (i >= 60)
                {
                    notwaitmin.Items.Add(i / 60 + "m " + i % 60 + "s");
                    notwaitmax.Items.Add(i / 60 + "m " + i % 60 + "s");
                    i += 3;
                }
                else
                {
                    notwaitmin.Items.Add(i+"s");
                    notwaitmax.Items.Add(i+"s");
                }
            }
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notwaitmax_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notwaitmax.SelectedIndex <= notwaitmin.SelectedIndex)
            {
                notwaitmax.SelectedIndex = notwaitmin.SelectedIndex + 1;
                MessageBox.Show("A Notification max nem lehet kissebb mint a notification min.");
            }
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            setoptions();
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
}
