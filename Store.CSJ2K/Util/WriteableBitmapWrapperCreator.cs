namespace CSJ2K.Util
{
	public class WriteableBitmapWrapperCreator : IBitmapWrapperCreator
	{
		public IBitmapWrapper Create(int width, int height, int numberOfComponents)
		{
			return new WriteableBitmapWrapper(width, height, numberOfComponents);
		}
	}
}