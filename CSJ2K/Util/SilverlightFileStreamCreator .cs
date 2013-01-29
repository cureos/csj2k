using System;
using System.IO;

namespace CSJ2K.Util
{
#if MEF
	[System.ComponentModel.Composition.Export(typeof(IFileStreamCreator))]
#endif
	public class SilverlightFileStreamCreator : IFileStreamCreator
	{
#if MEF
		#region CONSTRUCTORS

		public SilverlightFileStreamCreator()
		{
			FileStreamFactory.RegisterCreator(this);
		}

		#endregion
#endif

		#region METHODS

		public Stream Create(string path, string mode)
		{
			throw new NotImplementedException("File stream I/O not implemented for Silverlight.");
		}
		
		public static void Register()
		{
			FileStreamFactory.RegisterCreator(new SilverlightFileStreamCreator());
		}

		#endregion
	}
}