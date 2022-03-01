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
        int i,j;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadSettings();
            gbHidden.Visible = false;
        }

        private void loadSettings()
        {
            //add dates to combo boxes
            i = 1;
            while (i < 32)
            {
                cboDay.Items.Add(i.ToString());
                i = i + 1;
            }
            i = 1;
            while (i<12)
            {
                cboMonth.Items.Add(i.ToString());
                i=i + 1;
            }
            i = 2021;
            while (i<2025)
            {
                cboYear.Items.Add(i.ToString());
                i= i + 1;
            }
            //retrieved saved values and put them in combo boxes
            cboDay.Text = Properties.Settings.Default.bDay.ToString();
            cboMonth.Text = Properties.Settings.Default.bMonth.ToString();
            cboYear.Text = Properties.Settings.Default.bYear.ToString();
            //add times to time combo box
            i = 8;
            string strTime;
            while (i<20)
            {
                j = 0;
                while (j<70)
                {
                    if (j == 0)
                    {
                        strTime = i.ToString() + ":00";
                    }
                    else
                    {
                        strTime = i.ToString() + ":" + j.ToString();
                    }
                    j = j + 10;
                }
                i = i + 1;
            }
            //retrieved saved time and add it to combo box
            cboTime.Text = Properties.Settings.Default.bTime.ToString();
            //load program properties
            loadSavedInfo();
            //load ftp properties
            loadFTP();
        }

        private void loadFTP()
        {
            txtUsername.Text = Properties.Settings.Default.bUsername.ToString();
            txtPort.Text = Properties.Settings.Default.bPort.ToString();
            txtHost.Text = Properties.Settings.Default.bHost.ToString();
            txtPassword.Text = Properties.Settings.Default.bPassword.ToString();
        }

        private void loadSavedInfo()
        {
            //load properties such as name
            txtUsed.Text = Properties.Settings.Default.bUsed.ToString();
            //load saved christmas settings
            chkSavedChristmas.Checked = Properties.Settings.Default.bSavedBox;
            this.Text = txtUsed.Text;
            if (chkSavedChristmas.Checked == true)
            {
                gbSaved.Visible = true;
            }
            else
            {
                gbSaved.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void cmdHidden_Click(object sender, EventArgs e)
        {
            if (cmdHidden.Text=="Show Hidden Info")
            {
                cmdHidden.Text = "Hide Hidden Info";
                gbHidden.Visible = true;
            }
            else
            {
                cmdHidden.Text = "Show Hidden Info";
                gbHidden.Visible= false;
            }
        }

        private void cmdCancelUsed_Click(object sender, EventArgs e)
        {
            loadSavedInfo();
        }

        private void cmdUsedSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.bUsed = txtUsed.Text;
            Properties.Settings.Default.bSavedBox = chkSavedChristmas.Checked;
            Properties.Settings.Default.Save();
            loadSavedInfo();
        }

        private void cmdFTPCancel_Click(object sender, EventArgs e)
        {
            loadFTP();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(@"http://www.4k-photos.co.uk/download_bookings1.php");
            using (HttpWebResponse resp = (HttpWebResponse)wr.GetResponse())
            {
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string val = sr.ReadToEnd();
                MessageBox.Show(val);
                textBox1.Text = val;
            }
        }
    }
}
