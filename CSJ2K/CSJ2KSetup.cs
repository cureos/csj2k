#if !WINDOWS_PHONE
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

#if !WINDOWS_PHONE
		[Import]
#endif
		public IFileInfoCreator FileInfoCreator { get; set; }

#if !WINDOWS_PHONE
		[Import]
#endif
		public IFileStreamCreator FileStreamCreator { get; set; }

#if !WINDOWS_PHONE
		[Import]
#endif
		public IBitmapWrapperCreator BitmapWrapperCreator { get; set; }

#if !WINDOWS_PHONE
		[Import]
#endif
		public IMsgLogger MsgLogger { get; set; }

		#endregion

		#region METHODS

		public static void RegisterManually()
		{
#if WINDOWS_PHONE
			var setup = new CSJ2KSetup { BitmapWrapperCreator = new WriteableBitmapWrapperCreator() };
#endif
		}

		#endregion
	}
}