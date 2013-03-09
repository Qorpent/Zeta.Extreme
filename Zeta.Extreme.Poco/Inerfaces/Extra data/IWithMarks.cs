#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithMarks.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithMarks<TargetType, LinkType, MarkType> : IWithMarksBase
		where LinkType : IMarkLink<TargetType, MarkType> where MarkType : IMark {
		IList<LinkType> MarkLinks { get; set; }
		}

	public interface IWithMarks<TargetType, LinkType> : IWithMarks<TargetType, LinkType, IMark>
		where LinkType : IMarkLink<TargetType, IMark> {}
}