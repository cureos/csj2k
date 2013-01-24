using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSJ2K.Util;

namespace Store.CSJ2K
{
    public class StoreFileInfo : IFileInfo
    {
	    public string Name { get; private set; }
	    public string FullName { get; private set; }
	    public bool Exists { get; private set; }
	    public bool Delete()
	    {
		    throw new NotImplementedException();
	    }
    }
}
