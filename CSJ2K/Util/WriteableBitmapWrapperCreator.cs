#if !WINDOWS_PHONE
#if SILVERLIGHT
using System.ComponentModel.Composition;
#else
using System.Composition;
#endif
#endif

namespace CSJ2K.Util
{
#if !WINDOWS_PHONE
	[Export(typeof(IBitmapWrapperCreator))]
#endif
	public class WriteableBitmapWrapperCreator : IBitmapWrapperCreator
	{
		public WriteableBitmapWrapperCreator()
		{
			BitmapWrapperFactory.RegisterCreator(this);
		}

		public IBitmapWrapper Create(int width, int height, int numberOfComponents)
		{
			return new WriteableBitmapWrapper(width, height, numberOfComponents);
		}
	}
}