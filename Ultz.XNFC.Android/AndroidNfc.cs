using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.Nfc.Tech;

namespace Ultz.XNFC.Android
{
    public class AndroidNfc : ICrossNearFieldCommunication
    {
        private NfcAdapter _adapter;

        public event EventHandler<TagDetectedEventArgs> TagDetected;
        public bool Enabled => _adapter.IsEnabled;

        public bool Available => Application.Context.CheckCallingOrSelfPermission(Manifest.Permission.Nfc) ==
                                 Permission.Granted && _adapter != null;

        public object Association { get; set; }

        public Task StartListeningAsync()
        {
            if (!Enabled)
                throw new InvalidOperationException("NFC not available");

            if (!Available) // todo: offer possibility to open dialog
                throw new InvalidOperationException("NFC is not enabled");

            var activity = (Activity)Association;
            var tagDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            tagDetected.AddDataType("*/*");
            var filters = new[] { tagDetected };
            var intent = new Intent(activity, activity.GetType()).AddFlags(ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(activity, 0, intent, 0);
            _adapter.EnableForegroundDispatch(activity, pendingIntent, filters,
                new[] { new[] { Java.Lang.Class.FromType(typeof(Ndef)).Name } });
            Debug.WriteLine("Started Listening Async - XNFC");
            return Task.CompletedTask;
        }

        public Task StopListeningAsync()
        {
            Debug.WriteLine("Stopped Listening Async - XNFC");
            _adapter?.DisableForegroundDispatch((Activity)Association);
            return Task.CompletedTask;
        }

        public void Initialize()
        {
            Debug.WriteLine("Initialized - XNFC");
            _adapter = NfcAdapter.GetDefaultAdapter((Activity)Association);
        }

        public void Callback(object obj)
        {
            Debug.WriteLine("Callback: called - XNFC");
            var intent = (Intent)obj;
            var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
            var messages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages)?.Cast<NdefMessage>().ToList();

            Debug.WriteLine($"Callback: {messages?.Count} messages - XNFC");
            if (messages == null)
            {
                return;
            }

            foreach (var mess in messages)
            {
                var message = mess.GetRecords();
                var atag = new AndroidTag(tag, mess);
                foreach (var msg in message)
                    Debug.WriteLine("Callback: " + Encoding.UTF8.GetString(msg.GetPayload()));
                TagDetected?.Invoke(this,
                    new TagDetectedEventArgs()
                    {
                        Records = atag.Records,
                        Tag = atag
                    });
            }
        }

        public static NDefTypeNameFormat GetTypeNameFormat(short nativeRecordTnf)
        {
            switch (nativeRecordTnf)
            {
                case NdefRecord.TnfAbsoluteUri:
                    return NDefTypeNameFormat.AbsoluteUri;
                case NdefRecord.TnfEmpty:
                    return NDefTypeNameFormat.Empty;
                case NdefRecord.TnfExternalType:
                    return NDefTypeNameFormat.External;
                case NdefRecord.TnfMimeMedia:
                    return NDefTypeNameFormat.Media;
                case NdefRecord.TnfUnchanged:
                case NdefRecord.TnfUnknown:
                    return NDefTypeNameFormat.Unchanged;
                case NdefRecord.TnfWellKnown:
                    return NDefTypeNameFormat.WellKnown;
                default:
                    return NDefTypeNameFormat.Unknown;
            }
        }

        public static short GetTypeNameFormat(NDefTypeNameFormat nativeRecordTnf)
        {
            switch (nativeRecordTnf)
            {
                case NDefTypeNameFormat.AbsoluteUri:
                    return NdefRecord.TnfAbsoluteUri;
                case NDefTypeNameFormat.Empty:
                    return NdefRecord.TnfEmpty;
                case NDefTypeNameFormat.External:
                    return NdefRecord.TnfExternalType;
                case NDefTypeNameFormat.Media:
                    return NdefRecord.TnfMimeMedia;
                case NDefTypeNameFormat.Unchanged:
                    return NdefRecord.TnfUnchanged;
                case NDefTypeNameFormat.WellKnown:
                    return NdefRecord.TnfWellKnown;
                default:
                    return NdefRecord.TnfUnknown;
            }
        }
    }

    public class AndroidTag : ITag
    {
        private Ndef _ndef;
        private Tag _base;
        private NdefMessage _message;

        public AndroidTag(Tag @base, NdefMessage message)
        {
            Records = message.GetRecords().Select(x => new NfcDefRecord() { Payload = x.GetPayload(), TypeNameFormat = AndroidNfc.GetTypeNameFormat(x.Tnf) }).ToArray();
            _ndef = Ndef.Get(@base);
            _base = @base;
            _message = message;
        }
        public bool Readable => true;
        public bool Writable => _ndef?.IsWritable ?? false;
        public NfcDefRecord[] Records { get; }
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