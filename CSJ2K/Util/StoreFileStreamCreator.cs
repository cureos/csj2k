using System.Composition;
using System.IO;

namespace CSJ2K.Util
{
	[Export(typeof(IFileStreamCreator))]
	public class StoreFileStreamCreator : IFileStreamCreator
	{
		public StoreFileStreamCreator()
		{
			FileStreamFactory.RegisterCreator(this);
		}

		public Stream Create(string path, string mode)
		{
			return new StoreFileStream(path, mode);
		}
	}
}