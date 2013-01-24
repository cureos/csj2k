using System.IO;

namespace CSJ2K.Util
{
	internal class FileInfoAdapter : IFileInfoAdapter
	{
		#region CONSTRUCTORS

		static FileInfoAdapter()
		{
			FileInfoFactory.RegisterAdapter(new FileInfoAdapter());
		}

		private FileInfoAdapter()
		{
		}

		#endregion

		#region METHODS

		public IFileInfo CreateFileInfo(string name)
		{
			return new FileInfoImpl(name);
		}

		#endregion
	}
}