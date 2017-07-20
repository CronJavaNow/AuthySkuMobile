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
    public static class Scanner
    {
        public static int regValu(int numberOne, int numberTwo)
        {
            var value = numberOne + numberTwo;
            return value;
        }


        public static void wakeScanner(Button scanButton, TextBox textBox, bool scannerMode)
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

            if (MyData.Result == Symbol.Barcode2.Results.E_SCN_READTIMEOUT)   // Barcode was not scanned within 8 secs
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
    }
}
