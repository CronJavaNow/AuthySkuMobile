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
    public partial class Form5 : Form
    {

        public bool scannerActive = true;
        public bool scannerMode = false;
        public string wareHouse = "2143";

        public Form5()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (scannerMode)
            {
                scannerMode = false;
                label7.Text = "False";
            }
            else
            {
                scannerMode = true;
                label7.Text = "True";
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (scannerActive)
            {
                scannerActive = false;
                label1.Text = "False";

                //hide false text boxes "buttons"
                skuButton.Visible = false;
                locationButton.Visible = false;
            }
            else
            {
                scannerActive = true;
                label1.Text = "True";

                //show false text boxes "buttons"
                skuButton.Visible = true;
                locationButton.Visible = true;

                skuButton.Text = skuTextBox.Text.ToString();
                locationButton.Text = locationTextBox.Text.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";
            button1.Enabled = false;

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText("Settings.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Settings setting = (Settings)serializer.Deserialize(file, typeof(Settings));

                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create("http://" + setting.Host + "/api/" + setting.Token + "/removeItem/" + skuTextBox.Text + "/" + qtyTextBox.Text + "/" + locationTextBox.Text + "/" + wareHouse);
                request.ContentType = "application/json";
                request.Method = "GET";

                ((HttpWebRequest)request).AllowWriteStreamBuffering = true;
                var httpResponse = (HttpWebResponse)request.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                    if (htmlAttributes["RemoveItemStatus"] == "Success")
                    {
                        label6.Text = "Success";
                        button1.Enabled = true;
                    }
                    else
                    {
                        label6.Text = "Fail";
                        button1.Enabled = true;
                    }
                }
            }
        }

        private void skuButton_Click(object sender, EventArgs e)
        {
            Scanner.wakeScanner(skuButton, skuTextBox, scannerMode);
        }

        private void locationButton_Click(object sender, EventArgs e)
        {
            Scanner.wakeScanner(locationButton, locationTextBox, scannerMode);
        }
    }
}