using System;

namespace CSJ2K.Util
{
	public class BitmapFactory
	{
		#region FIELDS

		private static IBitmapAdapter _adapter;

		#endregion

#if DOTNET
		#region CONSTRUCTORS

		static BitmapFactory()
		{
			RegisterAdapter(new BitmapAdapter());
		}

		#endregion
#endif

		#region METHODS

		public static void RegisterAdapter(IBitmapAdapter adapter)
		{
			if (_adapter != null) throw new InvalidOperationException("Bitmap adapter can only be registered once.");
			_adapter = adapter;
		}

		internal static IBitmap Create(int width, int height, int numberOfComponents)
		{
			return _adapter.CreateBitmap(width, height, numberOfComponents);
		}

		#endregion
	}
}