using System.IO;
using System.IO.IsolatedStorage;
using CSJ2K.j2k.util;

namespace CSJ2K.Util
{
#if MEF
	[System.ComponentModel.Composition.Export(typeof(IMsgLogger))]
#endif
	public class SilverlightMsgLogger : IMsgLogger
	{
#if MEF
		#region CONSTRUCTORS

		public SilverlightMsgLogger()
		{
			Register();
		}

		#endregion
#endif

		#region METHODS

		public static void Register()
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