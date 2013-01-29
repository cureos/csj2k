using System;
using System.IO;
using System.Threading.Tasks;
using CSJ2K.j2k.util;
using Windows.Storage;

namespace CSJ2K.Util
{
#if MEF
	[System.Composition.Export(typeof(IMsgLogger))]
#endif
	public class StoreMsgLogger : IMsgLogger
	{
#if MEF
		#region CONSTRUCTORS

		public StoreMsgLogger()
		{
			Register();
		}

		#endregion
#endif

		#region METHODS

		public static void Register()
		{
			Task.Run(async () =>
			{
				var outFile =
					await
					ApplicationData.Current.LocalFolder.CreateFileAsync("csj2k.out",
																		CreationCollisionOption.ReplaceExisting);
				var outputStream = (await outFile.OpenAsync(FileAccessMode.ReadWrite)).AsStreamForWrite();
				var errFile =
					await
					ApplicationData.Current.LocalFolder.CreateFileAsync("csj2k.err",
																		CreationCollisionOption.ReplaceExisting);
				var errorStream = (await errFile.OpenAsync(FileAccessMode.ReadWrite)).AsStreamForWrite();

				FacilityManager.DefaultMsgLogger = new StreamMsgLogger(outputStream, errorStream, 132);
			});
		}

		#endregion
	}
}