using System;
using System.ComponentModel.Composition;
using System.IO;

namespace CSJ2K.Util
{
	[Export(typeof(IFileStreamCreator))]
	public class DotnetFileStreamCreator : IFileStreamCreator
	{
		#region CONSTRUCTORS

		public DotnetFileStreamCreator()
		{
			FileStreamFactory.RegisterCreator(this);
		}

		#endregion

		#region METHODS

		public Stream Create(string path, string mode)
		{
			throw new NotImplementedException("File stream I/O not implemented for Silverlight.");
		}
		
		#endregion
	}
}