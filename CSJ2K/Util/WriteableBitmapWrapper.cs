// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;

#if NETFX_CORE
    using System.Runtime.InteropServices.WindowsRuntime;
    using Windows.UI.Xaml.Media.Imaging;
#elif SILVERLIGHT
    using System.Windows.Media.Imaging;
#else
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
#endif

    internal class WriteableBitmapWrapper : IBitmapWrapper
    {
        #region FIELDS

        private const int SizeOfArgb = 4;

        private readonly int _width;

        private readonly int _height;

        private readonly int _numberOfComponents;

        private readonly byte[] _bytes;

        #endregion

        #region CONSTRUCTORS

        internal WriteableBitmapWrapper(int width, int height, int numberOfComponents)
        {
            _width = width;
            _height = height;
            _numberOfComponents = numberOfComponents;
            _bytes = new byte[SizeOfArgb * width * height];
        }

        #endregion

        #region PROPERTIES

        public object Bitmap
        {
            get
            {
#if NETFX_CORE
                var wbm = new WriteableBitmap(_width, _height);
                _bytes.CopyTo(0, wbm.PixelBuffer, 0, _bytes.Length);
                return wbm;
#elif SILVERLIGHT
                var wbm = new WriteableBitmap(_width, _height);
                Buffer.BlockCopy(_bytes, 0, wbm.Pixels, 0, _bytes.Length);
                return wbm;
#else
                var wbm = new WriteableBitmap(_width, _height, 96.0, 96.0, PixelFormats.Pbgra32, null);
                wbm.Lock();
                Marshal.Copy(_bytes, 0, wbm.BackBuffer, _bytes.Length);
                wbm.AddDirtyRect(new Int32Rect(0, 0, _width, _height));
                wbm.Unlock();
                return wbm;
#endif
            }
        }

        #endregion

        #region METHODS

        public void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
        {
            switch (_numberOfComponents)
            {
                case 1:
                case 3:
                    var i = SizeOfArgb * (rowIndex + lineIndex * rowWidth);
                    var j = 0;
                    for (var k = 0; k < rowWidth; ++k)
                    {
                        _bytes[i++] = rowValues[j++];
                        _bytes[i++] = rowValues[j++];
                        _bytes[i++] = rowValues[j++];
                        _bytes[i++] = 0xff;
                    }
                    break;
                case 4:
                    Array.Copy(
                        rowValues,
                        0,
                        _bytes,
                        SizeOfArgb * (rowIndex + lineIndex * rowWidth),
                        SizeOfArgb * rowWidth);
                    break;
                default:
                    throw new InvalidOperationException("Number of components must be one of 1, 3 or 4.");
            }
        }

        #endregion
    }
}
