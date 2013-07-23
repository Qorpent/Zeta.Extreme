namespace Zeta.Extreme.Developer.Analyzers {
	/// <summary>
	/// Условия поиска для анализаторов
	/// </summary>
	public class SearchFilter {
		/// <summary>
		/// Дефолтный поиск
		/// </summary>
		public SearchFilter() {
			CollectValues = true;
			IncludeDoc = true;
			IncludeReferences = true;
		}
		/// <summary>
		/// Записывать в атрибуты варианты их значений
		/// </summary>
		public bool CollectValues { get; set; }

		/// <summary>
		/// Корень документации
		/// </summary>
		public string DocRoot { get; set; }


		/// <summary>
		/// Включать ссылки в выдачу
		/// </summary>
		public bool IncludeReferences { get; set; }
		/// <summary>
		/// Включать документацию
		/// </summary>
		public bool IncludeDoc { get; set; }

		/// <summary>
		/// При каком количестве значений происходит возврат псевдо-значения "много"
		/// </summary>
		public int AttributeValueLimit { get; set; }
		/// <summary>
		/// При каком количестве референсов происходит возврат псевдо-референса "много"
		/// </summary>
		public int ReferenceLimit { get; set; }

		/// <summary>
		/// Приведение к строке для ключа
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return "sf:" + CollectValues+":avl:"+AttributeValueLimit+":arl:"+ReferenceLimit+
			":ir:"+IncludeReferences+":id:"+IncludeDoc+":dr:"+DocRoot;
		}
	}
}