using System;
using System.Collections.Generic;
using Qorpent.Config;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Общая реализация результата пробы
	/// </summary>
	public class ProbeResult : ConfigBase, IProbeResult {
		/// <summary>
		/// Опция обратной ссылки на пробу
		/// </summary>
		public const string PROBE = "probe";
		/// <summary>
		/// 
		/// </summary>
		public const string TOTALDURATION = "totalduration";
		/// <summary>
		/// 
		/// </summary>
		public const string SUBRESULTS = "subresults";

		/// <summary>
		/// 
		/// </summary>
		public const string RESULTTYPE = "resulttype";
		/// <summary>
		/// 
		/// </summary>
		public const string ERROR = "error";
		/// <summary>
		/// 
		/// </summary>
		public const string MESSAGE = "message";

		/// <summary>
		/// Обратная ссылка на пробу
		/// </summary>
		public IProbe Probe {
			get { return Get<IProbe>(PROBE); }
			set { Set(PROBE, value); }
		}

		/// <summary>
		/// Полное время выполнения
		/// </summary>
		public TimeSpan TotalDuration {
			get { return Get<TimeSpan>(TOTALDURATION); }
			set { Set(TOTALDURATION, value); }
		}

		/// <summary>
		/// Подрезультаты
		/// </summary>
		public IList<IProbeResult> SubResults {
			get { return Get<IList<IProbeResult>>(SUBRESULTS); }
			set { Set(SUBRESULTS, value); }
		}

		/// <summary>
		/// Тип результата
		/// </summary>
		public ProbeResultType ResultType
		{
			get { return Get(RESULTTYPE,ProbeResultType.Undefined); }
			set { Set(RESULTTYPE, value); }
		}

		/// <summary>
		/// Тип результата
		/// </summary>
		public Exception Error
		{
			get { return Get<Exception>(ERROR); }
			set { Set(ERROR, value); }
		}
		/// <summary>
		/// Тип результата
		/// </summary>
		public string Message
		{
			get { return Get<string>(MESSAGE); }
			set { Set(MESSAGE, value); }
		}
	}
}