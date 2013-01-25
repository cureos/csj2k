namespace CSJ2K.Util
{
	internal class DotnetFileInfoAdapter : IFileInfoAdapter
	{
		#region METHODS

		public IFileInfo CreateFileInfo(string fileName)
		{
			return new DotnetFileInfo(fileName);
		}

		#endregion
	}
}