using System.Composition;

namespace CSJ2K.Util
{
	[Export(typeof(IFileInfoCreator))]
	public class StoreFileInfoCreator : IFileInfoCreator
	{
		public IFileInfo Create(string name)
		{
			return new StoreFileInfo(name);
		}
	}
}