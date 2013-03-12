#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithOwn.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.PocoClasses;

namespace Zeta.Extreme.Model.Inerfaces.Partial {
	public interface IWithOwn {
		int IntOwn { get; set; }
		SubpartRelation Own { get; set; }
	}
}