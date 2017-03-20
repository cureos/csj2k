// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;

using CSJ2K.j2k.image;

using UnityEngine;

namespace CSJ2K.Util
{
    internal class UnityImageSource : PortableImageSource
    {
        #region CONSTRUCTORS

        private UnityImageSource(Texture2D texture)
            : base(texture.width, texture.height, 1, 1, null, null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region METHODS

        internal static BlkImgDataSrc Create(object imageObject)
        {
            var texture = imageObject as Texture2D;
            return texture == null ? null : new UnityImageSource(texture);
        }

        #endregion
    }
}
