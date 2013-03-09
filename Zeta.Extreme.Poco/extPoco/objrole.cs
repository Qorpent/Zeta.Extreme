#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : objrole.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Application;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco {
	public partial class objrole : IMainObjectRole {
		public virtual int CountObjects() {
			return myapp.storage.Get<IZetaMainObject>().First<IZetaMainObject, int>(
				"select count(x.Id) from Org x where x.Otrasl=?", Id);
		}
	}
}