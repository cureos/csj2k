// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System.IO;

    public class StoreFileStreamCreator : IFileStreamCreator
    {
        #region METHODS

        public Stream Create(string path, string mode)
        {
            return new StoreFileStream(path, mode);
        }

        public static void Register()
        {
            FileStreamFactory.RegisterCreator(new StoreFileStreamCreator());
        }

        #endregion
    }
}
