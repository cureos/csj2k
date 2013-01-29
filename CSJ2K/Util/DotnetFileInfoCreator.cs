namespace CSJ2K.Util
{
#if MEF
#if NETFX_CORE
	[System.Composition.Export(typeof(IFileInfoCreator))]
#else
	[System.ComponentModel.Composition.Export(typeof(IFileInfoCreator))]
#endif
#endif
	public class DotnetFileInfoCreator : IFileInfoCreator
	{
#if MEF
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

		public static void Register()
		{
			FileInfoFactory.RegisterCreator(new DotnetFileInfoCreator());
		}

		#endregion
	}
}