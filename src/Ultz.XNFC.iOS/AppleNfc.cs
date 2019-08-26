using System;
using System.Threading.Tasks;

namespace Ultz.XNFC.iOS
{
    /// <inheritdoc/>
    public class AppleNfc : ICrossNearFieldCommunication
    {
        /// <inheritdoc/>
        public event EventHandler<TagDetectedEventArgs> TagDetected;

        /// <inheritdoc/>
        public bool Enabled { get; }

        /// <inheritdoc/>
        public bool Available { get; }

        /// <inheritdoc/>
        public object Association { get; set; }

        /// <inheritdoc/>
        public bool CanScan { get; }

        /// <inheritdoc/>
        public bool Scanning { get; }

        /// <inheritdoc/>
        public Task StartListeningAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> TryStartListeningAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task StopListeningAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> TryStopListeningAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Callback(object obj)
        {
            throw new NotImplementedException();
        }
    }
}