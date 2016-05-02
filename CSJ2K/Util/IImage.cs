// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public interface IImage
    {
        #region PROPERTIES

        int Width { get; }

        int Height { get; }

        int NumberOfComponents { get; }

        byte[] Bytes { get; }

        #endregion

        #region METHODS

        T As<T>();

        void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues);

        byte[] GetComponent(int number);

        #endregion
    }
}
