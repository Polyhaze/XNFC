// This file is part of XNFC.
//
// You may modify and distribute XNFC under the terms
// of the MIT license. See the LICENSE file for details.

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ultz.XNFC.Android", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo("Ultz.XNFC.iOS", AllInternalsVisible = true)]

namespace Ultz.XNFC
{
    /// <inheritdoc/>
    public class TagDetectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagDetectedEventArgs"/> class.
        /// </summary>
        /// <param name="tag">Tag's content.</param>
        internal TagDetectedEventArgs(ITag tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// <see cref="ITag"/>'s content.
        /// </summary>
        public NfcDefRecord[] Records => Tag.Records;

        /// <summary>
        /// The tag that triggered the NFC device.
        /// </summary>
        public ITag Tag { get; }
    }
}