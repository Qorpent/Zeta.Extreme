#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IThemaHelper.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.BizProcess.Reports;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Стандартный хелпер для тем (для веба)
	/// </summary>
	public interface IThemaHelper {
		/// <summary>
		/// 	Получить тему
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IThema get(string code);

		/// <summary>
		/// 	Получить конфигурацию
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IThemaConfiguration getcfg(string code);

		/// <summary>
		/// 	Получить форму
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IInputTemplate getform(string code);

		/// <summary>
		/// 	Получить отчет
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		IReportDefinition getreport(string code);

		/// <summary>
		/// 	Получить дочерние
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		IThema[] getchildren(IThema thema);

		/// <summary>
		/// 	Получить дочерние (по коду)
		/// </summary>
		/// <param name="themacode"> </param>
		/// <returns> </returns>
		IThema[] getchildren(string themacode);

		/// <summary>
		/// 	Получить группы
		/// </summary>
		/// <returns> </returns>
		IThema[] getgroups();

		/// <summary>
		/// 	Получить корневые темы для группы
		/// </summary>
		/// <param name="group"> </param>
		/// <returns> </returns>
		IThema[] getgrouproots(IThema group);

		/// <summary>
		/// 	Получить корневые темы для группы (по коду)
		/// </summary>
		/// <param name="groupcode"> </param>
		/// <returns> </returns>
		IThema[] getgrouproots(string groupcode);

		/// <summary>
		/// 	Проверка, что тема тестовая
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		bool istest(IThema thema);

		/// <summary>
		/// 	Проверка, что тема в разработке
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		bool isdesign(IThema thema);

		/// <summary>
		/// 	Проверка, что тема - админская
		/// </summary>
		/// <param name="thema"> </param>
		/// <returns> </returns>
		bool isadmin(IThema thema);
	}
}