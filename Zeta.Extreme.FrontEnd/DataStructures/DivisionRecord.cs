using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Запись о дивизионе
	/// </summary>
	[Serialize]
	public class DivisionRecord
	{
		/// <summary>
		/// код дивизиона
		/// </summary>
		public string code;
		/// <summary>
		/// Название дивизиона
		/// </summary>
		public string name;

		/// <summary>
		/// Индекс дивизиона
		/// </summary>
		public int idx;
	}
}