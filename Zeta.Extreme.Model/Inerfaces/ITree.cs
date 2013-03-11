#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ITree.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface ITree<T> : IWithPath {
		T Parent { get; set; }
		IList<T> Children { get; set; }
	}
}