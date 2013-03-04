#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IConfiguration.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Интерфейс конфигурации типизированной темы (конкретного типа)
	/// </summary>
	/// <typeparam name="T"> </typeparam>
	public interface IConfiguration<T> : IWithCode, IWithName {
		/// <summary>
		/// 	Тип темы (имя)
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		string Role { get; set; }

		/// <summary>
		/// 	Ссылка
		/// </summary>
		string Url { get; set; }

		/// <summary>
		/// 	Шаблон
		/// </summary>
		string Template { get; set; }

		/// <summary>
		/// 	Признак наличия ошибки
		/// </summary>
		bool IsError { get; set; }

		/// <summary>
		/// 	Конфигурация темы
		/// </summary>
		IThemaConfiguration Thema { get; set; }

		/// <summary>
		/// 	Команда на формирование конфигурации
		/// </summary>
		/// <returns> </returns>
		T Configure();
	}
}