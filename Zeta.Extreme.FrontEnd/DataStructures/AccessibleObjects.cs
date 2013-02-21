using Qorpent.Serialization;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Структура доступных старших объектов
	/// </summary>
	[Serialize]
	public class AccessibleObjects {
		/// <summary>
		/// Доступные дивизионы
		/// </summary>
		[Serialize] public DivisionRecord[] divs { get; set; }
		/// <summary>
		/// Доступные объекты
		/// </summary>
		[Serialize] public ObjectRecord[] objs { get; set; }
	}
}