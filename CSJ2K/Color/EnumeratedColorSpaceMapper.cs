// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Text;

using BlkImgDataSrc = CSJ2K.j2k.image.BlkImgDataSrc;
using DataBlk = CSJ2K.j2k.image.DataBlk;

namespace CSJ2K.Color
{
    /// <summary> This class provides Enumerated ColorSpace API for the CSJ2K imaging chain
    /// by implementing the BlkImgDataSrc interface, in particular the getCompData
    /// and getInternCompData methods.
    /// 
    /// </summary>
    /// <seealso cref="ColorSpace">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public class EnumeratedColorSpaceMapper : ColorSpaceMapper
    {
        /// <summary> Ctor which creates an ICCProfile for the image and initializes
        /// all data objects (input, working, and output).
        /// 
        /// </summary>
        /// <param name="src">-- Source of image data
        /// </param>
        /// <param name="csMap">-- provides colorspace info
        /// </param>
        public EnumeratedColorSpaceMapper(BlkImgDataSrc src, ColorSpace csMap)
            : base(src, csMap)
        {
        }

        /// <summary> Returns, in the blk argument, a block of image data containing the
        /// specifed rectangular area, in the specified component. The data is
        /// returned, as a copy of the internal data, therefore the returned data
        /// can be modified "in place".
        /// 
        /// The rectangular area to return is specified by the 'ulx', 'uly', 'w'
        /// and 'h' members of the 'blk' argument, relative to the current
        /// tile. These members are not modified by this method. The 'offset' of
        /// the returned data is 0, and the 'scanw' is the same as the block's
        /// width. See the 'DataBlk' class.
        /// 
        /// If the data array in 'blk' is 'null', then a new one is created. If
        /// the data array is not 'null' then it is reused, and it must be large
        /// enough to contain the block's data. Otherwise an 'IndexOutOfRangeException' 
        /// is thrown.
        /// 
        /// The returned data has its 'progressive' attribute set to that of the
        /// input data.
        /// 
        /// </summary>
        /// <param name="out_Renamed">Its coordinates and dimensions specify the area to
        /// return. If it contains a non-null data array, then it must have the
        /// correct dimensions. If it contains a null data array a new one is
        /// created. The fields in this object are modified to return the data.
        /// 
        /// </param>
        /// <param name="c">The index of the component from which to get the data. Only 0
        /// and 3 are valid.
        /// 
        /// </param>
        /// <returns> The requested DataBlk
        /// 
        /// </returns>
        /// <seealso cref="getInternCompData">
        /// 
        /// </seealso>
        public override DataBlk getCompData(DataBlk out_Renamed, int c)
        {
            return src.getCompData(out_Renamed, c);
        }

        /// <summary> Returns, in the blk argument, a block of image data containing the
        /// specifed rectangular area, in the specified component. The data is
        /// returned, as a reference to the internal data, if any, instead of as a
        /// copy, therefore the returned data should not be modified.
        /// 
        /// The rectangular area to return is specified by the 'ulx', 'uly', 'w'
        /// and 'h' members of the 'blk' argument, relative to the current
        /// tile. These members are not modified by this method. The 'offset' and
        /// 'scanw' of the returned data can be arbitrary. See the 'DataBlk' class.
        /// 
        /// This method, in general, is more efficient than the 'getCompData()'
        /// method since it may not copy the data. However if the array of returned
        /// data is to be modified by the caller then the other method is probably
        /// preferable.
        /// 
        ///If possible, the data in the returned 'DataBlk' should be the
        /// internal data itself, instead of a copy, in order to increase the data
        /// transfer efficiency. However, this depends on the particular
        /// implementation (it may be more convenient to just return a copy of the
        /// data). This is the reason why the returned data should not be modified.
        /// 
        /// If the data array in <code>blk</code> is <code>null</code>, then a new one
        /// is created if necessary. The implementation of this interface may
        /// choose to return the same array or a new one, depending on what is more
        /// efficient. Therefore, the data array in <code>blk</code> prior to the
        /// method call should not be considered to contain the returned data, a
        /// new array may have been created. Instead, get the array from
        /// <code>blk</code> after the method has returned.
        /// 
        /// The returned data may have its 'progressive' attribute set. In this
        /// case the returned data is only an approximation of the "final" data.
        /// 
        /// </summary>
        /// <param name="out_Renamed">Its coordinates and dimensions specify the area to return,
        /// relative to the current tile. Some fields in this object are modified
        /// to return the data.
        /// 
        /// </param>
        /// <param name="c">The index of the component from which to get the data.
        /// 
        /// </param>
        /// <returns> The requested DataBlk
        /// 
        /// </returns>
        /// <seealso cref="getCompData">
        /// 
        /// </seealso>
        public override DataBlk getInternCompData(DataBlk out_Renamed, int c)
        {
            return src.getInternCompData(out_Renamed, c);
        }

        public override string ToString()
        {
            int i;
            StringBuilder rep_nComps = new StringBuilder("ncomps= ").Append(Convert.ToString(ncomps));

            StringBuilder rep_fixedValue = new StringBuilder("fixedPointBits= (");
            StringBuilder rep_shiftValue = new StringBuilder("shiftValue= (");
            StringBuilder rep_maxValue = new StringBuilder("maxValue= (");

            for (i = 0; i < ncomps; ++i)
            {
                if (i != 0)
                {
                    rep_shiftValue.Append(", ");
                    rep_maxValue.Append(", ");
                    rep_fixedValue.Append(", ");
                }

                rep_shiftValue.Append(Convert.ToString(shiftValueArray[i]));
                rep_maxValue.Append(Convert.ToString(maxValueArray[i]));
                rep_fixedValue.Append(Convert.ToString(fixedPtBitsArray[i]));
            }

            rep_shiftValue.Append(")");
            rep_maxValue.Append(")");
            rep_fixedValue.Append(")");

            StringBuilder rep = new StringBuilder("[EnumeratedColorSpaceMapper ");
            rep.Append(rep_nComps);
            rep.Append(eol).Append("  ").Append(rep_shiftValue);
            rep.Append(eol).Append("  ").Append(rep_maxValue);
            rep.Append(eol).Append("  ").Append(rep_fixedValue);

            return rep.Append("]").ToString();
        }
    }
}
