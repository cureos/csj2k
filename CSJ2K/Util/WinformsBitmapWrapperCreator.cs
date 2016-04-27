// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public class WinformsBitmapWrapperCreator : IBitmapWrapperCreator
    {
        #region PROPERTIES

        public bool IsDefault
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region METHODS

        public IBitmapWrapper Create(int width, int height, int numberOfComponents)
        {
            return new WinformsBitmapWrapper(width, height, numberOfComponents);
        }

        #endregion
    }
}
