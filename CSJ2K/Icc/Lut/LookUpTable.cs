// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using ICCCurveType = CSJ2K.Icc.Tags.ICCCurveType;

namespace CSJ2K.Icc.Lut
{
	/// <summary> Toplevel class for a lut.  All lookup tables must
	/// extend this class.
	/// 
	/// </summary>
	/// <version> 	1.0
	/// </version>
	/// <author> 	Bruce A. Kern
	/// </author>
	public abstract class LookUpTable
	{
		/// <summary>End of line string.             </summary>
		protected internal static readonly System.String eol = System.Environment.NewLine;

		/// <summary>The curve data                  </summary>
		protected internal ICCCurveType curve = null;

		/// <summary>Number of values in created lut </summary>
		protected internal int dwNumInput = 0;
		
		
		/// <summary> For subclass usage.</summary>
		/// <param name="curve">The curve data  
		/// </param>
		/// <param name="dwNumInput">Number of values in created lut
		/// </param>
		protected internal LookUpTable(ICCCurveType curve, int dwNumInput)
		{
			this.curve = curve;
			this.dwNumInput = dwNumInput;
		}
	}
}
