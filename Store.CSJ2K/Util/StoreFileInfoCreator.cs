namespace CSJ2K.Util
{
	public class StoreFileInfoCreator : IFileInfoCreator
	{
		public IFileInfo Create(string name)
		{
			return new StoreFileInfo(name);
		}
	}
}