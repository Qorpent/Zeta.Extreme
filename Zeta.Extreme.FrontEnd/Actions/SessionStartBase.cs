using System;
using Comdiv.Zeta.Model;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Базовое действие для стартующих сессию
	/// </summary>
	public class SessionStartBase : FormServerActionBase {
		/// <summary>
		/// Исходный шаблон
		/// </summary>
		protected IInputTemplate _realform;
		/// <summary>
		/// Целевой объект
		/// </summary>
		protected IZetaMainObject _realobj;
		/// <summary>
		/// Код формы
		/// </summary>
		[Bind(Required = true)] protected string form = "";
		/// <summary>
		/// Код объекта
		/// </summary>
		[Bind(Required = true)] protected int obj = 0;
		/// <summary>
		/// Период
		/// </summary>
		[Bind(Required = true)] protected int period = 0;
		/// <summary>
		/// Год
		/// </summary>
		[Bind(Required = true)] protected int year = 0;

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
	}
}