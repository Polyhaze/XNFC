using System;
using System.Linq;
using Android.Nfc;
using Android.Nfc.Tech;

namespace Ultz.XNFC.Android
{
    /// <inheritdoc/>
    public class AndroidTag : ITag
    {
        private Ndef _ndef;
        private Tag _base;

        /// <inheritdoc/>
        public AndroidTag(Tag @base, NdefMessage message)
        {
            Records = message.GetRecords().Select(x => new NfcDefRecord() { Payload = x.GetPayload(), TypeNameFormat = AndroidNfc.GetTypeNameFormat(x.Tnf) }).ToArray();
            _ndef = Ndef.Get(@base);
            _base = @base;
        }

        /// <inheritdoc/>
        public bool Readable => true;

        /// <inheritdoc/>
        public bool Writable => _ndef?.IsWritable ?? false;

        /// <inheritdoc/>
        public NfcDefRecord[] Records { get; }

        /// <inheritdoc/>
        public bool Write(params NfcDefRecord[] records)
        {
            var message = new NdefMessage(records.Select(x =>
                new NdefRecord(AndroidNfc.GetTypeNameFormat(x.TypeNameFormat), null, null, x.Payload)).ToArray());
            if (_ndef != null)
            {
                if (!Writable)
                    return false;
                _ndef.Connect();
                // NFC tags can only store a small amount of data, this depends on the type of tag its.
                var size = message.ToByteArray().Length;
                if (_ndef.MaxSize < size)
                {
                    throw new InvalidOperationException("Not enough space on NFC tag");
                }

                _ndef.WriteNdefMessage(message);
                return true;
            }

            var format = NdefFormatable.Get(_base);
            format.Connect();
            format.Format(message);
            return true;
        }
    }
}