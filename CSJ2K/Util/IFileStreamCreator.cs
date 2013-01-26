using System.IO;

namespace CSJ2K.Util
{
	public interface IFileStreamCreator
	{
		Stream Create(string path, string mode);
	}
}