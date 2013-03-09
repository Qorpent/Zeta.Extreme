#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapRowMark.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IOlapRowMark<RowType> :
		IWithId, IWithVersion,
		IMarkLink<RowType> //,
		// IWithOlapRow<RowType>
		where RowType : IOlapRow {}
}