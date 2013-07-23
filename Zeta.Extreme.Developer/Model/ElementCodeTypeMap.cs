using System.Collections.Generic;
using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// 
	/// </summary>
	[Serialize]
	public class ElementCodeTypeMap {
		/// <summary>
		/// 
		/// </summary>
		public ElementCodeTypeMap() {
			References = new List<ItemReference>();
		}
		/// <summary>
		/// Путь к элементу
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// Сопоставленный тип кода
		/// </summary>
		[Serialize]
		public CodeElementType Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Serialize]
		public IList<ItemReference> References { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public Documentation Doc { get;  set; }

		/// <summary>
		/// Количество ссылок
		/// </summary>
		[Serialize]
		public int RefCount { get; set; }

		/// <summary>
		/// Имена задействованных тегов
		/// </summary>
		[SerializeNotNullOnly]
		public string TagNames { get; set; }
	}
}