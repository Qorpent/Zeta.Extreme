#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IOlapDetailObject.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public interface IOlapDetailObject<M, D> :
		IDimension, IWithRange,
		IOlapDetailObjectBase<M>
		where M : IOlapMainObjectBase
		where D : IOlapDetailObjectBase<M> {}
}