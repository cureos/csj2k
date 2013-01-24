using System;
using System.IO;

namespace CSJ2K.Util
{
	internal class FileInfoImpl : IFileInfo
	{
		#region FIELDS

		private readonly FileInfo _fileInfo;

		#endregion

		#region CONSTRUCTORS

		internal FileInfoImpl(string name)
		{
			_fileInfo = new FileInfo(name);
		}

		#endregion

		#region PROPERTIES

		public string Name { get { return _fileInfo.Name; } }

		public string FullName { get { return _fileInfo.FullName; } }

		public bool Exists { get { return _fileInfo.Exists; } }

		#endregion

		#region METHODS

		public bool Delete()
		{
			try
			{
				_fileInfo.Delete();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		} 

		#endregion
	}
}