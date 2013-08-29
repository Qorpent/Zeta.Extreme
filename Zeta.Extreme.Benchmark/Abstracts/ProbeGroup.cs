using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zeta.Extreme.Benchmark
{
	/// <summary>
	/// Группа, коллекция проб
	/// </summary>
	public class ProbeGroup : ProbeBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected override void ExecuteSubProbes(IProbeResult result, bool @async) {
			result.SubResults = new List<IProbeResult>();
			if (@async) {
				ExecuteGroupAsync(result);
			}
			else {
				ExecuteGroupSync(result);
			}
			if (result.SubResults.Any(r => r.ResultType == ProbeResultType.Error)) {
				result.ResultType = ProbeResultType.Error;
				result.Error = new Exception("error in child probes");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="result"></param>
		protected virtual void ExecuteGroupSync(IProbeResult result) {
			foreach (var c in Children) {
				result.SubResults.Add(c.ExecuteSync());
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="result"></param>
		protected virtual void ExecuteGroupAsync(IProbeResult result) {
			var tasks = Children.Select(_ => _.ExecuteAsync()).ToArray();
			Task.WaitAll(tasks);
			foreach (var t in tasks) {
				result.SubResults.Add(t.Result);
			}
		}

		/// <summary>
		/// Проверяет состояние игнора пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected override bool CheckIgnore(ProbeResult result, out string message) {
			message = "";
			if (null == Children || 0 == Children.Count) {
				message = "no child probes configured";
				return true;
			}
			return false;
		}

		/// <summary>
		/// Перекрыть для выполнения собственно терминальной пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected override void ExecuteSelf(IProbeResult result, bool async) {
			result.ResultType = ProbeResultType.Ignored;
			result.Message = "no probes in group";
		}
	}
}
