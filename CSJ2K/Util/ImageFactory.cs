// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    public class ImageFactory
    {
        #region FIELDS

        private static IImageCreator _creator;

        #endregion

        #region CONSTRUCTORS

        static ImageFactory()
        {
#if DOTNET
		    _creator = J2kSetup.GetSinglePlatformInstance<IImageCreator>();
#else
            _creator = J2kSetup.GetDefaultPlatformInstance<IImageCreator>();
#endif
        }

        #endregion

        #region METHODS

        internal static IImage New(int width, int height, int numberOfComponents)
        {
            return _creator.Create(width, height, numberOfComponents);
        }

        #endregion
    }
}