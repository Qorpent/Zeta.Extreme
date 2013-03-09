#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IUsrRow.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model {
	public interface IUsrRow : ISimpleEntityDataPattern, IWithCode {
		bool Active { get; set; }
		IZetaUnderwriter Usr { get; set; }
		IZetaRow Row { get; set; }
	}
}