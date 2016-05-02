// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using CoreGraphics;

    using CoreImage;

    using UIKit;

    internal class IOSImage : ImageBase<CGImage>
    {
        #region CONSTRUCTORS

        internal IOSImage(int width, int height, int numberOfComponents)
            : base(width, height, numberOfComponents)
        {
        }

        #endregion

        #region METHODS

        public override T As<T>()
        {
            if (typeof(T) == typeof(UIImage))
            {
                return (T)(object)new UIImage((CGImage)this.GetImageObject());
            }

            if (typeof(T) == typeof(CIImage))
            {
                return (T)(object)new CIImage((CGImage)this.GetImageObject());
            }

            return (T)this.GetImageObject();
        }

        protected override object GetImageObject()
        {
            var length = this.Bytes.Length;
            var bytes = new byte[length];

            // Switch byte representation from BGRA to RGBA
            for (var k = 0; k < length; k += 4)
            {
                bytes[k] = this.Bytes[k + 2];
                bytes[k + 1] = this.Bytes[k + 1];
                bytes[k + 2] = this.Bytes[k];
                bytes[k + 3] = this.Bytes[k + 3];
            }

            using (
                var context = new CGBitmapContext(
                    bytes,
                    this.Width,
                    this.Height,
                    8,
                    SizeOfArgb * this.Width,
                    CGColorSpace.CreateDeviceRGB(),
                    CGImageAlphaInfo.PremultipliedLast))
            {
                return context.ToImage();
            }
        }

        #endregion
    }
}
