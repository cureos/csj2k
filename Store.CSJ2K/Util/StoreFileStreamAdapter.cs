using System.IO;
using CSJ2K.Util;

namespace Store.CSJ2K
{
	public class StoreFileStreamAdapter : IFileStreamAdapter
	{
		public Stream CreateFileStream(string path, string mode)
		{
			return new StoreFileStream(path, mode);
		}
	}
}