// This file is part of XNFC.
//
// You may modify and distribute XNFC under the terms
// of the MIT license. See the LICENSE file for details.

namespace Ultz.XNFC
{
    /// <summary>
    /// Chunk of data in <see cref="Ultz.XNFC.ITag"/>.
    /// </summary>
    public class NfcDefRecord
    {
        /// <summary>
        /// Format of <see cref="Payload"/>s content.
        /// </summary>
        public NDefTypeNameFormat TypeNameFormat { get; set; }

        /// <summary>
        /// Payload.
        /// </summary>
        public byte[] Payload { get; set; }
    }
}
