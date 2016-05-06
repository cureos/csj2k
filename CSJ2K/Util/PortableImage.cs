// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;

    public sealed class PortableImage : IImage
    {
        #region FIELDS

        private const int SizeOfArgb = 4;

        #endregion

        #region CONSTRUCTORS

        internal PortableImage(int width, int height, int numberOfComponents)
        {
            this.Width = width;
            this.Height = height;
            this.NumberOfComponents = numberOfComponents;
            this.Bytes = new byte[SizeOfArgb * width * height];
        }

        #endregion

        #region PROPERTIES

        internal int Width { get; }

        internal int Height { get; }

        public int NumberOfComponents { get; }

        internal byte[] Bytes { get; }

        #endregion

        #region METHODS

        public T As<T>()
        {
            var image = ImageFactory.New(this.Width, this.Height, this.Bytes);
            return image.As<T>();
        }

        internal void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
        {
            switch (this.NumberOfComponents)
            {
                case 1:
                case 3:
                    var i = SizeOfArgb * (rowIndex + lineIndex * rowWidth);
                    var j = 0;
                    for (var k = 0; k < rowWidth; ++k)
                    {
                        this.Bytes[i++] = rowValues[j++];
                        this.Bytes[i++] = rowValues[j++];
                        this.Bytes[i++] = rowValues[j++];
                        this.Bytes[i++] = 0xff;
                    }

                    break;
                case 4:
                    Array.Copy(
                        rowValues,
                        0,
                        this.Bytes,
                        SizeOfArgb * (rowIndex + lineIndex * rowWidth),
                        SizeOfArgb * rowWidth);
                    break;
                default:
                    throw new InvalidOperationException("Number of components must be one of 1, 3 or 4.");
            }
        }

        public byte[] GetComponent(int number)
        {
            if (number < 0 || number >= this.NumberOfComponents)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            var length = this.Bytes.Length;
            var component = new byte[length >> 2];

            for (int i = number, k = 0; i < length; i += 4, ++k)
            {
                component[k] = this.Bytes[i];
            }

            return component;
        }

        #endregion
    }
}
