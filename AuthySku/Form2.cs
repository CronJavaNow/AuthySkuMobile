using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Symbol;
using Symbol.Attributes;
using Symbol.Barcode2;

using Newtonsoft.Json;

using System.Net;
using System.IO;

namespace AuthySku
{
    public partial class Form2 : Form
    {
        public bool settingsLock = true;

        public Form2()
        {
            InitializeComponent();

            menuItem1.Text = "Locked";
            menuItem2.Enabled = false;
            menuItem2.Text = "";

            label2.Text = "Crackling Campfire" + "\r\n" + "Rev 1.0.72117";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (settingsLock)
            {
                settingsLock = false;

                menuItem1.Text = "Lock";
                menuItem2.Enabled = true;
                menuItem2.Text = "Settings";
            }
            else
            {
                settingsLock = true;

                menuItem1.Text = "Locked";
                menuItem2.Enabled = false;
                menuItem2.Text = ""; 
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {

            File.Delete("Settings.json");
            new Form1().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }
    }
}