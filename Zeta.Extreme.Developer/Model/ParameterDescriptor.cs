using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Model
{
	/// <summary>
	/// Описание параметра
	/// </summary>
	[Serialize]
	public class ParameterDescriptor {
		/// <summary>
		/// Элементы с определениями параметра
		/// </summary>
		[SerializeNotNullOnly] public ElementDescriptor[] Definitions;

		/// <summary>
		/// Элементы с ссылками на параметр
		/// </summary>
		[SerializeNotNullOnly] public ElementDescriptor[] References;
		/// <summary>
		/// Документация на параметр
		/// </summary>
		[SerializeNotNullOnly] public Documentation Doc;
		/// <summary>
		/// Код параметра
		/// </summary>
		[Serialize]
		public string Code { get; set; }
		/// <summary>
		/// Имя параметра
		/// </summary>
		[Serialize]
		public string Name { get; set; }
	}
}
