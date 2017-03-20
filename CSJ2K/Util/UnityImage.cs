// Copyright (c) 2007-2017 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;

using UnityEngine;

namespace CSJ2K.Util
{
    internal class UnityImage : ImageBase<Texture2D>
    {
        #region CONSTRUCTORS

        internal UnityImage(int width, int height, byte[] bytes)
            : base(width, height, bytes)
        {
        }

        #endregion

        #region METHODS

        protected override object GetImageObject()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
