using System;
using Windows.UI.Xaml.Media.Imaging;

namespace CSJ2K.Util
{
	internal class StoreBitmap : IBitmap
	{
		#region FIELDS

		private readonly WriteableBitmap _bitmap;
		private readonly int _numberOfComponents;

		#endregion

		#region CONSTRUCTORS

		internal StoreBitmap(int width, int height, int numberOfComponents)
		{
			_bitmap = new WriteableBitmap(width, height);
			_numberOfComponents = numberOfComponents;
		}

		#endregion

		#region PROPERTIES

		public object BitmapObject
		{
			get { return _bitmap; }
		}

		#endregion

		#region METHODS

		public void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
		{
			var bytes = new int[4 * rowWidth];
			switch (_numberOfComponents)
			{
				case 1:
				case 3:
					var i = 0;
					var j = 0;
					for (var k = 0; k < rowWidth; ++k)
					{
						bytes[i++] = rowValues[j++];
						bytes[i++] = rowValues[j++];
						bytes[i++] = rowValues[j++];
						bytes[i++] = 0xff;
					}
					break;
				case 4:
					Array.Copy(rowValues, bytes, 4 * rowWidth);
					break;
				default:
					throw new InvalidOperationException("Number of components must be one of 1, 3 or 4.");
			}
			using (var context = _bitmap.GetBitmapContext())
			{
				BitmapContext.BlockCopy(bytes, 0, context, 4 * (rowIndex + lineIndex * rowWidth), 4 * rowWidth);
			}
		}
		
		#endregion
	}
}