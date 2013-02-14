using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ќписывает €чейку данных
	/// </summary>
	[Serialize]
	public class OutCell {
		/// <summary>
		/// ”никальный »ƒ €чейки
		/// </summary>
		public string i;
		/// <summary>
		/// «начение €чейки
		/// </summary>
		[SerializeNotNullOnly]
		public string v;

		/// <summary>
		/// —сылка на Id €чейки в Ѕƒ
		/// </summary>
		[SerializeNotNullOnly]
		public int c;
	}
}