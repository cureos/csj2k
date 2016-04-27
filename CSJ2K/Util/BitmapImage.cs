// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    internal class BitmapImage : IImage
    {
        #region FIELDS

        private readonly Bitmap _bitmap;

        #endregion

        #region CONSTRUCTORS

        internal BitmapImage(int width, int height, int numberOfComponents)
        {
            PixelFormat pixelFormat;
            switch (numberOfComponents)
            {
                case 1:
                case 3:
                    pixelFormat = PixelFormat.Format24bppRgb;
                    break;
                case 4:
                    pixelFormat = PixelFormat.Format32bppArgb;
                    break;
                default:
                    throw new InvalidOperationException(
                        "Unsupported PixelFormat.  " + numberOfComponents + " components.");
            }

            _bitmap = new Bitmap(width, height, pixelFormat);
        }

        #endregion

        #region METHODS

        public T As<T>()
        {   
            if (!typeof(Image).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException(
                    string.Format(
                        "Cannot cast to '{0}'; type must be assignable from '{1}'",
                        typeof(T).Name,
                        typeof(Image).Name));
            }

            return (T)(object)_bitmap;
        }

        public void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
        {
            var dstdata = _bitmap.LockBits(
                new Rectangle(rowIndex, lineIndex, rowWidth, 1),
                ImageLockMode.ReadWrite,
                _bitmap.PixelFormat);

            var ptr = dstdata.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(rowValues, 0, ptr, rowValues.Length);
            _bitmap.UnlockBits(dstdata);
        }

        #endregion
    }
}
