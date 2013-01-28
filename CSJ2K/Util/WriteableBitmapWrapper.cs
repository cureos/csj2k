using System;

#if NETFX_CORE
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media.Imaging;
#endif

namespace CSJ2K.Util
{
	internal class WriteableBitmapWrapper : IBitmapWrapper
	{
		#region FIELDS

		private const int SizeOfArgb = 4;

		private readonly int _width;
		private readonly int _height;
		private readonly int _numberOfComponents;
		private readonly byte[] _bytes;

		#endregion

		#region CONSTRUCTORS

		internal WriteableBitmapWrapper(int width, int height, int numberOfComponents)
		{
			_width = width;
			_height = height;
			_numberOfComponents = numberOfComponents;
			_bytes = new byte[SizeOfArgb * width * height];
		}

		#endregion

		#region PROPERTIES

		public object Bitmap
		{
			get { return BitmapFactory.New(_width, _height).FromByteArray(_bytes); }
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