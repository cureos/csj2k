// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public interface IImage
    {
        #region PROPERTIES

        object Bitmap { get; }

        #endregion

        #region METHODS

        void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues);

        #endregion
    }
}
