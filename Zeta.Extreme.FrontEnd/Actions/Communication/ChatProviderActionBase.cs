#region LICENSE

// Copyright 2012-2013 Fagim Sadykov
// Project: Zeta.Extreme.FrontEnd
// Original file :ChatProviderActionBase.cs
// Branch: ZEUS
// This code is produced especially for ZEUS PROJECT and
// can be used only with agreement from Fagim Sadykov
// and ZEUS PROJECTS'S owner

#endregion

using System.Linq;
using Qorpent.Mvc;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.Communication {
	/// <summary>
	///     Абстрактное действие для общих управляющих компонентов для глобального чата
	/// </summary>
	public abstract class ChatProviderActionBase : FormServerActionBase {
		/// <summary>
		/// 	Third part of execution - setup system-bound internal state here (called after validate, but before authorize)
		/// </summary>
		protected override void Prepare() {
			base.Prepare();
			MyFormServer.ReadyToServeForms.Wait();
			_provider = ResolveService<IFormChatProvider>();
			if (null == _provider) {
				throw new ValidationException("IFormChatProvider not configured");
			}
		}
		/// <summary>
		/// Возвращает список избранных объектов
		/// </summary>
		/// <returns></returns>
		protected int[] GetMyOwnObjects() {
			return
				new AccessibleObjectsHelper().GetAccessibleObjects(Context.User)
				                             .objs.Where(_ => _.ismyobj)
				                             .Select(_ => _.id)
				                             .ToArray();
		}


		/// <summary>
		/// Все доступные предприятия
		/// </summary>
		/// <returns></returns>
		protected int[] GetMyAccesibleObjects() {
			return
				new AccessibleObjectsHelper().GetAccessibleObjects(Context.User)
											 .objs
											 .Select(_ => _.id)
											 .ToArray();
		}
		/// <summary>
		/// Список доступных форм
		/// </summary>
		/// <returns></returns>
		protected string[] GetMyFormCodes() {
			return new BizProcessEnumerator().GetAllBizProcesses(Context.User).SelectMany(
				_=>new[]{_.Code+"A.in",_.Code+"B.in"}
			).ToArray();
		}
		/// <summary>
		/// Провайдер работы с чатом
		/// </summary>
		protected IFormChatProvider _provider;
	}
}