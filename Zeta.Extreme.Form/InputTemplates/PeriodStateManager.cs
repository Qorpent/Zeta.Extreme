using System;
using System.Collections.Generic;
using System.Data;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Persistence;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// ������� �������� �������� ��������
	/// </summary>
	public class PeriodStateManager : IPeriodStateManager
	{
		/// <summary>
		/// �������
		/// </summary>
		public string System { get; set; }

		/// <summary>
		/// ��
		/// </summary>
		public string Database { get; set; }
		void indatabase(Action<IDbConnection> action)
		{
			using (var c = myapp.ioc.getConnection(System))
			{
				c.WellOpen();
				if (Database.hasContent())
				{
					c.ChangeDatabase(Database);
				}
				action(c);
			}

		}

		/// <summary>
		/// �������� ��� ������
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		public PeriodStateRecord[] All(int year)
		{
			PeriodStateRecord[] result = null;
			indatabase(c => result = c.ExecuteOrm<PeriodStateRecord>("select * from usm.periodstate where year = " + year,
			                                                         (IParametersProvider)null));
			return result;
		}

		/// <summary>
		/// �������� ������ �� ���� � �������
		/// </summary>
		/// <param name="year"></param>
		/// <param name="period"></param>
		/// <returns></returns>
		public PeriodStateRecord Get(int year, int period)
		{
			var result = new PeriodStateRecord();
			result.Year = year;
			result.Period = period;
			IDictionary<string, object> dict = null;
			indatabase(c => dict = c.ExecuteDictionary("select state,deadline,udeadline from usm.periodstate where year=" + year + " and period=" + period, null));
			result.State = dict.get("state", () => result.State);
			result.DeadLine = dict.get("deadline", () => result.DeadLine);
			result.UDeadLine = dict.get("udeadline", () => result.UDeadLine);
			return result;
		}

		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="record"></param>
		public void UpdateState(PeriodStateRecord record)
		{
			indatabase(c => c.ExecuteNonQuery("exec usm.set_period_state @year=" + record.Year + ",@period=" + record.Period + ",@state=" + (record.State ? 1 : 0), (IParametersProvider)null));
		}

		/// <summary>
		/// �������� �������
		/// </summary>
		/// <param name="record"></param>
		public void UpdateDeadline(PeriodStateRecord record)
		{
			indatabase(c => c.ExecuteNonQuery(@"exec usm.set_period_deadline @year=" + record.Year + ",@period=" + record.Period + ",@deadline=@date",
			                                  new Dictionary<string, object> { { "@date", record.DeadLine } }
				                ));
		}

		/// <summary>
		/// �������� ������� �� ����������
		/// </summary>
		/// <param name="record"></param>
		public void UpdateUDeadline(PeriodStateRecord record)
		{
			indatabase(c => c.ExecuteNonQuery(@"exec usm.set_period_udeadline @year=" + record.Year + ",@period=" + record.Period + ",@deadline=@date",
			                                  new Dictionary<string, object> { { "@date", record.UDeadLine } }
				                ));
		}
	}
}