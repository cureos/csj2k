namespace CSJ2K.Util
{
#if !DOTNET
	[System.Composition.Export(typeof(IFileInfoCreator))]
#endif
	internal class DotnetFileInfoCreator : IFileInfoCreator
	{
#if !DOTNET
		#region CONSTRUCTORS

		public DotnetFileInfoCreator()
		{
			FileInfoFactory.RegisterCreator(this);
		}

		#endregion
#endif
		#region METHODS

		public IFileInfo Create(string fileName)
		{
			return new DotnetFileInfo(fileName);
		}

		#endregion
	}
}