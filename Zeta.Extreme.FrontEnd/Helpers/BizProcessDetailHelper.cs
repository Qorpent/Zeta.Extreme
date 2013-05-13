using System.Linq;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// Вспомогательный класс для подготовки паспорта формы
	/// </summary>
	public class BizProcessDetailHelper {
		/// <summary>
		/// Возвращает детализированную информацию по форме и зависимостям
		/// </summary>
		/// <param name="thema"></param>
		/// <param name="withIns"></param>
		/// <param name="withOuts"></param>
		/// <returns></returns>
		public object GetDetails(IThema thema, bool withIns  = true, bool withOuts = true) {
			return new
				{
					code = thema.Code,
					name = thema.Name,
					process = thema.GetParameter("bizprocess.process", ""),
					dependOn =withIns?  thema.IncomeLinks.Where(_ => _.Type == "biz.dep").Select(_ => GetDetails(_.Source,true,false)).ToArray() :null,
					requiredFor =withOuts? thema.OutcomeLinks.Where(_ => _.Type == "biz.dep").Select(_ => GetDetails(_.Target,false,true)).ToArray():null ,
				};
		}
	}
}