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
    public partial class Form3 : Form
    {
        public bool scannerActive = true;
        public bool scannerMode = true;
        public string wareHouse = "2143";

        public Form3()
        {
            InitializeComponent();
            textBox2.Focus();
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
                this.Text = "AuthySku|Scan: " + scannerActive.ToString();

                //hide false text boxes "buttons"
                button2.Visible = false;
                button3.Visible = false;
            }
            else
            {
                scannerActive = true;
                this.Text = "AuthySku|Scan: " + scannerActive.ToString();

                //show false text boxes "buttons"
                button2.Visible = true;
                button3.Visible = true;

                button2.Text = textBox1.Text.ToString();
                button3.Text = textBox3.Text.ToString();
            }
        }



        private void wakeScanner(Button scanButton, TextBox textBox)
        {

            scanButton.Enabled = false;

            Symbol.Generic.Device MyDevice = Symbol.Barcode.Device.AvailableDevices[0];

            Barcode2 MyBarcode = new Barcode2(MyDevice.ToString());

            MyBarcode.Enable();

            MyBarcode.Config.Reader.ReaderSpecific.LaserSpecific.AimDuration = 500;
            if (scannerMode)
            {
                MyBarcode.Config.TriggerMode = TRIGGERMODES.HARD;
            }
            else
            {
                MyBarcode.Config.TriggerMode = TRIGGERMODES.SOFT_ONCE;
            }

            ScanData MyData = MyBarcode.ScanWait(8000); // 5 seconds timeout

            if (MyData.Result == Symbol.Barcode2.Results.E_SCN_READTIMEOUT)   // Barcode was not scanned within 5 secs
            {
                scanButton.Text = "No Scan Found";
                textBox.Text = "No Scan Found";
                MyBarcode.Dispose();
                //enable add product button
                scanButton.Enabled = true;
            }
            if (MyData.Result == Symbol.Barcode2.Results.SUCCESS) // Barcode was scanned successfully
            {
                scanButton.Text = MyData.ToString();
                textBox.Text = MyData.ToString();
                MyBarcode.Dispose();
                //enable add product button
                scanButton.Enabled = true;
            }
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
                WebRequest request = WebRequest.Create("http://" + setting.Host + "/api/" + setting.Token + "/addItem/" + textBox1.Text + "/" + textBox2.Text + "/" + textBox3.Text + "/" + wareHouse);
                request.ContentType = "application/json";
                request.Method = "GET";

                ((HttpWebRequest)request).AllowWriteStreamBuffering = true;
                var httpResponse = (HttpWebResponse)request.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                    if (htmlAttributes["AddItemStatus"] == "Success")
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (scannerActive)
            {
                wakeScanner(button2, textBox1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (scannerActive)
            {
                wakeScanner(button3, textBox3);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }
    }
}