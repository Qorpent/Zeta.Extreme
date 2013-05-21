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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/LockFormAction.cs
#endregion

using System;
using System.Linq;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.StateManagement;

namespace Zeta.Extreme.FrontEnd.Actions.States {
	/// <summary>
	/// 	Возвращает текущий статус сохранения
	/// </summary>
	[Action("zefs.setstate")]
	public class SetStateAction : SessionStartBase {
		/// <summary>
		/// Новый статус
		/// </summary>
		[Bind]public string NewState { get; set; }

		/// <summary>
		/// Комментарий к блокировке
		/// </summary>
		[Bind]public string Comment { get; set; }

		/// <summary>
		/// Текущая сессия
		/// </summary>
		[Bind]public string Session { get; set; }
		FormStateType state;

		/// <summary>
		/// 	Third part of execution - setup system-bound internal state here (called after validate, but before authorize)
		/// </summary>
		protected override void Prepare()
		{
			base.Prepare();
			if (!FormStateType.TryParse(NewState, true, out state)) {
				throw new Exception("illegal type "+NewState);
			}
		}
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			MyFormServer.ReadyToServeForms.Wait();
			var mysession = MyFormServer.CreateSession(_realform, _realobj, year, period,null);
			mysession.InitStateMode = true;
			if (state == FormStateType.Closed) {
				mysession.Start();
				mysession.WaitData();
			}
			return mysession.SetState(state,Comment);
		}
	}


	
}