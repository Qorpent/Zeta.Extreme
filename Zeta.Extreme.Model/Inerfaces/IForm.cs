#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IForm.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	public interface IForm : IWithId, IWithCode, IWithVersion {
		int Year { get; set; }
		int Period { get; set; }
		string Template { get; set; }
		IList<IFormState> States { get; set; }
		string CurrentState { get; set; }
		 IZetaMainObject Object { get; set; }
		IFormState GetLastState();
		IFormState GetLastBlock();
	                         }
}