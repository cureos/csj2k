// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{

    /// <summary> Class Description
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public class LookUpTableFPGamma : LookUpTableFP
    {
        internal double dfE;

        public LookUpTableFPGamma(ICCCurveType curve, int dwNumInput) : base(curve, dwNumInput)
        {
            // Gamma exponent for inverse transformation
            dfE = ICCCurveType.CurveGammaToDouble(curve.entry(0));
            for (var i = 0; i < dwNumInput; i++)
            {
                lut[i] = (float) Math.Pow((double) i / (dwNumInput - 1), dfE);
            }
        }

        /// <summary> Create an abbreviated string representation of a 16 bit lut.</summary>
        /// <returns> the lut as a String
        /// </returns>
        public override string ToString()
        {
            var rep = new System.Text.StringBuilder("[LookUpTableGamma ");
            rep.Append("dfe= " + dfE);
            rep.Append(", nentries= " + lut.Length);
            return rep.Append("]").ToString();
        }
    }
}
