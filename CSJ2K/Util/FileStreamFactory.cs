using System;
using System.IO;

namespace CSJ2K.Util
{
	public class FileStreamFactory
	{
		#region FIELDS

		private static IFileStreamAdapter _adapter;

		#endregion

#if DOTNET
		#region CONSTRUCTORS

		static FileStreamFactory()
		{
			RegisterAdapter(new DotnetFileStreamAdapter());
		}

		#endregion
#endif

		#region METHODS

		public static void RegisterAdapter(IFileStreamAdapter adapter)
		{
			if (adapter == null) throw new ArgumentNullException("adapter");
			if (_adapter != null) throw new InvalidOperationException("File stream adapter can only be registered once.");
			_adapter = adapter;
		}

		internal static Stream Create(string path, string mode)
		{
			if (_adapter == null) throw new InvalidOperationException("No file stream adapter is registered.");
			if (path == null) throw new ArgumentNullException("path");
			if (mode == null) throw new ArgumentNullException("mode");

			return _adapter.CreateFileStream(path, mode);
		}
		
		#endregion
	}
}