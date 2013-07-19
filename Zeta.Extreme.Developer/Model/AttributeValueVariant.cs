using System.Collections.Generic;
using Qorpent.Dsl;
using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Вариант значения атрибута
	/// </summary>
	[Serialize]
	public class AttributeValueVariant {
		/// <summary>
		/// Вариант значения по умолчнию
		/// </summary>
		public AttributeValueVariant() {
			References = new List<ItemReference>();
		}
		/// <summary>
		/// Родительский атрибут
		/// </summary>
		[IgnoreSerialize]
		public AttributeDescriptor Parent { get; set; }
		/// <summary>
		/// Документация
		/// </summary>
		[SerializeNotNullOnly]
		public Documentation Doc { get; set; }
		/// <summary>
		/// Явное указкние кол-ва референсов
		/// </summary>
		public int ReferenceCount { get; set; }
		/// <summary>
		/// Значение
		/// </summary>
		[Serialize]
		public string Value { get; set; }
		/// <summary>
		/// Позиции в исходных документах
		/// </summary>
		[Serialize]
		public IList<ItemReference> References { get; private set; }
	}
}