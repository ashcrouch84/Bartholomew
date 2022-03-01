using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Web;
using System.Net;
using System.IO;

namespace Bartholomew
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
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(@"http://www.4k-photos.co.uk/download_bookings1.php");
            using (HttpWebResponse resp = (HttpWebResponse) wr.GetResponse())
            {
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string val = sr.ReadToEnd();
                MessageBox.Show(val);
                textBox1.Text = val;
            }
        }
    }
}
