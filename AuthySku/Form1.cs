using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Symbol;
using Symbol.Barcode2;

using Newtonsoft.Json;

using System.Net;
using System.IO;

namespace AuthySku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Shown(Object sender, EventArgs e)
        {

        }

        private void nukeForm()
        {
            new Form2().Show();
            this.Hide();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = textBox1.Text;
            string token = textBox2.Text;

            // Keep from user requesting more call while loading.
            button1.Enabled = false;

            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create("http://" + host + "/api/" + token);
            request.ContentType = "application/json";
            request.Method = "GET";

            ((HttpWebRequest)request).AllowWriteStreamBuffering = true;
            var httpResponse = (HttpWebResponse)request.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                if (htmlAttributes["account"] == "active")
                {

                    Settings setting = new Settings
                    {
                        Host = host,
                        Token = token
                    };

                    using (StreamWriter file = File.CreateText("Settings.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, setting);
                    }

                    new Form2().Show();
                    this.Hide();
                    button1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("not active");
                    button1.Enabled = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;
            }
            else
            {
                if (File.Exists("Settings.json"))
                {
                    Form2 form2 = new Form2();
                    form2.Show();
                    form2.Focus();
                    this.Hide();
                }
                timer2.Enabled = false;
                panel2.Visible = false;
            }
        }
    }
}
