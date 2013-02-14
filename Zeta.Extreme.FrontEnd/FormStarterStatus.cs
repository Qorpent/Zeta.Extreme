using Qorpent.Mvc;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Действие, возвращающее статус загрузки приложения
	/// </summary>
	[Action("exf.serverstate")]
	public class FormServerStateAction : ActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return new
				{
					hibernate = new {
					status = FormServer.Default.HibernateLoad.Status,
					error = FormServer.Default.HibernateLoad.Error.ToStr()
					},
					meta = new
					{
						status = FormServer.Default.MetaCacheLoad.Status,
						error = FormServer.Default.MetaCacheLoad.Error.ToStr()
					},
					formulas = new
					{
						status = FormServer.Default.CompileFormulas.Status,
						error = FormServer.Default.CompileFormulas.Error.ToStr()
					},
					themas = new
					{
						status = FormServer.Default.LoadThemas.Status,
						error = FormServer.Default.LoadThemas.Error.ToStr()
					},

				};
		}
	}
}