using System;
using System.Threading.Tasks;

namespace Ultz.XNFC
{
    public interface ICrossNearFieldCommunication
    {
        event EventHandler<TagDetectedEventArgs> TagDetected;
        bool Enabled { get; }
        bool Available { get; }
        object Association { get; set; }
        Task StartListeningAsync();
        Task StopListeningAsync();
        void Initialize();
        void Callback(object obj);
    }
    public enum NDefTypeNameFormat
    {
        AbsoluteUri,
        Empty,
        Media,
        External,
        WellKnown,
        Unchanged,
        Unknown
    }
    public class NfcDefRecord
    {
        public NDefTypeNameFormat TypeNameFormat { get; set; }
        public byte[] Payload { get; set; }
    }

    public class TagDetectedEventArgs : EventArgs
    {
        public NfcDefRecord[] Records { get; set; }
        public ITag Tag { get; set; }
    }

    public interface ITag
    {
        bool Readable { get; }
        bool Writable { get; }
        NfcDefRecord[] Records { get; }
        bool Write(params NfcDefRecord[] records);
    }
}