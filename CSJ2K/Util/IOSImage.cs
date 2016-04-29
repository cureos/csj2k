// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System.Runtime.InteropServices;

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

		#region PROPERTIES

		protected override ByteOrder Order { get { return ByteOrder.RGBA; } }

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
			using (
                var context = new CGBitmapContext(
					this.Bytes,
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
