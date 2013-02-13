#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IThema.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Security.Principal;
using Comdiv.Model.Interfaces;
using Comdiv.Reporting;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Базовый интерфейс темы
	/// </summary>
	public interface IThema : IPseudoThema {
		/// <summary>
		/// 	Ссылка на фабрику, создавшую тему
		/// </summary>
		IThemaFactory Factory { get; set; }

		/// <summary>
		/// 	Роль доступа к теме
		/// </summary>
		string Role { get; set; }

		/// <summary>
		/// 	Признак того, что тема - группа
		/// </summary>
		bool IsGroup { get; set; }

		/// <summary>
		/// 	Имя группы
		/// </summary>
		string Group { get; set; }

		/// <summary>
		/// 	Параметры темы
		/// </summary>
		IDictionary<string, object> Parameters { get; }

		/// <summary>
		/// 	Признак видимости
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// 	Порядок в списках
		/// </summary>
		int Idx { get; set; }

		/// <summary>
		/// 	Признак того - что тема - шаблон
		/// </summary>
		bool IsTemplate { get; set; }

		/// <summary>
		/// 	Родительская тема
		/// </summary>
		IThema ParentThema { get; set; }

		/// <summary>
		/// 	Имя родительской темы
		/// </summary>
		string Parent { get; set; }

		/// <summary>
		/// 	Дочерние темы
		/// </summary>
		IList<IThema> Children { get; }

		/// <summary>
		/// 	Признак избранной темы (для локальной копии)
		/// </summary>
		bool IsFavorite { get; set; }

		/// <summary>
		/// 	Контейнер ошибки, возникшей при обработке темы
		/// </summary>
		Exception Error { get; set; }

		/// <summary>
		/// 	Получить все формы
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IInputTemplate> GetAllForms();

		/// <summary>
		/// 	Получить все отчеты
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IReportDefinition> GetAllReports();

		/// <summary>
		/// 	Получить конкретную форму
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IInputTemplate GetForm(string code);

		/// <summary>
		/// 	Получить конкретный отчет
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IReportDefinition GetReport(string code);

		/// <summary>
		/// 	Сделать копию темы для конкретного пользователя
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		IThema Personalize(IPrincipal usr);

		/// <summary>
		/// 	Получить все встроенные документы
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IDocument> GetAllDocuments();

		/// <summary>
		/// 	Получить все встроенные команды
		/// </summary>
		/// <returns> </returns>
		IEnumerable<ICommand> GetAllCommands();

		/// <summary>
		/// 	Приспособить тему под конкретный период
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		IThema Accomodate(IZetaMainObject obj, int year, int period);

		/// <summary>
		/// 	Расширенный метод подготовки темы к периоду с учетом кэша состояний
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="statecache"> </param>
		/// <returns> </returns>
		IThema Accomodate(IZetaMainObject obj, int year, int period, IDictionary<string, object> statecache);

		/// <summary>
		/// 	Получить конкретный документ
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IDocument GetDocument(string code);

		/// <summary>
		/// 	Получить конкретную команду
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		ICommand GetCommand(string code);

		/// <summary>
		/// 	Получить состав группы
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IThema> GetGroup();

		/// <summary>
		/// 	Признак активности темы для конкретного пользователя
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		bool IsActive(IPrincipal usr);

		/// <summary>
		/// 	Расчет видимости темы для текущего пользователя
		/// </summary>
		/// <returns> </returns>
		bool IsVisible();

		/// <summary>
		/// 	Расчет видимости темы для указанного пользователя
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		bool IsVisible(IPrincipal usr);

		/// <summary>
		/// 	Типизированная оболочка расчета параметра
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="code"> </param>
		/// <param name="def"> </param>
		/// <returns> </returns>
		T GetParameter<T>(string code, T def);
	}
}