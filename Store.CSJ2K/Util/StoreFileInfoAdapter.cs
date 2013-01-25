namespace CSJ2K.Util
{
	public class StoreFileInfoAdapter : IFileInfoAdapter
	{
		public IFileInfo CreateFileInfo(string name)
		{
			return new StoreFileInfo(name);
		}
	}
}