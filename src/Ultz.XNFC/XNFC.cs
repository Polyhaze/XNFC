// This file is part of XNFC.
//
// You may modify and distribute XNFC under the terms
// of the MIT license. See the LICENSE file for details.

using System;

namespace Ultz.XNFC
{
    public class XNFC
    {
        private static Type _type;

        private static ICrossNearFieldCommunication _nfc;

        public static object Association { get; private set; }

        /// <summary>
        /// Get's the api to use.
        /// </summary>
        /// <returns>Api to use.</returns>
        public static ICrossNearFieldCommunication GetApi()
        {
            return (ICrossNearFieldCommunication) Activator.CreateInstance(_type);
        }

        /// <summary>
        /// Starts scanning for nfc tags.
        /// </summary>
        /// <param name="nfc"><see cref="ICrossNearFieldCommunication"/> to use.</param>
        public async static void GloballyStartScanning(ICrossNearFieldCommunication nfc)
        {
            _nfc = nfc;
            _nfc.Association = Association;
            _nfc.Initialize();
            await _nfc.StartListeningAsync();
        }

        /// <summary>
        /// Attempts to start scanning for nfc tags.
        /// </summary>
        /// <param name="nfc"><see cref="ICrossNearFieldCommunication"/> to use.</param>
        public async static void TryGloballyStartScanning(ICrossNearFieldCommunication nfc)
        {
            try
            {
                _nfc = nfc;
                _nfc.Association = Association;
                _nfc.Initialize();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to start scanning - XNFC");
                Console.WriteLine($"Error: {e}");
                return;
            }

            if (!await _nfc?.TryStartListeningAsync())
            {
                Console.WriteLine("Unable to start scanning, check console for error - XNFC");
            }
        }

        /// <summary>
        /// Stops scanning for nfc tags.
        /// </summary>
        public async static void GloballyStopScanning()
        {
            await _nfc?.StopListeningAsync();
            _nfc = null;
        }

        /// <summary>
        /// Attempts to stop scanning for nfc tags.
        /// </summary>
        public async static void TryGloballyStopScanning()
        {
            if (await _nfc?.TryStopListeningAsync())
            {
                _nfc = null;
            }
            else
            {
                Console.WriteLine("Unable to stop scanning, check console for error - XNFC");
            }
        }

        /// <summary>
        /// Calls <see cref="ICrossNearFieldCommunication.Callback"/>.
        /// </summary>
        /// <param name="obj"><see cref="ICrossNearFieldCommunication.Callback(object)"/> parameter.</param>
        public static void GlobalCallback(object obj)
        {
            _nfc?.Callback(obj);
        }

        /// <summary>
        /// Updates the Association.
        /// </summary>
        /// <param name="obj">Association parameter.</param>
        public static void GloballyAssociate(object obj)
        {
            Association = obj;
        }

        internal static void Register<T>()
            where T : ICrossNearFieldCommunication
        {
            _type = typeof(T);
        }
    }
}