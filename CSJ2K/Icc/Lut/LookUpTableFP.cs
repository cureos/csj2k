// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{

    /// <summary> Toplevel class for a float [] lut.
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public abstract class LookUpTableFP : LookUpTable
    {

        /// <summary>The lut values. </summary>
        public float[] lut;

        /// <summary> Factory method for getting a lut from a given curve.</summary>
        /// <param name="curve"> the data
        /// </param>
        /// <param name="dwNumInput">the size of the lut 
        /// </param>
        /// <returns> the lookup table
        /// </returns>
        public static LookUpTableFP createInstance(ICCCurveType curve, int dwNumInput)
        {

            if (curve.nEntries == 1)
                return new LookUpTableFPGamma(curve, dwNumInput);
            else
                return new LookUpTableFPInterp(curve, dwNumInput);
        }

        /// <summary> Construct an empty lut</summary>
        /// <param name="dwNumInput">the size of the lut t lut.
        /// </param>
        /// <param name="curve">max output value of the lut
        /// </param>
        protected internal LookUpTableFP(ICCCurveType curve, int dwNumInput) : base(curve, dwNumInput)
        {
            lut = new float[dwNumInput];
        }

        /// <summary> lut accessor</summary>
        /// <param name="index">of the element
        /// </param>
        /// <returns> the lut [index]
        /// </returns>
        public float elementAt(int index)
        {
            return lut[index];
        }
    }
}
