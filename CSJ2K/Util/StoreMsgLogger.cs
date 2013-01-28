using System;
using System.Composition;
using System.IO;
using CSJ2K.j2k.util;

namespace CSJ2K.Util
{
	[Export(typeof(IMsgLogger))]
	public class StoreMsgLogger : IMsgLogger
	{
		#region CONSTRUCTORS

		static StoreMsgLogger()
		{
			OutputStream = new MemoryStream();
			ErrorStream = new MemoryStream();
		}

		public StoreMsgLogger()
		{
			FacilityManager.DefaultMsgLogger = new StreamMsgLogger(OutputStream, ErrorStream, 132);
		}

		#endregion

		#region PROPERTIES

		public static Stream OutputStream { get; private set; }

		public static Stream ErrorStream { get; private set; }

		#endregion
	}
}