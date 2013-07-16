using System.Collections.Generic;
using Qorpent.Serialization;
using Zeta.Extreme.Developer.Analyzers;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Описатель атрибуты
	/// </summary>
	[Serialize]
	public class AttributeDescriptor {
		/// <summary>
		/// Описатель атрибута по умолчанию
		/// </summary>
		public AttributeDescriptor() {
			ValueVariants =  new List<AttributeValueVariant>();
		}
		/// <summary>
		/// Имя атрибута
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Явное указание кол-ва вариантов
		/// </summary>
		public int VariantCount { get; set; }

		/// <summary>
		/// Явное указкние кол-ва референсов
		/// </summary>
		public int ReferenceCount { get; set; }
		/// <summary>
		/// Варианты значений с точками применения
		/// </summary>
		[Serialize]
		public IList<AttributeValueVariant> ValueVariants { get; private set; } 
	}
}