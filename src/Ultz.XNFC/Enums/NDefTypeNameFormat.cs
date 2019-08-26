// This file is part of XNFC.
//
// You may modify and distribute XNFC under the terms
// of the MIT license. See the LICENSE file for details.

namespace Ultz.XNFC
{
    /// <summary>
    /// Format of the NDEF tag content.
    /// </summary>
    public enum NDefTypeNameFormat
    {
        /// <summary>
        /// URI based on the type field.
        /// </summary>
        AbsoluteUri,

        /// <summary>
        /// Tag has no content.
        /// </summary>
        Empty,

        /// <summary>
        /// MIME type based on the type field.
        /// </summary>
        Media,

        /// <summary>
        /// URI based on the URN in the type field.
        /// </summary>
        External,

        /// <summary>
        /// MIME type or URI depending on the Record Type Definition (RTD)
        /// </summary>
        WellKnown,

        /// <summary>
        /// Invalid content in first record.
        /// </summary>
        Unchanged,

        /// <summary>
        /// Tag has content but we don't know the type of content.
        /// </summary>
        Unknown,
    }
}
