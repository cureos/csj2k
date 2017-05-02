// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;

namespace CSJ2K.Icc.Lut
{
    /// <summary> Exception thrown by MonochromeTransformTosRGB.
    /// 
    /// </summary>
    /// <seealso cref="MonochromeTransformTosRGB">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public class MonochromeTransformException : Exception
    {
        /// <summary> Contruct with message</summary>
        /// <param name="msg">returned by getMessage()
        /// </param>
        internal MonochromeTransformException(string msg) : base(msg)
        {
        }

        /// <summary> Empty constructor</summary>
        internal MonochromeTransformException()
        {
        }
    }
}
