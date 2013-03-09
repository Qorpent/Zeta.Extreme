#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapDetailObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;

namespace Comdiv.Olap.Model {
	public interface IOlapDetailObject<M, D> :
		IDimension, IWithRange,
		IOlapDetailObjectBase<M>,
		IWithDetailGroupLinks<M, D>
		where M : IOlapMainObjectBase
		where D : IOlapDetailObjectBase<M> {}
}