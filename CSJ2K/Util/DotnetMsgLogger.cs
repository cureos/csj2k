using System;
using CSJ2K.j2k.util;

namespace CSJ2K.Util
{
#if MEF
	[System.ComponentModel.Composition.Export(typeof(IMsgLogger))]
#endif
	public class DotnetMsgLogger : IMsgLogger
	{
#if MEF
		#region CONSTRUCTORS

		public DotnetMsgLogger()
		{
			Register();
		}

		#endregion
#endif

		#region METHODS

		public static void Register()
		{
			FacilityManager.DefaultMsgLogger = new StreamMsgLogger(Console.OpenStandardOutput(), Console.OpenStandardError(), 78);
		}

		#endregion
	}
}