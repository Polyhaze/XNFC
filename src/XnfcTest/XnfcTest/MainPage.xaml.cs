using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultz.XNFC;
using Xamarin.Forms;

namespace XnfcTest
{
    public partial class MainPage : ContentPage
    {
        private ICrossNearFieldCommunication _xnfc;
        public MainPage()
        {
            _xnfc = XNFC.GetApi();
            _xnfc.TagDetected += XnfcOnTagDetected;
            InitializeComponent();
        }

        private void XnfcOnTagDetected(object sender, TagDetectedEventArgs e)
        {
            Console.WriteLine("Receipt of " + Encoding.ASCII.GetString(e.Records[0].Payload));
            Device.BeginInvokeOnMainThread(async () =>
            {
                var x = await DisplayAlert("NFC", Encoding.ASCII.GetString(e.Records[0].Payload), "Write", "Ok");
                if (x)
                {
                    e.Tag.Write(new NfcDefRecord()
                    {
                        Payload = new byte[] {0x44, 0x79, 0x6c, 0x61, 0x6e, 0x20, 0x50, 0x65, 0x72, 0x6b, 0x73},
                        TypeNameFormat = NDefTypeNameFormat.Unknown
                    });
                }
            });
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Text == "Scanning")
            {
                XNFC.GloballyStopScanning();
                btn.Text = "Scan";
            }
            else
            {
                XNFC.GloballyStartScanning(_xnfc);
                btn.Text = "Scanning";
            }
        }
    }
}
