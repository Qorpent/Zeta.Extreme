namespace Zeta.Extreme.Developer.MetaStorage.Tree
{
	/// <summary>
	/// 
	/// </summary>
	public enum AttributeType {
		/// <summary>
		/// Никакой
		/// </summary>
		None,
		/// <summary>
		/// Прячущийся бул
		/// </summary>
		Bool,
		/// <summary>
		/// Явный бул
		/// </summary>
		Bool10,
		/// <summary>
		/// Значение
		/// </summary>
		Value,
	}
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
		/// <summary>
		/// Стандартный мапинг меток для строк по AP-107
		/// </summary>
		public static TagMap[] DefaultRowMarks = new[] {
				new TagMap("0PARTITION") {Ignore = true},
				new TagMap("0SA") {Element="sum"},
				new TagMap("0NOSUM", AttributeType.Bool,"nosum"),
				new TagMap("0AA"){Error=true},
			};
		/// <summary>
		/// Стандартный мапинг тегов длястрок  по AP-108
		/// </summary>
		public static TagMap[] DefaultRowTags = new[] {
				new TagMap("active",AttributeType.Bool),
				new TagMap("vgrp"),
				new TagMap("olap_tmc"){Group="olap"},
			};
		
	}
}
