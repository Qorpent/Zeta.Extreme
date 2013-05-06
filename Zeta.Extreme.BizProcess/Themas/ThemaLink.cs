namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// Связь между темами
	/// </summary>
	public class ThemaLink {
		/// <summary>
		///	Источник
		/// </summary>
		public IThema Source { get; set; }
		/// <summary>
		/// Цель
		/// </summary>
		public IThema Target { get; set; }
		/// <summary>
		/// Тип
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// Значение
		/// </summary>
		public string Value { get; set; }
	}
}