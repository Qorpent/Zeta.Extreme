#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/GetUserInfoAction.cs
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
		[Bind(Required = true, ValidatePattern = @"^([\w\d_\.]+\\)?[\w\d_\.]+$")] protected string login;
	}
}