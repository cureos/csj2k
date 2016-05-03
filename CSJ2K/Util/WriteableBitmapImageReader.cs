// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using CSJ2K.j2k.image;
    using CSJ2K.j2k.image.input;

    internal class WriteableBitmapImageReader : ImgReader
    {
        #region FIELDS

        private readonly WriteableBitmap wbm;

        private readonly int rb;

        #endregion

        #region CONSTRUCTORS

        private WriteableBitmapImageReader(WriteableBitmap wbm)
        {
            this.wbm = wbm;
            this.w = wbm.PixelWidth;
            this.h = wbm.PixelHeight;
            this.nc = GetNumberOfComponents(wbm.Format);
            this.rb = GetRangeBits(wbm.Format);
        }

        #endregion

        #region METHODS

        public override void close()
        {
            // Do nothing.;
        }

        public override bool isOrigSigned(int c)
        {
            if (c < 0 || c >= this.nc)
            {
                throw new ArgumentOutOfRangeException("c");
            }

            return false;
        }

        public override int getFixedPoint(int c)
        {
            if (c < 0 || c >= this.nc)
            {
                throw new ArgumentOutOfRangeException("c");
            }

            return 0;
        }

        public override DataBlk getInternCompData(DataBlk blk, int c)
        {
            if (c < 0 || c >= this.nc)
            {
                throw new ArgumentOutOfRangeException("c");
            }

            Array data = new int[blk.w * blk.h];
            // TODO Implement pixel reading.

            blk.offset = 0;
            blk.scanw = blk.w;
            blk.progressive = false;
            blk.Data = data;

            return blk;
        }

        public override DataBlk getCompData(DataBlk blk, int c)
        {
            var newBlk = new DataBlkInt(blk.ulx, blk.uly, blk.w, blk.h);
            return this.getInternCompData(newBlk, c);
        }

        public override int getNomRangeBits(int c)
        {
            if (c < 0 || c >= this.nc)
            {
                throw new ArgumentOutOfRangeException("c");
            }

            return this.rb;
        }

        public static ImgReader Create(object imageObject)
        {
            var wbm = imageObject as WriteableBitmap;
            return wbm == null ? null : new WriteableBitmapImageReader(wbm);
        }

        private static int GetNumberOfComponents(PixelFormat pixelFormat)
        {
            if (pixelFormat.Equals(PixelFormats.BlackWhite) || pixelFormat.Equals(PixelFormats.Gray2)
                || pixelFormat.Equals(PixelFormats.Gray4) || pixelFormat.Equals(PixelFormats.Gray8)
                || pixelFormat.Equals(PixelFormats.Gray16))
            {
                return 1;
            }

            if (pixelFormat.Equals(PixelFormats.Bgra32) || pixelFormat.Equals(PixelFormats.Bgr24)
                || pixelFormat.Equals(PixelFormats.Bgr32) || pixelFormat.Equals(PixelFormats.Pbgra32)
                || pixelFormat.Equals(PixelFormats.Rgb24))
            {
                return 3;
            }

            throw new ArgumentOutOfRangeException("pixelFormat");
        }

        private static int GetRangeBits(PixelFormat pixelFormat)
        {
            return pixelFormat.BitsPerPixel;
        }

        #endregion
    }
}
