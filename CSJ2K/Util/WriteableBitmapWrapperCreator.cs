#if MEF
#if NETFX_CORE
using System.Composition;
#else
using System.ComponentModel.Composition;
#endif
#endif

namespace CSJ2K.Util
{
#if MEF
	[Export(typeof(IBitmapWrapperCreator))]
#endif
	public class WriteableBitmapWrapperCreator : IBitmapWrapperCreator
	{
#if MEF
		#region CONSTRUCTORS

		public WriteableBitmapWrapperCreator()
		{
			BitmapWrapperFactory.RegisterCreator(this);
		}

		#endregion
#endif

		#region METHODS

		public IBitmapWrapper Create(int width, int height, int numberOfComponents)
		{
			return new WriteableBitmapWrapper(width, height, numberOfComponents);
		}
		
		public static void Register()
		{
			BitmapWrapperFactory.RegisterCreator(new WriteableBitmapWrapperCreator());
		}

		#endregion
	}
}