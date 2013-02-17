#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaFactory.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Security.Principal;
using Comdiv.Reporting;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Фабрика тем
	/// </summary>
	public interface IThemaFactory : IDisposable {
		/// <summary>
		/// 	Кэш тем
		/// </summary>
		IDictionary<string, object> Cache { get; }

		/// <summary>
		/// 	Исходный XML
		/// </summary>
		string SrcXml { get; set; }

		/// <summary>
		/// 	Версия
		/// </summary>
		DateTime Version { get; set; }

		/// <summary>
		/// 	Получить тему по коду
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IThema Get(string code);

		/// <summary>
		/// 	Получить все темы
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IThema> GetAll();

		/// <summary>
		/// 	Получить дефиницию отчета
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IReportDefinition GetReport(string code);

		/// <summary>
		/// 	Получить тему в адаптации на текущего пользователя
		/// </summary>
		/// <returns> </returns>
		IEnumerable<IThema> GetForUser();

		/// <summary>
		/// 	Получить тему в адаптации на конкретного пользователя
		/// </summary>
		/// <param name="usr"> </param>
		/// <returns> </returns>
		IEnumerable<IThema> GetForUser(IPrincipal usr);

		/// <summary>
		/// 	Получить шаблон формы
		/// </summary>
		/// <param name="code"> </param>
		/// <param name="throwerror"> </param>
		/// <returns> </returns>
		IInputTemplate GetForm(string code, bool throwerror = false);

		/// <summary>
		/// 	Очистить кэш пользователя
		/// </summary>
		/// <param name="usrname"> </param>
		void CleanUser(string usrname);
	}
}