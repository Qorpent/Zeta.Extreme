using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Описывает периоды, сгруппированные по типу
	/// </summary>
	[Serialize]
	public class PeriodTypeGroup {
		/// <summary>
		/// Тип
		/// </summary>
		public PeriodType type;
		/// <summary>
		/// Набор периодов
		/// </summary>
		public PeriodRecord[] periods;
	}
}