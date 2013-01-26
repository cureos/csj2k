using System;
using System.IO;

namespace CSJ2K.Util
{
	public class FileStreamFactory
	{
		#region FIELDS

		private static IFileStreamCreator _creator;

		#endregion

#if DOTNET
		#region CONSTRUCTORS

		static FileStreamFactory()
		{
			RegisterCreator(new DotnetFileStreamCreator());
		}

		#endregion
#endif

		#region METHODS

		public static void RegisterCreator(IFileStreamCreator creator)
		{
			if (creator == null) throw new ArgumentNullException("creator");
			if (_creator != null) throw new InvalidOperationException("File stream creator can only be registered once.");
			_creator = creator;
		}

		internal static Stream New(string path, string mode)
		{
			if (_creator == null) throw new InvalidOperationException("No file stream creator is registered.");
			if (path == null) throw new ArgumentNullException("path");
			if (mode == null) throw new ArgumentNullException("mode");

			return _creator.Create(path, mode);
		}
		
		#endregion
	}
}