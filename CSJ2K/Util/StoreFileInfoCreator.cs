namespace CSJ2K.Util
{
#if MEF
	[System.Composition.Export(typeof(IFileInfoCreator))]
#endif
	public class StoreFileInfoCreator : IFileInfoCreator
	{
#if MEF
		#region CONSTRUCTORS

		public StoreFileInfoCreator()
		{
			FileInfoFactory.RegisterCreator(this);
		}
		
		#endregion
#endif

		#region METHODS

		public IFileInfo Create(string name)
		{
			return new StoreFileInfo(name);
		}
		
		public static void Register()
		{
			FileInfoFactory.RegisterCreator(new StoreFileInfoCreator());
		}

		#endregion
	}
}