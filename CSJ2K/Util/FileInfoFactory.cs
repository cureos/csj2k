using System;

namespace CSJ2K.Util
{
	public static class FileInfoFactory
	{
		#region FIELDS

		private static IFileInfoAdapter _adapter;

		#endregion

#if DOTNET
		#region CONSTRUCTORS

		static FileInfoFactory()
		{
			RegisterAdapter(new DotnetFileInfoAdapter());
		}

		#endregion
#endif

		#region METHODS

		public static void RegisterAdapter(IFileInfoAdapter adapter)
		{
			if (_adapter != null) throw new InvalidOperationException("File info adapter can only be registered once.");
			_adapter = adapter;
		}

		internal static IFileInfo Create(string fileName)
		{
			if (fileName == null) throw new ArgumentNullException("fileName");
			return _adapter.CreateFileInfo(fileName);
		}

		#endregion
	}
}