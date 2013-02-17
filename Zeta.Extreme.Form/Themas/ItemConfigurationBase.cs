#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ItemConfigurationBase.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Xml.Linq;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Базовая конфигурация элемента темы
	/// </summary>
	/// <typeparam name="T"> </typeparam>
	public abstract class ItemConfigurationBase<T> : IConfiguration<T> {
		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		public ItemConfigurationBase() {
			Active = true;
		}

		/// <summary>
		/// 	Признак активности элемента
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// 	Значение элемента
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// 	Ссылка на конфигурацию - контейнер
		/// </summary>
		public IThemaConfiguration Thema { get; set; }

		/// <summary>
		/// 	Команда на конфигурирование
		/// </summary>
		/// <returns> </returns>
		public abstract T Configure();

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// 	Тип элемента
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 	Код элемента
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 	URL  элемента
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 	Название элемента
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 	Имя шаблона ??
		/// </summary>
		public string Template { get; set; }

		/// <summary>
		/// 	Признак наличия ошибки
		/// </summary>
		public bool IsError {
			get { return _isError || getErrorInternal(); }
			set { _isError = value; }
		}

		/// <summary>
		/// 	Перегружаемый метод для поиска ошибок
		/// </summary>
		/// <returns> </returns>
		protected virtual bool getErrorInternal() {
			return false;
		}

		/// <summary>
		/// 	Парамтеры элемента
		/// </summary>
		public readonly IList<TypedParameter> Parameters = new List<TypedParameter>();

		/// <summary>
		/// 	Предупреждения по элементу
		/// </summary>
		public readonly IList<string> Warrnings = new List<string>();

		/// <summary>
		/// 	Исходный XML шаблока
		/// </summary>
		public XElement TemplateXml;

		/// <summary>
		/// 	Признак наличия ошибок
		/// </summary>
		private bool _isError;
	}
}