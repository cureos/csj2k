// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;

    using Android.Graphics;

    internal class AndroidBitmapImage : ImageBase<Bitmap>
    {
        #region CONSTRUCTORS

        internal AndroidBitmapImage(int width, int height, byte[] bytes)
            : base(width, height, bytes)
        {
        }

        #endregion

        #region METHODS

        protected override object GetImageObject()
        {
            var pixels = new int[this.Width * this.Height];
            Buffer.BlockCopy(this.Bytes, 0, pixels, 0, this.Bytes.Length);

            return Bitmap.CreateBitmap(pixels, this.Width, this.Height, Bitmap.Config.Argb8888);
        }

        #endregion
    }
}
