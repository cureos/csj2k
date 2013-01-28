using System.ComponentModel.Composition;
using System.IO;
using System.IO.IsolatedStorage;
using CSJ2K.j2k.util;

namespace CSJ2K.Util
{
	[Export(typeof(IMsgLogger))]
	public class SilverlightMsgLogger : IMsgLogger
	{
		#region CONSTRUCTORS

		public SilverlightMsgLogger()
		{
			using (var isolatedFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				FacilityManager.DefaultMsgLogger =
					new StreamMsgLogger(
						new IsolatedStorageFileStream("csj2k.out", FileMode.Create, FileAccess.ReadWrite, isolatedFile),
						new IsolatedStorageFileStream("csj2k.err", FileMode.Create, FileAccess.ReadWrite, isolatedFile), 78);
			}
		}

		#endregion
	}
}