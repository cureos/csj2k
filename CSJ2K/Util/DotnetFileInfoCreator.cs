namespace CSJ2K.Util
{
	internal class DotnetFileInfoCreator : IFileInfoCreator
	{
		#region METHODS

		public IFileInfo Create(string fileName)
		{
			return new DotnetFileInfo(fileName);
		}

		#endregion
	}
}