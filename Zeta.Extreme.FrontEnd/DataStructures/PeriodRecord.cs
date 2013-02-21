using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Запись о периоде
	/// </summary>
	[Serialize]
	public class PeriodRecord {
		/// <summary>
		/// Ид периода (ClassicId)
		/// </summary>
		public int id;
		/// <summary>
		/// Название периода
		/// </summary>
		public string name;
		/// <summary>
		/// Тип периода
		/// </summary>
		public PeriodType type;
		/// <summary>
		/// Индекс периода в рамках типа
		/// </summary>
		public int idx;
	}
}