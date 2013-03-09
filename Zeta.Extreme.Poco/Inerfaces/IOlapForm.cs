#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapForm.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IOlapForm<RowType> : IOlapRow,
	                                      ITree<RowType>, IWthRefTo<RowType> where RowType : IOlapForm<RowType> {}
}