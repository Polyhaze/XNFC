// This file is part of XNFC.
//
// You may modify and distribute XNFC under the terms
// of the MIT license. See the LICENSE file for details.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ultz.XNFC.Android", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo("Ultz.XNFC.iOS", AllInternalsVisible = true)]

namespace Ultz.XNFC
{
    /// <summary>
    /// Provides common NFC logic.
    /// </summary>
    public interface ICrossNearFieldCommunication
    {
        /// <summary>
        /// Fires when a NFC tag is detected.
        /// </summary>
        event EventHandler<TagDetectedEventArgs> TagDetected;

        /// <summary>
        /// If the device's nfc reader is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// If the device has a nfc reader.
        /// </summary>
        bool Available { get; }

        /// <summary>
        /// If the device can scan for NFC tags.
        /// </summary>
        bool CanScan { get; }

        /// <summary>
        /// If we are currently scanning for NFC tags.
        /// </summary>
        bool Scanning { get; }

        /// <summary>
        /// Anything the NFC is associated with.
        /// </summary>
        object Association { get; set; }

        /// <summary>
        /// Makes <see cref="ICrossNearFieldCommunication"/> start listening for NFC tags.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StartListeningAsync();

        /// <summary>
        /// Attempts to make <see cref="ICrossNearFieldCommunication"/> start listening for NFC tags.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> TryStartListeningAsync();

        /// <summary>
        /// Makes <see cref="ICrossNearFieldCommunication"/> stop listening for NFC tags.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StopListeningAsync();

        /// <summary>
        /// Attempts to make <see cref="ICrossNearFieldCommunication"/> stop listening for NFC tags.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> TryStopListeningAsync();

        /// <summary>
        /// Initializes <see cref="ICrossNearFieldCommunication"/>.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Invokes logic that fires <see cref="TagDetected"/>.
        /// </summary>
        /// <param name="obj">object with <see cref="ITag"/> logic.</param>
        void Callback(object obj);
    }
}