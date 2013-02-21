using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Запись о старшем объекте
	/// </summary>
	[Serialize]
	public class ObjectRecord
	{
		/// <summary>
		/// Ид периода (ClassicId)
		/// </summary>
		public int id;
		/// <summary>
		/// Название периода
		/// </summary>
		public string name;
		/// <summary>
		/// Дивизион
		/// </summary>
		public string div;
		/// <summary>
		/// Индекс периода в рамках типа
		/// </summary>
		public int idx;
	}
}