using Comdiv.Zeta.Model.ExtremeSupport;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// Внутренняя конструкция для описания участка скрипта в терминах сочетания объкта-колонки
	/// </summary>
	internal struct ObjColQueryGeneratorStruct {
		/// <summary>
		/// Id объекта
		/// </summary>
		public int o;
		/// <summary>
		/// Id колонки
		/// </summary>
		public int c;
		/// <summary>
		/// Тип объекта
		/// </summary>
		public ObjType t;
		/// <summary>
		/// Тип детали
		/// </summary>
		public DetailMode m;
	}
}