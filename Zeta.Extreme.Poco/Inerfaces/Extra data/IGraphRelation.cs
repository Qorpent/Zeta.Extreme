#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IGraphRelation.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IGraphRelation : IWithId, IWithVersion, IWithCode {
		IZetaMainObject Root { get; set; }
		IZetaMainObject Target { get; set; }
		GraphRelationType Type { get; set; }
		string TypeDetail { get; set; }
	}
}