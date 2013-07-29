using System.Collections.Generic;
using System.Xml.Linq;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Описатель для элементов
	/// </summary>
	[Serialize]
	public class ElementDescriptor {

		/// <summary>
		/// 
		/// </summary>
		public ElementDescriptor() {
			Children = new List<ElementDescriptor>();
		}

		/// <summary>
		/// 
		/// </summary>
		public ElementDescriptor(XElement e)
		{
			Children = new List<ElementDescriptor>();
			Source = e;
			var desc = e.Describe(true);
			this.Code = desc.Code;
			this.Name = desc.Name;
			this.TagName = e.Name.LocalName;
			this.Reference = new ItemReference(e);
		}

		/// <summary>
		/// Исходный элемент
		/// </summary>
		[SerializeNotNullOnly]
		public XElement Source { get; set; }
		/// <summary>
		/// Ссылка на исходный код
		/// </summary>
		[SerializeNotNullOnly]
		public ItemReference Reference { get; set; }
		/// <summary>
		/// Документация
		/// </summary>
		[SerializeNotNullOnly]
		public Documentation Doc { get; set; }

		/// <summary>
		/// Непосредственное значение параметра
		/// </summary>
		[SerializeNotNullOnly]
		public string Value { get; set; }
		/// <summary>
		/// Код элемента
		/// </summary>
		[SerializeNotNullOnly]
		public string Code { get; set; }

		/// <summary>
		/// Имя элемента кода
		/// </summary>
		[SerializeNotNullOnly]
		public string Name { get; set; }

		/// <summary>
		/// Имя физического элемента
		/// </summary>
		[SerializeNotNullOnly]
		public string TagName { get; set; }

		/// <summary>
		/// Тип кода элемента
		/// </summary>
		[SerializeNotNullOnly]
		public CodeElementType Type { get; set; }

		/// <summary>
		/// Родительский элемент кода
		/// </summary>
		[IgnoreSerialize]
		public ElementDescriptor Parent { get; set; }

		/// <summary>
		/// Дочерние элементы кода
		/// </summary>
		[SerializeNotNullOnly]
		public IList<ElementDescriptor> Children { get; private set; }
	}
}