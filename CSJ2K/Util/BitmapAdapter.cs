namespace CSJ2K.Util
{
	internal class BitmapAdapter : IBitmapAdapter
	{
		#region METHODS

		public IBitmap CreateBitmap(int width, int height, int numberOfComponents)
		{
			return new BitmapImpl(width, height, numberOfComponents);
		}
		
		#endregion
	}
}