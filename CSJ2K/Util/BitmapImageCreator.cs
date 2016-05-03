// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using CSJ2K.j2k.image;

    public class BitmapImageCreator : IImageCreator
    {
        #region FIELDS

        private static readonly IImageCreator Instance = new BitmapImageCreator();

        #endregion

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

        public static void Register()
        {
            ImageFactory.Register(Instance);
        }

        public IImage Create(int width, int height, int numberOfComponents)
        {
            return new BitmapImage(width, height, numberOfComponents);
        }

        public BlkImgDataSrc CreateEncodableSource(object imageObject)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
