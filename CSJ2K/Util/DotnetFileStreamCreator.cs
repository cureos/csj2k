using System;
using System.IO;

namespace CSJ2K.Util
{
	public class DotnetFileStreamCreator : IFileStreamCreator
	{
		#region METHODS

		public Stream Create(string path, string mode)
		{
			if (mode.Equals("rw", StringComparison.OrdinalIgnoreCase))
				return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			if (mode.Equals("r", StringComparison.OrdinalIgnoreCase))
				return new FileStream(path, FileMode.Open, FileAccess.Read);
			throw new ArgumentException(String.Format("File mode: {0} not supported.", mode), "mode");
		}
		
		#endregion
	}
}