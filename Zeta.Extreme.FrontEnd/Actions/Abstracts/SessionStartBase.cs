#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SessionStartBase.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using System.Security;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.FrontEnd.Helpers;
using Zeta.Extreme.Model;
using Zeta.Extreme.Poco.Inerfaces;

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
		}

		/// <summary>
		/// 	Авторизует предприятие и форму
		/// </summary>
		protected override void Authorize() {
			base.Authorize();
			var acessobject = new AccessibleObjectsHelper().GetAccessibleObjects();
			if (!acessobject.objs.Any(_ => _.id == _realobj.Id)) {
				throw new SecurityException("try access not allowed object");
			}
			if (!Application.Roles.IsInRole(Application.Principal.CurrentUser, _realform.Role)) {
				throw new SecurityException("try access not allowed form");
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
	}
}