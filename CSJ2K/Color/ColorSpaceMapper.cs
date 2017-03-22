// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;

using CSJ2K.Icc;
using CSJ2K.j2k.image;
using CSJ2K.j2k.util;

namespace CSJ2K.Color
{

    /// <summary> This is the base class for all modules in the colorspace and icc
    /// profiling steps of the decoding chain.  It is responsible for the
    /// allocation and iniitialization of all working storage.  It provides
    /// several utilities which are of generic use in preparing DataBlks
    /// for use and provides default implementations for the getCompData
    /// and getInternCompData methods.
    /// 
    /// </summary>
    /// <seealso cref="ColorSpace">
    /// </seealso>
    /// <version> 	1.0
    /// </version>
    /// <author> 	Bruce A. Kern
    /// </author>
    public abstract class ColorSpaceMapper : ImgDataAdapter, BlkImgDataSrc
    {
        /// <summary> Returns the parameters that are used in this class and implementing
        /// classes. It returns a 2D String array. Each of the 1D arrays is for a
        /// different option, and they have 3 elements. The first element is the
        /// option name, the second one is the synopsis and the third one is a long
        /// description of what the parameter is. The synopsis or description may
        /// be 'null', in which case it is assumed that there is no synopsis or
        /// description of the option, respectively. Null may be returned if no
        /// options are supported.
        /// 
        /// </summary>
        public static string[][] ParameterInfo { get; } = {
                    new[]
                        {
                            "IcolorSpacedebug", null,
                            "Print debugging messages during colorspace mapping.",
                            "off"
                        }
                };

        /// <summary> Arrange for the input DataBlk to receive an
        /// appropriately sized and typed data buffer
        /// </summary>
        /// <seealso cref="DataBlk">
        /// </seealso>
        protected static DataBlk InternalBuffer
        {
            set
            {
                switch (value.DataType)
                {
                    case DataBlk.TYPE_INT:
                        if (value.Data == null || ((int[])value.Data).Length < value.w * value.h) value.Data = new int[value.w * value.h];
                        break;

                    case DataBlk.TYPE_FLOAT:
                        if (value.Data == null || ((float[])value.Data).Length < value.w * value.h)
                        {
                            value.Data = new float[value.w * value.h];
                        }

                        break;

                    default:
                        throw new ArgumentException("Invalid output datablock type");
                }
            }
        }

        /// <summary>The prefix for ICC Profiler options </summary>
        public const char OPT_PREFIX = 'I';

        /// <summary>Platform dependant end of line String. </summary>
        protected internal static readonly string eol = Environment.NewLine;

        // Temporary data buffers needed during profiling.
        protected DataBlkInt[] inInt; // Integer input data.

        protected DataBlkFloat[] inFloat; // Floating point input data.

        protected DataBlkInt[] workInt; // Input data shifted to zero-offset

        protected DataBlkFloat[] workFloat; // Input data shifted to zero-offset.

        protected int[][] dataInt; // Points to input data.

        protected float[][] dataFloat; // Points to input data.

        protected float[][] workDataFloat; // References working data pixels.

        protected int[][] workDataInt; // References working data pixels.


        /* input data parameters by component */
        protected int[] shiftValueArray;

        protected int[] maxValueArray;

        protected int[] fixedPtBitsArray;

        /// <summary>Parameter Specs </summary>
        protected ParameterList pl;

        /// <summary>ColorSpace info </summary>
        protected ColorSpace csMap;

        /// <summary>Number of image components </summary>
        protected int ncomps;

        /// <summary>The image source. </summary>
        protected BlkImgDataSrc src;

        /// <summary>The image source data per component. </summary>
        protected DataBlk[] srcBlk;

        public sealed class ComputedComponents
        {
            private readonly int h;

            private readonly int w;

            private readonly int ulx;

            private readonly int uly;

            private readonly int offset;

            private readonly int scanw;

            public ColorSpaceMapper EnclosingInstance { get; }

            public ComputedComponents(ColorSpaceMapper enclosingInstance)
            {
                EnclosingInstance = enclosingInstance;
                h = w = ulx = uly = offset = scanw = -1;
            }

            public ComputedComponents(ColorSpaceMapper enclosingInstance, DataBlk db)
            {
                EnclosingInstance = enclosingInstance;
                h = db.h;
                w = db.w;
                ulx = db.ulx;
                uly = db.uly;
                offset = db.offset;
                scanw = db.scanw;
            }

            public bool Equals(ComputedComponents cc)
            {
                return h == cc.h && w == cc.w && ulx == cc.ulx && uly == cc.uly && offset == cc.offset
                       && scanw == cc.scanw;
            }
        }

        protected readonly ComputedComponents computed;

        /// <summary> Copy the DataBlk geometry from source to target
        /// DataBlk and assure that the target has an appropriate
        /// data buffer.
        /// </summary>
        /// <param name="tgt">has its geometry set.
        /// </param>
        /// <param name="src">used to get the new geometric parameters.
        /// </param>
        protected static void copyGeometry(DataBlk tgt, DataBlk src)
        {
            tgt.offset = 0;
            tgt.h = src.h;
            tgt.w = src.w;
            tgt.ulx = src.ulx;
            tgt.uly = src.uly;
            tgt.scanw = src.w;

            // Create data array if necessary
            InternalBuffer = tgt;
        }

