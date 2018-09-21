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

namespace WindowsFormsApplication7
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
            string code;
            string s;
            string[] val = new string[] {""};
            bool lcb = false;
            for (int x = 1; x < 50000; x++)
            {
                string urlDemo = "https://bddatabase.net/us/item/"+x+"/";
                WebRequest request = WebRequest.Create(urlDemo);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                //STATUS
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string line;
                string kk = "meta name=\"description\"";
                //string m = reader.ReadToEnd();
                //File.AppendAllText(@"U:\id.txt", m);
                for (int i = 0; i < 800; i++)
                {
                    line = reader.ReadLine();
                    if (i<=10)
                    {
                        if (line.Contains(kk))
                        {
                            string toBeSearched = "Id: ";
                            int ix = line.IndexOf(toBeSearched);

                            if (ix != -1)
                            {
                                code = line.Substring(ix + toBeSearched.Length);
                                s = code.Substring(0, code.IndexOf("."));
                                val = s.Split(new string[] { "-" }, StringSplitOptions.None);
                               
                            }
                        }
                    }
                    if (i >= 500 && i <= 800)
                    {
                            if (line.Contains("Bound"))
                            {
                                lcb = true;
                            }
                    }
                  

                }
                if (!lcb)
                {
                    File.AppendAllText(@"U:\idname.txt", "ID: " + val[0] + "   Name: " + val[1] + Environment.NewLine);
                    File.AppendAllText(@"U:\id.txt", val[0] + Environment.NewLine);
                    File.AppendAllText(@"U:\name.txt", val[1] + Environment.NewLine);
                }
                lcb = false;
                reader.Close();
                response.Close();

            }
        }
    }
}
