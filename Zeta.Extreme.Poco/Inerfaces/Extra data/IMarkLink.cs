#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IMarkLink.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IMarkLink<TargetType, MarkType> : IMarkLinkBase, IWithMark<MarkType> where MarkType : IMark {
		TargetType Target { get; set; }
	}

	public interface IMarkLink<TargetType> : IMarkLink<TargetType, IMark> {}
}