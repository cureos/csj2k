namespace CSJ2K.Util
{
	public class WinformsBitmapWrapperCreator : IBitmapWrapperCreator
	{
		#region METHODS

		public IBitmapWrapper Create(int width, int height, int numberOfComponents)
		{
			return new WinformsBitmapWrapper(width, height, numberOfComponents);
		}

        public static void Register()
        {
            BitmapWrapperFactory.RegisterCreator(new WinformsBitmapWrapperCreator());
        }

        #endregion
    }
}