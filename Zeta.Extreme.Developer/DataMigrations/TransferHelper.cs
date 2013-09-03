using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Zeta.Extreme.Developer.DataMigrations
{
	/// <summary>
	/// Вспомогательный класс для генерации скриптов на превращение формулы в первичную
	/// строку
	/// </summary>
	public class TransferHelper
	{
	
		/// <summary>
		/// Создает скрипт для переноса строки
		/// </summary>
		/// <returns></returns>
		public string GenerateScript(TransferTask task) {
			
			var sb = new StringBuilder();
			int cnt = 0;
			var session = new Session();
			IList<Query> _agenda = new List<Query>();
			int c = 0;
			foreach (var s in task.GetTransferRecords()) {
				cnt++;
				c++;
				var q = (Query) session.Register(s.CreateQuery());
				q.Data = s;
				_agenda.Add(q);
				if (cnt >= 200) {
					cnt = 0;
					session.WaitPreparation();
					session.WaitEvaluation();
					foreach (var rq in _agenda) {
						if (rq.Result.NumericResult != 0) {
							if (null == rq.Obj.Native) continue;
							var sql = ((TransferRecord) rq.Data).CreateUpdateSqlQuery(rq.Result.NumericResult);
							sb.AppendFormat(@"/*{0,-4} [ {8} ] {2} {3,-6} {4,-10} {5,-4} {6} {7}*/
{1}

", c, sql, rq.Time.Year, rq.Time.Period, rq.Row.Code, rq.Col.Code, rq.Currency, rq.Obj.Name, rq.Result.NumericResult.ToString("#,0.######", CultureInfo.GetCultureInfo("Ru-ru")));
						}
					}
					_agenda.Clear();
					session = new Session();
				}

			}
			return sb.ToString();
		}
		
	}
}
