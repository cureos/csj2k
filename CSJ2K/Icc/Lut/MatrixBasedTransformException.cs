// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Icc.Lut
{
    /// <summary> Thrown by MatrixBasedTransformTosRGB
    /// 
    /// </summary>
    /// <seealso cref="MatrixBasedTransformTosRGB">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public class MatrixBasedTransformException : System.Exception
    {
        /// <summary> Contruct with message</summary>
        /// <param name="msg">returned by getMessage()
        /// </param>
        internal MatrixBasedTransformException(string msg) : base(msg)
        {
        }


        /// <summary> Empty constructor</summary>
        internal MatrixBasedTransformException()
        {
        }
    }
}
