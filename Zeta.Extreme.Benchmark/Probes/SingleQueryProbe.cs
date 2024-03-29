﻿using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Benchmark.Probes
{

	/// <summary>
	/// Простая временная проба на выполнение одиночного теста
	/// </summary>
	public class SingleQueryProbe : ProbeBase
	{
		private IQuery _query;
		private ISession _session;

		/// <summary>
		/// Перекрыть для полноценной конфигурации
		/// </summary>
		protected override void InternalInitialize()
		{
			base.InternalInitialize();
			var cfg = GetConfig();
			this._query = cfg.Query;
			this._session = cfg.Session;
		}

		/// <summary>
		/// Проверяет состояние игнора пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected override bool CheckIgnore(ProbeResult result, out string message) {
			message = "";
			if (null == _query) {
				message = "запрос не указан";
				return true;
			}
			if (null == _session) {
				message = "сессия не настроена";
				return true;
			}
			return false;
		}

		/// <summary>
		/// Просто выполняет запрос в указанной сессии
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected override void ExecuteSelf(IProbeResult result, bool async) {
			QueryResult qresult = null;
			if (async) {
				var realq = _session.Register(_query);
				_session.Execute();
				qresult = realq.GetResult();

			}
			else {
				qresult = _session.AsSerial().Eval(_query);
				
			}
			if (!qresult.IsComplete) {
				result.ResultType = ProbeResultType.Error;
				result.Error = qresult.Error;
			}
			else {
				FillResult(qresult,result);
			}
		}

		private void FillResult(QueryResult qresult, IProbeResult result) {
			var q = (Query) _query;
			result.Set("evaltype", q.EvaluationType);
			result.Set("querycache", q.GetCacheKey());
			result.Set("value", qresult.NumericResult);
			result.Set("isnull", qresult.IsNull);
			result.Set("cellid", qresult.CellId);
		}
	}
}
