#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapForm.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces.Partial;

namespace Zeta.Extreme.Model.Inerfaces.Bases {
	public interface IOlapForm<RowType> : IOlapRow,
	                                      ITree<RowType>, IWthRefTo<RowType> where RowType : IOlapForm<RowType> {}
}