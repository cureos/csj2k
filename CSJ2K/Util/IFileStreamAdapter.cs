using System.IO;

namespace CSJ2K.Util
{
	public interface IFileStreamAdapter
	{
		Stream CreateFileStream(string path, string mode);
	}
}