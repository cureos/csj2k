using System;
using System.IO;

namespace CSJ2K.Util
{
	public class FileStreamFactory
#if DOTNET
		: IFileStreamAdapter
#endif
	{
		#region FIELDS

		private static IFileStreamAdapter _adapter;

		#endregion

#if DOTNET
		#region CONSTRUCTORS

		static FileStreamFactory()
		{
			RegisterFactory(new FileStreamFactory());
		}

		private FileStreamFactory()
		{
		}

		#endregion
#endif

		#region METHODS

		public static void RegisterFactory(IFileStreamAdapter adapter)
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

#if DOTNET
		public Stream CreateFileStream(string path, string mode)
		{
			if (mode.Equals("rw", StringComparison.OrdinalIgnoreCase))
				return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			if (mode.Equals("r", StringComparison.OrdinalIgnoreCase))
				return new FileStream(path, FileMode.Open, FileAccess.Read);
			throw new ArgumentException(String.Format("File mode: {0} not supported.", mode), "mode");
		}
#endif
		
		#endregion
	}
}