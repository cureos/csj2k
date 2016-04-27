// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K.Util
{
    using System;
    using System.IO;

    using CSJ2K.j2k.util;

    using Windows.Storage;

    public class StoreMsgLogger : StreamMsgLogger
    {
        #region CONSTRUCTORS

        public StoreMsgLogger()
            : base(GetLogFile("csj2k.out"), GetLogFile("csj2k.err"), 132)
        {
        }

        #endregion

        #region METHODS

        private static Stream GetLogFile(string fileName)
        {
            var file =
                ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting)
                    .AsTask()
                    .Result;
            return file.OpenStreamForWriteAsync().Result;
        }

        #endregion
    }
}
