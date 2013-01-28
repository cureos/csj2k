using System;
using System.Composition;
using CSJ2K.j2k.util;

namespace CSJ2K.Util
{
	[Export(typeof(IMsgLogger))]
	public class DotnetMsgLogger : IMsgLogger
	{
		#region CONSTRUCTORS

		public DotnetMsgLogger()
		{
			FacilityManager.DefaultMsgLogger = new StreamMsgLogger(Console.OpenStandardOutput(), Console.OpenStandardError(), 78);
		}

		#endregion
	}
}