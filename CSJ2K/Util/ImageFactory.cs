// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public static class ImageFactory
    {
        #region FIELDS

        private static IImageCreator _creator;

        #endregion

        #region CONSTRUCTORS

        static ImageFactory()
        {
            _creator = J2kSetup.GetDefaultPlatformInstance<IImageCreator>();
        }

        #endregion

        #region METHODS

        public static void Register(IImageCreator creator)
        {
            _creator = creator;
        }

        internal static IImage New(int width, int height, int numberOfComponents)
        {
            return _creator.Create(width, height, numberOfComponents);
        }

        #endregion
    }
}