#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaMainObjectMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model {
	public interface IZetaMainObjectMark :
		IItemDataPattern,
		IMarkLink<IZetaMainObject> //,
		//IWithMainObject<IZetaMainObject> 
	{}
}