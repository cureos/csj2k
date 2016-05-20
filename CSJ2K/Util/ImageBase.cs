// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;
    using System.Reflection;


    public abstract class ImageBase<TBase> : IImage
    {
        #region FIELDS

        protected const int SizeOfArgb = 4;

        #endregion

        #region CONSTRUCTORS

        protected ImageBase(int width, int height, byte[] bytes)
        {
            this.Width = width;
            this.Height = height;
            this.Bytes = bytes;
        }

        #endregion

        #region PROPERTIES

        protected int Width { get; }

        protected int Height { get; }

        protected byte[] Bytes { get; }

        #endregion

        #region METHODS

        public virtual T As<T>()
        {
#if NETFX_CORE || NETSTANDARD
            if (!typeof(TBase).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
#else
            if (!typeof(TBase).IsAssignableFrom(typeof(T)))
#endif
            {
                throw new InvalidCastException(
                    string.Format(
                        "Cannot cast to '{0}'; type must be assignable from '{1}'",
                        typeof(T).Name,
                        typeof(TBase).Name));
            }

            return (T)this.GetImageObject();
        }

        protected abstract object GetImageObject();

        #endregion
    }
}
