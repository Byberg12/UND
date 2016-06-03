using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;


namespace Game_Patcher
{
   
    public partial class Form1 : Form
    {
        private string host = "http://91.232.61.209/MMO_Patches/";
        private string gameName = "noNameRpg.exe";
        private string remoteFolder = "Release/Build";
        private string folder = "Game";
        private Queue<string> _downloadUrls = new Queue<string>();
        List<string> folders = new List<string>();
        List<string> urls = new List<string>();
        int currentCount = 0;
        

        private bool UpdateRequired = false;

        public Form1()
        {
            InitializeComponent();
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            label3.Text = "Version 0.0.7";
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                try
                {
                    WebClient preCheck = new WebClient();
                    
                    string pageContent = preCheck.DownloadString(host + "index.php");

                    XDocument doc = XDocument.Parse(pageContent);
                    List<string> list = doc.Root.Elements("path")
                                .Select(element => element.Value)
                                .ToList();
                    foreach (string value in list)
                    {
                        List<string> nodes = value.Split('|').ToList<string>();
                        if (nodes[0] == "folder")
                        {
                            string remoteFolderName = nodes[1];
                            string localFolderName = nodes[1].Replace(remoteFolder+"/",folder+"/");
                            
                            // Handle Folders
                            if (Directory.Exists(localFolderName))
                            {
                            }
                            else
                            {
                                folders.Add(localFolderName);
                            }
                        }
                        if (nodes[0] == "file")
                        {
                            // Handle Files
                            string remoteFileHash = nodes[1];
                            string remoteFileName = nodes[2].Replace("\\", "/");
                            string localFileName = nodes[2].Replace(remoteFolder + "/", folder + "/");

                            if (File.Exists(localFileName))
                            {
                                if (GetMD5HashFromFile(localFileName) != remoteFileHash)
                                {
                                    File.Delete(localFileName);
                                    urls.Add(host + remoteFileName);
                                }
                            }
                            else
                            {
                                urls.Add(host + remoteFileName);
                            }
                        }

                    }
                    if (urls.Count() > 0)
                    {
                        UpdateRequired = true;
                        label1.BeginInvoke(new Action(() =>
                        {
                            label1.Text = urls.Count() + " new updates available.";
                        }));

                        button1.BeginInvoke(new Action(() =>
                        {
                            button1.Text = "Update";
                            button1.Enabled = true;
                        }));
                    }
                    else
                    {
                        label1.BeginInvoke(new Action(() =>
                        {
                            label1.Text = "Game is up to date!";
                        }));

                        button1.BeginInvoke(new Action(() =>
                        {
                            button1.Text = "PLAY";
                            button1.Enabled = true;
                        }));
                    }

                }
                catch (WebException ex)
                {
                }
            }).Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri(host + "news.php");
            label1.Text = "Checking for updates...";
            button1.Text = "Scanning";
            button1.Enabled = false;
            label2.Text = "";
  

        }
        private void downloadFile()
        {            

            foreach (string url in urls)
            {
                _downloadUrls.Enqueue(url);
            }
            // Starts the download
            button1.Text = "Updating";
            button1.Enabled = false;
            progressBar1.Visible = true;
            label1.Visible = true;


            DownloadFile();
        }
        private void DownloadFile()
        {
            if (_downloadUrls.Any())
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = _downloadUrls.Dequeue();
                string FileName = url.Replace(host + remoteFolder, folder);
                
                client.DownloadFileAsync(new Uri(url), FileName);
                label1.Text = "Downloading: "+FileName;

                currentCount++;
                double current = double.Parse(currentCount.ToString());
                double totUrls = double.Parse(urls.Count().ToString());
                double percentage = (currentCount/totUrls)*100;
                int roundedPerc = int.Parse(Math.Truncate(percentage).ToString());

                label2.Text = String.Format("File {0} of {1} ( {2} )", currentCount, urls.Count(), roundedPerc.ToString()+"%");
                progressBar2.Value = roundedPerc;
                return;
            }

            // End of the download
            button1.Text = "PLAY";
            label1.Text = "Download Complete...";
            progressBar2.Value = 100;
            button1.Enabled = true;
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                throw e.Error;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            
            DownloadFile();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(UpdateRequired)
            {
                foreach(string thisFolder in folders)
                {
                    Directory.CreateDirectory(thisFolder);
                }
                downloadFile();
                UpdateRequired = false;
            }

            if(button1.Enabled == true && button1.Text == "PLAY")
            {
                Process.Start(folder+"\\"+ gameName);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
