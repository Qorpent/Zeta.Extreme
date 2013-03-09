#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithMarksBase.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IWithMarksBase {
		IList<IMarkLinkBase> GetMarkLinks();
		bool IsMarkSeted(string code);
		void RemoveMark(IMark mark);
	}
}