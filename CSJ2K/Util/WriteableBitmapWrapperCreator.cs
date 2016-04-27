// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public class WriteableBitmapWrapperCreator : IBitmapWrapperCreator
    {
        #region PROPERTIES

        /// <summary>
        /// Gets whether or not this type is classified as a default manager.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region METHODS

        public IBitmapWrapper Create(int width, int height, int numberOfComponents)
        {
            return new WriteableBitmapWrapper(width, height, numberOfComponents);
        }

        #endregion
    }
}
