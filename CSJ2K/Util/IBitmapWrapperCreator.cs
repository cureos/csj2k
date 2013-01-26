namespace CSJ2K.Util
{
	public interface IBitmapWrapperCreator
	{
		IBitmapWrapper Create(int width, int height, int numberOfComponents);
	}
}