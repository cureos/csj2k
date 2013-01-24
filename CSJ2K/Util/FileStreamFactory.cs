using System;
using System.IO;

namespace CSJ2K.Util
{
	public class FileStreamFactory
#if DOTNET
		: IFileStreamFactory
#endif
	{
		#region FIELDS

		private static IFileStreamFactory _factory;

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

		public static void RegisterFactory(IFileStreamFactory factory)
		{
			if (factory == null) throw new ArgumentNullException("factory");
			if (_factory != null) throw new InvalidOperationException("File stream factory can only be registered once.");
			_factory = factory;
		}

		internal static Stream Create(string path, string mode)
		{
			if (_factory == null) throw new InvalidOperationException("No file stream factory is registered.");
			if (path == null) throw new ArgumentNullException("path");
			if (mode == null) throw new ArgumentNullException("mode");

			return _factory.CreateFileStream(path, mode);
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