using System.IO;

namespace CSJ2K.Util
{
	public interface IFileStreamFactory
	{
		Stream CreateFileStream(string path, string mode);
	}
}