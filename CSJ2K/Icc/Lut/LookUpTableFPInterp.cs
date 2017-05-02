// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Text;
using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{

    /// <summary> An interpolated floating point lut
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A.Kern
    /// </author>
    public class LookUpTableFPInterp : LookUpTableFP
    {
        /// <summary> Create an abbreviated string representation of a 16 bit lut.</summary>
        /// <returns> the lut as a String
        /// </returns>
        public override string ToString()
        {
            var rep = new StringBuilder("[LookUpTable32 ").Append(" nentries= " + lut.Length);
            return rep.Append("]").ToString();
        }

        /// <summary> Construct the lut from the curve data</summary>
        /// <oaram>   curve the data </oaram>
        /// <oaram>   dwNumInput the lut size </oaram>
        public LookUpTableFPInterp(ICCCurveType curve, int dwNumInput) : base(curve, dwNumInput)
        {
            var dfRatio = (curve.nEntries - 1) / (double) (dwNumInput - 1);

            for (var i = 0; i < dwNumInput; i++)
            {
                var dfTargetIndex = i * dfRatio; // Target index into interpolation table
                var dfLowIndex = Math.Floor(dfTargetIndex); // FP indices of interpolation points
                var dwLowIndex = (int) dfLowIndex; // Indices of interpolation points
                var dfHighIndex = Math.Ceiling(dfTargetIndex); // FP indices of interpolation points
                var dwHighIndex = (int) dfHighIndex; // Indices of interpolation points
                if (dwLowIndex == dwHighIndex)
                {
                    lut[i] = (float) ICCCurveType.CurveToDouble(curve.entry(dwLowIndex));
                }
                else
                {
                    var dfLow = ICCCurveType.CurveToDouble(curve.entry(dwLowIndex)); // Interpolation values
                    var dfHigh = ICCCurveType.CurveToDouble(curve.entry(dwHighIndex)); // Interpolation values
                    lut[i] = (float) (dfLow + (dfHigh - dfLow) * (dfTargetIndex - dfLowIndex));
                }
            }
        }
    }
}
