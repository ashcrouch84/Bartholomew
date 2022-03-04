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
using Renci.SshNet;

namespace Bartholomew
{
    public partial class Form1 : Form
    {
        int i,j;
        List<string> ftp_list = new List<string>();
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
            //load backup location
            txtBackup.Text = Properties.Settings.Default.bBackupLocation.ToString();
        }

        private void loadFTP()
        {
            txtUsername.Text = Properties.Settings.Default.bFTPUsername.ToString();
            txtPort.Text = Properties.Settings.Default.bFTPPort.ToString();
            txtHost.Text = Properties.Settings.Default.bFTPHost.ToString();
            txtPassword.Text = Properties.Settings.Default.bFTPPassword.ToString();

            txtFTPAdult.Text = Properties.Settings.Default.bFTPAdult.ToString();
            txtFTPChild.Text = Properties.Settings.Default.bFTPChild.ToString();
            txtFTPFamily.Text = Properties.Settings.Default.bFTPFamily.ToString();
            txtFTPFamily.Text = Properties.Settings.Default.bFTPFolder.ToString();
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

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            retrieveBookings();
        }

        private void retrieveBookings()
        {

        }

        private void cmdDownloadBackup_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to download all information from server? This may take some time and is only advised before or after opening hours or during longer breaks.", "Download all information?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                downloadBackup();
            }
        }

        private void downloadBackup()
        {
            tabControl2.Visible = false;
            cmdCheckPassword.Enabled = true;
            txtPassword.Enabled = true;
           
            string c, b;
            //access server
            string Host = Properties.Settings.Default.bFTPHost.ToString();
            int Port = Int32.Parse(Properties.Settings.Default.bFTPPort.ToString());
            string Username = Properties.Settings.Default.bFTPUsername.ToString();
            string Password = Properties.Settings.Default.bFTPPassword.ToString();
            string strAdult = Properties.Settings.Default.bFTPAdult.ToString();
            string strChild = Properties.Settings.Default.bFTPChild.ToString();
            string strFamily = Properties.Settings.Default.bFTPFamily.ToString();
            string strFolder = Properties.Settings.Default.bFTPFolder.ToString();

            ftp_list.Clear();
            using (var sftp = new SftpClient(Host, Port, Username, Password))
            {
                sftp.Connect(); //connect to server

                //download questions
                string strRemoteFolder = Properties.Settings.Default.bFTPFolder.ToString() + "//Questions.txt";
                string strLocalFolder = Properties.Settings.Default.bBackupLocation.ToString() + "//Questions.txt";

                using (var file = File.OpenWrite(strLocalFolder))
                {
                    sftp.DownloadFile(strRemoteFolder, file);//download file
                }

                //count files in different folders
                string strRemoteFolderC = Properties.Settings.Default.bFTPChild.ToString();
                List<string> child_list = new List<string>();
                var toReturn = sftp.ListDirectory(strRemoteFolderC).ToList(); //download a list of files on server

                string strRemoteFolderA = Properties.Settings.Default.bFTPAdult.ToString();
                List<string> adult_list = new List<string>();
                var toReturn1 = sftp.ListDirectory(strRemoteFolderA).ToList(); //download a list of files on server

                string strRemoteFolderF = Properties.Settings.Default.bFTPFamily.ToString();
                List<string> family_list = new List<string>();
                var toReturn2 = sftp.ListDirectory(strRemoteFolderF).ToList(); //download a list of files on server

                //progress bar maths

                int pTotal = toReturn.Count + toReturn1.Count + toReturn2.Count;
                int pAmount = Convert.ToInt32(pTotal / 100);

                child_list = sftp.ListDirectory(strRemoteFolderC).Where(f => !f.IsDirectory).Select(f => f.Name).ToList();
                adult_list = sftp.ListDirectory(strRemoteFolderA).Where(f => !f.IsDirectory).Select(f => f.Name).ToList();
                family_list = sftp.ListDirectory(strRemoteFolderF).Where(f => !f.IsDirectory).Select(f => f.Name).ToList();

                try
                {
                    //download child folders
                    i = 0;
                    while (i < child_list.Count) //cycle through list of files
                    {
                        c = Properties.Settings.Default.bFTPChild + "/" + child_list[i]; //update download file from sftp
                        b = Properties.Settings.Default.bBackupLocation + "\\" + child_list[i];//update download folder to pc 
                        using (var file = File.OpenWrite(b))
                        {
                            sftp.DownloadFile(c, file);//download file
                        }

                        i = i + 1;//next
                    }

                    //download adult folders
                    i = 0;
                    while (i < adult_list.Count) //cycle through list of files
                    {
                        c = Properties.Settings.Default.bFTPChild + "/" + adult_list[i]; //update download file from sftp
                        b = Properties.Settings.Default.bBackupLocation + "\\" + adult_list[i];//update download folder to pc 
                        try
                        {
                            using (var file = File.OpenWrite(b))
                            {
                                sftp.DownloadFile(c, file);//download file
                            }
                        }
                        catch
                        { }
                        i = i + 1;//next
                    }

                    //download family folders
                    i = 0;
                    while (i < family_list.Count) //cycle through list of files
                    {
                        c = Properties.Settings.Default.bFTPFamily + "/" + family_list[i]; //update download file from sftp
                        b = Properties.Settings.Default.bBackupLocation + "\\" + family_list[i];//update download folder to pc 
                        try
                        {
                            using (var file = File.OpenWrite(b))
                            {
                                sftp.DownloadFile(c, file);//download file
                            }
                        }
                        catch
                        { }
                        i = i + 1;//next
                    }

                    MessageBox.Show("Download completed successfully", "Download finished", MessageBoxButtons.OK);
                }
                catch
                {
                    MessageBox.Show("Download Failed, please check the setup folders and try again", "Failed to download", MessageBoxButtons.OK);
                }
            }
        }

        private void cmdFTPSave_Click(object sender, EventArgs e)
        {
            string Host = txtHost.Text;
            int Port = Int32.Parse(txtPort.Text); 
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;
            bool success = false;

            try
            {
                using (var sftp = new SftpClient(Host, Port, Username, Password))
                {
                    sftp.Connect();
                    success = true;
                    sftp.Disconnect();
                }
            }
            catch
            {
                MessageBox.Show("FTP site couldn't be reached with new credentials, please reneter. These details have not been saved","FTP Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                success = false;
            }

            if (success==true)
            {
                Properties.Settings.Default.bFTPHost=txtHost.Text;
                Properties.Settings.Default.bFTPPassword=txtPassword.Text;
                Properties.Settings.Default.bFTPUsername=txtUsername.Text;
                Properties.Settings.Default.bFTPPort = Int32.Parse(txtPort.Text);

                Properties.Settings.Default.bFTPAdult = txtFTPAdult.Text;
                Properties.Settings.Default.bFTPChild = txtFTPChild.Text;
                Properties.Settings.Default.bFTPFamily = txtFTPFamily.Text;
                Properties.Settings.Default.bFTPFolder = txtFTPFolder.Text;

                Properties.Settings.Default.Save();

            }
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
