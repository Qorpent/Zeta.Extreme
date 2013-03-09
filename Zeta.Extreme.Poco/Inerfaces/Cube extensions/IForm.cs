#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IForm.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model {
	public interface IForm : IWithId, IWithCode, IWithVersion,
	                         // IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
	                         IWithMainObject<IZetaMainObject> {
		int Year { get; set; }
		int Period { get; set; }
		string Template { get; set; }
		IList<IFormState> States { get; set; }
		string CurrentState { get; set; }
		FormStates State { get; }
		IFormState GetLastState();
		IFormState GetLastBlock();
	                         }
}