using System;
using CSJ2K.Util;
using CSJ2K.j2k.util;

namespace CSJ2K
{
	public static class WpfJ2kImage
	{
		public static void Initialize()
		 {
			 FacilityManager.DefaultMsgLogger = new StreamMsgLogger(Console.OpenStandardOutput(), Console.OpenStandardError(), 78);
			 BitmapWrapperFactory.RegisterCreator(new WriteableBitmapWrapperCreator());
			 FileInfoFactory.RegisterCreator(new DotnetFileInfoCreator());
			 FileStreamFactory.RegisterCreator(new DotnetFileStreamCreator());
		 }
	}
}