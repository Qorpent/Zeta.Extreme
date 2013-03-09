#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IDetailRow.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface IDetailRow : ISimpleEntityDataPattern, IWithCode {
		bool Active { get; set; }
		IZetaDetailObject Detail { get; set; }
		IZetaRow Row { get; set; }
	}
}