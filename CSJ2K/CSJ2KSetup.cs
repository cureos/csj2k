#if SILVERLIGHT
using System.ComponentModel.Composition;
#else
using System.Composition;
#endif
using CSJ2K.Util;

namespace CSJ2K
{
	public class CSJ2KSetup
	{
		// ReSharper disable UnusedMember.Local
		#region PROPETIES

		[Import]
		public IFileInfoCreator FileInfoCreator { get; set; }

		[Import]
		public IFileStreamCreator FileStreamCreator { get; set; }

		[Import]
		public IBitmapWrapperCreator BitmapWrapperCreator { get; set; }

		[Import]
		public IMsgLogger MsgLogger { get; set; }

		#endregion
		// ReSharper restore UnusedMember.Local
	}
}