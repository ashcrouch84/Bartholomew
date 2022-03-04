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
using System.Collections.Specialized;

namespace Bartholomew
{
    public partial class Form1 : Form
    {
        int i,j,intList;
        int intTime1, intTime2;
        List<string> ftp_list = new List<string>();
        List<string> lstRef = new List<string>();
        List<string> lstFamilyNames = new List<string>();
        List<string> lstCount = new List<string>();
        List<string> lstQuestions = new List<string>();
        List<string> lstAll = new List<string>();
        List<Button> buttonsAdded = new List<Button>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadSettings();
            gbHidden.Visible = false;
            loadQuestions();
        }

        private void loadQuestions()
        {
            //load question choices
            cboQuestionDisplays.Items.Clear();
            cboQuestionDisplays.Items.Add("Bartholomew");
            cboQuestionDisplays.Items.Add("Reindeers");
            cboQuestionDisplays.Items.Add("Santa");
            cboQuestionDisplays.Items.Add("Toy");
            cboQuestionDisplays.SelectedIndex = Properties.Settings.Default.bQuestionChoice;
            //the question.txt file has a properties for each area above which needs to be set for each area
            //set in order above
            int intChoice = cboQuestionDisplays.SelectedIndex + 6;

            try
            {
                string Host = Properties.Settings.Default.bFTPHost.ToString();
                int Port = Int32.Parse(Properties.Settings.Default.bFTPPort.ToString());
                string Username = Properties.Settings.Default.bFTPUsername.ToString();
                string Password = Properties.Settings.Default.bFTPPassword.ToString();
                ftp_list.Clear();
                List<string> list = new List<string>();

                //download questions
                string strRemoteFolder = Properties.Settings.Default.bFTPFolder.ToString() + "//Questions.txt";
                string strLocalFolder = Properties.Settings.Default.bBackupLocation.ToString() + "//Questions.txt";
                using (var sftp = new SftpClient(Host, Port, Username, Password))
                {
                    sftp.Connect(); //connect to server

                    using (var file = File.OpenWrite(strLocalFolder))
                    {
                        sftp.DownloadFile(strRemoteFolder, file);//download file
                    }

                    sftp.Disconnect();
                }

                //read downloaded questions file
                var fileStream = new FileStream(strLocalFolder, FileMode.Open, System.IO.FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                fileStream.Close();
                i = 0;
                lstChildHideQuestions.Items.Clear();
                lstChildShowQuestions.Items.Clear();
                lstAdultHideQuestions.Items.Clear();
                lstAdultShowQuestions.Items.Clear();
                lstFamilyHideQuestions.Items.Clear();
                lstFamilyShowQuestions.Items.Clear();
                lstChildHideQuestionsID.Items.Clear();
                lstChildShowQuestionsID.Items.Clear();
                lstAdultHideQuestionsID.Items.Clear();
                lstAdultShowQuestionsID.Items.Clear();
                lstFamilyHideQuestionsID.Items.Clear();
                lstFamilyShowQuestionsID.Items.Clear();

                while (i < list.Count)
                {
                    List<string> questionsSplit = list[i].ToString().Split(',').ToList<string>();
                    if (string.Join("", questionsSplit[4].Split('"')) == "Child")
                    {
                        if (string.Join("", questionsSplit[intChoice].Split('"')) == "Show")
                        {
                            lstChildShowQuestions.Items.Add(questionsSplit[1].Replace("\"",""));
                            lstChildShowQuestionsID.Items.Add(questionsSplit[0].Replace("\"", "").ToString());
                        }
                        else
                        {
                            lstChildHideQuestions.Items.Add(questionsSplit[1].Replace("\"", ""));
                            lstChildHideQuestionsID.Items.Add(questionsSplit[0].ToString().Replace("\"", ""));
                        }
                    }
                    if (string.Join("", questionsSplit[4].Split('"')) == "Adult")
                    {
                        if (string.Join("", questionsSplit[intChoice].Split('"')) == "Show")
                        {
                            lstAdultShowQuestions.Items.Add(questionsSplit[1].Replace("\"", ""));
                            lstAdultShowQuestionsID.Items.Add(questionsSplit[0].ToString().Replace("\"", ""));
                        }
                        else
                        {
                            lstAdultHideQuestions.Items.Add(questionsSplit[1].Replace("\"", ""));
                            lstAdultHideQuestionsID.Items.Add(questionsSplit[0].ToString().Replace("\"", ""));
                        }
                    }
                    if (string.Join("", questionsSplit[4].Split('"')) == "Family")
                    {
                        if (string.Join("", questionsSplit[intChoice].Split('"')) == "Show")
                        {
                            lstFamilyShowQuestions.Items.Add(questionsSplit[1].Replace("\"", ""));
                            lstFamilyHideQuestionsID.Items.Add(questionsSplit[0].ToString().Replace("\"", "")); 
                        }
                        else
                        {
                            lstFamilyHideQuestions.Items.Add(questionsSplit[1].Replace("\"", ""));
                            lstFamilyHideQuestionsID.Items.Add(questionsSplit[0].ToString().Replace("\"", ""));
                        }
                    }
                    lstQuestions.Add(list[i].ToString());
                    i = i + 1;
                }

                try { lblChildShow1.Text = lstChildShowQuestions.Items[0].ToString(); lblChildShow1.Visible = Visible; lstChildShow1.Visible = Visible; } catch { lblChildShow1.Text = ""; lblChildShow1.Visible = false; lstChildShow1.Visible = false; }
                try { lblChildShow2.Text = lstChildShowQuestions.Items[1].ToString(); lblChildShow2.Visible = Visible; lstChildShow2.Visible = Visible; } catch { lblChildShow2.Text = ""; lblChildShow2.Visible = false; lstChildShow2.Visible = false; }
                try { lblChildShow3.Text = lstChildShowQuestions.Items[2].ToString(); lblChildShow3.Visible = Visible; lstChildShow3.Visible = Visible; } catch { lblChildShow3.Text = ""; lblChildShow3.Visible = false; lstChildShow3.Visible = false; }
                try { lblChildShow4.Text = lstChildShowQuestions.Items[3].ToString(); lblChildShow4.Visible = Visible; lstChildShow4.Visible = Visible; } catch { lblChildShow4.Text = ""; lblChildShow4.Visible = false; lstChildShow4.Visible = false; }
                try { lblChildShow5.Text = lstChildShowQuestions.Items[4].ToString(); lblChildShow5.Visible = Visible; lstChildShow5.Visible = Visible; } catch { lblChildShow5.Text = ""; lblChildShow5.Visible = false; lstChildShow5.Visible = false; }
                try { lblChildShow6.Text = lstChildShowQuestions.Items[5].ToString(); lblChildShow6.Visible = Visible; lstChildShow6.Visible = Visible; } catch { lblChildShow6.Text = ""; lblChildShow6.Visible = false; lstChildShow6.Visible = false; }
                try { lblChildShow7.Text = lstChildShowQuestions.Items[6].ToString(); lblChildShow7.Visible = Visible; lstChildShow7.Visible = Visible; } catch { lblChildShow7.Text = ""; lblChildShow7.Visible = false; lstChildShow7.Visible = false; }
                try { lblChildShow8.Text = lstChildShowQuestions.Items[7].ToString(); lblChildShow8.Visible = Visible; lstChildShow8.Visible = Visible; } catch { lblChildShow8.Text = ""; lblChildShow8.Visible = false; lstChildShow8.Visible = false; }
                try { lblChildShow9.Text = lstChildShowQuestions.Items[8].ToString(); lblChildShow9.Visible = Visible; lstChildShow9.Visible = Visible; } catch { lblChildShow9.Text = ""; lblChildShow9.Visible = false; lstChildShow9.Visible = false; }
                try { lblChildShow10.Text = lstChildShowQuestions.Items[9].ToString(); lblChildShow10.Visible = Visible; lstChildShow10.Visible = Visible; } catch { lblChildShow10.Text = ""; lblChildShow10.Visible = false; lstChildShow10.Visible = false; }

                try { lblAdultShow1.Text = lstAdultShowQuestions.Items[0].ToString(); lblAdultShow1.Visible = Visible; lstAdultShow1.Visible = Visible; } catch { lblAdultShow1.Text = ""; lblAdultShow1.Visible = false; lstAdultShow1.Visible = false; }
                try { lblAdultShow2.Text = lstAdultShowQuestions.Items[1].ToString(); lblAdultShow2.Visible = Visible; lstAdultShow2.Visible = Visible; } catch { lblAdultShow2.Text = ""; lblAdultShow2.Visible = false; lstAdultShow2.Visible = false; }
                try { lblAdultShow3.Text = lstAdultShowQuestions.Items[2].ToString(); lblAdultShow3.Visible = Visible; lstAdultShow3.Visible = Visible; } catch { lblAdultShow3.Text = ""; lblAdultShow3.Visible = false; lstAdultShow3.Visible = false; }
                try { lblAdultShow4.Text = lstAdultShowQuestions.Items[3].ToString(); lblAdultShow4.Visible = Visible; lstAdultShow4.Visible = Visible; } catch { lblAdultShow4.Text = ""; lblAdultShow4.Visible = false; lstAdultShow4.Visible = false; }
                try { lblAdultShow5.Text = lstAdultShowQuestions.Items[4].ToString(); lblAdultShow5.Visible = Visible; lstAdultShow5.Visible = Visible; } catch { lblAdultShow5.Text = ""; lblAdultShow5.Visible = false; lstAdultShow5.Visible = false; }
                try { lblAdultShow6.Text = lstAdultShowQuestions.Items[5].ToString(); lblAdultShow6.Visible = Visible; lstAdultShow6.Visible = Visible; } catch { lblAdultShow6.Text = ""; lblAdultShow6.Visible = false; lstAdultShow6.Visible = false; }
                try { lblAdultShow7.Text = lstAdultShowQuestions.Items[6].ToString(); lblAdultShow7.Visible = Visible; lstAdultShow7.Visible = Visible; } catch { lblAdultShow7.Text = ""; lblAdultShow7.Visible = false; lstAdultShow7.Visible = false; }
                try { lblAdultShow8.Text = lstAdultShowQuestions.Items[7].ToString(); lblAdultShow8.Visible = Visible; lstAdultShow8.Visible = Visible; } catch { lblAdultShow8.Text = ""; lblAdultShow8.Visible = false; lstAdultShow8.Visible = false; }
                try { lblAdultShow9.Text = lstAdultShowQuestions.Items[8].ToString(); lblAdultShow9.Visible = Visible; lstAdultShow9.Visible = Visible; } catch { lblAdultShow9.Text = ""; lblAdultShow9.Visible = false; lstAdultShow9.Visible = false; }
                try { lblAdultShow10.Text = lstAdultShowQuestions.Items[9].ToString(); lblAdultShow10.Visible = Visible; lstAdultShow10.Visible = Visible; } catch { lblAdultShow10.Text = ""; lblAdultShow10.Visible = false; lstAdultShow10.Visible = false; }

                try { lblFamilyShow1.Text = lstFamilyShowQuestions.Items[0].ToString(); lblFamilyShow1.Visible = Visible; lstFamilyShow1.Visible = Visible; } catch { lblFamilyShow1.Text = ""; lblFamilyShow1.Visible = false; lstFamilyShow1.Visible = false; }
                try { lblFamilyShow2.Text = lstFamilyShowQuestions.Items[1].ToString(); lblFamilyShow2.Visible = Visible; lstFamilyShow2.Visible = Visible; } catch { lblFamilyShow2.Text = ""; lblFamilyShow2.Visible = false; lstFamilyShow2.Visible = false; }
                try { lblFamilyShow3.Text = lstFamilyShowQuestions.Items[2].ToString(); lblFamilyShow3.Visible = Visible; lstFamilyShow3.Visible = Visible; } catch { lblFamilyShow3.Text = ""; lblFamilyShow3.Visible = false; lstFamilyShow3.Visible = false; }
                try { lblFamilyShow4.Text = lstFamilyShowQuestions.Items[3].ToString(); lblFamilyShow4.Visible = Visible; lstFamilyShow4.Visible = Visible; } catch { lblFamilyShow4.Text = ""; lblFamilyShow4.Visible = false; lstFamilyShow4.Visible = false; }
                try { lblFamilyShow5.Text = lstFamilyShowQuestions.Items[4].ToString(); lblFamilyShow5.Visible = Visible; lstFamilyShow5.Visible = Visible; } catch { lblFamilyShow5.Text = ""; lblFamilyShow5.Visible = false; lstFamilyShow5.Visible = false; }
                try { lblFamilyShow6.Text = lstFamilyShowQuestions.Items[5].ToString(); lblFamilyShow6.Visible = Visible; lstFamilyShow6.Visible = Visible; } catch { lblFamilyShow6.Text = ""; lblFamilyShow6.Visible = false; lstFamilyShow6.Visible = false; }
                try { lblFamilyShow7.Text = lstFamilyShowQuestions.Items[6].ToString(); lblFamilyShow7.Visible = Visible; lstFamilyShow7.Visible = Visible; } catch { lblFamilyShow7.Text = ""; lblFamilyShow7.Visible = false; lstFamilyShow7.Visible = false; }
                try { lblFamilyShow8.Text = lstFamilyShowQuestions.Items[7].ToString(); lblFamilyShow8.Visible = Visible; lstFamilyShow8.Visible = Visible; } catch { lblFamilyShow8.Text = ""; lblFamilyShow8.Visible = false; lstFamilyShow8.Visible = false; }
                try { lblFamilyShow9.Text = lstFamilyShowQuestions.Items[8].ToString(); lblFamilyShow9.Visible = Visible; lstFamilyShow9.Visible = Visible; } catch { lblFamilyShow9.Text = ""; lblFamilyShow9.Visible = false; lstFamilyShow9.Visible = false; }
                try { lblFamilyShow10.Text = lstFamilyShowQuestions.Items[9].ToString(); lblFamilyShow10.Visible = Visible; lstFamilyShow10.Visible = Visible; } catch { lblFamilyShow10.Text = ""; lblFamilyShow10.Visible = false; lstFamilyShow10.Visible = false; }

                //try { lblChildHide1.Text = lstChildHideQuestions.Items[0].ToString(); lblChildHide1.Visible = Visible; lstChildHide1.Visible = Visible; } catch { lblChildHide1.Text = ""; lblChildHide1.Visible = false; lstChildHide1.Visible = false; }
                lblChildHide1.Text = "Childs Name";
                try { lblChildHide2.Text = lstChildHideQuestions.Items[1].ToString(); lblChildHide2.Visible = Visible; lstChildHide2.Visible = Visible; } catch { lblChildHide2.Text = ""; lblChildHide2.Visible = false; lstChildHide2.Visible = false; }
                try { lblChildHide3.Text = lstChildHideQuestions.Items[2].ToString(); lblChildHide3.Visible = Visible; lstChildHide3.Visible = Visible; } catch { lblChildHide3.Text = ""; lblChildHide3.Visible = false; lstChildHide3.Visible = false; }
                try { lblChildHide4.Text = lstChildHideQuestions.Items[3].ToString(); lblChildHide4.Visible = Visible; lstChildHide4.Visible = Visible; } catch { lblChildHide4.Text = ""; lblChildHide4.Visible = false; lstChildHide4.Visible = false; }
                try { lblChildHide5.Text = lstChildHideQuestions.Items[4].ToString(); lblChildHide5.Visible = Visible; lstChildHide5.Visible = Visible; } catch { lblChildHide5.Text = ""; lblChildHide5.Visible = false; lstChildHide5.Visible = false; }
                try { lblChildHide6.Text = lstChildHideQuestions.Items[5].ToString(); lblChildHide6.Visible = Visible; lstChildHide6.Visible = Visible; } catch { lblChildHide6.Text = ""; lblChildHide6.Visible = false; lstChildHide6.Visible = false; }
                try { lblChildHide7.Text = lstChildHideQuestions.Items[6].ToString(); lblChildHide7.Visible = Visible; lstChildHide7.Visible = Visible; } catch { lblChildHide7.Text = ""; lblChildHide7.Visible = false; lstChildHide7.Visible = false; }
                try { lblChildHide8.Text = lstChildHideQuestions.Items[7].ToString(); lblChildHide8.Visible = Visible; lstChildHide8.Visible = Visible; } catch { lblChildHide8.Text = ""; lblChildHide8.Visible = false; lstChildHide8.Visible = false; }
                try { lblChildHide9.Text = lstChildHideQuestions.Items[8].ToString(); lblChildHide9.Visible = Visible; lstChildHide9.Visible = Visible; } catch { lblChildHide9.Text = ""; lblChildHide9.Visible = false; lstChildHide9.Visible = false; }
                try { lblChildHide10.Text = lstChildHideQuestions.Items[9].ToString(); lblChildHide10.Visible = Visible; lstChildHide10.Visible = Visible; } catch { lblChildHide10.Text = ""; lblChildHide10.Visible = false; lstChildHide10.Visible = false; }

                lblAdultHide1.Text = "Adults Name";
                //try { lblAdultHide1.Text = lstAdultHideQuestions.Items[0].ToString(); lblAdultHide1.Visible = Visible; lstAdultHide1.Visible = Visible; } catch { lblAdultHide1.Text = ""; lblAdultHide1.Visible = false; lstAdultHide1.Visible = false; }
                try { lblAdultHide2.Text = lstAdultHideQuestions.Items[1].ToString(); lblAdultHide2.Visible = Visible; lstAdultHide2.Visible = Visible; } catch { lblAdultHide2.Text = ""; lblAdultHide2.Visible = false; lstAdultHide2.Visible = false; }
                try { lblAdultHide3.Text = lstAdultHideQuestions.Items[2].ToString(); lblAdultHide3.Visible = Visible; lstAdultHide3.Visible = Visible; } catch { lblAdultHide3.Text = ""; lblAdultHide3.Visible = false; lstAdultHide3.Visible = false; }
                try { lblAdultHide4.Text = lstAdultHideQuestions.Items[3].ToString(); lblAdultHide4.Visible = Visible; lstAdultHide4.Visible = Visible; } catch { lblAdultHide4.Text = ""; lblAdultHide4.Visible = false; lstAdultHide4.Visible = false; }
                try { lblAdultHide5.Text = lstAdultHideQuestions.Items[4].ToString(); lblAdultHide5.Visible = Visible; lstAdultHide5.Visible = Visible; } catch { lblAdultHide5.Text = ""; lblAdultHide5.Visible = false; lstAdultHide5.Visible = false; }
                try { lblAdultHide6.Text = lstAdultHideQuestions.Items[5].ToString(); lblAdultHide6.Visible = Visible; lstAdultHide6.Visible = Visible; } catch { lblAdultHide6.Text = ""; lblAdultHide6.Visible = false; lstAdultHide6.Visible = false; }
                try { lblAdultHide7.Text = lstAdultHideQuestions.Items[6].ToString(); lblAdultHide7.Visible = Visible; lstAdultHide7.Visible = Visible; } catch { lblAdultHide7.Text = ""; lblAdultHide7.Visible = false; lstAdultHide7.Visible = false; }
                try { lblAdultHide8.Text = lstAdultHideQuestions.Items[7].ToString(); lblAdultHide8.Visible = Visible; lstAdultHide8.Visible = Visible; } catch { lblAdultHide8.Text = ""; lblAdultHide8.Visible = false; lstAdultHide8.Visible = false; }
                try { lblAdultHide9.Text = lstAdultHideQuestions.Items[8].ToString(); lblAdultHide9.Visible = Visible; lstAdultHide9.Visible = Visible; } catch { lblAdultHide9.Text = ""; lblAdultHide9.Visible = false; lstAdultHide9.Visible = false; }
                try { lblAdultHide10.Text = lstAdultHideQuestions.Items[9].ToString(); lblAdultHide10.Visible = Visible; lstAdultHide10.Visible = Visible; } catch { lblAdultHide10.Text = ""; lblAdultHide10.Visible = false; lstAdultHide10.Visible = false; }

                try { lblFamilyHide1.Text = lstFamilyHideQuestions.Items[0].ToString(); lblFamilyHide1.Visible = Visible; lstFamilyHide1.Visible = Visible; } catch { lblFamilyHide1.Text = ""; lblFamilyHide1.Visible = false; lstFamilyHide1.Visible = false; }
                try { lblFamilyHide2.Text = lstFamilyHideQuestions.Items[1].ToString(); lblFamilyHide2.Visible = Visible; lstFamilyHide2.Visible = Visible; } catch { lblFamilyHide2.Text = ""; lblFamilyHide2.Visible = false; lstFamilyHide2.Visible = false; }
                try { lblFamilyHide3.Text = lstFamilyHideQuestions.Items[2].ToString(); lblFamilyHide3.Visible = Visible; lstFamilyHide3.Visible = Visible; } catch { lblFamilyHide3.Text = ""; lblFamilyHide3.Visible = false; lstFamilyHide3.Visible = false; }
                try { lblFamilyHide4.Text = lstFamilyHideQuestions.Items[3].ToString(); lblFamilyHide4.Visible = Visible; lstFamilyHide4.Visible = Visible; } catch { lblFamilyHide4.Text = ""; lblFamilyHide4.Visible = false; lstFamilyHide4.Visible = false; }
                try { lblFamilyHide5.Text = lstFamilyHideQuestions.Items[4].ToString(); lblFamilyHide5.Visible = Visible; lstFamilyHide5.Visible = Visible; } catch { lblFamilyHide5.Text = ""; lblFamilyHide5.Visible = false; lstFamilyHide5.Visible = false; }
                try { lblFamilyHide6.Text = lstFamilyHideQuestions.Items[5].ToString(); lblFamilyHide6.Visible = Visible; lstFamilyHide6.Visible = Visible; } catch { lblFamilyHide6.Text = ""; lblFamilyHide6.Visible = false; lstFamilyHide6.Visible = false; }
                try { lblFamilyHide7.Text = lstFamilyHideQuestions.Items[6].ToString(); lblFamilyHide7.Visible = Visible; lstFamilyHide7.Visible = Visible; } catch { lblFamilyHide7.Text = ""; lblFamilyHide7.Visible = false; lstFamilyHide7.Visible = false; }
                try { lblFamilyHide8.Text = lstFamilyHideQuestions.Items[7].ToString(); lblFamilyHide8.Visible = Visible; lstFamilyHide8.Visible = Visible; } catch { lblFamilyHide8.Text = ""; lblFamilyHide8.Visible = false; lstFamilyHide8.Visible = false; }
                try { lblFamilyHide9.Text = lstFamilyHideQuestions.Items[8].ToString(); lblFamilyHide9.Visible = Visible; lstFamilyHide9.Visible = Visible; } catch { lblFamilyHide9.Text = ""; lblFamilyHide9.Visible = false; lstFamilyHide9.Visible = false; }
                try { lblFamilyHide10.Text = lstFamilyHideQuestions.Items[9].ToString(); lblFamilyHide10.Visible = Visible; lstFamilyHide10.Visible = Visible; } catch { lblFamilyHide10.Text = ""; lblFamilyHide10.Visible = false; lstFamilyHide10.Visible = false; }

            }
            catch
            {

            }
        }

        private void loadSettings()
        {
            //add dates to combo boxes
            i = 1;
            while (i < 32)
            {
                cboDay.Items.Add(i.ToString());
                cboDay1.Items.Add(i.ToString());
                i = i + 1;
            }
            i = 1;
            while (i<13)
            {
                cboMonth.Items.Add(i.ToString());
                cboMonth1.Items.Add(i.ToString());
                i =i + 1;
            }
            i = 2021;
            while (i<2025)
            {
                cboYear.Items.Add(i.ToString());
                cboYear1.Items.Add(i.ToString());
                i = i + 1;
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
                while (j<60)
                {
                    if (j == 0)
                    {
                        strTime = i.ToString() + ":00";
                    }
                    else
                    {
                        strTime = i.ToString() + ":" + j.ToString();
                    }
                    cboTime.Items.Add(strTime);
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
            //local save location
            txtLocal.Text = Properties.Settings.Default.bLocalFolder.ToString();
            gbHidden.Visible = false;
            cmdSaved.Enabled = false;
            loadSavedXmas();
            loadBackups();
        }

        private void loadBackups()
        {
            List<string> strBackups = new List<string>();
            strBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
            txtNearby1.Text = strBackups[0];
            txtNearby2.Text = strBackups[1];
            txtNearby3.Text = strBackups[2];
            txtNearby4.Text = strBackups[3];
            txtNearby5.Text= strBackups[4];
            txtNearby6.Text = strBackups[5];
            txtNearby7.Text = strBackups[6];
            txtNearby8.Text = strBackups[7];
        }

        private void loadSavedXmas()
        {
            txtLocalSaveFolder.Text = Properties.Settings.Default.bSavedLocationLocal.ToString();
            txtFTPSaveFolder.Text = Properties.Settings.Default.bSavedLocationFTP.ToString();
            if (Properties.Settings.Default.bSavedType==0)
            {
                rbLocalSaved.Checked = true;
            }
            else
            {
                rbFTPSaved.Checked = true;
            }
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
            txtFTPFolder.Text = Properties.Settings.Default.bFTPFolder.ToString();
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
            cmdSaved.Enabled = false;
            gbHidden.Visible=false;
            cmdHidden.Text = "Show Hidden Info";
            lblReference.Text = "";
            lblFamily.Text = "";


            List<Button> buttons = gbFamilies.Controls.OfType<Button>().ToList();
            foreach (Button btn in buttons)
            {
                //btn.Click -= new EventHandler(this.b_Click); //It's unnecessary
                gbFamilies.Controls.Remove(btn);
                btn.Dispose();
            }
            clearListBoxes();
            retrieveBookings();
        }

        private void clearListBoxes()
        {
            lstAdultShow1.Items.Clear();
            lstAdultShow2.Items.Clear();
            lstAdultShow3.Items.Clear();
            lstAdultShow4.Items.Clear();
            lstAdultShow5.Items.Clear();
            lstAdultShow6.Items.Clear();
            lstAdultShow7.Items.Clear();
            lstAdultShow8.Items.Clear();
            lstAdultShow9.Items.Clear();
            lstAdultShow10.Items.Clear();
        
            lstChildShow1.Items.Clear();
            lstChildShow2.Items.Clear();
            lstChildShow3.Items.Clear();
            lstChildShow4.Items.Clear();
            lstChildShow5.Items.Clear();
            lstChildShow6.Items.Clear();
            lstChildShow7.Items.Clear();
            lstChildShow8.Items.Clear();
            lstChildShow9.Items.Clear();
            lstChildShow10.Items.Clear();
            
            lstFamilyShow1.Items.Clear();
            lstFamilyShow2.Items.Clear();
            lstFamilyShow3.Items.Clear();
            lstFamilyShow4.Items.Clear();
            lstFamilyShow5.Items.Clear();
            lstFamilyShow6.Items.Clear();
            lstFamilyShow7.Items.Clear();
            lstFamilyShow8.Items.Clear();
            lstFamilyShow9.Items.Clear();
            lstFamilyShow10.Items.Clear();
        }

        private void retrieveBookings()
        {
            bool dsuccess = false;
            //attempt to connect to fusemetrix and pull bookings from today
            //this is downloadBooking22.php found on sferver
            {
               

                try
                {
                    //retrieve list of bookings for this date and time
                    //send date and time to php code
                    string sTime = cboTime.Text;
                    string sDate = cboDay.Text + "/" + cboMonth.Text + "/" + cboYear.Text;
                    NameValueCollection nv = new NameValueCollection();
                    nv.Add("QTime", sTime);
                    nv.Add("QDate", sDate);

                    var url = "https://4k-photos.co.uk/downloadBooking22.php";

                    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Method = "POST";

                    httpRequest.ContentType = "application/x-www-form-urlencoded";

                    var data = "QDate=" + sDate + "&QTime=" + sTime;

                    //this sends the request for date and time to a php script on the server which in turn 
                    //creates a new file with the username date time and bookings1.txt
                    //eg 23/12/2021 10:00 would be 231220211000Bookings1.txt
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                    }

                    //open the txt file and read the contents to a list
                    var list = new List<string>();

                    //download new file
                    string Host = Properties.Settings.Default.bFTPHost.ToString();
                    int Port = Int32.Parse(Properties.Settings.Default.bFTPPort.ToString());
                    string Username = Properties.Settings.Default.bFTPUsername.ToString();
                    string Password = Properties.Settings.Default.bFTPPassword.ToString();
               

                    string strTime = sTime.Replace(":", "");
                    string strName = cboDay.Text + cboMonth.Text + cboYear.Text + strTime.ToString() + "Bookings1.txt";
                    string strRemoteFolder = strName;
                    string strLocalFolder = Properties.Settings.Default.bLocalFolder.ToString() + "\\" + strName.ToString();
                    using (var sftp = new SftpClient(Host, Port, Username, Password))
                    {
                        sftp.Connect();

                        //download new booking file
                        using (var file = File.OpenWrite(strLocalFolder))
                        {
                            sftp.DownloadFile(strRemoteFolder, file);//download file
                        }

                        sftp.Disconnect();
                    }
                    readThisTimesBookings();

                    dsuccess = true;
                }
                catch
                {
                 MessageBox.Show("Can't read FTP site, attempting to read local save","Can't connect to FTP",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    dsuccess = false;
                }
            }

            //if can't access the internet/connect to fusemetrix
            //read the file created today of all bookings
            if (dsuccess == false)
            {

                try
                {
                    //read saved file from today with this mornings data
                    var list = new List<string>();
                    string strLocalFolder = Properties.Settings.Default.bBackupLocation.ToString() + "//" + cboDay.Text + cboMonth.Text + cboYear.Text + "BookingsBart.txt";
                    var fileStream = new FileStream(strLocalFolder, FileMode.Open, System.IO.FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            list.Add(line);
                        }
                    }
                    fileStream.Close();

                    //read through file and find the time of this booking
                    string strTime1 = cboTime.Text;
                    string strTime2 = cboTime.Items[cboTime.SelectedIndex + 1].ToString();
                    //first time slot - actual booking
                    i = 0;
                    while (i < list.Count)
                    {
                        if (list[i].ToString() == strTime1.ToString())
                        {
                            intTime1 = i;
                            i = list.Count;
                        }
                        i = i + 1;
                    }
                    //second time slot - next booking
                    i = 0;
                    while (i < list.Count)
                    {
                        if (list[i].ToString() == strTime2.ToString())
                        {
                            intTime2 = i;
                            i = list.Count;
                        }
                        i = i + 1;
                    }

                    //create a new text file using the data between the two above integers

                    string path = Properties.Settings.Default.bLocalFolder.ToString() + "\\" + cboDay.Text + cboMonth.Text + cboYear.Text + cboTime.Text.Replace(":", "") + "Bookings1.txt";
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            i = intTime1;
                            while (i < intTime2)
                            {
                                sw.WriteLine(list[i].ToString());
                                i = i + 1;
                            }
                        }
                    }

                    readThisTimesBookings();

                    dsuccess = true;
                }
                catch
                {
                    MessageBox.Show("Can't read or find local folder, attempting to read from nearby machines", "No local save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dsuccess = false;
                }
            }

            if (dsuccess == false)
            { 
                //if it can't read the ftp site or local folders, check nearby machines
                List<string> lstNearby = new List<string>();
                lstNearby = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                i = 0;
                var list1 = new List<string>();
                while (i < lstNearby.Count)
                {
                    string strNearby = lstNearby[i].ToString() + "//" + cboDay.Text + cboMonth.Text + cboYear.Text + "BookingsBart.txt";
                    try
                    {
                        var fileStream = new FileStream(strNearby, FileMode.Open, System.IO.FileAccess.Read);
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            string line;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                list1.Add(line);
                            }
                        }
                        fileStream.Close();
                        i = lstNearby.Count;
                        dsuccess = true;

                        //read through file and find the time of this booking
                        string strTime1 = cboTime.Text;
                        string strTime2 = cboTime.Items[cboTime.SelectedIndex + 1].ToString();
                        //first time slot - actual booking
                        i = 0;
                        while (i < list1.Count)
                        {
                            if (list1[i].ToString() == strTime1.ToString())
                            {
                                intTime1 = i;
                                i = list1.Count;
                            }
                            i = i + 1;
                        }
                        //second time slot - next booking
                        i = 0;
                        while (i < list1.Count)
                        {
                            if (list1[i].ToString() == strTime2.ToString())
                            {
                                intTime2 = i;
                                i = list1.Count;
                            }
                            i = i + 1;
                        }

                        //create a new text file using the data between the two above integers

                        string path = Properties.Settings.Default.bLocalFolder.ToString() + "\\" + cboDay.Text + cboMonth.Text + cboYear.Text + cboTime.Text.Replace(":", "") + "Bookings1.txt";
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                i = intTime1;
                                while (i < intTime2)
                                {
                                    sw.WriteLine(list1[i].ToString());
                                    i = i + 1;
                                }
                            }
                        }

                        readThisTimesBookings();
                    }
                    catch
                    {
                        i = i + 1;
                        dsuccess = false;
                    }
                }
            }

            if (dsuccess==false)
            {
                MessageBox.Show("Can't download bookings for this session, this is probably down to missing data and no internet connection", "Missing session information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void readThisTimesBookings()
        {
            string sTime = cboTime.Text;
            string strTime = sTime.Replace(":", "");
            string strName = cboDay.Text + cboMonth.Text + cboYear.Text + strTime.ToString() + "Bookings1.txt";
            string strLocalFolder = Properties.Settings.Default.bLocalFolder.ToString() + "\\" + strName.ToString();
            
            lstAll.Clear();
            //read new file of decoded customer information
            var fileStream = new FileStream(strLocalFolder, FileMode.Open, System.IO.FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    lstAll.Add(line);
                }
            }
            fileStream.Close();

            //count the amount of families in the session
            lstCount.Clear();
            lstFamilyNames.Clear();
            i = 0;
            while (i < lstAll.Count)
            {
                List<string> lstItems = new List<string>();
                lstItems = lstAll[i].Split(',').ToList();
                lstCount.Add(lstItems[0].ToString());
                if (lstItems[1].ToString()=="Address")
                {
                    List<string> lstFName = new List<string>();
                    lstFName = lstItems[2].Split(' ').ToList();
                    try
                    {
                        lstFamilyNames.Add(lstFName[lstFName.Count-1].ToString());
                    }
                    catch
                    {
                        lstFamilyNames.Add(lstItems[2].ToString());
                    }
                }
                i = i + 1;
            }

            //retrieve the booking references
            lstRef.Clear();
            lstRef = lstCount.Distinct().ToList();


            //create buttons for each family and add them to family group box
            i = 0;
            int inty = 19;
            while (i < lstRef.Count)
            {
                Button familyButtons = new Button();
                gbFamilies.Controls.Add(familyButtons);
                familyButtons.Text = lstFamilyNames[i].ToString() + "   " + lstRef[i].ToString();
                familyButtons.Location = new Point(6, inty);
                familyButtons.Size = new Size(100, 40);
                familyButtons.Click += new EventHandler(this.MyButtonHandler);
                buttonsAdded.Insert(0, familyButtons);
                inty = inty + 45;
                i = i + 1;
            }
        }

        void MyButtonHandler(object sender, EventArgs e)
        {
            clearListBoxes();
            List<string> lstBParts = new List<String>();
            lstBParts = sender.ToString().Split(' ').ToList();
            int intR = Int32.Parse(lstBParts[lstBParts.Count-1]);
            string strF = lstBParts[lstBParts.Count - 4].ToString();
            lblReference.Text = intR.ToString();
            lblFamily.Text = strF + " Family";

            i = 0;
            while (i<lstAll.Count)
            {
                List<string> lstParts = new List<string>();
                lstParts = lstAll[i].ToString().Split(',').ToList();
                //check reference number
                if (lstParts[0].ToString() == lblReference.Text)
                {
                    //check to see if the line is about children
                    if (lstParts[1].ToString() == "Child")
                    {
                        //fill in child names
                        lstChildShow1.Items.Add(lstParts[2].ToString());
                        lstChildHide1.Items.Add(lstParts[2].ToString());
                        //split out the questions about children
                        j = 0;
                        while(j<lstParts.Count)
                        {
                            List<string> lstID = lstParts[j].ToString().Split(']').ToList();
                            try { if (lstID[1].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[0].ToString()) { lstChildShow2.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[2].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[1].ToString()) { lstChildShow3.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[3].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[2].ToString()) { lstChildShow4.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[4].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[3].ToString()) { lstChildShow5.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[5].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[4].ToString()) { lstChildShow6.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[6].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[5].ToString()) { lstChildShow7.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[7].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[6].ToString()) { lstChildShow8.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[8].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[7].ToString()) { lstChildShow9.Items.Add(lstID[1].ToString()); }} catch { }
                            try { if (lstID[19].ToString().Replace("[", "") == lstChildShowQuestionsID.Items[8].ToString()) { lstChildShow10.Items.Add(lstID[1].ToString()); }} catch { }

                            try { if (lstID[1].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[0].ToString()) { lstChildHide2.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[2].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[1].ToString()) { lstChildHide3.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[3].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[2].ToString()) { lstChildHide4.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[4].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[3].ToString()) { lstChildHide5.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[5].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[4].ToString()) { lstChildHide6.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[6].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[5].ToString()) { lstChildHide7.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[7].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[6].ToString()) { lstChildHide8.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[8].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[7].ToString()) { lstChildHide9.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[19].ToString().Replace("[", "") == lstChildHideQuestionsID.Items[8].ToString()) { lstChildHide10.Items.Add(lstID[1].ToString()); } } catch { }

                            j = j + 1;
                        }
                    }
                    if (lstParts[1].ToString() == "Adult")
                    {
                        //add adult names
                        lstAdultShow1.Items.Add(lstParts[2].ToString());
                        lstAdultHide1.Items.Add(lstParts[2].ToString());
                        //split out the questions about adults
                        j = 0;
                        while (j < lstParts.Count)
                        {
                            List<string> lstID = lstParts[j].ToString().Split(']').ToList();
                            try { if (lstID[1].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[0].ToString()) { lstAdultShow2.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[2].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[1].ToString()) { lstAdultShow3.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[3].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[2].ToString()) { lstAdultShow4.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[4].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[3].ToString()) { lstAdultShow5.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[5].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[4].ToString()) { lstAdultShow6.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[6].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[5].ToString()) { lstAdultShow7.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[7].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[6].ToString()) { lstAdultShow8.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[8].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[7].ToString()) { lstAdultShow9.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[19].ToString().Replace("[", "") == lstAdultShowQuestionsID.Items[8].ToString()) { lstAdultShow10.Items.Add(lstID[1].ToString()); } } catch { }

                            try { if (lstID[1].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[0].ToString()) { lstAdultHide2.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[2].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[1].ToString()) { lstAdultHide3.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[3].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[2].ToString()) { lstAdultHide4.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[4].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[3].ToString()) { lstAdultHide5.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[5].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[4].ToString()) { lstAdultHide6.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[6].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[5].ToString()) { lstAdultHide7.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[7].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[6].ToString()) { lstAdultHide8.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[8].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[7].ToString()) { lstAdultHide9.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[19].ToString().Replace("[", "") == lstAdultHideQuestionsID.Items[8].ToString()) { lstAdultHide10.Items.Add(lstID[1].ToString()); } } catch { }

                            j = j + 1;
                        }
                    }

                    if (lstParts[1].ToString() == "Family")
                    {
                        //split out the questions about family
                        j = 0;
                        while (j < lstParts.Count)
                        {
                            List<string> lstID = lstParts[j].ToString().Split(']').ToList();
                            try { if (lstID[1].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[0].ToString()) { lstFamilyShow2.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[2].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[1].ToString()) { lstFamilyShow3.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[3].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[2].ToString()) { lstFamilyShow4.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[4].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[3].ToString()) { lstFamilyShow5.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[5].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[4].ToString()) { lstFamilyShow6.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[6].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[5].ToString()) { lstFamilyShow7.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[7].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[6].ToString()) { lstFamilyShow8.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[8].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[7].ToString()) { lstFamilyShow9.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[19].ToString().Replace("[", "") == lstFamilyShowQuestionsID.Items[8].ToString()) { lstFamilyShow10.Items.Add(lstID[1].ToString()); } } catch { }

                            try { if (lstID[1].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[0].ToString()) { lstFamilyHide2.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[2].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[1].ToString()) { lstFamilyHide3.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[3].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[2].ToString()) { lstFamilyHide4.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[4].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[3].ToString()) { lstFamilyHide5.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[5].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[4].ToString()) { lstFamilyHide6.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[6].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[5].ToString()) { lstFamilyHide7.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[7].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[6].ToString()) { lstFamilyHide8.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[8].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[7].ToString()) { lstFamilyHide9.Items.Add(lstID[1].ToString()); } } catch { }
                            try { if (lstID[19].ToString().Replace("[", "") == lstFamilyHideQuestionsID.Items[8].ToString()) { lstFamilyHide10.Items.Add(lstID[1].ToString()); } } catch { }

                            j = j + 1;
                        }
                    }
                }
                i = i + 1;
            }

            cmdSaved.Enabled = true;

        }

        private void cmdDownloadBackup_Click(object sender, EventArgs e)
        {
            if (cboDay1.SelectedIndex == -1 || cboYear1.SelectedIndex == -1 || cboMonth1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a day, month and year first", "Missing dates", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to download all information from server? This may take some time and is only advised before or after opening hours or during longer breaks.", "Download all information?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    downloadBackup();
                }
            }
        }

        private void downloadBackup()
        {
            
            tabControl2.Visible = false;
            cmdCheckPin.Enabled = true;
            txtPassword.Enabled = true;
            bool bsuccess,bsuccess1;
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
                    bsuccess = true;
                }
                catch
                {
                    bsuccess = false;
                    MessageBox.Show("Download Failed, please check the setup folders and try again", "Failed to download", MessageBoxButtons.OK);
                }

                //connect to fusemetrix and create a backupfile of all the bookings for today
                string sDate = cboDay.Text + "/" + cboMonth.Text + "/" + cboYear.Text;
                NameValueCollection nv = new NameValueCollection();
                nv.Add("QDate", sDate);

                WebClient wc = new WebClient();
                byte[] ret = wc.UploadValues("https://4k-photos.co.uk/Bart.php", nv);

                var url = "https://4k-photos.co.uk/downloadAllBookings22.php";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";

                httpRequest.ContentType = "application/x-www-form-urlencoded";

                var data = "QDate=" + sDate;

                //this sends the request for date and time to a php script on the server which in turn 
                //creates a new file with the username date time and bookings1.txt
                //eg 23/12/2021 10:00 would be 231220211000Bookings1.txt
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                }

                //download the backup file
                try
                {
                    strRemoteFolder = cboDay.Text+cboMonth.Text+cboYear.Text+"BookingsBart.txt";

                    strLocalFolder = Properties.Settings.Default.bBackupLocation.ToString() + "//" + strRemoteFolder;
                    using (var sftp1 = new SftpClient(Host, Port, Username, Password))
                    {
                        sftp1.Connect();

                        //download new booking file
                        using (var file = File.OpenWrite(strLocalFolder))
                        {
                            sftp1.DownloadFile(strRemoteFolder, file);//download file
                        }

                        sftp1.Disconnect();
                    }
                    bsuccess1 = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error");
                    bsuccess1 = false;
                }

                if (bsuccess == true && bsuccess1==true)
                {
                    MessageBox.Show("Download completed successfully", "Download finished", MessageBoxButtons.OK);
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

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change the amount of days depending on the month of the year
            //february - 28 days (unless leap year)
            if (cboMonth.Text == "February" && cboMonth.Items.Count > 28 && cboMonth.Items.Count <28)
            {
                cboMonth.Items.Clear();
                i = 1;
                while (i<29)
                {
                    cboMonth.Items.Add(i);
                    i = i + 1;
                }
            }
            //April, June,September,Novmeber - 30 days
            if (cboMonth.Text == "November" && cboMonth.Text == "April" && cboMonth.Text == "June" && cboMonth.Text == "September" && cboMonth.Items.Count > 31 && cboMonth.Items.Count < 31)
            {
                cboMonth.Items.Clear();
                i = 1;
                while (i < 31)
                {
                    cboMonth.Items.Add(i);
                    i = i + 1;
                }
            }
            //January, March, May, JUly,August,October,December = 31 days
            if (cboMonth.Text == "August" && cboMonth.Text == "October" && cboMonth.Text == "December" && cboMonth.Text == "January" && cboMonth.Text == "March" && cboMonth.Text == "May" && cboMonth.Text == "July" && cboMonth.Items.Count > 32 && cboMonth.Items.Count < 32)
            {
                cboMonth.Items.Clear();
                i = 1;
                while (i < 31)
                {
                    cboMonth.Items.Add(i);
                    i = i + 1;
                }
            }
            cboMonth1.SelectedIndex = cboMonth.SelectedIndex;
        }

        private void cmdBackupBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBackup.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.bBackupLocation = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            tabControl2.Visible=false;
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            tabControl2.Visible = false;
        }

        private void cmdCheckPin_Click(object sender, EventArgs e)
        {
            checkPin();
        }

        private void checkPin()
        {
            if (txtPin.Text == "")
            {
                MessageBox.Show("Please enter a pin first","No Pin",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtPin.Text = "";
                txtPin.Focus();
            }
            else
            {
                if (txtPin.Text == Properties.Settings.Default.bPin)
                {
                    tabControl2.Visible = true;
                    txtPin.Text = "";
                }
                else
                {
                    MessageBox.Show("Pin is incorrect, please try again","Pin wrong",MessageBoxButtons.OK,MessageBoxIcon.None);
                    txtPin.Text = "";
                    txtPin.Focus();
                }
            }
        }

        private void txtPin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                checkPin();
            }
        }

        private void cmdSavePin_Click(object sender, EventArgs e)
        {
            if (txtOldPin.Text == "" || txtNewPin1.Text == "" || txtNewPin2.Text =="")
            {
                MessageBox.Show("Please ensure you have entered all the pins above", "Missing Pins", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (txtOldPin.Text == Properties.Settings.Default.bPin)
                {
                    if (txtNewPin1.Text == txtNewPin2.Text)
                    {
                        Properties.Settings.Default.bPin = txtNewPin2.Text;
                        Properties.Settings.Default.Save();
                        txtOldPin.Text = "";
                        txtNewPin2.Text = "";
                        txtNewPin1.Text = "";
                        MessageBox.Show("New Pin has been saved", "Saved Pin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("New pins aren't the same, please reenter","Pins error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        txtNewPin1.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Old pin is incorrect, please retry", "Pin Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtOldPin.Text = "";
                    txtOldPin.Focus();
                }

            }
        }

        private void chkSavedChristmas_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void cmdLocal_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLocal.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.bLocalFolder = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdSaved_Click(object sender, EventArgs e)
        {
                //if there is a family, run the save function depending on if local or ftp
                //if on ftp
                if (rbFTPSaved.Checked == true)
                {
                    try
                    {

                    }
                    catch
                    {
                        MessageBox.Show("Failed to save to internet, check to see if you are supposed to be saving to ftp and if all the settings are correct; The internet maybe down", "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                //if saved locally
                if (rbLocalSaved.Checked == true)
                {
                    try
                    {
                        string path = Properties.Settings.Default.bSavedLocationLocal.ToString() + "\\" + lblReference.Text + ".txt";
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                i = intTime1;
                                while (i < intTime2)
                                {
                                    sw.WriteLine(lblReference.Text);
                                    i = i + 1;
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Failed to save locally, check to see if you are supposed to be saving to ftp and if all the settings are correct; The internet maybe down", "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
        }

        private void cmdCancelXmasSaved_Click(object sender, EventArgs e)
        {
            loadSavedXmas();
        }

        private void cmdSavedXmasFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLocalSaveFolder.Text = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.bSavedLocationLocal = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void rbLocalSaved_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bSavedType = 0;
            Properties.Settings.Default.Save();
        }

        private void rbFTPSaved_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bSavedType = 1;
            Properties.Settings.Default.Save();
        }

        private void cmdSavedXmasSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.bSavedLocationFTP=txtFTPSaveFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void cboDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDay1.SelectedIndex = cboDay.SelectedIndex;
        }

        private void cboMonth1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change the amount of days depending on the month of the year
            //february - 28 days (unless leap year)
            if (cboMonth1.Text == "February" && cboMonth1.Items.Count > 28 && cboMonth1.Items.Count < 28)
            {
                cboMonth1.Items.Clear();
                i = 1;
                while (i < 29)
                {
                    cboMonth1.Items.Add(i);
                    i = i + 1;
                }
            }
            //April, June,September,Novmeber - 30 days
            if (cboMonth1.Text == "November" && cboMonth1.Text == "April" && cboMonth1.Text == "June" && cboMonth1.Text == "September" && cboMonth1.Items.Count > 31 && cboMonth1.Items.Count < 31)
            {
                cboMonth1.Items.Clear();
                i = 1;
                while (i < 31)
                {
                    cboMonth1.Items.Add(i);
                    i = i + 1;
                }
            }
            //January, March, May, JUly,August,October,December = 31 days
            if (cboMonth1.Text == "August" && cboMonth1.Text == "October" && cboMonth1.Text == "December" && cboMonth1.Text == "January" && cboMonth1.Text == "March" && cboMonth1.Text == "May" && cboMonth1.Text == "July" && cboMonth1.Items.Count > 32 && cboMonth1.Items.Count < 32)
            {
                cboMonth1.Items.Clear();
                i = 1;
                while (i < 31)
                {
                    cboMonth1.Items.Add(i);
                    i = i + 1;
                }
            }
            cboMonth.SelectedIndex = cboMonth1.SelectedIndex;
        }

        private void cboDay1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboDay.SelectedIndex = cboDay1.SelectedIndex;
        }

        private void cboYear1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboYear.SelectedIndex = cboYear1.SelectedIndex;
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboYear1.SelectedIndex = cboYear.SelectedIndex;
        }

        private void cmdNearby1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby1.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();   
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[0] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i< lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i==lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby2.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[1] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby3.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[2] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby4.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[3] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby5.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[4] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby6_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby6.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[5] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby7_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby7.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[6] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cmdNearby8_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtNearby8.Text = folderBrowserDialog1.SelectedPath;
                List<String> lstBackups = new List<String>();
                lstBackups = Properties.Settings.Default.bBackupLocations.Split(',').ToList();
                lstBackups[7] = folderBrowserDialog1.SelectedPath;
                string strBackup = "";
                i = 0;
                while (i < lstBackups.Count)
                {
                    strBackup = strBackup + lstBackups[i].ToString();
                    i = i + 1;
                    if (i == lstBackups.Count)
                    { }
                    else
                    {
                        strBackup = strBackup + ",";
                    }
                }
                Properties.Settings.Default.bBackupLocations = strBackup;
                Properties.Settings.Default.Save();
            }
        }

        private void cboQuestionDisplays_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmdChoiceUpdate_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.bQuestionChoice = cboQuestionDisplays.SelectedIndex;
            Properties.Settings.Default.Save();
            loadQuestions();
        }

        private void lstChildShow1_Click(object sender, EventArgs e)
        {
            intList = lstChildShow1.SelectedIndex;
            updateChildLists();
        }

        private void updateChildLists()
        {
            try { lstChildShow1.SelectedIndex = intList; } catch { }
            try { lstChildShow2.SelectedIndex = intList; } catch { }
            try { lstChildShow3.SelectedIndex = intList; } catch { }
            try { lstChildShow4.SelectedIndex = intList; } catch { }
            try { lstChildShow5.SelectedIndex = intList; } catch { }
            try { lstChildShow6.SelectedIndex = intList; } catch { }
            try { lstChildShow7.SelectedIndex = intList; } catch { }
            try { lstChildShow8.SelectedIndex = intList; } catch { }
            try { lstChildShow9.SelectedIndex = intList; } catch { }
            try { lstChildShow10.SelectedIndex = intList; } catch { }

            try { lstChildHide1.SelectedIndex = intList; } catch { }
            try { lstChildHide2.SelectedIndex = intList; } catch { }
            try { lstChildHide3.SelectedIndex = intList; } catch { }
            try { lstChildHide4.SelectedIndex = intList; } catch { }
            try { lstChildHide5.SelectedIndex = intList; } catch { }
            try { lstChildHide6.SelectedIndex = intList; } catch { }
            try { lstChildHide7.SelectedIndex = intList; } catch { }
            try { lstChildHide8.SelectedIndex = intList; } catch { }
            try { lstChildHide9.SelectedIndex = intList; } catch { }
            try { lstChildHide10.SelectedIndex = intList; } catch { }
        }

        private void updateAdultList()
        {
            try { lstAdultShow1.SelectedIndex = intList; } catch { }
            try { lstAdultShow2.SelectedIndex = intList; } catch { }
            try { lstAdultShow3.SelectedIndex = intList; } catch { }
            try { lstAdultShow4.SelectedIndex = intList; } catch { }
            try { lstAdultShow5.SelectedIndex = intList; } catch { }
            try { lstAdultShow6.SelectedIndex = intList; } catch { }
            try { lstAdultShow7.SelectedIndex = intList; } catch { }
            try { lstAdultShow8.SelectedIndex = intList; } catch { }
            try { lstAdultShow9.SelectedIndex = intList; } catch { }
            try { lstAdultShow10.SelectedIndex = intList; } catch { }

            try { lstAdultHide1.SelectedIndex = intList; } catch { }
            try { lstAdultHide2.SelectedIndex = intList; } catch { }
            try { lstAdultHide3.SelectedIndex = intList; } catch { }
            try { lstAdultHide4.SelectedIndex = intList; } catch { }
            try { lstAdultHide5.SelectedIndex = intList; } catch { }
            try { lstAdultHide6.SelectedIndex = intList; } catch { }
            try { lstAdultHide7.SelectedIndex = intList; } catch { }
            try { lstAdultHide8.SelectedIndex = intList; } catch { }
            try { lstAdultHide9.SelectedIndex = intList; } catch { }
            try { lstAdultHide10.SelectedIndex = intList; } catch { }
        }

        private void updateFamilyList()
        {
            try { lstFamilyShow1.SelectedIndex = intList; } catch { }
            try { lstFamilyShow2.SelectedIndex = intList; } catch { }
            try { lstFamilyShow3.SelectedIndex = intList; } catch { }
            try { lstFamilyShow4.SelectedIndex = intList; } catch { }
            try { lstFamilyShow5.SelectedIndex = intList; } catch { }
            try { lstFamilyShow6.SelectedIndex = intList; } catch { }
            try { lstFamilyShow7.SelectedIndex = intList; } catch { }
            try { lstFamilyShow8.SelectedIndex = intList; } catch { }
            try { lstFamilyShow9.SelectedIndex = intList; } catch { }
            try { lstFamilyShow10.SelectedIndex = intList; } catch { }

            try { lstFamilyHide1.SelectedIndex = intList; } catch { }
            try { lstFamilyHide2.SelectedIndex = intList; } catch { }
            try { lstFamilyHide3.SelectedIndex = intList; } catch { }
            try { lstFamilyHide4.SelectedIndex = intList; } catch { }
            try { lstFamilyHide5.SelectedIndex = intList; } catch { }
            try { lstFamilyHide6.SelectedIndex = intList; } catch { }
            try { lstFamilyHide7.SelectedIndex = intList; } catch { }
            try { lstFamilyHide8.SelectedIndex = intList; } catch { }
            try { lstFamilyHide9.SelectedIndex = intList; } catch { }
            try { lstFamilyHide10.SelectedIndex = intList; } catch { }
        }

        private void lstChildShow2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void lstChildShow3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstChildShow2_Click(object sender, EventArgs e)
        {
            intList = lstChildShow2.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow3_Click(object sender, EventArgs e)
        {
            intList = lstChildShow3.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow4_Click(object sender, EventArgs e)
        {
            intList = lstChildShow4.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow5_Click(object sender, EventArgs e)
        {
            intList = lstChildShow5.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow6_Click(object sender, EventArgs e)
        {
            intList = lstChildShow6.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow7_SelectedIndexChanged(object sender, EventArgs e)
        {
            intList = lstChildShow7.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow8_Click(object sender, EventArgs e)
        {
            intList = lstChildShow8.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow9_Click(object sender, EventArgs e)
        {
            intList = lstChildShow9.SelectedIndex;
            updateChildLists();
        }

        private void lstChildShow10_Click(object sender, EventArgs e)
        {
            intList = lstChildShow10.SelectedIndex;
            updateChildLists();
        }

        private void lstAdultShow10_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow10.SelectedIndex;
            updateChildLists();
        }

        private void lstAdultShow1_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow1.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow2_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow2.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow3_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow3.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow4_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow4.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow5_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow5.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow6_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow6.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow7_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow7.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow8_SelectedIndexChanged(object sender, EventArgs e)
        {
            intList = lstAdultShow8.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultShow9_Click(object sender, EventArgs e)
        {
            intList = lstAdultShow9.SelectedIndex;
            updateAdultList();
        }

        private void lstFamilyShow1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstFamilyShow1_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow1.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow2_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow2.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow3_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow3.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow4_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow4.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow5_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow5.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow6_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow6.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow7_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow7.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow8_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow8.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow9_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow9.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyShow10_Click(object sender, EventArgs e)
        {
            intList = lstFamilyShow10.SelectedIndex;
            updateFamilyList();
        }

        private void lstChildShowQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstChildHide1_Click(object sender, EventArgs e)
        {
            intList = lstChildHide1.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide2_Click(object sender, EventArgs e)
        {
            intList = lstChildHide2.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide3_Click(object sender, EventArgs e)
        {
            intList = lstChildHide3.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide4_Click(object sender, EventArgs e)
        {
            intList = lstChildHide4.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide5_Click(object sender, EventArgs e)
        {
            intList = lstChildHide5.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide6_Click(object sender, EventArgs e)
        {
            intList = lstChildHide6.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide7_Click(object sender, EventArgs e)
        {
            intList = lstChildHide7.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide8_Click(object sender, EventArgs e)
        {
            intList = lstChildHide8.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide9_Click(object sender, EventArgs e)
        {
            intList = lstChildHide9.SelectedIndex;
            updateChildLists();
        }

        private void lstChildHide10_Click(object sender, EventArgs e)
        {
            intList = lstChildHide10.SelectedIndex;
            updateChildLists();
        }

        private void lstAdultHide1_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide1.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide2_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide2.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide3_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide3.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide4_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide4.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide5_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide5.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide6_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide6.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide7_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide7.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide8_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide8.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide9_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide9.SelectedIndex;
            updateAdultList();
        }

        private void lstAdultHide10_Click(object sender, EventArgs e)
        {
            intList = lstAdultHide10.SelectedIndex;
            updateAdultList();
        }

        private void lstFamilyHide1_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide1.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide2_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide2.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide3_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide3.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide4_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide4.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide5_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide5.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide6_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide6.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide7_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide7.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide8_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide8.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide9_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide9.SelectedIndex;
            updateFamilyList();
        }

        private void lstFamilyHide10_Click(object sender, EventArgs e)
        {
            intList = lstFamilyHide10.SelectedIndex;
            updateFamilyList();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void gbShow_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lstFamilyShowQuestionsID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstAdultHideQuestionsID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstChildShow1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
          
        }
    }
}
