// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Text;

using CSJ2K.j2k.image;

namespace CSJ2K.Color
{
    /// <summary> This class resamples the components of an image so that
    /// all have the same number of samples.  The current implementation
    /// only handles the case of 2:1 upsampling.
    /// 
    /// </summary>
    /// <seealso cref="ColorSpace">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public class Resampler : ColorSpaceMapper
    {
        /// <summary> Ctor resamples a BlkImgDataSrc so that all components
        /// have the same number of samples.
        /// 
        /// Note the present implementation does only two to one
        /// respampling in either direction (row, column).
        /// 
        /// </summary>
        /// <param name="src">-- Source of image data
        /// </param>
        /// <param name="csMap">-- provides colorspace info
        /// </param>
        public Resampler(BlkImgDataSrc src, ColorSpace csMap)
            : base(src, csMap)
        {
            int c;

            // Calculate the minimum and maximum subsampling factor
            // across all channels.
            int minX = src.getCompSubsX(0);
            int minY = src.getCompSubsY(0);
            int maxX = minX;
            int maxY = minY;

            for (c = 1; c < ncomps; ++c)
            {
                minX = Math.Min(minX, src.getCompSubsX(c));
                minY = Math.Min(minY, src.getCompSubsY(c));
                maxX = Math.Max(maxX, src.getCompSubsX(c));
                maxY = Math.Max(maxY, src.getCompSubsY(c));
            }

            // Throw an exception for other than 2:1 sampling.
            if ((maxX != 1 && maxX != 2) || (maxY != 1 && maxY != 2))
            {
                throw new ColorSpaceException("Upsampling by other than 2:1 not" + " supported");
            }
        }

        /// <summary> Return a DataBlk containing the requested component
        /// upsampled by the scale factor applied to the particular
        /// scaling direction
        /// 
        /// Returns, in the blk argument, a block of image data containing the
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
        /// <param name="outblk">Its coordinates and dimensions specify the area to
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
        /// <seealso cref="getCompData">
        /// </seealso>
        public override DataBlk getInternCompData(DataBlk outblk, int c)
        {
            // If the scaling factor of this channel is 1 in both
            // directions, simply return the source DataBlk.
            if (src.getCompSubsX(c) == 1 && src.getCompSubsY(c) == 1) return src.getInternCompData(outblk, c);

            int wfactor = src.getCompSubsX(c);
            int hfactor = src.getCompSubsY(c);
            if ((wfactor != 2 && wfactor != 1) || (hfactor != 2 && hfactor != 1)) throw new ArgumentException("Upsampling by other than 2:1" + " not supported");

            int leftedgeOut; // offset to the start of the output scanline
            int rightedgeOut; // offset to the end of the output

            // scanline + 1
            int leftedgeIn; // offset to the start of the input scanline  

            var y0Out = outblk.uly;
            var y1Out = y0Out + outblk.h - 1;

            var x0Out = outblk.ulx;
            var x1Out = x0Out + outblk.w - 1;

            var y0In = y0Out / hfactor;
            var y1In = y1Out / hfactor;

            var x0In = x0Out / wfactor;
            var x1In = x1Out / wfactor;

            // Calculate the requested height and width, requesting an extra
            // row and or for upsampled channels.
            int reqW = x1In - x0In + 1;
            int reqH = y1In - y0In + 1;

            // Initialize general input and output indexes
            int kOut = -1;
            int kIn = -1;
            int yIn;

            switch (outblk.DataType)
            {
                case DataBlk.TYPE_INT:

                    DataBlkInt inblkInt = new DataBlkInt(x0In, y0In, reqW, reqH);
                    inblkInt = (DataBlkInt)src.getInternCompData(inblkInt, c);
                    dataInt[c] = inblkInt.DataInt;

                    // Reference the working array   
                    int[] outdataInt = (int[])outblk.Data;

                    // Create data array if necessary
                    if (outdataInt == null || outdataInt.Length != outblk.w * outblk.h)
                    {
                        outdataInt = new int[outblk.h * outblk.w];
                        outblk.Data = outdataInt;
                    }

                    // The nitty-gritty.
                    for (int yOut = y0Out; yOut <= y1Out; ++yOut)
                    {
                        yIn = yOut / hfactor;

                        leftedgeIn = inblkInt.offset + (yIn - y0In) * inblkInt.scanw;
                        leftedgeOut = outblk.offset + (yOut - y0Out) * outblk.scanw;
                        rightedgeOut = leftedgeOut + outblk.w;

                        kIn = leftedgeIn;
                        kOut = leftedgeOut;

                        if ((x0Out & 0x1) == 1)
                        {
                            // first is odd do the pixel once.
                            outdataInt[kOut++] = dataInt[c][kIn++];
                        }

                        if ((x1Out & 0x1) == 0)
                        {
                            // last is even adjust loop bounds
                            rightedgeOut--;
                        }

                        while (kOut < rightedgeOut)
                        {
                            outdataInt[kOut++] = dataInt[c][kIn];
                            outdataInt[kOut++] = dataInt[c][kIn++];
                        }

                        if ((x1Out & 0x1) == 0)
                        {
                            // last is even do the pixel once.
                            outdataInt[kOut++] = dataInt[c][kIn];
                        }
                    }

                    outblk.progressive = inblkInt.progressive;
                    break;

                case DataBlk.TYPE_FLOAT:

                    DataBlkFloat inblkFloat = new DataBlkFloat(x0In, y0In, reqW, reqH);
                    inblkFloat = (DataBlkFloat)src.getInternCompData(inblkFloat, c);
                    dataFloat[c] = inblkFloat.DataFloat;

                    // Reference the working array   
                    float[] outdataFloat = (float[])outblk.Data;

                    // Create data array if necessary
                    if (outdataFloat == null || outdataFloat.Length != outblk.w * outblk.h)
                    {
                        outdataFloat = new float[outblk.h * outblk.w];
                        outblk.Data = outdataFloat;
                    }

                    // The nitty-gritty.
                    for (int yOut = y0Out; yOut <= y1Out; ++yOut)
                    {
                        yIn = yOut / hfactor;

                        leftedgeIn = inblkFloat.offset + (yIn - y0In) * inblkFloat.scanw;
                        leftedgeOut = outblk.offset + (yOut - y0Out) * outblk.scanw;
                        rightedgeOut = leftedgeOut + outblk.w;

                        kIn = leftedgeIn;
                        kOut = leftedgeOut;

                        if ((x0Out & 0x1) == 1)
                        {
                            // first is odd do the pixel once.
                            outdataFloat[kOut++] = dataFloat[c][kIn++];
                        }

                        if ((x1Out & 0x1) == 0)
                        {
                            // last is even adjust loop bounds
                            rightedgeOut--;
                        }

                        while (kOut < rightedgeOut)
                        {
                            outdataFloat[kOut++] = dataFloat[c][kIn];
                            outdataFloat[kOut++] = dataFloat[c][kIn++];
                        }

                        if ((x1Out & 0x1) == 0)
                        {
                            // last is even do the pixel once.
                            outdataFloat[kOut++] = dataFloat[c][kIn];
                        }
                    }

                    outblk.progressive = inblkFloat.progressive;
                    break;

                case DataBlk.TYPE_SHORT:
                case DataBlk.TYPE_BYTE:
                default:
                    // Unsupported output type. 
                    throw new ArgumentException("invalid source datablock " + "type");
            }

            return outblk;
        }

        /// <summary> Return an appropriate String representation of this Resampler instance.</summary>
        public override string ToString()
        {
            StringBuilder rep = new StringBuilder("[Resampler: ncomps= " + ncomps);
            StringBuilder body = new StringBuilder("  ");
            for (int i = 0; i < ncomps; ++i)
            {
                body.Append(eol);
                body.Append("comp[");
                body.Append(i);
                body.Append("] xscale= ");
                body.Append(imgdatasrc.getCompSubsX(i));
                body.Append(", yscale= ");
                body.Append(imgdatasrc.getCompSubsY(i));
            }

            rep.Append(ColorSpace.indent("  ", body));
            return rep.Append("]").ToString();
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
        /// <param name="outblk">Its coordinates and dimensions specify the area to
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
        public override DataBlk getCompData(DataBlk outblk, int c)
        {
            return getInternCompData(outblk, c);
        }

        /// <summary> Returns the height in pixels of the specified component in the
        /// overall image.
        /// </summary>
        public override int getCompImgHeight(int c)
        {
            return src.getCompImgHeight(c) * src.getCompSubsY(c);
        }

        /// <summary> Returns the width in pixels of the specified component in the
        /// overall image.
        /// </summary>
        public override int getCompImgWidth(int c)
        {
            return src.getCompImgWidth(c) * src.getCompSubsX(c);
        }

        /// <summary> Returns the component subsampling factor in the horizontal
        /// direction, for the specified component.
        /// </summary>
        public override int getCompSubsX(int c)
        {
            return 1;
        }

        /// <summary> Returns the component subsampling factor in the vertical
        /// direction, for the specified component.
        /// </summary>
        public override int getCompSubsY(int c)
        {
            return 1;
        }

        /// <summary> Returns the height in pixels of the specified tile-component.</summary>
        public override int getTileCompHeight(int t, int c)
        {
            return src.getTileCompHeight(t, c) * src.getCompSubsY(c);
        }

        /// <summary> Returns the width in pixels of the specified tile-component..</summary>
        public override int getTileCompWidth(int t, int c)
        {
            return src.getTileCompWidth(t, c) * src.getCompSubsX(c);
        }
    }
}
