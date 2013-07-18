using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Документация на элементы синтаксиса
	/// </summary>
	[Serialize]
	public class Documentation
	{
		/// <summary>
		/// Ключ документа
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Краткое описание, имя
		/// </summary>
		[SerializeNotNullOnly]
		public string Name { get; set; }

		/// <summary>
		/// Полное описание
		/// </summary>
		[SerializeNotNullOnly]
		public string Comment { get; set; }

		/// <summary>
		/// Признак устаревшего параметра
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsObsolete {
			get { return !string.IsNullOrWhiteSpace(Obsolete); }
		}
		/// <summary>
		/// Информация об устаревании элемента синтаксиса
		/// </summary>
		public string Obsolete { get; set; }
	}
}