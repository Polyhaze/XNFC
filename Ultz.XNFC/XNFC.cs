using System;
using System.Threading.Tasks;

namespace Ultz.XNFC
{
    public class XNFC
    {
        private static Type _type;
        private static ICrossNearFieldCommunication _nfc;
        private static object _association;
        public static ICrossNearFieldCommunication GetApi()
        {
            return (ICrossNearFieldCommunication) Activator.CreateInstance(_type);
            
        }

        public static void GloballyStartScanning(ICrossNearFieldCommunication nfc)
        {
            _nfc = nfc;
            _nfc.Association = _association;
            _nfc.Initialize();
            _nfc.StartListeningAsync().GetAwaiter().GetResult();
        }

        public static void GloballyStopScanning()
        {
            _nfc?.StopListeningAsync().GetAwaiter().GetResult();
            _nfc = null;
        }

        public static void GlobalCallback(object obj)
        {
            _nfc?.Callback(obj);
        }

        public static void GloballyAssociate(object obj)
        {
            _association = obj;
        }
        public static void Register<T>() where T : ICrossNearFieldCommunication
        {
            _type = typeof(T);
        }
    }
}