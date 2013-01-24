namespace CSJ2K.Util
{
	internal class FileInfoAdapter : IFileInfoAdapter
	{
		#region METHODS

		public IFileInfo CreateFileInfo(string name)
		{
			return new FileInfoImpl(name);
		}

		#endregion
	}
}