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
				var tasks = Children.Select(_ => _.ExecuteAsync()).ToArray();
				Task.WaitAll(tasks);
				foreach (var t in tasks) {
					result.SubResults.Add(t.Result);
				}
			}
			else {
				foreach (var c in Children) {
					result.SubResults.Add(c.ExecuteSync());
				}
			}
		}

		/// <summary>
		/// Перекрыть для выполнения собственно терминальной пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected override void ExecuteSelf(IProbeResult result, bool async) {
			
		}
	}
}
