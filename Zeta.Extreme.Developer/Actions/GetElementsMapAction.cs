using System.Linq;
using Qorpent.Mvc;

namespace Zeta.Extreme.Developer.Actions
{
	/// <summary>
	/// Действие, возвращающее матрицу типов ключей и типов кода, встретившегося в источниках
	/// </summary>
	[Action("zdev.getelementsmap")]
	public class GetElementsMapAction:AnalyzerActionBase
	{
		/// <summary>
		/// Вызывет аналитически метод оценки мапинга ключей
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return Analyzer.GetElementTypeMap().ToArray();
		}
	}
}
