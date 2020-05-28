using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class optionsmenu : Form
    {
        public optionsmenu()
        {
            InitializeComponent();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ManualClose);

            textBox_prefix.Text = Properties.Settings.Default.fileprefix;
            textBox_savefolder.Text = Properties.Settings.Default.savefolder;
            checkBox_subfolder.Checked = Properties.Settings.Default.datefolder;

            if (string.IsNullOrEmpty(textBox_savefolder.Text) || !Directory.Exists(textBox_savefolder.Text))
            {
                textBox_savefolder.BackColor = Color.Pink;
            }

            textBox_savefolder.TextChanged += new EventHandler(TextBox1_TextChanged);
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            //Do something
            textBox_savefolder.BackColor = Color.White;
        }

        private void ManualClose(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.fileprefix = textBox_prefix.Text;
            Properties.Settings.Default.savefolder = textBox_savefolder.Text;
            Properties.Settings.Default.datefolder = checkBox_subfolder.Checked;
            Properties.Settings.Default.Save();
        }

        private void button_viewfolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox_savefolder.Text) || string.IsNullOrEmpty(textBox_savefolder.Text))
            {
                textBox_savefolder.BackColor = Color.Pink;
            }

            try
            {
                Process.Start(textBox_savefolder.Text);
                textBox_savefolder.BackColor = Color.White;
            }
            catch
            {
                textBox_savefolder.BackColor = Color.Pink;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
