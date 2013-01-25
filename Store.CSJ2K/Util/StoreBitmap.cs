using System;
using Windows.UI.Xaml.Media.Imaging;

namespace CSJ2K.Util
{
	internal class StoreBitmap : IBitmap
	{
		#region FIELDS

		private const int SizeOfArgb = 4;

		private readonly WriteableBitmap _bitmap;
		private readonly byte[] _bytes;
		private readonly int _numberOfComponents;

		#endregion

		#region CONSTRUCTORS

		internal StoreBitmap(int width, int height, int numberOfComponents)
		{
			_bitmap = new WriteableBitmap(width, height);
			_bytes = new byte[SizeOfArgb * width * height];
			_numberOfComponents = numberOfComponents;
		}

		#endregion

		#region PROPERTIES

		public object BitmapObject
		{
			get { return _bitmap.FromByteArray(_bytes); }
		}

		#endregion

		#region METHODS

		public void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
		{
			switch (_numberOfComponents)
			{
				case 1:
				case 3:
					var i = SizeOfArgb * (rowIndex + lineIndex * rowWidth);
					var j = 0;
					for (var k = 0; k < rowWidth; ++k)
					{
						_bytes[i++] = rowValues[j++];
						_bytes[i++] = rowValues[j++];
						_bytes[i++] = rowValues[j++];
						_bytes[i++] = 0xff;
					}
					break;
				case 4:
					Array.Copy(rowValues, 0, _bytes, SizeOfArgb * (rowIndex + lineIndex * rowWidth), SizeOfArgb * rowWidth);
					break;
				default:
					throw new InvalidOperationException("Number of components must be one of 1, 3 or 4.");
			}
		}
		
		#endregion
	}
}