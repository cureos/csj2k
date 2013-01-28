using System.Composition;
using System.IO;

namespace CSJ2K.Util
{
	[Export(typeof(IFileStreamCreator))]
	public class StoreFileStreamCreator : IFileStreamCreator
	{
		public Stream Create(string path, string mode)
		{
			return new StoreFileStream(path, mode);
		}
	}
}