// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public class WriteableBitmapWrapperCreator : IBitmapWrapperCreator
    {
        #region METHODS

        public IBitmapWrapper Create(int width, int height, int numberOfComponents)
        {
            return new WriteableBitmapWrapper(width, height, numberOfComponents);
        }

        public static void Register()
        {
            BitmapWrapperFactory.RegisterCreator(new WriteableBitmapWrapperCreator());
        }

        #endregion
    }
}
