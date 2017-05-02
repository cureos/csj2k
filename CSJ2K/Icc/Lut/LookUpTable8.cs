// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System.Text;
using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{

    /// <summary> Toplevel class for a byte [] lut.
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public abstract class LookUpTable8 : LookUpTable
    {
        /// <summary>Maximum output value of the LUT </summary>
        protected internal byte dwMaxOutput;

        /// <summary>The lut values.                 </summary>
        // Maximum output value of the LUT
        protected internal byte[] lut;

        /// <summary> Create an abbreviated string representation of a 16 bit lut.</summary>
        /// <returns> the lut as a String
        /// </returns>
        public override string ToString()
        {
            var rep = new StringBuilder("[LookUpTable8 ");
            //int row, col;
            rep.Append("max= " + dwMaxOutput);
            rep.Append(", nentries= " + dwMaxOutput);
            return rep.Append("]").ToString();
        }

        public virtual string toStringWholeLut()
        {
            var rep = new StringBuilder("LookUpTable8" + eol);
            rep.Append("maxOutput = " + dwMaxOutput + eol);
            for (var i = 0; i < dwNumInput; ++i)
                rep.Append("lut[" + i + "] = " + lut[i] + eol);
            return rep.Append("]").ToString();
        }

        protected internal LookUpTable8(int dwNumInput, byte dwMaxOutput) : base(null, dwNumInput)
        {
            lut = new byte[dwNumInput];
            this.dwMaxOutput = dwMaxOutput;
        }

        /// <summary> Create the string representation of a 16 bit lut.</summary>
        /// <returns> the lut as a String
        /// </returns>
        protected internal LookUpTable8(ICCCurveType curve, int dwNumInput, byte dwMaxOutput) : base(curve, dwNumInput)
        {
            this.dwMaxOutput = dwMaxOutput;
            lut = new byte[dwNumInput];
        }

        /// <summary> lut accessor</summary>
        /// <param name="index">of the element
        /// </param>
        /// <returns> the lut [index]
        /// </returns>
        public byte elementAt(int index)
        {
            return lut[index];
        }
    }
}
