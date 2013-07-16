using Qorpent.Dsl;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Простое отображение атрибута
	/// </summary>
	public class SimpleAttributeDescriptor {
		/// <summary>
		/// Имя атрибута
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Значение атрибута
		/// </summary>
		public string Value { get; set; }
		/// <summary>
		/// Информация о позиции в исходном файле
		/// </summary>
		public LexInfo LexInfo { get; set; }
	}
}