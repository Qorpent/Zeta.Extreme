using Qorpent.IoC;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {
	///<summary>
	///	Определяет доступность сервера форм
	///</summary>
	[Action("zefs.registerclientstat",Role = "DEFAULT")]
	public class RegisterClientStatAction : ActionBase {
		
		/// <summary>
		/// Реализация регистратора событий
		/// </summary>
		[Inject]
		public IClientStatisticsRegistry ClientStatisticsRegistry { get; set; }

		/// <summary>
		/// 	Возвращает статус готовности сервера форм к обработке запросов
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			if (null != ClientStatisticsRegistry) {
				var req = ((MvcContext) Context).NativeAspContext.Request;
				ClientStatisticsRegistry.RegisterClientStatistics(Data,req.UserHostAddress+" ("+req.UserHostName+")",req.UserAgent) ;
			}
			return true;
		}

		/// <summary>
		/// Связанные данные клиентского лога
		/// </summary>
		[Bind] public string Data { get; set; }
	}
}