#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : objcls.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Application;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class objcls : IDetailObjectClass {
		public virtual int CountDetailObjects() {
			return myapp.storage.Get<obj>().First<IZetaDetailObject, int>(
				"select count(x.Id) from Subpart x where x.Type.Class=" + Id);
		}
	}
}