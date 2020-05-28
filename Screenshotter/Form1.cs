using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
        //static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern void GetWindowRect(IntPtr hWnd, out Rectangle rect);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        bool shotsActive;

        bool showNotActiveMessage;

        Thread t;

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ManualClose);

            //Load saved values.
            Properties.Settings.Default.Upgrade();
            timeintervalBox1.Text = Properties.Settings.Default.timeinterval;
            processnameBox1.Text = Properties.Settings.Default.processname;
            
            if (string.IsNullOrWhiteSpace(timeintervalBox1.Text))
                timeintervalBox1.Text = "60";

            outputBox1.Select();
            
            AddOutput("Welcome to Blendo Screenshotter!");
        }


        private void DoThisAllTheTime()
        {
            int secondsBetweenShots = int.Parse(timeintervalBox1.Text);

            DateTime nextShotTime = DateTime.Now.Add(new TimeSpan(0,0,0,0,500));//Short delay before initial screenshot.
            int lastTimeUpdateValue = 0;

            while (true)
            {
                DateTime timeNow = DateTime.Now;
                TimeSpan differential = nextShotTime.Subtract(timeNow);

                if (lastTimeUpdateValue != differential.Seconds)
                {
                    lastTimeUpdateValue = differential.Seconds;

                    string timeLeft = (differential.TotalSeconds > 59) ? string.Format("{0}m", (int)(differential.TotalSeconds / 60)) : string.Format("{0}s", ((int)differential.TotalSeconds + 1));

                    string countdownText = string.Format("Click here to stop. Next screenshot in {0}...", timeLeft);
                    string windowtitleText = string.Format("Screenshotter {0}", timeLeft);
                    MethodInvoker mi2 = delegate() { startstopButton1.Text = countdownText; this.Text = windowtitleText; };
                    this.Invoke(mi2);
                }

                if (differential.TotalDays <= 0 && differential.TotalMinutes <= 0 && differential.TotalSeconds <= 0 && differential.TotalHours <= 0)
                {
                    bool success = false;
                    MethodInvoker mi = delegate() { success = TakeScreenshot(); };
                    this.Invoke(mi);

                    if (success)
                    {
                        nextShotTime = DateTime.Now.Add(new TimeSpan(0, 0, secondsBetweenShots));
                        outputBox1.BackColor = Color.GreenYellow;
                    }
                    else
                    {
                        //If shot FAILS, then keep hammering until we're successful again.
                        nextShotTime = DateTime.Now.Add(new TimeSpan(0, 0, 1));
                        outputBox1.BackColor = Color.White;
                    }
                }
            }
        }

        private void ManualClose(object sender, FormClosingEventArgs e)
        {
            if (t != null)
                t.Abort();

            Properties.Settings.Default.timeinterval = timeintervalBox1.Text;
            Properties.Settings.Default.processname = processnameBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void startstopButton1_Click(object sender, EventArgs e)
        {
            if (shotsActive)
            {
                //Stop screenshots.
                shotsActive = false;                
                AddOutput("Stopping screenshots now.");
                outputBox1.BackColor = Color.White;


                timeintervalBox1.Enabled = true;
                processnameBox1.Enabled = true;

                if (t != null)
                {
                    t.Abort();
                }

                startstopButton1.Text = "Start taking screenshots";
                this.Text = "Screenshotter";
            }
            else
            {
                //Start the screenshots.

                //Sanity check.
                List<string> errorList = new List<string>();
                if (string.IsNullOrWhiteSpace(timeintervalBox1.Text))
                {
                    errorList.Add("Missing seconds between shots. Please enter a value.");
                }
                else
                {
                    int result;
                    if (!int.TryParse(timeintervalBox1.Text, out result))
                    {
                        errorList.Add("Invalid seconds between shots. Please enter a valid value.");
                    }
                }

                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.savefolder))
                {
                    errorList.Add("Missing save folder name. Please go to File > Options");
                }
                else
                {
                    if (!Directory.Exists(Properties.Settings.Default.savefolder))
                    {
                        errorList.Add("Cannot find save folder. Please go to File > Options");
                    }
                }

                if (string.IsNullOrWhiteSpace(processnameBox1.Text))
                {
                    errorList.Add("Missing window title name. Please enter a value.");
                }

                if (errorList.Count > 0)
                {
                    AddOutput(string.Empty);
                    AddOutput("-- ERRORS FOUND --");
                    for (int i = 0; i < errorList.Count; i++)
                    {
                        AddOutput(string.Format("{0}. {1}", (i+1), errorList[i]));
                    }
                    outputBox1.BackColor = Color.Pink;
                    return;
                }

                showNotActiveMessage = true;
                shotsActive = true;
                AddOutput("- - - - - - - - - - - - -");
                AddOutput("Starting screenshots now.");
                outputBox1.BackColor = Color.White;

                //Disable the text fields.
                processnameBox1.Enabled = false;
                timeintervalBox1.Enabled = false;

                startstopButton1.Text = "STARTING...";

                t = new System.Threading.Thread(DoThisAllTheTime);
                t.Start();
            }
        }

        private bool TakeScreenshot()
        {
            string activewindowTitle = GetActiveWindowTitle();

            if (string.Compare(activewindowTitle, processnameBox1.Text, true) != 0)
            {
                if (showNotActiveMessage)
                {
                    AddOutput(string.Format("'{0}' not active. Waiting...", processnameBox1.Text));
                    showNotActiveMessage = false;
                }

                return false;
            }

            //Bitmap image = CaptureWindow(ptr);
            Bitmap image = GetActiveWindowBitmap();

            if (image == null)
            {
                AddOutput("Failed to generate screenshot.");
                return false;
            }

            string filename = string.Format("{0}_{1}-{2}-{3}-{4}_{5}_{6}.png",
                Properties.Settings.Default.fileprefix,
                DateTime.Now.Year,
                DateTime.Now.Month.ToString("D2"),
                DateTime.Now.Day.ToString("D2"),
                DateTime.Now.Hour.ToString("D2"),
                DateTime.Now.Minute.ToString("D2"),
                DateTime.Now.Second.ToString("D2"));

            string filepath;

            if (!Properties.Settings.Default.datefolder)
            {
                filepath = Path.Combine(Properties.Settings.Default.savefolder, filename);
            }
            else
            {
                //Append date to filepath
                string todaysDateString = GetDatefoldername();

                filepath = Path.Combine(Properties.Settings.Default.savefolder, todaysDateString);

                if (!Directory.Exists(filepath))
                {
                    DirectoryInfo newDir = Directory.CreateDirectory(filepath);

                    if (!newDir.Exists)
                    {
                        AddOutput(string.Format("Failed to create folder: {0}", filepath));
                    }
                }

                filepath = Path.Combine(filepath, filename);
            }

            image.Save(filepath);

            if (File.Exists(filepath))
            {
                FileInfo newFile = new FileInfo(filepath);
                AddOutput(string.Format("Shot saved ({0})", newFile.Name));
            }
            else
            {
                AddOutput("Failed to save screenshot.");
                return false;
            }

            showNotActiveMessage = true;
            return true;
        }

        private string GetDatefoldername()
        {
            return string.Format("{0}_{1}_{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }


        public Bitmap GetActiveWindowBitmap()
        {
            Rectangle rect;
            GetWindowRect(GetForegroundWindow(), out rect);

            int width = rect.Width - rect.X;
            int height = rect.Height - rect.Y;

            if (width > 0 && height > 0)
            {
                Bitmap bitmap = new Bitmap(width, height);

                Graphics graphics = Graphics.FromImage(bitmap);

                graphics.CopyFromScreen(new Point(rect.X, rect.Y), new Point(0, 0), new Size(width, height));

                return bitmap;
            }
            else
            {
                return null;
            }
        }

        public string GetActiveWindowTitle()
        {
            IntPtr handle;
            int chars = 128;

            StringBuilder buffer = new StringBuilder(chars);

            handle = GetForegroundWindow();

            if (GetWindowText(handle, buffer, chars) > 0)
            {
                return buffer.ToString();
            }

            return string.Empty;
        }


        private IntPtr GetWindowPtr(string windowName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (string.Compare( pList.ProcessName, windowName, true) == 0)
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

        private void AddOutput(string text)
        {
            string timestamp = DateTime.Now.AddDays(1).ToString("hh:mm:ss");
            outputBox1.Items.Add(string.Format("{0} {1}", timestamp, text));
            //outputBox1.Items.Add(text);

            int nItems = (int)(outputBox1.Height / outputBox1.ItemHeight);
            outputBox1.TopIndex = outputBox1.Items.Count - nItems;

            this.Update();
            this.Refresh();
        }

        private void viewButton1_Click(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optionsmenu newmenu = new optionsmenu();
            newmenu.ShowDialog();

            if (string.IsNullOrEmpty(Properties.Settings.Default.savefolder))
            {
                AddOutput("Missing save folder.");
                AddOutput("Please go to File > Options");
                outputBox1.BackColor = Color.Pink;
                return;
            }
            else if (!Directory.Exists(Properties.Settings.Default.savefolder))
            {
                AddOutput(string.Format("Unable to find save folder: {0}", Properties.Settings.Default.savefolder));
                AddOutput("Please go to File > Options");
                outputBox1.BackColor = Color.Pink;
                return;
            }

            AddOutput("Updated options.");
            outputBox1.BackColor = Color.White;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Blendo Screenshotter\nby Brendon Chung\nScreenshot code by https://github.com/gavinkendall/autoscreen \nMay 2020\n\nUse this program to take screenshots at regular intervals. Useful for generating promotional material, development images, or time-lapse footage.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
    
}
