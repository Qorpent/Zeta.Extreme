#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : GetUserInfoAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// 	Возвращает реестр доступных объектов
	/// </summary>
	[Action("zeta.getuserinfo")]
	public class GetUserInfoAction : FormServerActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MyFormServer.MetaCacheLoad.Wait();
			return new UserInfoHelper().GetUserInfo(login);
		}

		/// <summary>
		/// 	Логин, по которому запрашиваются данные пользователя
		/// </summary>
		[Bind(Required = true, ValidatePattern = @"^[\w\d_\.]+\\[\w\d_\.]+$")] protected string login;
	}
}