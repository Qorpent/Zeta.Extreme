using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.FrontEnd.Helpers {
	/// <summary>
	/// Запись о теме
	/// </summary>
	[Serialize]
	public class BizProcessRecord {
		/// <summary>
		/// Код темы
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// Имя темы
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Признак того, что это группа
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsGroup { get; set; }

		/// <summary>
		/// Код группы
		/// </summary>
		[SerializeNotNullOnly]
		public string Group { get; set; }
		/// <summary>
		/// Код родительской темы
		/// </summary>
		[SerializeNotNullOnly]
		public string Parent { get; set; }

		/// <summary>
		/// Порядковый номер темы
		/// </summary>
		[SerializeNotNullOnly]
		public int Idx { get; set; }

		/// <summary>
		/// Back reference to thema
		/// </summary>
		[IgnoreSerialize]
		public IThema Thema { get; set; }
	}
}