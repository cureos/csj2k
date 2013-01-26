namespace CSJ2K.Util
{
	public interface IBitmapWrapper
	{
		#region PROPERTIES

		object Bitmap { get; }

		#endregion

		#region METHODS

		void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues);

		#endregion
	}
}