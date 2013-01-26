namespace CSJ2K.Util
{
	internal class WinformsBitmapWrapperCreator : IBitmapWrapperCreator
	{
		#region METHODS

		public IBitmapWrapper Create(int width, int height, int numberOfComponents)
		{
			return new WinformsBitmapWrapper(width, height, numberOfComponents);
		}
		
		#endregion
	}
}