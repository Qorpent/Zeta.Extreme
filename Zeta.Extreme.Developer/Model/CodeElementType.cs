using System;

namespace Zeta.Extreme.Developer.Model
{
	/// <summary>
	/// Класс для управления сведенными аналитическими единицами кода
	/// </summary>
	public static class AnalyticElementType
	{
		

	}

	/// <summary>
	/// Укрупненная категория элемента кода
	/// </summary>
	[Flags]
	public enum CodeElementCategory {
		/// <summary>
		/// Неопределенный	
		/// </summary>
		Undefined =0,
		/// <summary>
		/// Определение парамтера в целом
		/// </summary>
		ParamDef = CodeElementType.ParamDefLib | 
				CodeElementType.ParamDefRoot |
				CodeElementType.ReportParamDefLocalVar|
				CodeElementType.ReportParamDefLocalParam
				,
		ParamUsage = 

	}


	/// <summary>
	/// Тип элементов кода темы
	/// </summary>
	[Flags]
	public enum CodeElementType
	{
		/// <summary>
		/// Неопределенный
		/// </summary>
		Undefined = 0,
		/// <summary>
		/// Библиотека параметров
		/// </summary>
		ParamLib = 1,
		/// <summary>
		/// Параметр, определенные в библиотеке
		/// </summary>
		ParamDefLib = 2^1,
		/// <summary>
		/// Указание параметра в корне (обычно в импортируемом файле)
		/// </summary>
		ParamDefRoot = 2^2,
		/// <summary>
		/// Статический параметр, определенный локально
		/// </summary>
		ReportParamDefLocalVar = 2^3,
		/// <summary>
		/// Динамический параметр, определенный локально
		/// </summary>
		ReportParamDefLocalParam = 2 ^ 4,
		/// <summary>
		/// Параметр, определенный локально в форме
		/// </summary>
		ParamInForm = 2 ^ 5,
		/// <summary>
		/// Параметр, показываемый локально в отчете
		/// </summary>
		ParamShowReport = 2 ^ 6,
		/// <summary>
		///Параметр определяемый в наборе параметров (аналог колсета)
		/// </summary>
		ParamInParamset = 2 ^ 7,
		/// <summary>
		/// Набор параметров (аналог колсета)
		/// </summary>
		ParamSet = 2 ^ 8,
		/// <summary>
		/// Ссылка ASK на параметр в paramset
		/// </summary>
		ParamAskInParamset = 2 ^ 9,
		/// <summary>
		/// Запрос на парамтер в определении отчета
		/// </summary>
		ParamAskInReportDef = 2 ^ 10,
		/// <summary>
		/// Запрос параметра в репортсете
		/// </summary>
		ParamAskInReportSet = 2 ^ 11,
		/// <summary>
		/// Запрос параметра в расширении отчета
		/// </summary>
		ParamAskInReportSetEx = 2 ^ 12,
		
		/// <summary>
		/// Определение запроса параметра в колсете
		/// </summary>
		ParamAskReferenceInColset = 2 ^ 13,
	

		/// <summary>
		/// Утверждение на параметр в paramset
		/// </summary>
		ParamUseInParamset = 2 ^ 14,
		/// <summary>
		/// Утверждение на парамтер в определении отчета
		/// </summary>
		ParamUseInReportDef = 2 ^ 15,
		/// <summary>
		/// Утверждение параметра в репортсете
		/// </summary>
		ParamUseInReportSet = 2 ^ 16,
		/// <summary>
		/// Утверждение параметра в расширении отчета
		/// </summary>
		ParamUseInReportSetEx = 2^ 17,
	
		/// <summary>
		/// Утверждение  параметра в колсете
		/// </summary>
		ParamUseReferenceInColset = 2 ^ 18,


		/// <summary>
		/// Определение колсета
		/// </summary>
		Colset = 2 ^ 19,

		/// <summary>
		/// Импорт колсета в другой колсет
		/// </summary>
		ColsetImportIntoColset = 2 ^ 20,
		/// <summary>
		/// Колонка, определенная в колсете
		/// </summary>
		ColInColset = 2 ^ 21,
		/// <summary>
		/// Импорт, наследование тем
		/// </summary>
		Imports = 2 ^ 22,
		/// <summary>
		/// Импорт парамсета
		/// </summary>
		ImportParamset = 2 ^ 23,

		/// <summary>
		/// Колонка, определенная в отчете
		/// </summary>
		ColInReport = 2 ^ 24,
		/// <summary>
		/// Колонка, определенная в форме
		/// </summary>
		ColInForm = 2 ^ 25,

		
		/// <summary>
		/// Подключение расширения
		/// </summary>
		Extension = 2 ^ 26,
		/// <summary>
		/// Набор объектов
		/// </summary>
		Objset = 2 ^ 27,

