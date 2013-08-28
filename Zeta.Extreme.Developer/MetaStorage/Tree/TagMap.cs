namespace Zeta.Extreme.Developer.MetaStorage.Tree
{
	/// <summary>
	/// Разметка тегов и меток при выгоне в новый язык
	/// </summary>
	public class TagMap
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public TagMap(string name) {
			SourceName = name;
		}

		/// <summary>
		/// 
		/// </summary>
		public TagMap(string name, AttributeType type = AttributeType.Value, string rename=null)
		{
			SourceName = name;
			AttributeType = type;
			Attribute = rename;
		}

		/// <summary>
		/// Имя исходного элемента
		/// </summary>
		public string SourceName { get; set; }
		/// <summary>
		/// Признак игнора
		/// </summary>
		public bool Ignore { get; set; }
		/// <summary>
		/// Имя элемента
		/// </summary>
		public string Element { get; set; }
		/// <summary>
		/// Имя атрибута при переимновании
		/// </summary>
		public string Attribute { get; set; }

		/// <summary>
		/// Тип атрибута
		/// </summary>
		public AttributeType AttributeType { get; set; }

		/// <summary>
		/// Признак ошибки
		/// </summary>
		public bool Error { get; set; }
		/// <summary>
		/// Целевая группа
		/// </summary>
		public string Group { get; set; }
	}
}
