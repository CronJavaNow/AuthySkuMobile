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

        public Form3()
        {
            InitializeComponent();
            textBox2.Focus();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (scannerActive)
            {
                scannerActive = false;
                this.Text = "AuthySku|Scan: " + scannerActive.ToString();
            }
            else
            {
                scannerActive = true;
                this.Text = "AuthySku|Scan: " + scannerActive.ToString();
            }
        }



        private void wakeScanner(TextBox txtBox)
        {
            //disable add product button, this keeps scanner safe.
            button1.Enabled = false;

            Symbol.Generic.Device MyDevice = Symbol.Barcode.Device.AvailableDevices[0];

            Barcode2 MyBarcode = new Barcode2(MyDevice.ToString());

            MyBarcode.Enable();

            MyBarcode.Config.Reader.ReaderSpecific.LaserSpecific.AimDuration = 500;
            MyBarcode.Config.TriggerMode = TRIGGERMODES.HARD;

            ScanData MyData = MyBarcode.ScanWait(8000); // 5 seconds timeout

            if (MyData.Result == Symbol.Barcode2.Results.E_SCN_READTIMEOUT)   // Barcode was not scanned within 5 secs
            {
                txtBox.Text = "No Scan Found";
                MyBarcode.Dispose();
                //enable add product button
                button1.Enabled = true;
            }
            if (MyData.Result == Symbol.Barcode2.Results.SUCCESS) // Barcode was scanned successfully
            {
                txtBox.Text = MyData.ToString();
                MyBarcode.Dispose();
                //enable add product button
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (scannerActive)
            {
                wakeScanner(textBox1);
                
            }
        }
    }
}