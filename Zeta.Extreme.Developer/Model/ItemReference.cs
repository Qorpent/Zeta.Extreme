using System.Collections.Generic;
using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Ссылка на исходный контент
	/// </summary>
	[Serialize]
	public class ItemReference {
		private IEnumerable<ItemReference> _subReferences;

		/// <summary>
		/// Файл
		/// </summary>
		[SerializeNotNullOnly]
		public string File { get; set; }
		/// <summary>
		/// Строка
		/// </summary>
		[SerializeNotNullOnly]
		public int Line { get; set; }

		/// <summary>
		/// Главный контекст
		/// </summary>
		[SerializeNotNullOnly]
		public string MainContext { get; set; }

		/// <summary>
		/// Дочерний контекст
		/// </summary>
		[SerializeNotNullOnly]
		public string SubContext { get; set; }
	
		/// <summary>
		/// Дочерние ссылки
		/// </summary>
		[Serialize]
		public IEnumerable<ItemReference> Children {
			get { return _subReferences ?? (_subReferences = new List<ItemReference>()); }
			set { _subReferences = value; }
		}
	}
}