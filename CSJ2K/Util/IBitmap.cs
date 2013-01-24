namespace CSJ2K.Util
{
	public interface IBitmap
	{
		#region PROPERTIES

		object BitmapObject { get; }

		#endregion

		#region METHODS

		void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues);

		#endregion
	}
}