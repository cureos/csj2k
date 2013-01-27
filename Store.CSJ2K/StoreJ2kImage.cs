using System.IO;
using CSJ2K.Util;
using CSJ2K.j2k.util;
using Store.CSJ2K;

namespace CSJ2K
{
	public static class StoreJ2kImage
	{
		#region CONSTRUCTORS

		static StoreJ2kImage()
		{
			OutputStream = new MemoryStream();
			ErrorStream = new MemoryStream();
		}

		#endregion

		#region PROPERTIES

		public static MemoryStream OutputStream { get; private set; }

		public static MemoryStream ErrorStream { get; private set; }

		#endregion

		public static void Initialize()
		 {
			 FacilityManager.DefaultMsgLogger = new StreamMsgLogger(OutputStream, ErrorStream, 132);
			 BitmapWrapperFactory.RegisterCreator(new WriteableBitmapWrapperCreator());
			 FileInfoFactory.RegisterCreator(new StoreFileInfoCreator());
			 FileStreamFactory.RegisterCreator(new StoreFileStreamCreator());
		 }
	}
}