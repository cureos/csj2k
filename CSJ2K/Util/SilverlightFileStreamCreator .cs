// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;
    using System.IO;

    public class SilverlightFileStreamCreator : IFileStreamCreator
    {
        #region METHODS

        public Stream Create(string path, string mode)
        {
            throw new NotImplementedException("File stream I/O not implemented for Silverlight.");
        }

        public static void Register()
        {
            FileStreamFactory.RegisterCreator(new SilverlightFileStreamCreator());
        }

        #endregion
    }
}
