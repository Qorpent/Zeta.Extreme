#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IInputTemplate.cs
#endregion
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.XPath;
using Qorpent.Model;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Themas {
	/// <summary>
	/// 	Интрефейс формы ввода
	/// </summary>
	public interface IUserForm : IWithName, IWithCode, IWithRole, IMvcBasedInputTemplate {
		/// <summary>
		/// 	Корневая строка
		/// </summary>
		RowDescriptor Form { get; set; }

		/// <summary>
		/// 	Колсет
		/// </summary>
		IList<ColumnDesc> Values { get; }

		/// <summary>
		/// 	Параметры
		/// </summary>
		IDictionary<string, string> Parameters { get; set; }

		/// <summary>
		/// 	Источники дополнительных параметров (бибилиотеки)
		/// </summary>
		IList<IUserForm> Sources { get; }

		/// <summary>
		/// 	Дополнительные документы
		/// </summary>
		string AdvDocs { get; set; }

		/// <summary>
		/// 	Перенаправление периодов
		/// </summary>
		string PeriodRedirect { get; set; }

		/// <summary>
		/// 	Требование присоединять файлы
		/// </summary>
		string NeedFiles { get; set; }

		/// <summary>
		/// 	Требование к периодам присоединения файлов
		/// </summary>
		string NeedFilesPeriods { get; set; }

		/// <summary>
		/// 	Ссылка на исходную конфигурацию
		/// </summary>
		XPathNavigator SourceXmlConfiguration { get; set; }

		//NOTE: почему XPathNavigator??
		// IDictionary<string, InputQuery> Queries { get; }
		/// <summary>
		/// 	Специальный клиентский скрипт (может оставить)
		/// </summary>
		string Script { get; set; }

		/// <summary>
		/// 	Нестандартная команда сохранения
		/// </summary>
		string CustomSave { get; set; }

		/// <summary>
		/// 	Тип метода сохранения
		/// </summary>
		string SaveMethod { get; set; }

		/// <summary>
		/// 	Код связанного отчета
		/// </summary>
		string BindedReport { get; set; }

		/// <summary>
		/// 	Год шаблона
		/// </summary>
		int Year { get; set; }

		/// <summary>
		/// 	Период шаблона
		/// </summary>
		int Period { get; set; }

		//NOTE : тут нет объета так как для форм с несколькими предпрятиями идет один шаблон

		/// <summary>
		/// 	Прямая дата шаблона
		/// </summary>
		DateTime DirectDate { get; set; }

		/// <summary>
		/// 	Код подписи
		/// </summary>
		string UnderwriteCode { get; set; }

		/// <summary>
		/// 	Строка помощи
		/// </summary>
		string Help { get; set; }

		///<summary>
		///	Определитель автозаполнения
		///</summary>
		string AutoFillDescription { get; set; }

		/// <summary>
		/// 	Признак фильтрации на группу
		/// </summary>
		string ForGroup { get; set; }

		//   IList<InputField> Fields { get; }
		/// <summary>
		/// 	Фильтр по периодам
		/// </summary>
		int[] ForPeriods { get; set; }

		/// <summary>
		/// 	Имя фильтра по деталям (класс)
		/// </summary>
		string DetailFilterName { get; set; }

		/// <summary>
		/// 	Фильтр по деталям
		/// </summary>
		IDetailFilter DetailFilter { get; }

		//NOTE: надо четко разобраться, что из этого вообще используется???

		/// <summary>
		/// 	Разбивать по деталям
		/// </summary>
		bool DetailSplit { get; set; }
		/*
		/// <summary>
		/// 	Признак формы для деталей
		/// </summary>
		bool IsForDetail { get; }
		
		/// <summary>
		/// 	Признак формы для одной детали
		/// </summary>
		bool IsForSingleDetail { get; }
		*/
		/// <summary>
		/// 	Фиксированный список строк
		/// </summary>
		IList<string> FixedRowCodes { get; set; }
		/*
		/// <summary>
		/// 	Признак ввода для деталей
		/// </summary>
		bool IsInputForDetail { get; }
		*/
		/// <summary>
		/// 	Признак открытости формы
		/// </summary>
		bool IsOpen { get; set; }

		/// <summary>
		/// 	Роль на подпись
		/// </summary>
		string UnderwriteRole { get; set; }

		/// <summary>
		/// 	Смещение расписания
		/// </summary>
		int ScheduleDelta { get; set; }

		/// <summary>
		/// 	Класс расписания формы
		/// </summary>
		string ScheduleClass { get; set; }

		/// <summary>
		/// 	Признак использования только избранных строк
		/// </summary>
		bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// 	Опять признак формы для деталей, фигня какая-то
		/// </summary>
		bool InputForDetail { get; set; }

		/// <summary>
		/// 	Обратная ссылка на тему
		/// </summary>
		IThema Thema { get; set; }

		/// <summary>
		/// 	Список строк
		/// </summary>
		IList<RowDescriptor> Rows { get; }

		/// <summary>
		/// 	Признак показа колонки с единицей измерения
		/// </summary>
		bool ShowMeasureColumn { get; set; }

		/// <summary>
		/// 	Признак того, что форма проверена
		/// </summary>
		bool IsChecked { get; set; }

		/// <summary>
		/// 	Статус формы по умолчанию
		/// </summary>
		string DefaultState { get; set; }

		/// <summary>
		/// 	Признак избранности деталей??
		/// </summary>
		bool DetailFavorite { get; set; }

		/// <summary>
		/// 	Описание SQL оптимизации данной формы
		/// </summary>
		string SqlOptimization { get; set; }

		///// <summary>
		///// 	Ссылка на конфигурацию
		///// </summary>
		//InputConfiguration Configuration { get; set; }

		///<summary>
		///	Корень документа
		///</summary>
		string DocumentRoot { get; set; }

		/// <summary>
		/// 	Документы
		/// </summary>
		IDictionary<string, string> Documents { get; set; }

		/// <summary>
		/// 	Фиксированный код объекта
		/// </summary>
		string FixedObjectCode { get; set; }

		/// <summary>
		/// 	Фиксированный объект
		/// </summary>
		IZetaMainObject FixedObject { get; }

		/// <summary>
		/// 	Требуется скрипт предзагрузки
		/// </summary>
		bool NeedPreloadScript { get; set; }

		/// <summary>
		/// 	Используется быстрое обновление
		/// </summary>
		bool UseQuickUpdate { get; set; }

		/// <summary>
		/// 	Игнорируется статус периода
		/// </summary>
		bool IgnorePeriodState { get; set; }

		/// <summary>
		/// 	Зависимость от объекта
		/// </summary>
		bool IsObjectDependent { get; set; }

		/// <summary>
		/// 	Актуальность на год
		/// </summary>
		bool IsActualOnYear { get; }

		/// <summary>
		/// 	Использование бизтрана
		/// </summary>
		bool UseBizTranMatrix { get; set; }

		/// <summary>
		/// 	Форма бизтрана
		/// </summary>
		string Biztran { get; set; }

		/// <summary>
		/// 	Сессия обслуживания формы
		/// </summary>
		IFormSession AttachedSession { get; set; }

	

		/// <summary>
		/// 	Подготовить к периоду
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="directDate"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		IUserForm PrepareForPeriod(int year, int period, DateTime directDate, IZetaMainObject obj);

		/// <summary>
		/// 	Проверить соответствие периоду
		/// </summary>
		/// <returns> </returns>
		bool GetIsPeriodMatched();

		/// <summary>
		/// 	Клонировать
		/// </summary>
		/// <returns> </returns>
		IUserForm Clone();

		//IEnumerable<IZetaCell> GetCellsByTargets(Controller controller);
		// не переносится

		/// <summary>
		/// 	Gets the state (вычисляет текущий статус шаблона ввода на заполнение
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <param name="detail"> </param>
		/// <returns> </returns>
		string GetState(IZetaMainObject obj, IZetaDetailObject detail);

		/// <summary>
		/// 	Установить статус
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state);

		/// <summary>
		/// 	Обновить статус
		/// </summary>
		void RefreshState();

		

		

		/// <summary>
		/// 	ПОлучить полную видимость
		/// </summary>
		/// <returns> </returns>
		bool GetIsVisible();

		/// <summary>
		/// 	Получить статус по расписанию
		/// </summary>
		/// <returns> </returns>
		ScheduleState GetScheduleState();

		/// <summary>
		/// 	Проверить возможность установки статуса
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		string CanSetState(IZetaMainObject obj, IZetaDetailObject detail, string state);

		/// <summary>
		/// 	SQL кэш
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <returns> </returns>
		IDictionary<string, object> ReloadSqlCache(IZetaMainObject obj, int year, int period);

		/// <summary>
		/// 	Получить все колонки
		/// </summary>
		/// <returns> </returns>
		IEnumerable<ColumnDesc> GetAllColumns();

		/// <summary>
		/// 	Проверка открытости периода
		/// </summary>
		/// <returns> </returns>
		bool IsPeriodOpen();

		/// <summary>
		/// 	ПОлучить статус
		/// </summary>
		/// <param name="zetaMainObject"> </param>
		/// <param name="detail"> </param>
		/// <param name="statecache"> </param>
		/// <returns> </returns>
		string GetState(IZetaMainObject zetaMainObject, IZetaDetailObject detail, IDictionary<string, object> statecache);

		/// <summary>
		/// 	Проверить соответствие объекта форме
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		bool IsMatch(IZetaMainObject obj);

		/// <summary>
		/// 	Установить статус
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="skipcheck"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state, bool skipcheck = false, int parent = 0);
		/*
		/// <summary>
		/// 	Получить проверки строк
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj);

		/// <summary>
		/// 	Полуичить проверки строк
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj, ColumnDesc col = null);

		/// <summary>
		/// 	Получить класс проверенной строки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		string GetCheckedRowClass(IZetaRow row, IZetaMainObject obj);

		/// <summary>
		/// 	Получить стиль проверенной строки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		string GetCheckedRowStyle(IZetaRow row, IZetaMainObject obj);

		/// <summary>
		/// 	Получить класс проверенной ячейки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		string GetCheckedCellClass(IZetaRow row, IZetaMainObject obj, ColumnDesc col);

		/// <summary>
		/// 	Получить стиль проверенной ячейки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		string GetCheckedCellStyle(IZetaRow row, IZetaMainObject obj, ColumnDesc col);
		*/
		//IList<IFile> GetAttachedFiles(int objid, AttachedFileType filestype, int year = 0, int period = 0);
		// Будет переписано
		/// <summary>
		/// 	Очистить статусы
		/// </summary>
		void CleanupStates();

		/// <summary>
		/// 	Проверка соответсвия строки форме
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		bool IsValidRow(IZetaRow row);
		/*
		/// <summary>
		/// 	Получить контрольные строки
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		ControlPointResult[] GetControlPoints(IZetaMainObject obj);
		*/
		/// <summary>
		/// 	Получить группу колонок
		/// </summary>
		/// <returns> </returns>
		string GetColGroup();

		/// <summary>
		/// 	Получить рабочие объекты
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		List<IZetaMainObject> GetWorkingObjects(IZetaMainObject obj);

		/// <summary>
		/// 	Определить параметр
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		object ResolveParameter(string name);

		/// <summary>
		/// 	Настроить колсет
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="values"> </param>
		/// <returns> </returns>
		IList<ColumnDesc> AccomodateColumnSet(IZetaMainObject obj, IList<ColumnDesc> values);

		/// <summary>
		/// 	Асинхронный запуск кэша статусов
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		Task StartCanSetAsync(IZetaMainObject obj);
	}
}