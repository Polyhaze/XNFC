using System;
using System.Threading.Tasks;

namespace Ultz.XNFC.iOS
{
    public class AppleNfc : ICrossNearFieldCommunication
    {
        public event EventHandler<TagDetectedEventArgs> TagDetected;
        public bool Enabled { get; }
        public bool Available { get; }
        public object Association { get; set; }
        public Task StartListeningAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopListeningAsync()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Callback(object obj)
        {
            throw new NotImplementedException();
        }
    }
}