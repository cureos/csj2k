// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

namespace CSJ2K
{
    using CSJ2K.Util;

    public class CSJ2KSetup
    {
        #region METHODS

        public static void RegisterCreators()
        {
#if NETFX_CORE
            StoreMsgLogger.Register();
#elif SILVERLIGHT
#if !WINDOWS_PHONE
            SilverlightMsgLogger.Register();
#endif
#else
            DotnetMsgLogger.Register();
#endif
        }

        #endregion
    }
}
