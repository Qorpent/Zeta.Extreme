namespace Zeta.Extreme.Developer.Model
{
	/// <summary>
	/// Тип элементов кода темы
	/// </summary>
	public enum CodeElementType
	{
		/// <summary>
		/// Неопределенный
		/// </summary>
		Undefined,
		/// <summary>
		/// Библиотека параметров
		/// </summary>
		ParamLib,
		/// <summary>
		/// Параметр, определенные в библиотеке
		/// </summary>
		ParamDefLib,
		/// <summary>
		/// Указание параметра в корне (обычно в импортируемом файле)
		/// </summary>
		ParamDefRoot,
		/// <summary>
		/// Статический параметр, определенный локально
		/// </summary>
		ReportParamDefLocalVar,
		/// <summary>
		/// Динамический параметр, определенный локально
		/// </summary>
		ReportParamDefLocalParam,
		/// <summary>
		/// Параметр, определенный локально в форме
		/// </summary>
		ParamInForm,
		/// <summary>
		/// Параметр, показываемый локально в отчете
		/// </summary>
		ParamShowReport,
		/// <summary>
		///Параметр определяемый в наборе параметров (аналог колсета)
		/// </summary>
		ParamInParamset,
		//// <summary>
		//// Параметр, определенный локально
		//// </summary>
		//ReportParamDefLocal = ReportParamDefLocalVar | ReportParamDefLocalParam,
		//// <summary>
		//// Определение параметра
		//// </summary>
		//ParamDef = ReportParamDefLocal | ParamDefLib | ParamDefRoot,
		/// <summary>
		/// Набор параметров (аналог колсета)
		/// </summary>
		ParamSet,
		/// <summary>
		/// Ссылка ASK на параметр в paramset
		/// </summary>
		ParamAskInParamset,
		/// <summary>
		/// Запрос на парамтер в определении отчета
		/// </summary>
		ParamAskInReportDef,
		/// <summary>
		/// Запрос параметра в репортсете
		/// </summary>
		ParamAskInReportSet,
		/// <summary>
		/// Запрос параметра в расширении отчета
		/// </summary>
		ParamAskInReportSetEx,
		//// <summary>
		//// Определение парамтера на запрос в отчете
		//// </summary>
		//ParamAskInReport = ParamAskInReportDef | ParamAskInReportSet | ParamAskInReportSetEx,
		/// <summary>
		/// Определение запроса параметра в колсете
		/// </summary>
		ParamAskReferenceInColset,
		//// <summary>
		//// Запрос параметра в целом
		//// </summary>
		//ParamAsk = ParamAskInReport | ParamAskReferenceInColset,

		/// <summary>
		/// Утверждение на параметр в paramset
		/// </summary>
		ParamUseInParamset,
		/// <summary>
		/// Утверждение на парамтер в определении отчета
		/// </summary>
		ParamUseInReportDef,
		/// <summary>
		/// Утверждение параметра в репортсете
		/// </summary>
		ParamUseInReportSet,
		/// <summary>
		/// Утверждение параметра в расширении отчета
		/// </summary>
		ParamUseInReportSetEx,
		//// <summary>
		//// Утверждение параметера  в отчете
		//// </summary>
		//ParamUseInReport = ParamUseInReportDef | ParamUseInReportSet | ParamUseInReportSetEx,
		/// <summary>
		/// Утверждение  параметра в колсете
		/// </summary>
		ParamUseReferenceInColset,
		//// <summary>
		//// Утверждение параметра в целом
		//// </summary>
		//ParamUse = ParamUseInReport | ParamUseReferenceInColset,

		//// <summary>
		//// Ссылка на параметр
		//// </summary>
		//ParamRef = ParamUse | ParamAsk,

		/// <summary>
		/// Определение колсета
		/// </summary>
		Colset,

		/// <summary>
		/// Импорт колсета в другой колсет
		/// </summary>
		ColsetImportIntoColset,
		/// <summary>
		/// Колонка, определенная в колсете
		/// </summary>
		ColInColset,
		/// <summary>
		/// Импорт, наследование тем
		/// </summary>
		Imports,
		/// <summary>
		/// Импорт парамсета
		/// </summary>
		ImportParamset,

		/// <summary>
		/// Колонка, определенная в отчете
		/// </summary>
		ColInReport,
		/// <summary>
		/// Колонка, определенная в форме
		/// </summary>
		ColInForm,

		//// <summary>
		//// Определение колонки
		//// </summary>
		//Col = ColInReport | ColInColset | ColInForm,
		/// <summary>
		/// Правило для колонки
		/// </summary>
		ColCheckRule,
		/// <summary>
		/// Подключение расширения
		/// </summary>
		Extension,
		/// <summary>
		/// Набор объектов
		/// </summary>
		Objset,

		/// <summary>
		/// Тема, объявленная как унаследованная
		/// </summary>
		ThemaInherited,
		/// <summary>
		/// Тема объявленная прямо тегом Thema
		/// </summary>
		ThemaRooted,
		//// <summary>
		//// Определение темы
		//// </summary>
		//Thema = ThemaInherited | ThemaRooted,
		/// <summary>
		/// Определение автозамены языка
		/// </summary>
		SubstDefinition,
		/// <summary>
		/// Пользовательские расширения контента
		/// </summary>
		ContentExtensions,
		/// <summary>
		/// Генератор отчета
		/// </summary>
		ReportGeneration,
		/// <summary>
		/// Нутро генератора, генератор чего-либо в отчет
		/// </summary>
		GenerationEither,

		/// <summary>
		/// Объект в группе объектов
		/// </summary>		
		ObjectInObjset,

		/// <summary>
		/// Привязка объекта в отчет
		/// </summary>			
		ObjectInReport,

		/// <summary>
		/// Генератор объектов в отчет
		/// </summary>
		ObjsetGenerationIn,
		/// <summary>
		/// Генератор кондишинов
		/// </summary>
		ObjsetGenerationCond,
		/// <summary>
		/// Генератор фильтров
		/// </summary>
		ObjsetGenerationFilter,
		/// <summary>
		/// Генератор объектов 
		/// </summary>
		ObjsetGeneration,

		/// <summary>
		/// Исходное определение отчета
		/// </summary>
		ReportDef,
		/// <summary>
		/// Определение нутра отчета
		/// </summary>
		ReportSet,
		/// <summary>
		/// Расширение нутра отчета
		/// </summary>
		ReportSetEx,
		//// <summary>
		//// Определение отчета в целом
		//// </summary>
		//Report = ReportDef | ReportSet | ReportSetEx,

		/// <summary>
		/// Исходное определение формы
		/// </summary>
		FormDef,
		/// <summary>
		/// Пользовательское расширение
		/// </summary>
		FormProcesses,

		/// <summary>
		/// Определение нутра формы
		/// </summary>
		FormSet,
		/// <summary>
		/// Расширение нутра формы
		/// </summary>
		FormSetEx,
		//// <summary>
		//// Определение формы в целом
		//// </summary>
		//Form = FormDef | FormSet | FormSetEx,

		/// <summary>
		/// Импорт библиотеки в отчет
		/// </summary>
		UseLibReport,


		/// <summary>
		/// Импорт библиотеки в форму
		/// </summary>
		UseLibForm,
		//// <summary>
		//// Импорт библиотеки в целом
		//// </summary>
		//UseLib = UseLibReport | UseLibForm,
		/// <summary>
		/// Импорт колсета в форму
		/// </summary>
		ColsetImportIntoForm,
		/// <summary>
		/// Импорт колсета в отчет
		/// </summary>
		ColsetImportIntoReport,

		//// <summary>
		//// Импорт колсета в тему
		//// </summary>
		//ColsetImportIntoThema = ColsetImportIntoForm | ColsetImportIntoReport,
		//// <summary>
		//// Импорт колсета в целом
		//// </summary>
		//ColsetImport = ColsetImportIntoColset | ColsetImportIntoForm | ColsetImportIntoReport,

		/// <summary>
		/// Глобальное значение
		/// </summary>
		Global,
		/// <summary>
		/// Генератор глобалов
		/// </summary>
		GlobalGeneration,
		/// <summary>
		/// Строка среди строк
		/// </summary>
		RowInRows,
		/// <summary>
		/// Генератор строк
		/// </summary>
		RowsGeneration,
		/// <summary>
		/// Строки
		/// </summary>
		Rows,
		/// <summary>
		/// Строка в форме
		/// </summary>
		RowInForm,
		/// <summary>
		/// Строка в отчете
		/// </summary>
		RowInReport,
		/// <summary>
		/// Строка определяемая в наборе строк (аналог колсета, парамсета)
		/// </summary>
		RowInRowSet,
		/// <summary>
		/// Определение форм в расширении "процессы"
		/// </summary>
		ContentExtensionsForm,
	}
}