		/// <summary>
		/// Тема, объявленная как унаследованная
		/// </summary>
		ThemaInherited = 2 ^ 28,
		/// <summary>
		/// Тема объявленная прямо тегом Thema
		/// </summary>
		ThemaRooted = 2 ^ 29,
		
		/// <summary>
		/// Определение автозамены языка
		/// </summary>
		SubstDefinition = 2 ^ 30,
		/// <summary>
		/// Пользовательские расширения контента
		/// </summary>
		ContentExtensions = 2 ^ 31,
		/// <summary>
		/// Генератор отчета
		/// </summary>
		ReportGeneration = 2 ^ 32,
		/// <summary>
		/// Нутро генератора, генератор чего-либо в отчет
		/// </summary>
		GenerationEither = 2 ^ 33,

		/// <summary>
		/// Объект в группе объектов
		/// </summary>		
		ObjectInObjset = 2 ^ 34,

		/// <summary>
		/// Привязка объекта в отчет
		/// </summary>			
		ObjectInReport = 2 ^ 35,

		/// <summary>
		/// Генератор объектов в отчет
		/// </summary>
		ObjsetGenerationIn = 2 ^ 36,
		/// <summary>
		/// Генератор кондишинов
		/// </summary>
		ObjsetGenerationCond = 2 ^ 37,
		/// <summary>
		/// Генератор фильтров
		/// </summary>
		ObjsetGenerationFilter = 2 ^ 38,
		/// <summary>
		/// Генератор объектов 
		/// </summary>
		ObjsetGeneration = 2 ^ 39,

		/// <summary>
		/// Исходное определение отчета
		/// </summary>
		ReportDef = 2 ^ 40,
		/// <summary>
		/// Определение нутра отчета
		/// </summary>
		ReportSet = 2 ^ 41,
		/// <summary>
		/// Расширение нутра отчета
		/// </summary>
		ReportSetEx = 2 ^ 42,
		/// <summary>
		/// Спрятать параметры в отчете
		/// </summary>
		HideParamInReport = 2 ^ 43,
		

		/// <summary>
		/// Исходное определение формы
		/// </summary>
		FormDef = 2 ^ 44,
		/// <summary>
		/// Пользовательское расширение
		/// </summary>
		FormProcesses = 2 ^ 45,

		/// <summary>
		/// Определение нутра формы
		/// </summary>
		FormSet = 2 ^ 46,
		/// <summary>
		/// Расширение нутра формы
		/// </summary>
		FormSetEx = 2 ^ 47,
		/// <summary>
		/// Определение формы в целом
		/// </summary>
		Form = FormDef | FormSet | FormSetEx,

		/// <summary>
		/// Импорт библиотеки в отчет
		/// </summary>
		UseLibReport = 2 ^ 48,


		/// <summary>
		/// Импорт библиотеки в форму
		/// </summary>
		UseLibForm = 2 ^49,
		
		/// <summary>
		/// Импорт колсета в форму
		/// </summary>
		ColsetImportIntoForm = 2 ^ 50,
		/// <summary>
		/// Импорт колсета в отчет
		/// </summary>
		ColsetImportIntoReport = 2 ^ 51,

		
		/// <summary>
		/// Глобальное значение
		/// </summary>
		Global = 2 ^ 52,
		/// <summary>
		/// Генератор глобалов
		/// </summary>
		GlobalGeneration = 2 ^53,
		/// <summary>
		/// Строка среди строк
		/// </summary>
		RowInRows = 2 ^ 54,
		/// <summary>
		/// Генератор строк
		/// </summary>
		RowsGeneration = 2 ^55,
		/// <summary>
		/// Строки
		/// </summary>
		Rows = 2 ^ 56,
		/// <summary>
		/// Строка в форме
		/// </summary>
		RowInForm = 2 ^ 57,
		/// <summary>
		/// Строка в отчете
		/// </summary>
		RowInReport = 2 ^ 58,
		/// <summary>
		/// Строка определяемая в наборе строк (аналог колсета, парамсета)
		/// </summary>
		RowInRowSet = 2 ^ 59,
		/// <summary>
		/// Определение форм в расширении "процессы"
		/// </summary>
		ContentExtensionsForm = 2 ^ 60,
		/// <summary>
		/// Набор строк
		/// </summary>
		Rowset = 2 ^ 61,/// <summary>
		/// Правило для колонки
		/// </summary>
		ColCheckRule = 2 ^ 62,
		
		
	}
}