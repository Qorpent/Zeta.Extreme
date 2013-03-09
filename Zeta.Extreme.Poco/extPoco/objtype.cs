#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : objtype.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Application;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class objtype : IDetailObjectType {
		public virtual int CountDetailObjects() {
			return (int) myapp.storage.Get<IZetaDetailObject>().First<IZetaDetailObject, long>(
				"select count(x.Id) from Subpart x where x.Type.Class=" + Id);
		}
	}
}