#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetDataAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	���������� ������
	/// </summary>
	[Action("zefs.data")]
	public class GetDataAction : FormSessionActionBase {
		/// <summary>
		/// processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.GetNextChunk(startidx);
		}

		[Bind(Required = false)] private int startidx = 0;
	}
}