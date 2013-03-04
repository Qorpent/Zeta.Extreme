#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : InputTemplate.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensibility;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Logging;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.SaveSupport;
using Zeta.Extreme.Form.StateManagement;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.Meta;
using ColumnDesc = Zeta.Extreme.BizProcess.Themas.ColumnDesc;
using IConditionMatcher = Zeta.Extreme.BizProcess.Themas.IConditionMatcher;


namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	������ �����
	/// </summary>
	public class InputTemplate : IInputTemplate, ICloneable, BizProcess.Themas.IConditionMatcher {
		/// <summary>
		/// 	����������� �� ���������
		/// </summary>
		public InputTemplate() {
			Values = new List<ColumnDesc>();
			FixedRowCodes = new List<string>();
		}

		/// <summary>
		/// 	��������� ���� ��������
		/// </summary>
		public bool ApplyValueCourse { get; set; }

		/// <summary>
		/// 	������ �� ��������� ��������
		/// </summary>
		public IInversionContainer Container {
			get {
				if (_container.invalid()) {
					lock (this) {
						if (_container.invalid()) {
							Container = myapp.ioc;
						}
					}
				}
				return _container;
			}
			set { _container = value; }
		}

		private IStateManager StateManager {
			get { return _stateManager ?? (_stateManager = Container.get<IStateManager>() ?? StateManagement.StateManager.Default); }
			set { _stateManager = value; }
		}

		/// <summary>
		/// 	��� SQL - ������!
		/// </summary>
		public IDictionary<string, decimal> SqlCache { get; set; }

		/// <summary>
		/// 	���� ��� ����������� - ������!!
		/// </summary>
		public string Area { get; set; }

		/// <summary>
		/// 	������������ ����������
		/// </summary>
		public IScheduleChecker ScheduleChecker {
			get {
				if (_scheduleChecker == null) {
					if (ScheduleClass.hasContent()) {
						_scheduleChecker = InversionExtensions.typenameOrIoc<IScheduleChecker>(ScheduleClass);
					}
				}
				return _scheduleChecker;
			}
		}

		/// <summary>
		/// 	������� ������ �� ����� ����������
		/// </summary>
		public IDictionary<string, int> RedirectPeriodMap {
			get {
				if (null == _redirectPeriodMap) {
					if (PeriodRedirect.hasContent()) {
						_redirectPeriodMap = new Dictionary<string, int>();
						var variants = PeriodRedirect.split(false, true, '|');
						foreach (var variant in variants) {
							var test = variant.split(false, true, ':');
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
							var rules = rules_.split();
							foreach (var rule in rules) {
								var record_ = rule.split(false, true, '=');
								var record = new {from = record_[0].toInt(), to = record_[1].toInt()};
								_redirectPeriodMap[record.from + "_" + v] = record.to;
							}
						}
					}
				}
				return _redirectPeriodMap;
			}
		}

		/// <summary>
		/// 	������ ������������� �������� ��������
		/// </summary>
		public IList<IStateCheckInterceptor> StateInterceptors {
			get { return _stateInterceptors ?? (_stateInterceptors = Container.all<IStateCheckInterceptor>().ToList()); }
			set { _stateInterceptors = value; }
		}

		object ICloneable.Clone() {
			return Clone();
		}

		/// <summary>
		/// 	�������� ������������ ������ ��������
		/// </summary>
		/// <param name="condition"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		public bool IsConditionMatch(string condition) {
			if (condition.noContent()) {
				return true;
			}
			if (!Parameters.get("activecondition", () => false)) {
				return true;
			}
			var conds_ = Parameters.get("conditions", "");
			if (conds_.noContent()) {
				return false;
			}
			var conds = conds_.split();
			if (condition.Contains(",") || condition.Contains("|")) {
				var condsets = condition.split(false, true, '|');
				foreach (var condset in condsets) {
					var match = conds.containsAll(condset.split().ToArray());
					if (match) {
						return true;
					}
				}
				return false;
			}
			var e = PythonPool.Get();
			var cond = condition;
			try {
				cond = condition.replace(@"[\w\d_]+", m =>
					{
						if (m.Value.isIn("or", "and", "not")) {
							return m.Value;
						}
						var c = m.Value;
						if (conds.Contains(c)) {
							return " True ";
						}
						return " False ";
					}).Trim();
				var result = e.Execute<bool>(cond);
				return result;
			}
			catch (Exception) {
				throw new Exception("������ � " + cond);
			}
			finally {
				PythonPool.Release(e);
			}
		}

		/// <summary>
		/// 	������ ������������ �����
		/// </summary>
		public IFormSession AttachedSession { get; set; }

		/// <summary>
		/// 	������������� �������� AJAX-���������� (�����)
		/// </summary>
		public bool UseQuickUpdate { get; set; }

		/// <summary>
		/// 	������� �� ������� ��������
		/// </summary>
		public void CleanupStates() {
			cachedState = null;
			stateCache.Clear();
		}

		/// <summary>
		/// 	����� ��������� ���������
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
		/// 	���������� ����������� �������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="values"> </param>
		/// <returns> </returns>
		public IList<ColumnDesc> AccomodateColumnSet(IZetaMainObject obj, IList<ColumnDesc> values) {
			if (null == obj) {
				return values;
			}
			var balmet = obj.ResolveTag("BALMET");
			if (balmet.noContent()) {
				return values;
			}
			var balmets = balmet.split().Select(x => x.ToUpper()).ToArray();
			var result = new List<ColumnDesc>();
			foreach (var columnDesc in values) {
				if (columnDesc.RedirectBasis.noContent()) {
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
		/// 	����� ��������� ��������� �������� �������
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
			var th = Thema as EcoThema;
			if (null != th) {
				var splitobj = th.GetParameter("splittoobj", "");
				if (splitobj.hasContent()) {
					objects.Clear();
					var subobjtypes = splitobj.split();
					foreach (var subobjtype in subobjtypes) {
						if (subobjtype == "SELF") {
							objects.Add(obj);
						}
						else {
							foreach (var o in obj.AllChildren()) {
								if (o.ObjType.Code == subobjtype || o.ObjType.Class.Code == subobjtype) {
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
		/// 	�������� ������ �������
		/// </summary>
		/// <returns> </returns>
		public string GetColGroup() {
			if (null != Thema) {
				var firstyear = Thema.GetParameter("firstyear").toInt();
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
		/// 	�������� ���������� ������
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		public bool IsValidRow(IZetaRow row) {
			if (null == _excludes) {
				_excludes = new string[] {};
				if (Parameters.ContainsKey("excluderows")) {
					_excludes = Parameters.get("excluderows", "").split().ToArray();
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


		//NOTE: ����� �� �������������� � �� ��������� - ������ �����

		/// <summary>
		/// 	������ �� ����� ��������
		/// </summary>
		public string Biztran {
			get {
				var r = Parameters.get("biztran", "");
				if (r.hasContent()) {
					return r;
				}
				if (biztran.hasContent()) {
					return biztran;
				}
				if (null == Thema) {
					return "";
				}
				r = Thema.Parameters.get("biztran", "");
				return r;
			}
			set { biztran = value; }
		}

		//NOTE: �� ������ ������ �������������� ������ �� ����������
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

				return myapp.ioc.get<IDbfsRepository>().SearchBySpecialProc("usm.get_form_attachments",
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
		//TODO: ���� ��� �� ������� �����������
		/*
		/// <summary>
		/// 	�������� �������� �����
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj) {
			return GetRowChecks(row, obj, null);
		}
		
		///<summary>
		///	�������� �������� �����
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
		/// 	�������� ����� �������� �����
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public string GetCheckedRowClass(IZetaRow row, IZetaMainObject obj) {
			return GetRowChecks(row, obj).Select(x => x.RowClass).Where(x => x.hasContent()).Select(x => "_cr_" + x).concat(" ");
		}

		/// <summary>
		/// 	�������� ����� ����������� ������
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public string GetCheckedRowStyle(IZetaRow row, IZetaMainObject obj) {
			return GetRowChecks(row, obj).Select(x => x.RowStyle).Where(x => x.hasContent()).concat(" ");
		}
	

		/// <summary>
		/// 	�������� ����� ����������� ������
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
		/// 	�������� ����� ����������� ������
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
		/// 	��� �������������� �������
		/// </summary>
		public string FixedObjectCode { get; set; }

		/// <summary>
		/// 	������� ������ ������� �������
		/// </summary>
		public bool IgnorePeriodState {
			get {
				if (myapp.roles.IsInRole(myapp.usr, "IGNOREPERIODSTATE", false)) {
					return true;
				}
				if (null != Thema) {
					var y = Thema.GetParameter("firstyear").toInt();
					if (y > 0 && y > Year) {
						return true;
					}
				}
				return _ignorePeriodState;
			}
			set { _ignorePeriodState = value; }
		}

		/// <summary>
		/// 	�������� ����������� �������
		/// </summary>
		public bool IsObjectDependent { get; set; }

		/// <summary>
		/// 	�������� ����� (�����������)
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public IForm GetForm(IZetaMainObject obj) {
			var theform =
				myapp.storage.Get<IForm>().First(
					"from ENTITY x where x.Template = ? and x.Object.Id=? and x.Year =? and x.Period = ?",
					UnderwriteCode ?? Code, obj.Id, Year, Period
					);
			return theform;
		}

		/// <summary>
		/// 	����� - ��������� - ����������
		/// </summary>
		public IList<IInputTemplate> Sources {
			get { return sources; }
		}

		/// <summary>
		/// 	������ ������������
		/// </summary>
		public string DocumentRoot { get; set; }

		/// <summary>
		/// 	���������
		/// </summary>
		public IDictionary<string, string> Documents {
			get { return _documents; }
			set { _documents = value; }
		}

		/// <summary>
		/// 	�������� ������ �� ����
		/// </summary>
		public IThema Thema { get; set; }

		/// <summary>
		/// 	������ ����� �� ���������
		/// </summary>
		public string DefaultState { get; set; }

		/// <summary>
		/// 	������� ������������� ������ ��������� �����
		/// </summary>
		public bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// 	�������������� ���������
		/// </summary>
		public string AdvDocs { get; set; }

		/// <summary>
		/// 	����� ���������� �����
		/// </summary>
		public string ScheduleClass { get; set; }

		/// <summary>
		/// 	���������� ������������ �����
		/// </summary>
		public string NeedFiles { get; set; }

		/// <summary>
		/// 	���������� � �������� ������������� ������
		/// </summary>
		public string NeedFilesPeriods { get; set; }

		/// <summary>
		/// 	���� �������
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		/// 	������� ������ ������� � �������� ���������
		/// </summary>
		public bool ShowMeasureColumn { get; set; }

		/// <summary>
		/// 	������� ����, ��� ����� ���������
		/// </summary>
		public bool IsChecked { get; set; }

		/// <summary>
		/// 	������� ����������� �������??
		/// </summary>
		public bool DetailFavorite { get; set; }

		/// <summary>
		/// 	�������� SQL ����������� ������ �����
		/// </summary>
		public string SqlOptimization { get; set; }

		/// <summary>
		/// 	������ �� ������������
		/// </summary>
		public InputConfiguration Configuration { get; set; }

		/// <summary>
		/// 	������� ���������� �����
		/// </summary>
		public bool IsOpen { get; set; }

		/// <summary>
		/// 	���� �� �������
		/// </summary>
		public string UnderwriteRole { get; set; }

		/// <summary>
		/// 	����� ������� ����� ��� �������, ����� �����-��
		/// </summary>
		public bool InputForDetail { get; set; }

		/// <summary>
		/// 	�������� ������������ �������
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
		/// 	������� ����� ��� ����� ������
		/// </summary>
		public bool IsForSingleDetail {
			get { return IsForDetail && !DetailSplit; }
		}
		*/
		/// <summary>
		/// 	��� ������� �� ������� (�����)
		/// </summary>
		public string DetailFilterName { get; set; }

		/// <summary>
		/// 	������ �� �������
		/// </summary>
		public IDetailFilter DetailFilter {
			get {
				if (null == detailFilter && DetailFilterName.hasContent()) {
					detailFilter = DetailFilterName.get<IDetailFilter>();
					detailFilter.Configure(this);
				}
				return detailFilter;
			}
		}

		/// <summary>
		/// 	������������� ������ �����
		/// </summary>
		public IList<string> FixedRowCodes { get; set; }

		/// <summary>
		/// 	���� ������� - ���� ������������ �� ���-�� JSON - �������������
		/// </summary>
		public string TableView { get; set; }

		/// <summary>
		/// 	������� ���������� �� ������
		/// </summary>
		public string ForGroup { get; set; }

		/// <summary>
		/// 	������ �� ��������
		/// </summary>
		public int[] ForPeriods { get; set; }

		/// <summary>
		/// 	���������
		/// </summary>
		public IDictionary<string, string> Parameters {
			get { return parameters; }
			set { parameters = value; }
		}

		/// <summary>
		/// 	������ ������
		/// </summary>
		public string Help { get; set; }

		///<summary>
		///	������������ ��������������
		///</summary>
		public string AutoFillDescription { get; set; }


		//	public IDictionary<string, InputQuery> Queries {
		//        get { return queries; }
		//    }

		//private string state_ = null;
		/// <summary>
		/// 	Gets the state (��������� ������� ������ ������� ����� �� ����������
		/// </summary>
		/// <param name="obj"> The obj. </param>
		/// <param name="detail"> </param>
		/// <returns> </returns>
		public string GetState(IZetaMainObject obj, IZetaDetailObject detail) {
			return GetState(obj, detail, null);
		}

		/// <summary>
		/// 	Gets the state (��������� ������� ������ ������� ����� �� ����������
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
		/*
		/// <summary>
		/// 	������� ����� ��� �������
		/// </summary>
		public bool IsForDetail {
			get {
				if (null == Form) {
					return false;
				}
				if (null == Form.Target) {
					Form.Target = RowCache.get(Form.Code);
				}
				return Form.Target.isDetailForm();
			}
		}

		/// <summary>
		/// 	������� ����� ��� �������
		/// </summary>
		public bool IsInputForDetail {
			get {
				if (IsForDetail) {
					return true;
				}
				return InputForDetail;
			}
		}*/

		/// <summary>
		/// 	������ ����� �������� ���������
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
		/// 	����������� �������� �������
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
		/// 	�������� ����������� ���������� ������
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
		///	������������ ���� SQL - �����-�� �����
		///</summary>
		///<param name="obj"> </param>
		///<param name="year"> </param>
		///<param name="period"> </param>
		///<returns> </returns>
		public IDictionary<string, object> ReloadSqlCache(IZetaMainObject obj, int year, int period) {
			SqlCache = new Dictionary<string, decimal>();
			var result =
				Qorpent.Applications.Application.Current.DatabaseConnections.GetConnection("Default").ExecuteDictionaryReader(
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
		/// 	�������� ������ ������
		/// </summary>
		/// <returns> </returns>
		public IEnumerable<ColumnDesc> GetAllColumns() {
			if (_cachedcolumns == null) {
				_cachedcolumns = getAllColumns().ToList();
			}
			return _cachedcolumns;
		}

		/// <summary>
		/// 	�������� ���������� �������
		/// </summary>
		/// <returns> </returns>
		public bool IsPeriodOpen() {
			if (IgnorePeriodState) {
				return true;
			}
			return StateManager.GetPeriodState(Year, Period) == 1;
		}

		/// <summary>
		/// 	���������� ������
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="detail"> </param>
		/// <param name="state"> </param>
		/// <returns> </returns>
		public int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
			return SetState(obj, detail, state, false, 0);
		}

		/// <summary>
		/// 	�������� ������������ ��� ����
		/// </summary>
		public bool IsActualOnYear {
			get {
				if (null == Thema) {
					return true;
				}
				return Year >= Thema.Parameters.get("beginActualYear", 1900) &&
				       Year <= Thema.Parameters.get("endActualYear", 3000);
			}
		}

		/// <summary>
		/// 	������� ������������� ��������
		/// </summary>
		public bool UseBizTranMatrix {
			get { return _useBizTranMatrix || TableView == "biztranform"; }
			set { _useBizTranMatrix = value; }
		}

		/// <summary>
		/// 	��������� �������
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
				throw new Exception("������� ���������� ������������ ������ '" + m + "'");
			}
			if (UnderwriteCode.noContent()) {
				return 0;
			}
			AssumeExistsState(UnderwriteCode);
			//state_ = state;


			var periodmapper = Container.get<ILockPeriodMapper>();
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
					if (IgnorePeriodState || 0 == StateManager.GetPeriodState(Year, toperiod)) {
						continue;
					}
				}
				processStateRow(detail, obj, toperiod, state, "�������������", master);
			}

			StateManager.Process(this, obj, detail, state, master);

			myapp.getProfile().Set("refresh", "True");
			/*
			//� �������� �������� ����� ����������� �������� ���������� ������
			if (state == "0ISCHECKED") {
				var objid = obj == null ? detail.Object.Id : obj.Id();
				var detailid = detail == null ? 0 : detail.Id;
				Evaluator.DefaultCache.Clear(@"(?i)org\(" + objid + @"\)");
				if (detailid != 0) {
					Evaluator.DefaultCache.Clear(@"(?i)subpart\(" + detailid + @"\)");
				}
				Evaluator.InvokeChanged();
			}
			*/
			cachedState = state;
			return master;
		}


		/// <summary>
		/// 	�������� �������
		/// </summary>
		public void RefreshState() {
			stateCache.Clear();
		}

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string UnderwriteCode { get; set; }

		/// <summary>
		/// 	��� ������ ����������
		/// </summary>
		public string SaveMethod { get; set; }

		/// <summary>
		/// 	����������� ���������� ������ (����� ��������)
		/// </summary>
		public string Script { get; set; }

		/// <summary>
		/// 	��� ���������� ������
		/// </summary>
		public string BindedReport { get; set; }

		/// <summary>
		/// 	��������� �� �������
		/// </summary>
		public bool DetailSplit { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// 	������ �������
		/// </summary>
		public int Period { get; set; }

		/// <summary>
		/// 	������ ���� �������
		/// </summary>
		public DateTime DirectDate { get; set; }

		/// <summary>
		/// 	�������� ������
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
		/// 	������ �����
		/// </summary>
		public IList<RowDescriptor> Rows {
			get { return _rows; }
		}

		/// <summary>
		/// 	������
		/// </summary>
		public IList<ColumnDesc> Values { get; set; }


		/// <summary>
		/// 	����������� ����� � �������
		/// </summary>
		/// <param name="year"> </param>
		/// <param name="period"> </param>
		/// <param name="directDate"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public IInputTemplate PrepareForPeriod(int year, int period, DateTime directDate, IZetaMainObject obj) {
			log.debug(() => "start prepare for period " + year + " " + period);
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
		/// 	��������� ��� ��� MVC
		/// </summary>
		public string Controller { get; set; }

		/// <summary>
		/// 	������� ������������� �������������� ����
		/// </summary>
		public bool IsCustomView {
			get { return CustomView.hasContent(); }
		}

		/// <summary>
		/// 	��������������� ��������
		/// </summary>
		public string PeriodRedirect { get; set; }


		/// <summary>
		/// 	������ �� �������� ������������
		/// </summary>
		public XPathNavigator SourceXmlConfiguration { get; set; }

		/// <summary>
		/// 	������������� ���������������� ���
		/// </summary>
		public string CustomView { get; set; }

		/// <summary>
		/// 	�������� ������ �� ����������
		/// </summary>
		/// <returns> </returns>
		public ScheduleState GetScheduleState() {
			if (IsOpen) {
				var periodstate = new PeriodStateManager {System = "Default"}.Get(Year, Period);
				var deadline = periodstate.DeadLine;
				if (deadline.Year <= 1900) {
					return new ScheduleState {Date = DateExtensions.Begin, Overtime = ScheduleOvertime.None};
				}
				if (DateTime.Now > deadline) {
					return new ScheduleState {Date = deadline, Overtime = ScheduleOvertime.Fail};
				}
				if (DateTime.Now.AddDays(10) > deadline) {
					return new ScheduleState {Date = deadline, Overtime = ScheduleOvertime.Critical};
				}
			}
			return new ScheduleState {Date = DateExtensions.Begin, Overtime = ScheduleOvertime.None};
		}


		/// <summary>
		/// 	������������� ��� �����������
		/// </summary>
		public string CustomControllerType { get; set; }

		//NOTE: ������������� ������� �� MVC
		/*
        public void Prepare(IController controller) {
            if (CustomControllerType.hasContent()) {
                var preparator = CustomControllerType.toType().create<IControllerPreparator>();
                preparator.Prepare(controller);
            }
        }
		 */

		/// <summary>
		/// 	��� �����
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// 	�������� �����
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// 	�������� ����������
		/// </summary>
		public int ScheduleDelta { get; set; }


		// IList<InputField> Fields {
		//     get { return fields; }
		// }

		/// <summary>
		/// 	��������� �����
		/// </summary>
		/// <returns> </returns>
		public IInputTemplate Clone() {
			var result = new InputTemplate {ForPeriods = ForPeriods.Select(x => x).ToArray()};

			result.bindfrom(this, true);

			result.Thema = Thema;

			//  foreach (var pair in Queries) {
			//      result.Queries[pair.Key] = pair.Value;
			//    }
			//    foreach (InputField field in Fields) {
			//         result.Fields.Add(field);
			//   }
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

		//NOTE: ������������� �����������, �� ����� ������ MVC
		/*
        public IEnumerable<IZetaCell> GetCellsByTargets(Controller controller) {
            NameValueCollection valueCollection = controller.Form;
            return BindCells(valueCollection);
        }
		 */

		/// <summary>
		/// 	������������� ������� ����������
		/// </summary>
		public string CustomSave { get; set; }

		/// <summary>
		/// 	������������� ������
		/// </summary>
		public IZetaMainObject FixedObject {
			get {
				if (_fixedobj == null && FixedObjectCode.hasContent()) {
					_fixedobj = myapp.storage.Get<IZetaMainObject>().Load(FixedObjectCode);
				}
				return _fixedobj;
			}
		}

		/// <summary>
		/// 	�������� ������������� ������������ �������
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
			return _cansetstateCache.get(state, () =>
				{
					var result = "";
					StateManager.CanSet(this, obj, detail, state, out result);
					return result;
				});
		}

		private string internalGetState(IZetaMainObject obj, IZetaDetailObject detail, IDictionary<string, object> statecache) {
			obj = FixedObject ?? obj;
			if (UnderwriteCode.noContent()) {
				return "0ISOPEN";
			}
			if (statecache != null) {
				return statecache.get(UnderwriteCode + "_" + Period.ToString(), "0ISOPEN");
			}
			foreach (var interceptor in StateInterceptors) {
				var res = interceptor.GetState(this, obj, detail);
				if (res.hasContent()) {
					return res;
				}
			}
			var dstate = DefaultState;
			if (dstate.noContent()) {
				dstate = "0ISOPEN";
			}


			if (null == obj) {
				return dstate;
			}
			if (stateCache.ContainsKey(obj.Id)) {
				return stateCache[obj.Id];
			}
			//if (null != state_) return state_;
			if (UnderwriteCode.noContent()) {
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
			var result = StateManager.DoSet(obj.Id, Year, period, UnderwriteCode, Code, myapp.usrName, state, comment, parent);
#if OLDSTATES
            using (var s = new TemporaryTransactionSession())
            {
                var h = new InputRowHelper();
                IZetaDetailObject d = null;
                if (IsForDetail && !DetailSplit && null != detail)
                {
                    d = detail;
                }

                var c = h.Get(obj, d, RowCache.get(UnderwriteCode), new ColumnDesc
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
                c.Usr = myapp.usrName;
                myapp.storage.Get<IZetaCell>().Save(c);
#endif
			var conn = Qorpent.Applications.Application.Current.DatabaseConnections.GetConnection("Default"); //myapp.ioc.getSession().Connection;
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
			//myapp.ioc.getSession().Transaction.Enlist(command);
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
			var s = myapp.storage.Get<IZetaRow>();
			var row = s.Load(code);
			if (null == row) {
				var parentrow = s.Load("0STATE");
				if (null == parentrow) {
					parentrow = s.New();
					parentrow.Code = "0STATE";
					parentrow.Name = "������ ����";
					s.Save(parentrow);
				}
				row = s.New();
				row.Code = code;
				row.Name = Name;
				row.Parent = parentrow;
				s.Save(row);
			}
		}

		/// <summary>
		/// 	��������� ���������� ������
		/// </summary>
		/// <param name="period"> </param>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public int AccomodatePeriod(int period, IZetaMainObject obj) {
			obj = FixedObject ?? obj;
			if (RedirectPeriodMap == null) {
				return period;
			}

			var objperiodtype = "";
			if (null != obj) {
				if (obj.GroupCache.hasContent() && obj.GroupCache.Contains("/PR_")) {
					objperiodtype = obj.GroupCache.find(@"/(PR_\w+)/", 1);
				}
			}
			//if (null != obj){
			//    objperiodtype = Query.EvaluateRawConstant(obj, "ODPERIODTYPE").toInt();
			//}
			//NOTE: disabled by not functional usage and performance issue
			var newp = RedirectPeriodMap.get("0_" + objperiodtype, () => period, false, period);

			newp = RedirectPeriodMap.get(period + "_" + objperiodtype, () => newp, false, newp);
			return newp;
		}

		private readonly IDictionary<string, string> _cansetstateCache = new Dictionary<string, string>();
		private readonly IList<RowDescriptor> _rows = new List<RowDescriptor>();
		//  private readonly IList<InputField> fields = new List<InputField>();
		private readonly ILog log = logger.get("zinput");
		//    private readonly IDictionary<string, InputQuery> queries = new Dictionary<string, InputQuery>();
		private readonly IList<IInputTemplate> sources = new List<IInputTemplate>();
		private readonly IDictionary<int, string> stateCache = new Dictionary<int, string>();

		///<summary>
		///	����������� ������ ������� �������
		///</summary>
		public Task CanSetTask;

		private IList<ColumnDesc> _cachedcolumns;
		private IInversionContainer _container;
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