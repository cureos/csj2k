#if MEF
#if NETFX_CORE
using System.Composition;
#else
using System.ComponentModel.Composition;
#endif
#endif

using CSJ2K.Util;

namespace CSJ2K
{
	public class CSJ2KSetup
	{
		#region PROPERTIES

#if MEF
		[Import]
#endif
		public IFileInfoCreator FileInfoCreator { get; set; }

#if MEF
		[Import]
#endif
		public IFileStreamCreator FileStreamCreator { get; set; }

#if MEF
		[Import]
#endif
		public IBitmapWrapperCreator BitmapWrapperCreator { get; set; }

#if MEF
		[Import]
#endif
		public IMsgLogger MsgLogger { get; set; }

		#endregion

		#region METHODS

		public static void RegisterCreators()
		{
#if NETFX_CORE
			StoreMsgLogger.Register();
			StoreFileInfoCreator.Register();
			StoreFileStreamCreator.Register();
			WriteableBitmapWrapperCreator.Register();
#elif SILVERLIGHT
#if WINDOWS_PHONE
			WriteableBitmapWrapperCreator.Register();
#else
			SilverlightMsgLogger.Register();
			SilverlightFileStreamCreator.Register();
			WriteableBitmapWrapperCreator.Register();
#endif
#else
			DotnetMsgLogger.Register();
			DotnetFileInfoCreator.Register();
			DotnetFileStreamCreator.Register();
			WriteableBitmapWrapperCreator.Register();
#endif
		}

		#endregion
	}
}