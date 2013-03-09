#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaDetailObjectMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	[global::Zeta.Extreme.Poco.Deprecated.Classic("SubpartMark")]
	public interface IZetaDetailObjectMark :
		IWithId, IWithVersion,
		IMarkLink<IZetaDetailObject> //,
		//IWithDetailObject<IZetaDetailObject,IZetaMainObject> 
	{}
}