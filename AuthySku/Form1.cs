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
            //button1.Enabled = false;

            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create("http://192.168.1.128:3000" + "/api/" + token);
            request.ContentType = "application/json";
            request.Method = "GET";

            ((HttpWebRequest)request).AllowWriteStreamBuffering = true;
            var httpResponse = (HttpWebResponse)request.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);


                using (StreamWriter file = File.CreateText("Settings.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, htmlAttributes["rando"]);
                }
                MessageBox.Show(htmlAttributes["rando"]);
            }
        }
    }
}