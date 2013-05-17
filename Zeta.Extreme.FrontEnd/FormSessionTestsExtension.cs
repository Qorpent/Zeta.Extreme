using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Расширение для облегчения тестирования сессий
	/// </summary>
	public static class FormSessionTestsExtension {
		/// <summary>
		/// Test propose method to mark all Data as controlpoint results
		/// </summary>
		public static void SetAllAsControlPoints(IFormSession _session) {
			var session = (FormSession) _session;
			foreach (var cell in session.Data)
			{
				var q = cell.query.Copy();
				q.Result = new QueryResult(cell.v.ToDecimal());
				if (cell.query.Result != null && !cell.query.Result.IsComplete)
				{
					q.Result.IsComplete = false;
					q.Result.Error = cell.query.Result.Error;
				}
				if (cell.error != null) {
					q.Result.IsComplete = false;
					q.Result.Error = cell.error;
				}
				session._controlpoints.Add(new ControlPointResult
					{
						Col = new ColumnDesc(cell.query.Col.Code, cell.query.Time.Year, cell.query.Time.Period),
						Value = cell.v.ToDecimal(),
						Row = cell.query.Row.Native ?? new Row { Code = cell.query.Row.Code },
						Query = q
					});
			}
		}
	}
}