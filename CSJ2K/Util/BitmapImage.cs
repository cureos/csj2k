// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System.Drawing;
    using System.Drawing.Imaging;

    internal class BitmapImage : ImageBase<Image>
    {
        #region CONSTRUCTORS

        internal BitmapImage(int width, int height, int numberOfComponents)
            : base(width, height, numberOfComponents)
        {
        }

        #endregion

        #region METHODS

        protected override object GetImageObject()
        {
            var bitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);

            var dstdata = bitmap.LockBits(
                new Rectangle(0, 0, this.Width, this.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            var ptr = dstdata.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(this.Bytes, 0, ptr, this.Bytes.Length);
            bitmap.UnlockBits(dstdata);

            return bitmap;
        }

        #endregion
    }
}
