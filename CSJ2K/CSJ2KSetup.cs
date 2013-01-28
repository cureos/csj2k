using System.Composition;
using CSJ2K.Util;

namespace CSJ2K
{
	public class CSJ2KSetup
	{
		// ReSharper disable UnusedMember.Local
		#region PROPETIES

		[Import]
		private IFileInfoCreator FileInfoCreator { get; set; }

		[Import]
		private IFileStreamCreator FileStreamCreator { get; set; }

		[Import]
		private IBitmapWrapperCreator BitmapWrapperCreator { get; set; }

		[Import]
		private IMsgLogger MsgLogger { get; set; }

		#endregion
		// ReSharper restore UnusedMember.Local
	}
}