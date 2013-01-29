using System.IO;

namespace CSJ2K.Util
{
#if MEF
	[System.Composition.Export(typeof(IFileStreamCreator))]
#endif
	public class StoreFileStreamCreator : IFileStreamCreator
	{
#if MEF
		#region CONSTRUCTORS

		public StoreFileStreamCreator()
		{
			FileStreamFactory.RegisterCreator(this);
		}
		
		#endregion
#endif

		#region METHODS

		public Stream Create(string path, string mode)
		{
			return new StoreFileStream(path, mode);
		}
		
		public static void Register()
		{
			FileStreamFactory.RegisterCreator(new StoreFileStreamCreator());
		}

		#endregion
	}
}