using System;

namespace CSJ2K.Util
{
	public static class FileInfoFactory
	{
		#region FIELDS

		private static IFileInfoCreator _creator;

		#endregion

#if DOTNET
		#region CONSTRUCTORS

		static FileInfoFactory()
		{
			RegisterCreator(new DotnetFileInfoCreator());
		}

		#endregion
#endif

		#region METHODS

		public static void RegisterCreator(IFileInfoCreator creator)
		{
			if (_creator != null) throw new InvalidOperationException("File info creator can only be registered once.");
			_creator = creator;
		}

		internal static IFileInfo New(string fileName)
		{
			if (_creator == null) throw new InvalidOperationException("No file info creator is registered.");
			if (fileName == null) throw new ArgumentNullException("fileName");

			return _creator.Create(fileName);
		}

		#endregion
	}
}