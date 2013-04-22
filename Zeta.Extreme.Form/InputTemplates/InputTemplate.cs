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
// PROJECT ORIGIN: Zeta.Extreme.Form/InputTemplate.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Qorpent.Applications;
using Qorpent.Dsl.LogicalExpressions;
using Qorpent.IoC;
using Qorpent.Log;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.StateManagement;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using ColumnDesc = Zeta.Extreme.BizProcess.Themas.ColumnDesc;
using IConditionMatcher = Zeta.Extreme.BizProcess.Themas.IConditionMatcher;
using Qorpent.Utils.Extensions;


namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	Шаблон формы
	/// </summary>
	public class InputTemplate : IInputTemplate, ICloneable, IConditionMatcher {
		/// <summary>
		/// 	Конструктор по умолчанию
		/// </summary>
		public InputTemplate() {
			Values = new List<ColumnDesc>();
			FixedRowCodes = new List<string>();
		}

		/// <summary>
		/// 	Применять курс значения
		/// </summary>
		public bool ApplyValueCourse { get; set; }

		/// <summary>
		/// 	Ссылка на контейнер инверсии
		/// </summary>
		public IContainer Container {
			get { return _container ?? (_container = Application.Current.Container); }
			set { _container = value; }
		}

		private IStateManager StateManager {
			get { return _stateManager ?? (_stateManager = Container.Get<IStateManager>() ?? StateManagement.StateManager.Default); }
			set { _stateManager = value; }
		}

		/// <summary>
		/// 	Кэш SQL - убрать!
		/// </summary>
		public IDictionary<string, decimal> SqlCache { get; set; }

		/// <summary>
		/// 	Ареа для контроллера - убрать!!
		/// </summary>
		public string Area { get; set; }

		/// <summary>
		/// 	Проверяльщик расписаний
		/// </summary>
		public IScheduleChecker ScheduleChecker {
			get {
				return _scheduleChecker ?? (_scheduleChecker = Container.Get<IScheduleChecker>(ScheduleClass));
			}
		}

		/// <summary>
		/// 	Сменить период по карте редиректов
		/// </summary>
		public IDictionary<string, int> RedirectPeriodMap {
			get {
				if (null == _redirectPeriodMap) {
					if (PeriodRedirect.IsNotEmpty()) {
						_redirectPeriodMap = new Dictionary<string, int>();
						var variants = PeriodRedirect.SmartSplit(false, true, '|');
						foreach (var variant in variants) {
							var test = variant.SmartSplit(false, true, ':');
							var v = "";
							var rules_ = "";
							if (test.Count == 1) {
								v = "";
								rules_ = test[0];
							}
							else {
								v = test[0];
								rules_ = test[1];
							}
							var rules = rules_.SmartSplit();
							foreach (var rule in rules) {
								var record_ = rule.SmartSplit(false, true, '=');
								var record = new {from = record_[0].ToInt(), to = record_[1].ToInt()};
								_redirectPeriodMap[record.from + "_" + v] = record.to;
							}
						}
					}
				}
				return _redirectPeriodMap;
			}
		}

		/// <summary>
		/// 	Список перехватчиков проверки статусов
		/// </summary>
		public IList<IStateCheckInterceptor> StateInterceptors {
			get { return _stateInterceptors ?? (_stateInterceptors = Container.All<IStateCheckInterceptor>().ToList()); }
			set { _stateInterceptors = value; }
		}

		object ICloneable.Clone() {
			return Clone();
		}

		/// <summary>
		/// 	Проверка соответствия строки условиям
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public bool IsConditionMatch(string condition) {
			if (IsEmptyCondition(condition)) return true;
			if (IsNotConditionCheckActivated()) return true;
			var conds = GetConditionList();
			if (conds == null) return false;
			if (IsConditionListLike(condition)) {
				return EvaluateByListLikeCondition(condition, conds);
			}
			return EvaluateByScript(condition, conds);
		}

		private readonly ILogicalExpressionEvaluator _evaluator =
			Application.Current.Container.Get<ILogicalExpressionEvaluator>();
		private  bool EvaluateByScript(string condition, IEnumerable<string> conds) {
			var normalizedCondition = (" " + condition + " ").Replace("(", " ( ").Replace(")", " ) ")
				//fix not processable formulas
				.Replace(" and ", " & ").Replace(" or ", " | ").Replace(" not ", " ! ");
			//fix operators

			var soruce = LogicTermSource.Create(conds);
			return _evaluator.Eval(normalizedCondition, soruce);
		}

		private static bool EvaluateByListLikeCondition(string condition, IEnumerable<string> conds) {
			var condsets = condition.SmartSplit(false, true, '|');
			return condsets.Select(condset => conds.ContainsAll(condset.SmartSplit().ToArray())).Any(match => match);
		}

		private static bool IsConditionListLike(string condition) {
			return condition.Contains(",") || condition.Contains("|");
		}

		private IEnumerable<string> GetConditionList() {
			var conds_ = Parameters.SafeGet("conditions");
			if (conds_.IsEmpty()) {
				return null;
			}
			var conds = conds_.SmartSplit();
			return conds;
		}

		private static bool IsEmptyCondition(string condition) {
			if (condition.IsEmpty()) {
				return true;
			}
			return false;
		}

		private bool IsNotConditionCheckActivated() {
			if (!Parameters.SafeGet("activecondition").ToBool()) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// 	Сессия обслуживания формы
		/// </summary>
		public IFormSession AttachedSession { get; set; }

		/// <summary>
		/// 	Использование быстрого AJAX-обновления (устар)
		/// </summary>
		public bool UseQuickUpdate { get; set; }

		/// <summary>
		/// 	Команда на очистку статусов
		/// </summary>
		public void CleanupStates() {
			cachedState = null;
			stateCache.Clear();
		}

		/// <summary>
		/// 	Метод резолюции параметра
		/// </summary>
		/// <param name="name"> </param>
		/// <returns> </returns>
		public object ResolveParameter(string name) {
			if (Parameters.ContainsKey(name)) {
				return Parameters[name];
			}
			if (null != Thema) {
				return ((Thema) Thema).GetParameter(name);
			}
			return null;
		}

		/// <summary>
		/// 	Производит аккомодацию колсета
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="values"> </param>
		/// <returns> </returns>
		public IList<ColumnDesc> AccomodateColumnSet(IZetaMainObject obj, IList<ColumnDesc> values) {
			if (null == obj) {
				return values;
			}
			var balmet = obj.ResolveTag("BALMET");
			if (balmet.IsEmpty()) {
				return values;
			}
			var balmets = balmet.SmartSplit().Select(x => x.ToUpper()).ToArray();
			var result = new List<ColumnDesc>();
			foreach (var columnDesc in values) {
				if (columnDesc.RedirectBasis.IsEmpty()) {
					result.Add(columnDesc);
				}
				else {
					var basis = columnDesc.RedirectBasis.ToUpper();
					if (balmets.Contains(basis)) {
						result.Add(columnDesc);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 	Метод получения реального рабочего объекта
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public List<IZetaMainObject> GetWorkingObjects(IZetaMainObject obj) {
			var objects = new List<IZetaMainObject> {obj};
			if (null != FixedObject) {
				objects.Clear();
				objects.Add(FixedObject);
				return objects;
			}
#pragma warning disable 612,618
			var th = Thema as EcoThema;
#pragma warning restore 612,618
			if (null != th) {
				var splitobj = th.GetParameter("splittoobj", "");
				if (splitobj.IsNotEmpty()) {
					objects.Clear();
					var subobjtypes = splitobj.SmartSplit();
					foreach (var subobjtype in subobjtypes) {
						if (subobjtype == "SELF") {
							objects.Add(obj);
						}
						else {
							foreach (var o in obj.AllChildren()) {
#pragma warning disable 612,618
								if (o.ObjType.Code == subobjtype || o.ObjType.Class.Code == subobjtype) {
#pragma warning restore 612,618
									objects.Add(o);
								}
							}
						}
					}
				}
			}
			return objects;
		}


		/// <summary>
		/// 	Получить группу колонок
		/// </summary>
		/// <returns> </returns>
		public string GetColGroup() {
			if (null != Thema) {
				var firstyear = Thema.GetParameter("firstyear").ToInt();
				if (0 == firstyear) {
					return "";
				}
				if (Year >= firstyear) {
					return "";
				}
				return "HISTORY";
			}
			return "";
		}


		/// <summary>
		/// 	Проверка валидности строки
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		public bool IsValidRow(IZetaRow row) {
			if (null == _excludes) {
				_excludes = new string[] {};
				if (Parameters.ContainsKey("excluderows")) {
					_excludes = Parameters.SafeGet("excluderows").SmartSplit().ToArray();
				}
			}
			if (0 == _excludes.Length) {
				return true;
			}
			if (-1 == Array.IndexOf(_excludes, row.Code)) {
				return true;
			}
			return false;
		}


		//NOTE: более не поддерживается и не портиется - старые формы

		/// <summary>
		/// 	Ссылка на форму бизтрана
		/// </summary>
		public string Biztran {
			get {
				var r = Parameters.SafeGet("biztran");
				if (r.IsNotEmpty()) {
					return r;
				}
				if (biztran.IsNotEmpty()) {
					return biztran;
				}
				if (null == Thema) {
					return "";
				}
				r = Thema.Parameters.SafeGet("biztran","");
				return r;
			}
			set { biztran = value; }
		}

		//NOTE: на данный момент инфраструктура атачей не перенесена
		/*
		public IList<IFile> GetAttachedFiles(int objid, AttachedFileType filestype, int year = 0, int period = 0)
		{
			if (0 == year) year = Year;
			if (0 == period) period = Period;

			if(AttachedFileType.All == filestype) {
				return
					GetAttachedFiles(objid, AttachedFileType.Default, year, period)
						.Union(GetAttachedFiles(objid, AttachedFileType.Advanced, year, period))
						.Union(GetAttachedFiles(objid, AttachedFileType.Correlated, year, period))
						.Distinct().ToList();

			}

			if (AttachedFileType.Related == filestype)
			{
				return
					GetAttachedFiles(objid, AttachedFileType.Default, year, period)
						.Union(GetAttachedFiles(objid, AttachedFileType.Correlated, year, period))
						.Distinct().ToList();

			}

			if (AttachedFileType.Default == filestype) {

				return Application.Current.Container.Get<IDbfsRepository>().SearchBySpecialProc("usm.get_form_attachments",
				                                                            new Dictionary<string, object>
				                                                            	{
				                                                            		{"form", this.Code},
				                                                            		{"year", year.ToString()},
				                                                            		{"period", period.ToString()},
				                                                            		{"obj", objid.ToString()},
				                                                            	});
			}
			if(AttachedFileType.Advanced == filestype) {
				if (AdvDocs.hasContent())
				{
					var periods = AdvDocs.split(false, true, ';');
					var filelist = new List<IFile>();
					foreach (var p in periods)
					{
						var pmask = p.split(false, true, '=');
						if (pmask[0].toInt() == Period)
						{
							var toperiods = pmask[1].split();

							foreach (var tp in toperiods)
							{
								
								var files = GetAttachedFiles(objid,AttachedFileType.Default,year,tp.toInt());

								foreach (var f in files)
								{
									if (null == f) continue;

									if (
										(Period.isIn(1, 13) &&
										 TagHelper.Value(f.Tag, "period").isIn("13", "1")) ||
										(Period.isIn(2, 16) &&
										 TagHelper.Value(f.Tag, "period").isIn("16", "2")) ||
										(Period.isIn(3, 19) &&
										 TagHelper.Value(f.Tag, "period").isIn("19", "3")) ||
										(Period.isIn(4, 112) &&
										 TagHelper.Value(f.Tag, "period").isIn("112", "4"))

										)
									{
										continue;
										;
									}
									else
									{
										filelist.Add(f);
									}

								}
							}
							break;
						}
					}
					return filelist;
				}
			}
			if (AttachedFileType.Correlated == filestype)
			{
				if (AdvDocs.hasContent())
				{
					var periods = AdvDocs.split(false, true, ';');
					var cfilelist = new List<IFile>();
					foreach (var p in periods)
					{
						var pmask = p.split(false, true, '=');
						if (pmask[0].toInt() == Period)
						{
							var toperiods = pmask[1].split();

							foreach (var tp in toperiods)
							{

								var files = GetAttachedFiles(objid, AttachedFileType.Default, year, tp.toInt());

								foreach (var f in files)
								{
									if (null == f) continue;

									if (
										(Period.isIn(1, 13) &&
										 TagHelper.Value(f.Tag, "period").isIn("13", "1")) ||
										(Period.isIn(2, 16) &&
										 TagHelper.Value(f.Tag, "period").isIn("16", "2")) ||
										(Period.isIn(3, 19) &&
										 TagHelper.Value(f.Tag, "period").isIn("19", "3")) ||
										(Period.isIn(4, 112) &&
										 TagHelper.Value(f.Tag, "period").isIn("112", "4"))

										)
									{
										cfilelist.Add(f);
									}
									else
									{
										continue;
									}

								}
							}
							break;
						}
					}
					return cfilelist;
				}
			}

			throw new Exception("illegal file type "+filestype);
		}

		*/
		//TODO: надо это на клиента вытаскивать
		/*
		/// <summary>
		/// 	Получить проверки строк
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj) {
			return GetRowChecks(row, obj, null);
		}
		
		///<summary>
		///	Получить проверки строк
		///</summary>
		///<param name="row"> </param>
		///<param name="obj"> </param>
		///<param name="col"> </param>
		///<returns> </returns>
		public ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj, ColumnDesc col = null) {
			var result = new List<ColumnRowCheckCondition>();
			IEnumerable<ColumnDesc> checkers = null;
			if (col != null) {
				checkers = new[] {col};
			}
			else {
				checkers = GetAllColumns().Where(x => x.GetIsVisible(obj));
			}
			checkers = checkers.Where(x => x.RowCheckConditions.Length != 0);
			foreach (var checker in checkers) {
				var rules = checker.GetMatched(row, Decimal.MinValue);
				if (rules.Length != 0) {
					decimal val = 0;
					try {
						val = new Comdiv.Zeta.Data.Minimal.Query(new Zone(obj), row, checker, Thema).evaln();
					}
					catch (Exception ex) {
						myapp.errors.RegisterError(
							new Exception(
								string.Format("on {0} {1} {2} {3}", row.Code, col.Code, col.Year, col.Period), ex));
						continue;
					}
					var activerules = checker.GetMatched(row, val);
					foreach (var rule in activerules) {
						result.Add(rule);
					}
				}
			}
			return result.ToArray();
		}
		

		/// <summary>
		/// 	Получить класс проверки строк
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public string GetCheckedRowClass(IZetaRow row, IZetaMainObject obj) {
			return GetRowChecks(row, obj).Select(x => x.RowClass).Where(x => x.hasContent()).Select(x => "_cr_" + x).concat(" ");
		}

		/// <summary>
		/// 	Получить стиль проверяемой строки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public string GetCheckedRowStyle(IZetaRow row, IZetaMainObject obj) {
			return GetRowChecks(row, obj).Select(x => x.RowStyle).Where(x => x.hasContent()).concat(" ");
		}
	

		/// <summary>
		/// 	Получить класс проверенной ячейки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		public string GetCheckedCellClass(IZetaRow row, IZetaMainObject obj, ColumnDesc col) {
			return
				GetRowChecks(row, obj, col).Select(x => x.CellClass).Where(x => x.hasContent()).Select(x => "_cc_" + x).concat(" ");
		}

		/// <summary>
		/// 	Получить стиль проверенной ячейки
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <param name="col"> </param>
		/// <returns> </returns>
		public string GetCheckedCellStyle(IZetaRow row, IZetaMainObject obj, ColumnDesc col) {
			return GetRowChecks(row, obj, col).Select(x => x.CellStyle).Where(x => x.hasContent()).concat(" ");
		}
			 *  */

		/// <summary>
		/// 	Код фиксированного объекта
		/// </summary>
		public string FixedObjectCode { get; set; }

		/// <summary>
		/// 	Признак игнора статуса периода
		/// </summary>
		public bool IgnorePeriodState {
			get {
				
				if (null != Thema) {
					var y = Thema.GetParameter("firstyear").ToInt();
					if (y > 0 && y > Year) {
						return true;
					}
				}
				return _ignorePeriodState;
			}
			set { _ignorePeriodState = value; }
		}

		/// <summary>
		/// 	Проверка зависимости объекта
		/// </summary>
		public bool IsObjectDependent { get; set; }

		/// <summary>
		/// 	Формы - источники - библиотеки
		/// </summary>
		public IList<IInputTemplate> Sources {
			get { return sources; }
		}

		/// <summary>
		/// 	Корень документации
		/// </summary>
		public string DocumentRoot { get; set; }

		/// <summary>
		/// 	Документы
		/// </summary>
		public IDictionary<string, string> Documents {
			get { return _documents; }
			set { _documents = value; }
		}

		/// <summary>
		/// 	Обратная ссылка на тему
		/// </summary>
		public IThema Thema { get; set; }

		/// <summary>
		/// 	Статус формы по умолчанию
		/// </summary>
		public string DefaultState { get; set; }

		/// <summary>
		/// 	Признак использования только избранных строк
		/// </summary>
		public bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// 	Дополнительные документы
		/// </summary>
		public string AdvDocs { get; set; }

		/// <summary>
		/// 	Класс расписания формы
		/// </summary>
		public string ScheduleClass { get; set; }

		/// <summary>
		/// 	Требование присоединять файлы
		/// </summary>
		public string NeedFiles { get; set; }

		/// <summary>
		/// 	Требование к периодам присоединения файлов
		/// </summary>
		public string NeedFilesPeriods { get; set; }

		/// <summary>
		/// 	Роль доступа
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// 	Признак показа колонки с единицей измерения
		/// </summary>
		public bool ShowMeasureColumn { get; set; }

		/// <summary>
		/// 	Признак того, что форма проверена
		/// </summary>
		public bool IsChecked { get; set; }

		/// <summary>
		/// 	Признак избранности деталей??
		/// </summary>
		public bool DetailFavorite { get; set; }

		/// <summary>
		/// 	Описание SQL оптимизации данной формы
		/// </summary>
		public string SqlOptimization { get; set; }

		/// <summary>
		/// 	Ссылка на конфигурацию
		/// </summary>
		public InputConfiguration Configuration { get; set; }

		/// <summary>
		/// 	Признак открытости формы
		/// </summary>
		public bool IsOpen { get; set; }

		/// <summary>
		/// 	Роль на подпись
		/// </summary>
		public string UnderwriteRole { get; set; }

		/// <summary>
		/// 	Опять признак формы для деталей, фигня какая-то
		/// </summary>
		public bool InputForDetail { get; set; }

		/// <summary>
		/// 	Проверка соответствия периода
		/// </summary>
		/// <returns> </returns>
		public bool GetIsPeriodMatched() {
			if (ForPeriods == null || ForPeriods.Length == 0) {
				return true;
			}
			return ForPeriods.Contains(Period);
		}
		/*
		/// <summary>
		/// 	Признак формы для одной детали
		/// </summary>
		public bool IsForSingleDetail {
			get { return IsForDetail && !DetailSplit; }
		}
		*/
		/// <summary>
		/// 	Имя фильтра по деталям (класс)
		/// </summary>
		public string DetailFilterName { get; set; }

		/// <summary>
		/// 	Фильтр по деталям
		/// </summary>
		public IDetailFilter DetailFilter {
			get {
				if (null == detailFilter && DetailFilterName.IsNotEmpty()) {
					detailFilter = Container.Get<IDetailFilter>(DetailFilterName);
					detailFilter.Configure(this);
				}
				return detailFilter;
			}
		}

		/// <summary>
		/// 	Фиксированный список строк
		/// </summary>
		public IList<string> FixedRowCodes { get; set; }

		/// <summary>
		/// 	Надо убирать - либо переделывать на что-то JSON - совместимости
		/// </summary>
		public string TableView { get; set; }

		/// <summary>
		/// 	Признак фильтрации на группу
		/// </summary>
		public string ForGroup { get; set; }

		/// <summary>
		/// 	Фильтр по периодам
		/// </summary>
		public int[] ForPeriods { get; set; }

		/// <summary>
		/// 	Параметры
		/// </summary>
		public IDictionary<string, string> Parameters {
			get { return parameters; }
			set { parameters = value; }
		}

		/// <summary>
		/// 	Строка помощи
		/// </summary>
		public string Help { get; set; }

		///<summary>
		///	Определитель автозаполнения
		///</summary>
		public string AutoFillDescription { get; set; }


		//	public IDictionary<string, InputQuery> Queries {
		//        get { return queries; }
		//    }

		//private string state_ = null;
		/// <summary>
		/// 	Gets the state (вычисляет текущий статус шаблона ввода на заполнение
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <param name="detail"> </param>
		/// <returns> </returns>
		public string GetState(IZetaMainObject obj, IZetaDetailObject detail) {
			return GetState(obj, detail, null);
		}

		/// <summary>
		/// 	Gets the state (вычисляет текущий статус шаблона ввода на заполнение
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <param name="detail"> </param>
		/// <param name="statecache"> </param>
		/// <returns> </returns>
		public string GetState(IZetaMainObject obj, IZetaDetailObject detail, IDictionary<string, object> statecache) {
			if (!IsActualOnYear) {
				return "0ISBLOCK";
			}
			if (null == cachedState) {
				cachedState = internalGetState(obj, detail, statecache);
			}
			return cachedState;
		}
		

		/// <summary>
		/// 	Полный метод проверки видимости
		/// </summary>
		/// <returns> </returns>
		public bool GetIsVisible() {
			if (null == ForPeriods) {
				return true;
			}
			if (0 == ForPeriods.Length) {
				return true;
			}
			if (-1 == Array.IndexOf(ForPeriods, Period)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 	Асинхронная проверка статуса
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public Task StartCanSetAsync(IZetaMainObject obj) {
			if (CanSetTask != null) {
				return CanSetTask;
			}
			return CanSetTask = Task.Run(() =>
				{
					DirectCanSetState(obj, null, "0ISOPEN");
					DirectCanSetState(obj, null, "0ISBLOCK");
					DirectCanSetState(obj, null, "0ISCHECKED");
				});
		}

		/// <summary>
		/// 	Проверка возможности установить статус
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		public string CanSetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
			if (CanSetTask != null) {
				CanSetTask.Wait();
			}
			return DirectCanSetState(obj, detail, state);
		}

		///<summary>
		///	перезагрузка кэша SQL - какая-то фигня
		///</summary>
		///<param name="obj"> </param>
		///<param name="year"> </param>
		///<param name="period"> </param>
		///<returns> </returns>
		public IDictionary<string, object> ReloadSqlCache(IZetaMainObject obj, int year, int period) {
			SqlCache = new Dictionary<string, decimal>();
			var result =
				Application.Current.DatabaseConnections.GetConnection("Default").ExecuteDictionaryReader(
					"exec " + SqlOptimization + " @obj = @obj, @year = @year, @period = @period",
					new Dictionary<string, object>
						{
							{
								"obj", obj.Id
							},
							{
								"year", year
							},
							{
								"period", period
							}
						})
				;
			return result;
		}

		/// <summary>
		/// 	Получить полный колсет
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<ColumnDesc> GetAllColumns() {
			if (_cachedcolumns == null) {
				_cachedcolumns = getAllColumns().ToList();
			}
			return _cachedcolumns;
		}

		/// <summary>
		/// 	Проверка открытости периода
		/// </summary>
		/// <returns> </returns>
		public bool IsPeriodOpen() {
			if (Application.Current.Roles.IsInRole(Application.Current.Principal.CurrentUser, "IGNOREPERIODSTATE"))
			{
				return true;
			}
			if (IgnorePeriodState) {
				return true;
			}
			return StateManager.GetPeriodState(Year, Period) == 1;
		}

		/// <summary>
		/// 	Установить статус
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		public int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
			return SetState(obj, detail, state, false, 0);
		}

		/// <summary>
		/// 	Проверка актуальности для года
		/// </summary>
		public bool IsActualOnYear {
			get {
				if (null == Thema) {
					return true;
				}
				return Year >= Thema.Parameters.SafeGet("beginActualYear", 1900) &&
				       Year <= Thema.Parameters.SafeGet("endActualYear", 3000);
			}
		}

		/// <summary>
		/// 	Признак использования бизтрана
		/// </summary>
		public bool UseBizTranMatrix {
			get { return _useBizTranMatrix || TableView == "biztranform"; }
			set { _useBizTranMatrix = value; }
		}

		/// <summary>
		/// 	Установка статуса
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <param name="skipcheck"> </param>
		/// <param name="parent"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state, bool skipcheck = false,
		                    int parent = 0) {
			if (GetState(obj, detail) == state) {
				return parent;
			}
			obj = FixedObject ?? obj;
			cachedState = null;
			var m = "";
			var yes = true;

			if (!IsActualOnYear) {
				throw new Exception("try to set state for not actual year for this form (thema)");
			}

			if (!skipcheck) {
				yes = StateManager.CanSet(this, obj, null, state, out m, parent);
			}

			if (!yes) {
				throw new Exception("Попытка установить недопустимый статус '" + m + "'");
			}
			if (UnderwriteCode.IsEmpty()) {
				return 0;
			}
			AssumeExistsState(UnderwriteCode);
			//state_ = state;


			var periodmapper = Container.Get<ILockPeriodMapper>();
			var toperiods = new int[] {};
			if (null != periodmapper) {
				var op = LockOperation.None;
				if (state == "0ISBLOCK") {
					op = LockOperation.Block;
				}
				else if (state == "0ISOPEN") {
					op = LockOperation.Open;
				}
				toperiods = periodmapper.GetLockingPeriods(op, Period);
			}
			var master = processStateRow(detail, obj, Period, state, m ?? "", 0);
			if (0 != parent) {
				master = parent;
			}
			foreach (var toperiod in toperiods) {
				if (state == "0ISBLOCK") {
					var t = Clone();
					t.Period = toperiod;
					var currentstate = t.GetState(obj, detail);
					if (currentstate == "0ISCHECKED") {
						continue;
					}
				}
				if (state == "0ISOPEN") {
					if (IsPeriodOpen() || 0 == StateManager.GetPeriodState(Year, toperiod)) {
						continue;
					}
				}
				processStateRow(detail, obj, toperiod, state, "автоматически", master);
			}

			StateManager.Process(this, obj, detail, state, master);

			cachedState = state;
			return master;
		}


		/// <summary>
		/// 	Обновить статусы
		/// </summary>
		public void RefreshState() {
			stateCache.Clear();
		}

		/// <summary>
		/// 	Код подписи
		/// </summary>
		public string UnderwriteCode { get; set; }

		/// <summary>
		/// 	Тип метода сохранения
		/// </summary>
		public string SaveMethod { get; set; }

		/// <summary>
		/// 	Специальный клиентский скрипт (может оставить)
		/// </summary>
		public string Script { get; set; }

		/// <summary>
		/// 	Код связанного отчета
		/// </summary>
		public string BindedReport { get; set; }

		/// <summary>
		/// 	Разбивать по деталям
		/// </summary>
		public bool DetailSplit { get; set; }

		/// <summary>
		/// 	Год шаблона
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// 	Период шаблона
		/// </summary>
		public int Period { get; set; }

		/// <summary>
		/// 	Прямая дата шаблона
		/// </summary>
		public DateTime DirectDate { get; set; }

		/// <summary>
		/// 	Корневая строка
		/// </summary>
		public RowDescriptor Form {
			get {
				if (Rows.Count == 0) {
					return null;
				}
				return Rows[0];
			}
			set {
				if (Rows.Count > 0) {
					Rows[0] = value;
				}
				else {
					Rows.Add(value);
				}
			}
		}

		/// <summary>
		/// 	Список строк
		/// </summary>
		public IList<RowDescriptor> Rows {
			get { return _rows; }
		}

		/// <summary>
		/// 	Колсет
		/// </summary>
		public IList<ColumnDesc> Values { get; set; }


		/// <summary>
		/// 	Подготовить форму к периоду
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="directDate"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public IInputTemplate PrepareForPeriod(int year, int period, DateTime directDate, IZetaMainObject obj) {
			log.Debug( "start prepare for period " + year + " " + period);
			var accomodatedPeriod = AccomodatePeriod(period, obj);
			var result = Clone();
			result.Year = year;
			result.Period = accomodatedPeriod;
			result.DirectDate = directDate;
			foreach (var value in result.GetAllColumns()) {
				value.ApplyPeriod(year, accomodatedPeriod, directDate);
			}
			foreach (var parameter in Parameters) {
				result.Parameters[parameter.Key] = parameter.Value;
			}

			return result;
		}


		/// <summary>
		/// 	Непонятно что для MVC
		/// </summary>
		public string Controller { get; set; }

		/// <summary>
		/// 	Признак использования нестандартного вида
		/// </summary>
		public bool IsCustomView {
			get { return CustomView.IsNotEmpty(); }
		}

		/// <summary>
		/// 	Перенаправление периодов
		/// </summary>
		public string PeriodRedirect { get; set; }


		/// <summary>
		/// 	Ссылка на исходную конфигурацию
		/// </summary>
		public XPathNavigator SourceXmlConfiguration { get; set; }

		/// <summary>
		/// 	Нестандартный пользовательский вид
		/// </summary>
		public string CustomView { get; set; }

		/// <summary>
		/// 	Получить статус по расписанию
		/// </summary>
		/// <returns> </returns>
		public ScheduleState GetScheduleState() {
			if (IsOpen) {
				var periodstate = new PeriodStateManager {System = "Default"}.Get(Year, Period);
				var deadline = periodstate.DeadLine;
				if (deadline.Year <= 1900) {
					return new ScheduleState {Date = Qorpent.QorpentConst.Date.Begin, Overtime = ScheduleOvertime.None};
				}
				if (DateTime.Now > deadline) {
					return new ScheduleState {Date = deadline, Overtime = ScheduleOvertime.Fail};
				}
				if (DateTime.Now.AddDays(10) > deadline) {
					return new ScheduleState {Date = deadline, Overtime = ScheduleOvertime.Critical};
				}
			}
			return new ScheduleState {Date = Qorpent.QorpentConst.Date.Begin, Overtime = ScheduleOvertime.None};
		}


		/// <summary>
		/// 	Нестандартный тип контроллера
		/// </summary>
		public string CustomControllerType { get; set; }

		//NOTE: непереносимая повязка на MVC
		/*
        public void Prepare(IController controller) {
            if (CustomControllerType.hasContent()) {
                var preparator = CustomControllerType.toType().create<IControllerPreparator>();
                preparator.Prepare(controller);
            }
        }
		 */

		/// <summary>
		/// 	Код формы
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 	Название формы
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// 	Смещение расписания
		/// </summary>
		public int ScheduleDelta { get; set; }


		// IList<InputField> Fields {
		//     get { return fields; }
		// }

		/// <summary>
		/// 	Клонирует форму
		/// </summary>
		/// <returns> </returns>
		public IInputTemplate Clone() {
			var result = new InputTemplate();


			

			result.BindFrom(this, true);

			if(null!=this.ForPeriods)result.ForPeriods = ForPeriods.Select(x => x).ToArray();
			result.Thema = Thema;

	
			foreach (var row in Rows) {
				result.Rows.Add(row);
			}
			foreach (var code in FixedRowCodes) {
				result.FixedRowCodes.Add(code);
			}
			foreach (var value in Values) {
				var val = value.Clone();
				val.ConditionMatcher = this;
				result.Values.Add(val);
			}
			foreach (var parameter in parameters) {
				var key = parameter.Key ?? "NULL";
				result.Parameters[key] = parameter.Value;
			}

			foreach (var source in Sources) {
				result.Sources.Add(source);
			}

			foreach (var document in Documents) {
				result.Documents[document.Key] = document.Value;
			}

			return result;
		}

		//NOTE: несовместимое устаревание, не будет больше MVC
		/*
        public IEnumerable<IZetaCell> GetCellsByTargets(Controller controller) {
            NameValueCollection valueCollection = controller.Form;
            return BindCells(valueCollection);
        }
		 */

		/// <summary>
		/// 	Нестандартная команда сохранения
		/// </summary>
		public string CustomSave { get; set; }

		/// <summary>
		/// 	Фиксированный объект
		/// </summary>
		public IZetaMainObject FixedObject {
			get {
				if (_fixedobj == null && FixedObjectCode.IsNotEmpty()) {
					_fixedobj = MetaCache.Default.Get<IZetaMainObject>(FixedObjectCode);
				}
				return _fixedobj;
			}
		}

		/// <summary>
		/// 	Проверка необходимости загрузочного скрипта
		/// </summary>
		public bool NeedPreloadScript { get; set; }

		/// <summary>
		/// 	Determines whether the specified obj is match.
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <returns> <c>true</c> if the specified obj is match; otherwise, <c>false</c> . </returns>
		public bool IsMatch(IZetaMainObject obj) {
			return GroupFilterHelper.IsMatch(obj, ForGroup);
		}

		private string DirectCanSetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
			obj = FixedObject ?? obj;
			return _cansetstateCache.CachedGet(state, () =>
				{
					var result = "";
					StateManager.CanSet(this, obj, detail, state, out result);
					return result;
				});
		}

		private string internalGetState(IZetaMainObject obj, IZetaDetailObject detail, IDictionary<string, object> statecache) {
			obj = FixedObject ?? obj;
			if (UnderwriteCode.IsEmpty()) {
				return "0ISOPEN";
			}
			if (statecache != null) {
				return statecache.SafeGet(UnderwriteCode + "_" + Period.ToString(), "0ISOPEN");
			}
			foreach (var interceptor in StateInterceptors) {
				var res = interceptor.GetState(this, obj, detail);
				if (res.IsNotEmpty()) {
					return res;
				}
			}
			var dstate = DefaultState;
			if (dstate.IsEmpty()) {
				dstate = "0ISOPEN";
			}


			if (null == obj) {
				return dstate;
			}
			if (stateCache.ContainsKey(obj.Id)) {
				return stateCache[obj.Id];
			}
			//if (null != state_) return state_;
			if (UnderwriteCode.IsEmpty()) {
				return dstate;
			}

#if OLDSTATES
            DataRowSet drs = GetStateDrs(obj, detail);

            var result = drs.GetString();
            if (result.noContent()){
                result = dstate;
            }
#else
			var result = StateManager.DoGet(obj.Id, Year, Period, UnderwriteCode);
#endif
			//state_ = result;
			stateCache[obj.Id] = result;
			return result;
		}

		private IEnumerable<ColumnDesc> getAllColumns() {
			foreach (var r in Sources) {
				if (null != r) {
					foreach (var c in r.Values) {
						yield return c.Clone();
					}
				} //TODO check errors
			}
			foreach (var c in Values) {
				yield return c;
			}
		}


		private int processStateRow(IZetaDetailObject detail, IZetaMainObject obj, int period, string state,
		                            string comment, int parent) {
			var result = StateManager.DoSet(obj.Id, Year, period, UnderwriteCode, Code, Application.Current.Principal.CurrentUser.Identity.Name, state, comment, parent);
#if OLDSTATES
            using (var s = new TemporaryTransactionSession())
            {
                var h = new InputRowHelper();
                IZetaDetailObject d = null;
                if (IsForDetail && !DetailSplit && null != detail)
                {
                    d = detail;
                }

                var c = h.Get(obj, d, RowCache.SafeGet(UnderwriteCode), new ColumnDesc
                                                                        {
                                                                            Year = Year,
                                                                            Period = period,
                                                                            DirectDate =
                                                                                DirectDate,
                                                                            Target =
                                                                                myapp.storage.Get<IZetaColumn>().Load
                                                                                <IZetaColumn>(
                                                                                "0CONSTSTR")
                                                                        });

                c.RowData.StringValue = state;
                c.Usr = Application.Current.Principal.CurrentUserName;
                myapp.storage.Get<IZetaCell>().Save(c);
#endif
			var conn = Qorpent.Applications.Application.Current.DatabaseConnections.GetConnection("Default"); //Application.Current.Container.GetSession().Connection;
			var parameters = new Dictionary<string, object>();

			parameters["row"] = UnderwriteCode;
			parameters["obj"] = obj.Id;
			parameters["year"] = Year;
			parameters["period"] = period;
			parameters["newstate"] = state;
			parameters["template"] = Code;
			if (Rows.Count > 0) {
				parameters["trow"] = Rows[0].Code;
			}
			else {
				parameters["trow"] = "none";
			}
			var command =
				conn.CreateCommand(
					"exec usm.state_after_change @row=@row,@obj=@obj,@year=@year,@period=@period, @newstate=@newstate,@trow=@trow, @template = @template",
					parameters);
			//Application.Current.Container.GetSession().Transaction.Enlist(command);
			command.ExecuteNonQuery();
			//if (state == "0ISCHECKED") {
			//	Evaluator.DefaultCache.Clear();
			//}


#if OLDSTATES
                s.Commit();
            }
#endif

			return result;
		}

		private void AssumeExistsState(string code) {
			throw new NotSupportedException("this method not supported by extreme core");
		}

		/// <summary>
		/// 	Расчитать правильный период
		/// </summary>
		/// <param name="period"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public int AccomodatePeriod(int period, IZetaMainObject obj) {
			obj = FixedObject ?? obj;
			if (RedirectPeriodMap == null) {
				return period;
			}
			int newp = 0;
			var objperiodtype = "";
			if (null != obj) {
				if (obj.GroupCache.IsNotEmpty() && obj.GroupCache.Contains("/PR_")) {
					objperiodtype = obj.GroupCache.RegexFind(@"/(PR_\w+)/", 1);
					newp = RedirectPeriodMap.SafeGet(period+"_" + objperiodtype);
				}
				else {
					newp = RedirectPeriodMap.SafeGet(period + "_");
				}
			}
			if(0==newp)newp = period;
			return newp;
		}

		private readonly IDictionary<string, string> _cansetstateCache = new Dictionary<string, string>();
		private readonly IList<RowDescriptor> _rows = new List<RowDescriptor>();
		//  private readonly IList<InputField> fields = new List<InputField>();
		private readonly IUserLog log = Application.Current.LogManager.GetLog("zinput",null);
		//    private readonly IDictionary<string, InputQuery> queries = new Dictionary<string, InputQuery>();
		private readonly IList<IInputTemplate> sources = new List<IInputTemplate>();
		private readonly IDictionary<int, string> stateCache = new Dictionary<int, string>();

		///<summary>
		///	Асинхронная задача проврки статуса
		///</summary>
		public Task CanSetTask;

		private IList<ColumnDesc> _cachedcolumns;
		private IContainer _container;
		private IDictionary<string, string> _documents = new Dictionary<string, string>();
		private string[] _excludes;
		private IZetaMainObject _fixedobj;
		private bool _ignorePeriodState;
		private IDictionary<string, int> _redirectPeriodMap;
		private IScheduleChecker _scheduleChecker;
		private IList<IStateCheckInterceptor> _stateInterceptors;
		private IStateManager _stateManager;
		private bool _useBizTranMatrix;
		private string biztran = "";
		private string cachedState;
		private IDetailFilter detailFilter;
		private IDictionary<string, string> parameters = new Dictionary<string, string>();
	}
}