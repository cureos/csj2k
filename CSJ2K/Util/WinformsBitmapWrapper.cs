using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CSJ2K.Util
{
	internal class WinformsBitmapWrapper : IBitmapWrapper
	{
		#region FIELDS

		private readonly Bitmap _bitmap;

		#endregion

		#region CONSTRUCTORS

		internal WinformsBitmapWrapper(int width, int height, int numberOfComponents)
		{
			PixelFormat pixelFormat;
			switch (numberOfComponents)
			{
				case 1:
				case 3:
					pixelFormat = PixelFormat.Format24bppRgb;
					break;
				case 4:
					pixelFormat = PixelFormat.Format32bppArgb;
					break;
				default:
					throw new InvalidOperationException("Unsupported PixelFormat.  " + numberOfComponents + " components.");
			}

			_bitmap = new Bitmap(width, height, pixelFormat);
		}

		#endregion

		#region PROPERTIES

		public object Bitmap { get { return _bitmap; } }

		#endregion

		#region METHODS

		public void FillRow(int rowIndex, int lineIndex, int rowWidth, byte[] rowValues)
		{
			var dstdata = _bitmap.LockBits(new Rectangle(rowIndex, lineIndex, rowWidth, 1), ImageLockMode.ReadWrite,
			                                      _bitmap.PixelFormat);

			var ptr = dstdata.Scan0;
			System.Runtime.InteropServices.Marshal.Copy(rowValues, 0, ptr, rowValues.Length);
			_bitmap.UnlockBits(dstdata);
		}
 
		#endregion
	}
}