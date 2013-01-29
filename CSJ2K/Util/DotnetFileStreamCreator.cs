using System;
using System.IO;

namespace CSJ2K.Util
{
#if MEF
	[System.ComponentModel.Composition.Export(typeof(IFileStreamCreator))]
#endif
	public class DotnetFileStreamCreator : IFileStreamCreator
	{
#if MEF
		#region CONSTRUCTORS

		public DotnetFileStreamCreator()
		{
			FileStreamFactory.RegisterCreator(this);
		}

		#endregion
#endif

		#region METHODS

		public Stream Create(string path, string mode)
		{
			if (mode.Equals("rw", StringComparison.OrdinalIgnoreCase))
				return new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			if (mode.Equals("r", StringComparison.OrdinalIgnoreCase))
				return new FileStream(path, FileMode.Open, FileAccess.Read);
			throw new ArgumentException(String.Format("File mode: {0} not supported.", mode), "mode");
		}
		
		public static void Register()
		{
			FileStreamFactory.RegisterCreator(new DotnetFileStreamCreator());
		}

		#endregion
	}
}