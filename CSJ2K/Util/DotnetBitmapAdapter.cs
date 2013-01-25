namespace CSJ2K.Util
{
	internal class DotnetBitmapAdapter : IBitmapAdapter
	{
		#region METHODS

		public IBitmap CreateBitmap(int width, int height, int numberOfComponents)
		{
			return new DotnetBitmap(width, height, numberOfComponents);
		}
		
		#endregion
	}
}