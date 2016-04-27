// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.Runtime.InteropServices;

using Android.Graphics;
using Android.Util;

namespace CSJ2K.Util
{
    internal class AndroidBitmapImage : IImage
    {
        #region FIELDS

        private const int SizeOfArgb = 4;

        private readonly int _width;

        private readonly int _height;

        private readonly int _numberOfComponents;

        private readonly byte[] _bytes;

        #endregion

        #region CONSTRUCTORS

        internal AndroidBitmapImage(int width, int height, int numberOfComponents)
        {
            _width = width;
            _height = height;
            _numberOfComponents = numberOfComponents;
            _bytes = new byte[SizeOfArgb * width * height];
        }

        #endregion

        #region METHODS

        public T As<T>()
        {
            if (!typeof(Bitmap).IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException(
                    string.Format(
                        "Cannot cast to '{0}'; type must be assignable from '{1}'",
                        typeof(T).Name,
                        typeof(Bitmap).Name));
            }

            var pixels = new int[_width * _height];
            Buffer.BlockCopy(_bytes, 0, pixels, 0, _bytes.Length);

            return (T)(object)Bitmap.CreateBitmap(pixels, _width, _height, Bitmap.Config.Argb8888);
        }

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