        /// <summary> Factory method for creating instances of this class.</summary>
        /// <param name="src">-- source of image data
        /// </param>
        /// <param name="csMap">-- provides colorspace info
        /// </param>
        /// <returns> ColorSpaceMapper instance
        /// </returns>
        public static BlkImgDataSrc createInstance(BlkImgDataSrc src, ColorSpace csMap)
        {
            // Check parameters
            csMap.pl.checkList(OPT_PREFIX, ParameterList.toNameArray(ParameterInfo));

            // Perform ICCProfiling or ColorSpace tranfsormation.
            if (csMap.Method == ColorSpace.MethodEnum.ICC_PROFILED)
            {
                return ICCProfiler.createInstance(src, csMap);
            }

            switch (csMap.getColorSpace())
            {
                case ColorSpace.CSEnum.sRGB:
                    return new EnumeratedColorSpaceMapper(src, csMap);

                case ColorSpace.CSEnum.GreyScale:
                    return new EnumeratedColorSpaceMapper(src, csMap);

                case ColorSpace.CSEnum.sYCC:
                    return new SYccColorSpaceMapper(src, csMap);

                case ColorSpace.CSEnum.esRGB:
                    return new EsRgbColorSpaceMapper(src, csMap);

                case ColorSpace.CSEnum.Unknown:
                    return null;

                case ColorSpace.CSEnum.Illegal:
                default:
                    throw new ColorSpaceException("Bad color space specification in image");
            }
        }

        /// <summary> Ctor which creates an ICCProfile for the image and initializes
        /// all data objects (input, working, and output).
        /// 
        /// </summary>
        /// <param name="src">-- Source of image data
        /// </param>
        /// <param name="csMap">-- provides colorspace info
        /// 
        /// </param>
        protected internal ColorSpaceMapper(BlkImgDataSrc src, ColorSpace csMap)
            : base(src)
        {
            computed = new ComputedComponents(this);
            this.src = src;
            this.csMap = csMap;
            initialize();
        }

        /// <summary>General utility used by ctors </summary>
        private void initialize()
        {
            pl = csMap.pl;
            ncomps = src.NumComps;

            shiftValueArray = new int[ncomps];
            maxValueArray = new int[ncomps];
            fixedPtBitsArray = new int[ncomps];

            srcBlk = new DataBlk[ncomps];
            inInt = new DataBlkInt[ncomps];
            inFloat = new DataBlkFloat[ncomps];
            workInt = new DataBlkInt[ncomps];
            workFloat = new DataBlkFloat[ncomps];
            dataInt = new int[ncomps][];
            dataFloat = new float[ncomps][];
            workDataInt = new int[ncomps][];
            workDataFloat = new float[ncomps][];
            dataInt = new int[ncomps][];
            dataFloat = new float[ncomps][];

            /* For each component, get a reference to the pixel data and
			* set up working DataBlks for both integer and float output.
			*/
            for (int i = 0; i < ncomps; ++i)
            {
                shiftValueArray[i] = 1 << (src.getNomRangeBits(i) - 1);
                maxValueArray[i] = (1 << src.getNomRangeBits(i)) - 1;
                fixedPtBitsArray[i] = src.getFixedPoint(i);

                inInt[i] = new DataBlkInt();
                inFloat[i] = new DataBlkFloat();
                workInt[i] = new DataBlkInt { progressive = inInt[i].progressive };
                workFloat[i] = new DataBlkFloat { progressive = inFloat[i].progressive };
            }
        }

        /// <summary> Returns the number of bits, referred to as the "range bits",
        /// corresponding to the nominal range of the data in the specified
        /// component. If this number is <code>b</code> then for unsigned data the
        /// nominal range is between 0 and 2^b-1, and for signed data it is between
        /// -2^(b-1) and 2^(b-1)-1. For floating point data this value is not
        /// applicable.
        /// 
        /// </summary>
        /// <param name="c">The index of the component.
        /// 
        /// </param>
        /// <returns> The number of bits corresponding to the nominal range of the
        /// data. Fro floating-point data this value is not applicable and the
        /// return value is undefined.
        /// </returns>
        public virtual int getFixedPoint(int c)
        {
            return src.getFixedPoint(c);
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
        /// This method, in general, is less efficient than the
        /// 'getInternCompData()' method since, in general, it copies the
        /// data. However if the array of returned data is to be modified by the
        /// caller then this method is preferable.
        /// 
        /// If the data array in 'blk' is 'null', then a new one is created. If
        /// the data array is not 'null' then it is reused, and it must be large
        /// enough to contain the block's data. Otherwise an 'IndexOutOfRangeException' 
        /// is thrown.
        /// 
        /// The returned data may have its 'progressive' attribute set. In this
        /// case the returned data is only an approximation of the "final" data.
        /// 
        /// </summary>
        /// <param name="out_Renamed">Its coordinates and dimensions specify the area to return,
        /// relative to the current tile. If it contains a non-null data array,
        /// then it must be large enough. If it contains a null data array a new
        /// one is created. Some fields in this object are modified to return the
        /// data.
        /// 
        /// </param>
        /// <param name="c">The index of the component from which to get the data.
        /// 
        /// </param>
        /// <seealso cref="getInternCompData">
        /// 
        /// </seealso>
        public virtual DataBlk getCompData(DataBlk out_Renamed, int c)
        {
            return src.getCompData(out_Renamed, c);
        }

        /// <summary> Closes the underlying file or network connection from where the
        /// image data is being read.
        /// 
        /// </summary>
        public void close()
        {
            // Do nothing.
        }

        /// <summary> Returns true if the data read was originally signed in the specified
        /// component, false if not.
        /// 
        /// </summary>
        /// <param name="c">The index of the component, from 0 to C-1.
        /// 
        /// </param>
        /// <returns> true if the data was originally signed, false if not.
        /// 
        /// </returns>
        public bool isOrigSigned(int c)
        {
            return false;
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
        /// If possible, the data in the returned 'DataBlk' should be the
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
        public virtual DataBlk getInternCompData(DataBlk out_Renamed, int c)
        {
            return src.getInternCompData(out_Renamed, c);
        }
    }
}
