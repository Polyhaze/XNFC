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
    /// <inheritdoc/>
    public class AndroidNfc : ICrossNearFieldCommunication
    {
        private NfcAdapter _adapter;

        /// <inheritdoc/>
        public event EventHandler<TagDetectedEventArgs> TagDetected;

        /// <inheritdoc/>
        public bool Enabled => _adapter.IsEnabled;

        /// <inheritdoc/>
        public bool Available => Application.Context.CheckCallingOrSelfPermission(Manifest.Permission.Nfc) ==
                                 Permission.Granted && _adapter != null;

        /// <inheritdoc/>
        public object Association { get; set; }

        /// <inheritdoc/>
        public bool CanScan => Available && Enabled;

        /// <inheritdoc/>
        public bool Scanning { get; private set; }

        /// <inheritdoc/>
        public Task StartListeningAsync()
        {
            if (!Enabled)
            {
                throw new InvalidOperationException("NFC not available");
            }

            if (!Available) // todo: offer possibility to open dialog
            {
                throw new InvalidOperationException("NFC is not enabled");
            }

            var activity = (Activity)Association;

            var tagDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            tagDetected.AddDataType("*/*");

            var filters = new[] { tagDetected };

            var intent = new Intent(activity, activity.GetType()).AddFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(activity, 0, intent, 0);

            _adapter.EnableForegroundDispatch(activity, pendingIntent, filters,
                new[] { new[] { Java.Lang.Class.FromType(typeof(Ndef)).Name } });

            Scanning = true;
            Debug.WriteLine("Started Listening Async - XNFC");
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<bool> TryStartListeningAsync()
        {
            try
            {
                if (_adapter == null)
                {
                    Initialize();
                }

                if (CanScan)
                {
                    await StartListeningAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't start listening for nfc tags - XNFC");
                Console.WriteLine($"Error: {e}");
                return false;
            }
            return true;
        }

        /// <inheritdoc/>
        public Task StopListeningAsync()
        {
            _adapter?.DisableForegroundDispatch((Activity)Association);
            Scanning = false;
            Debug.WriteLine("Stopped Listening Async - XNFC");

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<bool> TryStopListeningAsync()
        {
            try
            {
                if (Scanning)
                {
                    await StopListeningAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't stop listening for nfc tags - XNFC");
                Console.WriteLine($"Error: {e}");
                return false;
            }
            return true;
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            _adapter = NfcAdapter.GetDefaultAdapter((Activity)Association);

            Debug.WriteLine("Initialized - XNFC");
        }

        /// <inheritdoc/>
        public void Callback(object obj)
        {
            Debug.WriteLine("Callback: called - XNFC");
            if (obj is Intent intent)
            {
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
                        new TagDetectedEventArgs(atag));
                }
            }
            else
            {
                Console.WriteLine($"obj is not Intent, can't do callback - XNFC");
            }
        }

        /// <summary>
        /// Get's the format's name
        /// </summary>
        /// <param name="nativeRecordTnf">format to get name of</param>
        /// <returns>Format's name</returns>
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

        /// <summary>
        /// Get's the format's short
        /// </summary>
        /// <param name="nativeRecordTnf">format to get short of</param>
        /// <returns>Format's short</returns>
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
}