// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{

    /// <summary> A Gamma based 32 bit lut.
    /// 
    /// </summary>
    /// <seealso cref="ICCCurveType">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>

    public class LookUpTable32Gamma : LookUpTable32
    {
        /* Construct the lut    
        *   @param curve data 
        *   @param dwNumInput size of lut  
        *   @param dwMaxOutput max value of lut   
        */
        public LookUpTable32Gamma(ICCCurveType curve, int dwNumInput, int dwMaxOutput) : base(curve, dwNumInput,
            dwMaxOutput)
        {
            var dfE = ICCCurveType.CurveGammaToDouble(curve.entry(0)); // Gamma exponent for inverse transformation
            for (var i = 0; i < dwNumInput; i++)
            {
                lut[i] =
                    (int) System.Math.Floor(System.Math.Pow((double) i / (dwNumInput - 1), dfE) * dwMaxOutput + 0.5);
            }
        }
    }
}