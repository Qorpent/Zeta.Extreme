#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPkgType.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface IPkgType : IEntityDataPattern, IWithPkgs,
	                            IWithFormBind {}
}