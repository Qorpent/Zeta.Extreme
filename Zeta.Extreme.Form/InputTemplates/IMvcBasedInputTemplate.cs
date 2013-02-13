using System;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// Устарелая часть интерфейса для форм, ориентирующаяся на серверную обработку и MVC
	/// </summary>
	//[Obsolete("не будет больше серверной генерации в таком виде")] 
	public interface IMvcBasedInputTemplate {
		/// <summary>
		/// Непонятно что для MVC
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string Controller { get; set; }

		/// <summary>
		/// Признак использования нестандартного вида
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] bool IsCustomView { get; }

		/// <summary>
		/// Нестандартный пользовательский вид
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string CustomView { get; set; }

		/// <summary>
		/// Нестандартный тип контроллера
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")] string CustomControllerType { get; set; }

		/// <summary>
		/// Вид таблицы
		/// </summary>
		[Obsolete("не будет больше серверной генерации в таком виде")]string TableView { get; set; }
	}
}