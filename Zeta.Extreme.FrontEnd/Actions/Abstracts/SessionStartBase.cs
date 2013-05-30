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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/SessionStartBase.cs
#endregion
using System;
using System.Linq;
using System.Security;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.FrontEnd.Helpers;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	Базовое действие для стартующих сессию
	/// </summary>
	public class SessionStartBase : FormServerActionBase {
		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			MyFormServer.ReadyToServeForms.Wait();
			if (!FormServer.Default.IsOk) {
				throw new Exception("Application not loaded properly!");
			}
			base.Validate();
			_realform = FormServer.Default.FormProvider.Get(form);
			if (null == _realform) {
				throw new Exception("form not found");
			}
			_realobj = MetaCache.Default.Get<IZetaMainObject>(obj);
			if (null == _realobj) {
				throw new Exception("obj not found");
			}
			if (subobj != 0) {
				_realsubobj = MetaCache.Default.Get<IZetaMainObject>(subobj);
			}
		}

		AuthorizeHelper authhelper = new AuthorizeHelper();

		/// <summary>
		/// 	Авторизует предприятие и форму
		/// </summary>
		protected override void Authorize() {
			base.Authorize();
			var acessobject = new AccessibleObjectsHelper().GetAccessibleObjects();
			if (!acessobject.objs.Any(_ => _.id == _realobj.Id)) {
				throw new SecurityException("try access not allowed object");
			}
			if (!authhelper.IsAllowed(_realform)) {
				throw new SecurityException("form is not allowed");
			}
		}

		/// <summary>
		/// 	Исходный шаблон
		/// </summary>
		protected IInputTemplate _realform;

		/// <summary>
		/// 	Целевой объект
		/// </summary>
		protected IZetaMainObject _realobj;

		/// <summary>
		/// 	Код формы
		/// </summary>
		[Bind(Required = true)] protected string form = "";

		/// <summary>
		/// 	Код объекта
		/// </summary>
		[Bind(Required = true)] protected int obj = 0;

		/// <summary>
		/// 	Период
		/// </summary>
		[Bind(Required = true)] protected int period = 0;

		/// <summary>
		/// 	Год
		/// </summary>
		[Bind(Required = true)] protected int year = 0;

		/// <summary>
		/// 	Подобъект
		/// </summary>
		[Bind(Required = false)] protected int subobj = 0;

		/// <summary>
		/// Реальный подобъект
		/// </summary>
		protected IZetaMainObject _realsubobj;
	}
}