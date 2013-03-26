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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/FormSessionActionBase.cs
#endregion
using System;
using System.Linq;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Базовое действие, работающее с сессией
	/// </summary>
	public abstract class FormSessionActionBase : FormServerActionBase {
		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize() {
			if(null!=FormServer.Default) {
				FormServer.Default.ReadyToServeForms.Wait();

				if (!FormServer.Default.IsOk) {
					throw new Exception("Application not loaded properly!");
				}
			}
			//up testblility
			MySession = MySession??  FormServer.Default.Sessions.First(_ => _.Uid == Session);
		}

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			base.Validate();
			if (null == MySession) {
				throw new Exception("отсутствует целевая сессия");
			}
		}

		/// <summary>
		/// 	Ссылка на текущую сессию
		/// </summary>
		public FormSession MySession { get; set; }

		/// <summary>
		/// 	Параметр кода сессии
		/// </summary>
		[Bind(Required = true)] public string Session = "";
	}
}