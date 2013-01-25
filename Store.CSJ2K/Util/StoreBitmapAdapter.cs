namespace CSJ2K.Util
{
	public class StoreBitmapAdapter : IBitmapAdapter
	{
		public IBitmap CreateBitmap(int width, int height, int numberOfComponents)
		{
			return new StoreBitmap(width, height, numberOfComponents);
		}
	}
}