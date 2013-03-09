#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaMainObjectMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IZetaMainObjectMark :
		IWithId, IWithVersion,
		IMarkLink<IZetaMainObject> //,
		//IWithMainObject<IZetaMainObject> 
	{}
}