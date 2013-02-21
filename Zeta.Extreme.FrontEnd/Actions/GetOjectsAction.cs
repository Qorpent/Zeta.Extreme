#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetOjectsAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	Возвращает реестр доступных объектов
	/// </summary>
	[Action("zefs.getobjects")]
	public class GetOjectsAction : FormServerActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MyFormServer.HibernateLoad.Wait();
			return new AccessibleObjectsHelper().GetAccessibleObjects();
		}
	}
}