// This file is part of XNFC.
//
// You may modify and distribute XNFC under the terms
// of the MIT license. See the LICENSE file for details.

namespace Ultz.XNFC
{
    /// <summary>
    /// Tag inside of NFC device, contains all content of NFC device.
    /// </summary>
    public interface ITag
    {
        /// <summary>
        /// If we can read the <see cref="ITag"/>'s content.
        /// </summary>
        bool Readable { get; }

        /// <summary>
        /// If we can write to the <see cref="ITag"/>.
        /// </summary>
        bool Writable { get; }

        /// <summary>
        /// <see cref="ITag"/>'s content.
        /// </summary>
        NfcDefRecord[] Records { get; }

        /// <summary>
        /// Writes <see cref="records"/> to this <see cref="ITag"/>.
        /// </summary>
        /// <param name="records">Content to write to the <see cref="ITag"/>.</param>
        /// <returns>If we was successful at writing all the content to the <see cref="ITag"/>.</returns>
        bool Write(params NfcDefRecord[] records);
    }
}
