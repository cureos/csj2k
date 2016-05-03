// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using CSJ2K.j2k.image;
    using CSJ2K.j2k.image.input;

    internal class BitmapImageReader : ImgReader
    {
        #region FIELDS

        private readonly Bitmap bitmap;

        private readonly int rb;

        #endregion

        #region CONSTRUCTORS

        private BitmapImageReader(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            this.w = bitmap.Width;
            this.h = bitmap.Height;
            this.nc = GetNumberOfComponents(bitmap.PixelFormat);
            this.rb = GetRangeBits(bitmap.PixelFormat);
        }

        #endregion

        #region METHODS

        public override void close()
        {
            this.bitmap.Dispose();
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

            var data = new int[blk.w * blk.h];
            for (int y = blk.uly, xy = 0; y < blk.uly + blk.h; ++y)
            {
                for (var x = blk.ulx; x < blk.ulx + blk.w; ++x, ++xy)
                {
                    var color = this.bitmap.GetPixel(x, y);
                    data[xy] = c == 0 ? color.R : c == 1 ? color.G : color.B;
                }
            }

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

        internal static ImgReader Create(object imageObject)
        {
            var bitmap = imageObject as Bitmap;
            return bitmap == null ? null : new BitmapImageReader(bitmap);
        }

        private static int GetNumberOfComponents(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    return 1;
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException("pixelFormat");
            }
        }

        private static int GetRangeBits(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppGrayScale:
                    return 16;
                case PixelFormat.Format1bppIndexed:
                    return 1;
                case PixelFormat.Format4bppIndexed:
                    return 4;
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 8;
                default:
                    throw new ArgumentOutOfRangeException("pixelFormat");
            }
        }

        #endregion
    }
}
