using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }



        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string code;
            string s;
            string line;
            string[] val = new string[] { "" };
            bool k = false;
            bool f = false;
            bool m = false;
            for (int x = 1; x < 100000; x++)
            {
                string urlDemo = "https://bddatabase.net/us/item/" + x + "/";
                WebRequest request = WebRequest.Create(urlDemo);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //string m = reader.ReadToEnd();
                //File.AppendAllText(@"U:\lo.txt",m + Environment.NewLine);
                f = false;
                for (int i = 0; i < 11; i++)
                {
                    line = reader.ReadLine();
                    if (line.Contains("meta name=\"description\""))
                    {
                        f = true;
                        string toBeSearched = "Id: ";
                        int ix = line.IndexOf(toBeSearched);

                        if (ix != -1)
                        {
                            code = line.Substring(ix + toBeSearched.Length);
                            s = code.Substring(0, code.IndexOf("."));
                            val = s.Split(new string[] { "-" }, StringSplitOptions.None);

                        }
                    }
                    else
                    {
                        k = true;
                    }
                }
                if (f)
                {
                    k = false;
                    for (int i = 10; i < 700; i++)
                    {
                            line = reader.ReadLine();
                            if (line.Contains("Bound"))
                            {
                                k = true;
                            }
                            if (line.Contains("Market Price"))
                            {
                                m = true;
                            }
                    }
                }



                if (!k && !m)
                {// "ID: " + val[0] + "   Name: " + 
                    File.AppendAllText(@"U:\idname.txt",val[1] + Environment.NewLine);
                }

                k = false;
                m = false;
                reader.Close();
                response.Close();
                label1.Text = x.ToString();
                label1.Refresh();
            }
        }
    }
}
