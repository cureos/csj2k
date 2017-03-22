// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;

namespace CSJ2K.Color
{
    /// <summary> This exception is thrown when the content of an
    /// image contains an incorrect colorspace box
    /// 
    /// </summary>
    /// <seealso cref="ColorSpaceMapper">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public class ColorSpaceException : Exception
    {
        /// <summary> Contruct with message</summary>
        /// <param name="msg">returned by getMessage()
        /// </param>
        public ColorSpaceException(string msg)
            : base(msg)
        {
        }
    }
}