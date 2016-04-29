// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K
{
    using CSJ2K.Util;

    public static class IOSSetup
    {
        public static void RegisterCretors()
        {
            DotnetFileInfoCreator.Register();
            DotnetFileStreamCreator.Register();
            DotnetMsgLogger.Register();
            IOSImageCreator.Register();
        }
    }
}