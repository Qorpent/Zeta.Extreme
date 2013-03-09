#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : zone.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Application;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class zone : IZetaZone {
		public virtual int CountObjects() {
			return myapp.storage.Get<IZetaMainObject>().First<IZetaMainObject, int>(
				"select count(x.Id) from Org x where x.Location.Region.Country=?", Id);
		}
	}
}