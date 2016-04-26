// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System.IO;
    using System.IO.IsolatedStorage;

    using CSJ2K.j2k.util;

    public class SilverlightMsgLogger : IMsgLogger
    {
        #region METHODS

        public static void Register()
        {
            using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                FacilityManager.DefaultMsgLogger =
                    new StreamMsgLogger(
                        new IsolatedStorageFileStream("csj2k.out", FileMode.Create, FileAccess.ReadWrite, isolatedFile),
                        new IsolatedStorageFileStream("csj2k.err", FileMode.Create, FileAccess.ReadWrite, isolatedFile),
                        78);
            }
        }

        #endregion
    }
}
