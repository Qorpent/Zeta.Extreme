using System;

namespace Zeta.Extreme.Developer.Model
{
	/// <summary>
	/// Тип элементов кода темы
	/// </summary>
	[Flags]
	public enum CodeElementType : long
	{
		//FREE 32,33,38,39,53,55

		/// <summary>
		/// Неопределенный
		/// </summary>
		Undefined = 1L<<63,
		/// <summary>
		/// Библиотека параметров
		/// </summary>
		ParamLib = 1,
		/// <summary>
		/// Параметр, определенные в библиотеке
		/// </summary>
		ParamDefLib = 1L<<1,
		/// <summary>
		/// Указание параметра в корне (обычно в импортируемом файле)
		/// </summary>
		ParamDefRoot = 1L<<2,
		/// <summary>
		/// Статический параметр, определенный локально
		/// </summary>
		ReportParamDefLocalVar = 1L<<3,
		/// <summary>
		/// Динамический параметр, определенный локально
		/// </summary>
		ReportParamDefLocalParam = 1L<< 4,
		/// <summary>
		/// Параметр, определенный локально в форме
		/// </summary>
		ParamInForm = 1L<< 5,
		/// <summary>
		/// Параметр, показываемый локально в отчете
		/// </summary>
		ParamShowReport = 1L<< 6,
		/// <summary>
		///Параметр определяемый в наборе параметров (аналог колсета)
		/// </summary>
		ParamInParamset = 1L<< 7,
		/// <summary>
		/// Набор параметров (аналог колсета)
		/// </summary>
		ParamSet = 1L<< 8,
		/// <summary>
		/// Ссылка ASK на параметр в paramset
		/// </summary>
		ParamAskInParamset = 1L<< 9,
		/// <summary>
		/// Запрос на парамтер в определении отчета
		/// </summary>
		ParamAskInReportDef = 1L<< 10,
		/// <summary>
		/// Запрос параметра в репортсете
		/// </summary>
		ParamAskInReportSet = 1L<< 11,
		/// <summary>
		/// Запрос параметра в расширении отчета
		/// </summary>
		ParamAskInReportSetEx = 1L<< 12,
		
		/// <summary>
		/// Определение запроса параметра в колсете
		/// </summary>
		ParamAskReferenceInColset = 1L<< 13,
	

		/// <summary>
		/// Утверждение на параметр в paramset
		/// </summary>
		ParamUseInParamset = 1L<< 14,
		/// <summary>
		/// Утверждение на парамтер в определении отчета
		/// </summary>
		ParamUseInReportDef = 1L<< 15,
		/// <summary>
		/// Утверждение параметра в репортсете
		/// </summary>
		ParamUseInReportSet = 1L<< 16,
		/// <summary>
		/// Утверждение параметра в расширении отчета
		/// </summary>
		ParamUseInReportSetEx = 1L<< 17,
	
		/// <summary>
		/// Утверждение  параметра в колсете
		/// </summary>
		ParamUseReferenceInColset = 1L<< 18,


		/// <summary>
		/// Определение колсета
		/// </summary>
		Colset = 1L<< 19,

		/// <summary>
		/// Импорт колсета в другой колсет
		/// </summary>
		ColsetImportIntoColset = 1L<< 20,
		/// <summary>
		/// Колонка, определенная в колсете
		/// </summary>
		ColInColset = 1L<< 21,
		/// <summary>
		/// Импорт, наследование тем
		/// </summary>
		Imports = 1L<< 22,
		/// <summary>
		/// Импорт парамсета
		/// </summary>
		ImportParamset = 1L<< 23,

		/// <summary>
		/// Колонка, определенная в отчете
		/// </summary>
		ColInReport = 1L<< 24,
		/// <summary>
		/// Колонка, определенная в форме
		/// </summary>
		ColInForm = 1L<< 25,

		
		/// <summary>
		/// Подключение расширения
		/// </summary>
		Extension = 1L<< 26,
		/// <summary>
		/// Набор объектов
		/// </summary>
		Objset = 1L<< 27,

		/// <summary>
		/// Тема, объявленная как унаследованная
		/// </summary>
		ThemaInherited = 1L<< 28,
		/// <summary>
		/// Тема объявленная прямо тегом Thema
		/// </summary>
		ThemaRooted = 1L<< 29,
		
		/// <summary>
		/// Определение автозамены языка
		/// </summary>
		SubstDefinition = 1L<< 30,
		/// <summary>
		/// Пользовательские расширения контента
		/// </summary>
		ContentExtensions = 1L<< 31,
		




		/// <summary>
		/// Объект в группе объектов
		/// </summary>		
		ObjectInObjset = 1L<< 34,

		/// <summary>
		/// Привязка объекта в отчет
		/// </summary>			
		ObjectInReport = 1L<< 35,

		/// <summary>
		/// Сам по себе генератор
		/// </summary>
		Generator =1L<<36,
		/// <summary>
		/// Нутро генератора
		/// </summary>
		GeneratorInternal = 1L<<37,
		

		/// <summary>
		/// Исходное определение отчета
		/// </summary>
		ReportDef = 1L<< 40,
		/// <summary>
		/// Определение нутра отчета
		/// </summary>
		ReportSet = 1L<< 41,
		/// <summary>
		/// Расширение нутра отчета
		/// </summary>
		ReportSetEx = 1L<< 42,
		/// <summary>
		/// Спрятать параметры в отчете
		/// </summary>
		HideParamInReport = 1L<< 43,
		

		/// <summary>
		/// Исходное определение формы
		/// </summary>
		FormDef = 1L<< 44,
		/// <summary>
		/// Пользовательское расширение
		/// </summary>
		FormProcesses = 1L<< 45,

		/// <summary>
		/// Определение нутра формы
		/// </summary>
		FormSet = 1L<< 46,
		/// <summary>
		/// Расширение нутра формы
		/// </summary>
		FormSetEx = 1L<< 47,
		
		/// <summary>
		/// Импорт библиотеки в отчет
		/// </summary>
		UseLibReport = 1L<< 48,


		/// <summary>
		/// Импорт библиотеки в форму
		/// </summary>
		UseLibForm = 1L<<49,
		
		/// <summary>
		/// Импорт колсета в форму
		/// </summary>
		ColsetImportIntoForm = 1L<< 50,
		/// <summary>
		/// Импорт колсета в отчет
		/// </summary>
		ColsetImportIntoReport = 1L<< 51,

		
		/// <summary>
		/// Глобальное значение
		/// </summary>
		Global = 1L<< 52,

		/// <summary>
		/// Строка среди строк
		/// </summary>
		RowInRows = 1L<< 54,

		/// <summary>
		/// Строки
		/// </summary>
		Rows = 1L<< 56,
		/// <summary>
		/// Строка в форме
		/// </summary>
		RowInForm = 1L<< 57,
		/// <summary>
		/// Строка в отчете
		/// </summary>
		RowInReport = 1L<< 58,
		/// <summary>
		/// Строка определяемая в наборе строк (аналог колсета, парамсета)
		/// </summary>
		RowInRowSet = 1L<< 59,
		/// <summary>
		/// Определение форм в расширении "процессы"
		/// </summary>
		ContentExtension = 1L<< 60,
		/// <summary>
		/// Набор строк
		/// </summary>
		Rowset = 1L<< 61,/// <summary>
		/// Правило для колонки
		/// </summary>
		ColCheckRule = 1L<< 62,
		
		
	}
}