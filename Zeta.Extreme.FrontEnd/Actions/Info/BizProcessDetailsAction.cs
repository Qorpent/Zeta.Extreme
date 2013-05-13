using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// 	Возвращает информацию о сессии
	/// </summary>
	[Action("zefs.formdetails")]
	public class BizProcessDetailsAction : FormServerActionBase
	{
		/// <summary>
		/// Код формы
		/// </summary>
		[Bind]public string Form { get; set; }
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			return new BizProcessDetailHelper().GetDetails(((ExtremeFormProvider)MyFormServer.FormProvider).Factory.Get(Form));
		}
	}
}