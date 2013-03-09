#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMarkLinkBase.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Comdiv.Model.Interfaces;

namespace Comdiv.Olap.Model {
	public interface IMarkLinkBase {
		[global::Zeta.Extreme.Poco.Deprecated.NoMap] IEntityDataPattern MarkLinkTarget { get; set; }

		[global::Zeta.Extreme.Poco.Deprecated.NoMap] IMark MarkLinkMark { get; set; }
	}
}