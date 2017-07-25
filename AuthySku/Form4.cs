using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Symbol;
using Symbol.Keyboard;
using Symbol.Barcode2;

using Newtonsoft.Json;

using System.Net;
using System.IO;

namespace AuthySku
{
    public partial class Form4 : Form
    {

        public bool scannerActive = true;
        public bool scannerMode = false;
        public string wareHouse = "2143";

        public Form4()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (scannerMode)
            {
                scannerMode = false;
                menuItem1.Text = "Trigger: Off";
            }
            else
            {
                scannerMode = true;
                menuItem1.Text = "Trigger: On";
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (scannerActive)
            {
                scannerActive = false;
                menuItem2.Text = "Scanner: Off";

                button3.Visible = false;
            }
            else
            {
                scannerActive = true;
                menuItem2.Text = "Scanner: On";

                button3.Visible = true;
                button3.Text = textBox2.Text.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox1.Text = "";

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText("Settings.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Settings setting = (Settings)serializer.Deserialize(file, typeof(Settings));

                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create("http://" + setting.Host + "/api/" + setting.Token + "/sureSearch/" + textBox2.Text);
                request.ContentType = "application/json";
                request.Method = "GET";

                ((HttpWebRequest)request).AllowWriteStreamBuffering = true;
                var httpResponse = (HttpWebResponse)request.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                    if (htmlAttributes["id"] == "null")
                    {
                        button3.BackColor = Color.Red;
                        textBox2.BackColor = Color.Red;

                        button1.Enabled = true;
                        
                    }
                    else
                    {
                        textBox1.Text = htmlAttributes["title"] +
                            "\r\n" + "---------------" + "\r\n" +
                            "QTY: " + htmlAttributes["qty"] + "   Cost: $" + htmlAttributes["cost"] + "   Price: $" + htmlAttributes["price"] +
                            "\r\n" + "---------------" + "\r\n" +
                            htmlAttributes["locations"] +
                            "\r\n" + "---------------" + "\r\n" +
                            "Created: " + htmlAttributes["created"] + "   Total Sold: " + htmlAttributes["sold"];

                        button3.BackColor = Color.Green;
                        textBox2.BackColor = Color.Green;

                        button1.Enabled = true;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Scanner.wakeScanner(button3, textBox2, scannerMode);

            button3.BackColor = Color.White;
            textBox2.BackColor = Color.White;
        }

        private void Form4_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Up
                textBox1.Focus();
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Down
                textBox1.Focus();
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = Color.White;
        }
    }
}