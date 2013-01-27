using System.IO;
using CSJ2K.Util;

namespace Store.CSJ2K
{
	public class StoreFileStreamCreator : IFileStreamCreator
	{
		public Stream Create(string path, string mode)
		{
			return new StoreFileStream(path, mode);
		}
	}
}