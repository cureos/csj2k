// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{
    /// <summary> An interpolated 8 bit lut
    /// 
    /// </summary>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A.Kern
    /// </author>
    public class LookUpTable8Interp : LookUpTable8
    {
        /// <summary> Construct the lut from the curve data</summary>
        /// <oaram>   curve the data </oaram>
        /// <oaram>   dwNumInput the lut size </oaram>
        /// <oaram>   dwMaxOutput the lut max value </oaram>
        public LookUpTable8Interp(ICCCurveType curve, int dwNumInput, byte dwMaxOutput) : base(curve, dwNumInput,
            dwMaxOutput)
        {
            var dfRatio = (curve.count - 1) / (double) (dwNumInput - 1);

            for (var i = 0; i < dwNumInput; i++)
            {
                var dfTargetIndex = i * dfRatio; // Target index into interpolation table
                var dfLowIndex = Math.Floor(dfTargetIndex); // FP indices of interpolation points
                var dwLowIndex = (int) dfLowIndex; // Indices of interpolation points
                var dfHighIndex = Math.Ceiling(dfTargetIndex); // FP indices of interpolation points
                var dwHighIndex = (int) dfHighIndex; // Indices of interpolation points

                double dfOut; // Output LUT value
                if (dwLowIndex == dwHighIndex)
                    dfOut = ICCCurveType.CurveToDouble(curve.entry(dwLowIndex));
                else
                {
                    var dfLow = ICCCurveType.CurveToDouble(curve.entry(dwLowIndex)); // Interpolation values
                    var dfHigh = ICCCurveType.CurveToDouble(curve.entry(dwHighIndex)); // Interpolation values
                    dfOut = dfLow + (dfHigh - dfLow) * (dfTargetIndex - dfLowIndex);
                }

                lut[i] = (byte) Math.Floor(dfOut * dwMaxOutput + 0.5);
            }
        }
    }
}
