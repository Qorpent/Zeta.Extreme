using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeta.Extreme.FrontEnd;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Benchmark.Probes
{
	/// <summary>
	/// Проба на загрузку полной формы
	/// </summary>
	public class FormLoadProbe:ProbeBase
	{
		/// <summary>
		/// Перекрыть для выполнения собственно терминальной пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected override void ExecuteSelf(IProbeResult result, bool async) {
			var cfg = GetConfig();
			var form = cfg.ThemaFactory.GetForm(cfg.FormTemplate);
			var obj = cfg.MetaCache.Get<IZetaMainObject>(cfg.FormObj);
			var session = new FormSession(form, cfg.FormYear, cfg.FormPeriod, obj);
			session.Start();
			session.WaitData();
			result.Set("stats", session.DataStatistics);
			result.Set("overalldatatime", session.OverallDataTime);
			result.Set("timetoprimary", session.TimeToPrimary);
			result.Set("timetostructure", session.TimeToStructure);
			result.Set("timetoprepare", session.TimeToPrepare);
			result.Set("totalcells", session.Data.Count);
		}


		/// <summary>
		/// Проверяет состояние игнора пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected override bool CheckIgnore(ProbeResult result, out string message) {
			message = "";
			var cfg = GetConfig();
			if (null == cfg.MetaCache) {
				message = "no meta cache";
				return true;
			}
			if (null == cfg.ThemaFactory)
			{
				message = "no meta factory";
				return true;
			}
			if (string.IsNullOrWhiteSpace(cfg.FormTemplate)) {
				message = "no form";
				return true;
			}
			if (0==cfg.FormObj)
			{
				message = "no obj";
				return true;
			}
			if (0 == cfg.FormYear)
			{
				message = "no year";
				return true;
			}
			if (0 == cfg.FormPeriod)
			{
				message = "no period";
				return true;
			}


			return false;

		}
	}
}
