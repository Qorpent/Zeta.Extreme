// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensibility;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Logging;
using Comdiv.Model.Interfaces;
using Comdiv.Persistence;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;
using Comdiv.Zeta.Web.Themas;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// ������ �����
	/// </summary>
	public class InputTemplate : IInputTemplate, ICloneable, IConditionMatcher {
        private readonly IList<RowDescriptor> _rows = new List<RowDescriptor>();
      //  private readonly IList<InputField> fields = new List<InputField>();
        private readonly ILog log = logger.get("zinput");
    //    private readonly IDictionary<string, InputQuery> queries = new Dictionary<string, InputQuery>();
        private readonly IList<IInputTemplate> sources = new List<IInputTemplate>();
        private readonly IDictionary<int, string> stateCache = new Dictionary<int, string>();
        private readonly IList<InputTarget> targets = new List<InputTarget>();
        private IList<ColumnDesc> _cachedcolumns;
        private IInversionContainer _container;
        private IDictionary<string, string> _documents = new Dictionary<string, string>();
		private IDictionary<string, int> _redirectPeriodMap;
        private IScheduleChecker _scheduleChecker;
        private string cachedState;
        private IDetailFilter detailFilter;
        private IDictionary<string, string> parameters = new Dictionary<string, string>();
        /// <summary>
        /// ������ ������������� �������� ��������
        /// </summary>
        public IList<IStateCheckInterceptor> StateInterceptors;
        /// <summary>
        /// ������������� �������� AJAX-���������� (�����)
        /// </summary>
        public bool UseQuickUpdate { get; set; }
		/// <summary>
		/// ������� �� ������� ��������
		/// </summary>
		public void CleanupStates() {
			this.cachedState = null;
			stateCache.Clear();
		}
	/// <summary>
	/// ����� ��������� ���������
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
		public object ResolveParameter(string name) {
			if(this.Parameters.ContainsKey(name)) {
				return this.Parameters[name];
			}
			if(null!=this.Thema) {
				return ((Thema) this.Thema).GetParameter(name);
			}
			return null;
		}
		/// <summary>
		/// ���������� ����������� �������
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public IList<ColumnDesc> AccomodateColumnSet(IZetaMainObject obj, IList<ColumnDesc> values ) {
			if(null==obj) return values;
			var balmet = obj.ResolveTag("BALMET");
			if(balmet.noContent()) return values;
			var balmets = balmet.split().Select(x => x.ToUpper()).ToArray();
			var result = new List<ColumnDesc>();
			foreach (var columnDesc in values) {
				if(columnDesc.RedirectBasis.noContent()) result.Add(columnDesc);
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
		/// ����� ��������� ��������� �������� �������
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    	public List<IZetaMainObject> GetWorkingObjects(IZetaMainObject obj)
		{
			var objects = new List<IZetaMainObject>() { obj };
			if(null!=this.FixedObject) {
				objects.Clear();
				objects.Add(this.FixedObject);
				return objects;
			}
			var th = this.Thema as EcoThema;
			if (null != th)
			{
				var splitobj = th.GetParameter("splittoobj", "");
				if (splitobj.hasContent())
				{
					objects.Clear();
					var subobjtypes = splitobj.split();
					foreach (var subobjtype in subobjtypes)
					{
						if (subobjtype == "SELF")
						{
							objects.Add(obj);
						}
						else
						{
							foreach (var o in obj.AllChildren())
							{
								if (o.ObjType.Code == subobjtype || o.ObjType.Class.Code == subobjtype)
								{
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
		/// �������� ������� ������
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public IList<ColumnDesc>  GetWorkingColset(IZetaMainObject obj) {
			var values = this.GetAllColumns().Where(
				v =>
				v.Visible &&
				v.GetIsVisible(obj) &&
				((this.GetColGroup().noContent() && v.Group.noContent()) ||
				 v.Group == this.GetColGroup()
				)
				).ToList();
			var th = this.Thema as EcoThema;
			if (null != th)
			{
				var rc = th.GetParameter("redirectcolset", "");
				if (rc.hasContent())
				{
					values = ColumnDesc.RedirectColset(rc, values);
				}

				
			}
			return values;
		}

		/// <summary>
		/// �������� ����������� �����
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
        public  ControlPointResult[] GetControlPoints( IZetaMainObject obj)
        {
            var result = new List<ControlPointResult>();
            if ((this.Rows[0].Code != "STUB") && this.Rows[0].Target != null)
            {

                foreach (var check in RowCache.GetControlPoints(this.Rows[0].Target))
                {
                    foreach (var col in GetAllColumns())
                    {
                        if (col.GetIsVisible(obj) && col.ControlPoint)
                        {
                            var zone = new Zone(obj);
                            var rd = new RowDescriptor(check);

                            var val =
                                new Query(zone, rd, col, Thema).eval().toDecimal();
                            result.Add(new ControlPointResult(){Row=check,Col =col, Value = val});
                        }
                    }
                }

            }
            return result.ToArray();

        }
		/// <summary>
		/// �������� ������ �������
		/// </summary>
		/// <returns></returns>
    	public string GetColGroup() {
    		if (null != this.Thema) {
    		    var firstyear = this.Thema.GetParameter("firstyear").toInt();
			    if (0 == firstyear) return "";
			    if (this.Year >= firstyear) return "";
				return "HISTORY";
    		}
    		return "";
    	}


    	private string[] _excludes = null;
		/// <summary>
		/// �������� ���������� ������
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public bool IsValidRow(IZetaRow row) {
			if(null==_excludes ) {
				_excludes = new string[]{};
				if (this.Parameters.ContainsKey("excluderows")) {
					_excludes = this.Parameters.get("excluderows", "").split().ToArray();
				}
			}
			if (0 == _excludes.Length) return true;
			if (-1 == Array.IndexOf(_excludes,row.Code)) return true;
			return false;
		}

      //  public Form FormMatrix { get; set; }
	  //NOTE: ����� �� �������������� � �� ��������� - ������ �����
    	private string biztran = "";
    	/// <summary>
    	/// ������ �� ����� ��������
    	/// </summary>
    	public string Biztran {
    		get {
				var r = Parameters.get("biztran", "");
				if (r.hasContent()) return r;
				if (biztran.hasContent()) return biztran;
                if (null == this.Thema) return "";
    			r = this.Thema.Parameters.get("biztran", "");
    			return r;
    		}
			set { biztran = value; }
    	}
		/// <summary>
		/// ����������� �� ���������
		/// </summary>
        public InputTemplate() {
            Values = new List<ColumnDesc>();
            FixedRowCodes = new List<string>();
            StateInterceptors = Container.all<IStateCheckInterceptor>().ToList();
            StateManager = Container.get<IStateManager>();
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
        /// <summary>
        /// �������� �������� �����
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj) {
            return GetRowChecks(row, obj, null);
        }

        /// <summary>
        ///�������� �������� �����
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public ColumnRowCheckCondition[] GetRowChecks(IZetaRow row, IZetaMainObject obj, ColumnDesc col = null) {
            var result = new List<ColumnRowCheckCondition>();
            IEnumerable<ColumnDesc> checkers = null;
            if(col!=null) {
                checkers = new[] {col};
            }else {
                checkers = this.GetAllColumns().Where(x => x.GetIsVisible(obj));
            }
             checkers =   checkers.Where(x => x.RowCheckConditions.Length != 0);
            foreach (var checker in checkers) {
                var rules = checker.GetMatched(row, Decimal.MinValue);
                if(rules.Length!=0) {
                    decimal val = 0;
                    try {
   
                        val = new Query(new Zone(obj), row, checker, this.Thema).evaln();
                    }catch(Exception ex) {
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
        /// �������� ����� �������� �����
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCheckedRowClass(IZetaRow row, IZetaMainObject obj) {
            return GetRowChecks(row, obj).Select(x => x.RowClass).Where(x => x.hasContent()).Select(x=>"_cr_"+x).concat(" ");
        }
        /// <summary>
        /// �������� ����� ����������� ������
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetCheckedRowStyle(IZetaRow row, IZetaMainObject obj)
        {
            return GetRowChecks(row, obj).Select(x => x.RowStyle).Where(x => x.hasContent()).concat(" ");
        }

        /// <summary>
        /// �������� ����� ����������� ������
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetCheckedCellClass(IZetaRow row, IZetaMainObject obj,ColumnDesc col)
        {
            return GetRowChecks(row, obj,col).Select(x => x.CellClass).Where(x => x.hasContent()).Select(x => "_cc_" + x).concat(" ");
        }
        /// <summary>
        /// �������� ����� ����������� ������
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetCheckedCellStyle(IZetaRow row, IZetaMainObject obj,ColumnDesc col)
        {
            return GetRowChecks(row, obj, col).Select(x => x.CellStyle).Where(x => x.hasContent()).concat(" ");
        } 

		/// <summary>
		/// ������ ��� ����� ������� - ��������� ��� �������������
		/// </summary>
        public bool UseFormMatrix { get; set; }
		
		/// <summary>
		/// ��������� ���� ��������
		/// </summary>
		public bool ApplyValueCourse { get; set; }
        /// <summary>
		/// ������ ��� ����� ������� - ��������� ��� �������������
        /// </summary>
        public string MatrixExRows { get; set; }
		/// <summary>
		/// ��� �������������� �������
		/// </summary>
        public string FixedObjectCode { get; set; }
		/// <summary>
		/// ������ ��� ����� ������� - ��������� ��� �������������
		/// </summary>
        public string MatrixExSqlHint { get; set; }
    	private bool _ignorePeriodState;
		/// <summary>
		/// ������� ������ ������� �������
		/// </summary>
    	public bool IgnorePeriodState {
    		get {
    			if(myapp.roles.IsInRole(myapp.usr,"IGNOREPERIODSTATE",false)) return true;
				if(null != this.Thema) {
					var y = this.Thema.GetParameter("firstyear").toInt();
                    if (y>0 && y > Year) return true;
                }
				return _ignorePeriodState;
    		}
    		set { _ignorePeriodState = value; }
    	}

    	/// <summary>
    	/// �������� ����������� �������
    	/// </summary>
    	public bool IsObjectDependent { get; set; }
		/// <summary>
		/// �������� ����� (�����������)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    	public IForm GetForm(IZetaMainObject obj) {
            var theform =
                     myapp.storage.Get<IForm>().First(
                         "from ENTITY x where x.Template = ? and x.Object.Id=? and x.Year =? and x.Period = ?",
                         UnderwriteCode ??  this.Code, obj.Id, this.Year, this.Period
                         );
            return theform;
        }
        /// <summary>
        /// ������ �� ��������� ��������
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

        /// <summary>
        /// ���� ��� ����� �����
        /// </summary>
        public IList<InputTarget> Targets {
            get { return targets; }
        }

        private IStateManager StateManager { get; set; }
        /// <summary>
        /// ��� SQL - ������!
        /// </summary>
        public IDictionary<string, decimal> SqlCache { get; set; }

		/// <summary>
		/// ���� ��� ����������� - ������!!
		/// </summary>
        public string Area { get; set; }
		/// <summary>
		/// ������������ ����������
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
		/// ������� ������ �� ����� ����������
		/// </summary>
        public IDictionary<string, int> RedirectPeriodMap {
            get {
                if (null == _redirectPeriodMap) {
                    if (PeriodRedirect.hasContent()) {
                        _redirectPeriodMap = new Dictionary<string, int>();
                        IList<string> variants = PeriodRedirect.split(false, true, '|');
                        foreach (string variant in variants) {
                            IList<string> test = variant.split(false, true, ':');
                            string v = "";
                            string rules_ = "";
                            if (test.Count == 1) {
                                v = "";
                                rules_ = test[0];
                            }
                            else {
                                v = test[0];
                                rules_ = test[1];
                            }
                            IList<string> rules = rules_.split();
                            foreach (string rule in rules) {
                                IList<string> record_ = rule.split(false, true, '=');
                                var record = new {from = record_[0].toInt(), to = record_[1].toInt()};
                                _redirectPeriodMap[record.from + "_" + v] = record.to;
                            }
                        }
                    }
                }
                return _redirectPeriodMap;
            }
        }

        #region ICloneable Members

        object ICloneable.Clone() {
            return Clone();
        }

        #endregion

        #region IInputTemplate Members
		/// <summary>
		/// ����� - ��������� - ����������
		/// </summary>
        public IList<IInputTemplate> Sources {
            get { return sources; }
        }

        /// <summary>
        /// ������ ������������
        /// </summary>
        public string DocumentRoot { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        public IDictionary<string, string> Documents {
            get { return _documents; }
            set { _documents = value; }
        }

		/// <summary>
		/// �������� ������ �� ����
		/// </summary>
		public IThema Thema { get; set; }

		/// <summary>
		/// ������ ����� �� ���������
		/// </summary>
		public string DefaultState { get; set; }

		/// <summary>
		/// ������� ������������� ������ ��������� �����
		/// </summary>
		public bool FavoriteRowsOnly { get; set; }

		/// <summary>
		/// �������������� ���������
		/// </summary>
		public string AdvDocs { get; set; }

		/// <summary>
		/// ����� ���������� �����
		/// </summary>
		public string ScheduleClass { get; set; }

		/// <summary>
		/// ���������� ������������ �����
		/// </summary>
		public string NeedFiles { get; set; }

		/// <summary>
		/// ���������� � �������� ������������� ������
		/// </summary>
		public string NeedFilesPeriods { get; set; }

        /// <summary>
        /// ���� �������
        /// </summary>
        public string Role { get; set; }

		/// <summary>
		/// ������� ������ ������� � �������� ���������
		/// </summary>
		public bool ShowMeasureColumn { get; set; }

		/// <summary>
		/// ������� ����, ��� ����� ���������
		/// </summary>
		public bool IsChecked { get; set; }

		/// <summary>
		/// ������� ����������� �������??
		/// </summary>
		public bool DetailFavorite { get; set; }

		/// <summary>
		/// �������� SQL ����������� ������ �����
		/// </summary>
		public string SqlOptimization { get; set; }

        /// <summary>
        /// ������ �� ������������
        /// </summary>
        public InputConfiguration Configuration { get; set; }

		/// <summary>
		/// ������� ���������� �����
		/// </summary>
		public bool IsOpen { get; set; }

		/// <summary>
		/// ���� �� �������
		/// </summary>
		public string UnderwriteRole { get; set; }

		/// <summary>
		/// ����� ������� ����� ��� �������, ����� �����-��
		/// </summary>
		public bool InputForDetail { get; set; }

        /// <summary>
        /// �������� ������������ �������
        /// </summary>
        /// <returns></returns>
        public bool GetIsPeriodMatched() {
            if (ForPeriods == null || ForPeriods.Length == 0) {
                return true;
            }
            return ForPeriods.Contains(Period);
        }

		/// <summary>
		/// ������� ����� ��� ����� ������
		/// </summary>
		public bool IsForSingleDetail {
            get { return IsForDetail && !DetailSplit; }
        }

		/// <summary>
		/// ��� ������� �� ������� (�����)
		/// </summary>
		public string DetailFilterName { get; set; }

		/// <summary>
		/// ������ �� �������
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
		/// ������������� ������ ����� 
		/// </summary>
		public IList<string> FixedRowCodes { get; set; }

        /// <summary>
        /// ���� ������� - ���� ������������ �� ���-�� JSON - �������������
        /// </summary>
        public string TableView { get; set; }

		/// <summary>
		/// ������� ���������� �� ������
		/// </summary>
		public string ForGroup { get; set; }

		/// <summary>
		/// ������ �� ��������
		/// </summary>
		public int[] ForPeriods { get; set; }

		/// <summary>
		/// ���������
		/// </summary>
		public IDictionary<string, string> Parameters {
            get { return parameters; }
            set { parameters = value; }
        }

		/// <summary>
		/// ������ ������
		/// </summary>
		public string Help { get; set; }

		/// <summary>
		///������������ ��������������
		/// </summary>
		public string AutoFillDescription { get; set; }


		
	//	public IDictionary<string, InputQuery> Queries {
    //        get { return queries; }
    //    }

        //private string state_ = null;
		/// <summary>
		///   Gets the state (��������� ������� ������ ������� ����� �� ����������
		/// </summary>
		/// <param name = "obj">The obj.</param>
		/// <param name="detail"> </param>
		/// <returns></returns>
		public string GetState(IZetaMainObject obj, IZetaDetailObject detail) {
            return GetState(obj, detail, null);
        }

		/// <summary>
		///   Gets the state (��������� ������� ������ ������� ����� �� ����������
		/// </summary>
		/// <param name = "obj">The obj.</param>
		/// <param name="detail"> </param>
		/// <param name="statecache"> </param>
		/// <returns></returns>
		public string GetState(IZetaMainObject obj, IZetaDetailObject detail,IDictionary<string ,object > statecache) {
			if (!IsActualOnYear) return "0ISBLOCK";
            if (null == cachedState) {
                cachedState = internalGetState(obj, detail, statecache);
            }
            return cachedState;
        }

		/// <summary>
		/// ������� ����� ��� �������
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
		/// ������� ����� ��� �������
		/// </summary>
		public bool IsInputForDetail {
            get {
                if (IsForDetail) {
                    return true;
                }
                return InputForDetail;
            }
        }

        /// <summary>
        /// ������ ����� �������� ���������
        /// </summary>
        /// <returns></returns>
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
	    ///����������� ������ ������� �������
	    /// </summary>
	    public Task CanSetTask;
		/// <summary>
		/// ����������� �������� �������
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public Task StartCanSetAsync(IZetaMainObject obj) {
			if(CanSetTask!=null) return CanSetTask;
			return CanSetTask = Task.Run(() =>
				{
					DirectCanSetState(obj, null, "0ISOPEN");
					DirectCanSetState(obj, null, "0ISBLOCK");
					DirectCanSetState(obj, null, "0ISCHECKED");
				});
		}
        IDictionary<string,string > _cansetstateCache = new Dictionary<string, string>();
        /// <summary>
        /// �������� ����������� ���������� ������
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="detail"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string CanSetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
	        if(CanSetTask!=null) {
				CanSetTask.Wait();
			}
	        return DirectCanSetState(obj, detail, state);
        }

	    private string DirectCanSetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
		    obj = FixedObject ?? obj;
		    return _cansetstateCache.get(state, () =>
			    {
				    string result = "";
				    StateManager.CanSet(this, obj, detail, state, out result);
				    return result;
			    });
	    }

	    /// <summary>
	    ///������������ ���� SQL - �����-�� �����
	    /// </summary>
	    /// <param name="obj"></param>
	    /// <param name="year"></param>
	    /// <param name="period"></param>
	    /// <returns></returns>
	    public IDictionary<string, object> ReloadSqlCache(IZetaMainObject obj, int year, int period) {
            SqlCache = new Dictionary<string, decimal>();
            IDictionary<string, object> result =
                myapp.ioc.getConnection().ExecuteDictionaryReader(
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
        /// �������� ������ ������
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnDesc> GetAllColumns() {
            if (_cachedcolumns == null) {
                _cachedcolumns = getAllColumns().ToList();
            }
            return _cachedcolumns;
        }

        /// <summary>
        /// �������� ���������� �������
        /// </summary>
        /// <returns></returns>
        public bool IsPeriodOpen() {
			if(IgnorePeriodState) return true;
            return StateManager.GetPeriodState(Year, Period) == 1;
        }

        /// <summary>
        /// ���������� ������
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="detail"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state) {
            return SetState(obj, detail, state, false,0);
        }

    	/// <summary>
    	/// �������� ������������ ��� ����
    	/// </summary>
    	public bool IsActualOnYear {
    		get {
				if(null==this.Thema) return true;
    			return Year >= Thema.Parameters.get("beginActualYear", 1900) &&
    			       Year <= Thema.Parameters.get("endActualYear", 3000);
    		}
    	}

    	private bool _useBizTranMatrix;
    	/// <summary>
    	/// ������� ������������� ��������
    	/// </summary>
    	public bool UseBizTranMatrix {
    		get { return _useBizTranMatrix || this.TableView=="biztranform"; }
    		set { _useBizTranMatrix = value; }
    	}

    	/// <summary>
    	/// ��������� �������
    	/// </summary>
    	/// <param name="obj"></param>
    	/// <param name="detail"></param>
    	/// <param name="state"></param>
    	/// <param name="skipcheck"></param>
    	/// <param name="parent"></param>
    	/// <returns></returns>
    	/// <exception cref="Exception"></exception>
    	public int SetState(IZetaMainObject obj, IZetaDetailObject detail, string state, bool skipcheck = false, int parent = 0) {
            if(GetState(obj,detail)==state) return parent;
            obj = FixedObject ?? obj;
            cachedState = null;
            string m = "";
            bool yes = true;

			if(!IsActualOnYear) throw new Exception("try to set state for not actual year for this form (thema)");

            if(!skipcheck) yes = StateManager.CanSet(this, obj, null, state, out m, parent);

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
                LockOperation op = LockOperation.None;
                if (state == "0ISBLOCK") {
                    op = LockOperation.Block;
                }
                else if (state == "0ISOPEN") {
                    op = LockOperation.Open;
                    
                }
                toperiods = periodmapper.GetLockingPeriods(op, Period);
            }
            int master = processStateRow(detail, obj, Period, state, m ?? "", 0);
            if(0!=parent) {
                master = parent;
            }
            foreach (int toperiod in toperiods) {
                if (state == "0ISBLOCK") {
                    IInputTemplate t = Clone();
                    t.Period = toperiod;
                    string currentstate = t.GetState(obj, detail);
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
            //� �������� �������� ����� ����������� �������� ���������� ������
            if (state == "0ISCHECKED") {
                int objid = obj == null ? detail.Object.Id : obj.Id();
                int detailid = detail == null ? 0 : detail.Id;
                Evaluator.DefaultCache.Clear(@"(?i)org\(" + objid + @"\)");
                if (detailid != 0) {
                    Evaluator.DefaultCache.Clear(@"(?i)subpart\(" + detailid + @"\)");
                }
                Evaluator.InvokeChanged();
            }

            cachedState = state;
            return master;
        }


        /// <summary>
        /// �������� �������
        /// </summary>
        public void RefreshState() {
            stateCache.Clear();
        }

		/// <summary>
		/// ��� �������
		/// </summary>
		public string UnderwriteCode { get; set; }

		/// <summary>
		/// ��� ������ ����������
		/// </summary>
		public string SaveMethod { get; set; }

		/// <summary>
		/// ����������� ���������� ������ (����� ��������)
		/// </summary>
		public string Script { get; set; }

		/// <summary>
		/// ��� ���������� ������
		/// </summary>
		public string BindedReport { get; set; }

		/// <summary>
		/// ��������� �� �������
		/// </summary>
		public bool DetailSplit { get; set; }

		/// <summary>
		/// ��� �������
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// ������ �������
		/// </summary>
		public int Period { get; set; }

		/// <summary>
		/// ������ ���� �������
		/// </summary>
		public DateTime DirectDate { get; set; }

		/// <summary>
		/// �������� ������
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
		/// ������ �����
		/// </summary>
		public IList<RowDescriptor> Rows {
            get { return _rows; }
        }

		/// <summary>
		/// ������
		/// </summary>
		public IList<ColumnDesc> Values { get; set; }

        /// <summary>
        /// �������� ������������ ������ ��������
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool IsConditionMatch(string condition){
            if(condition.noContent()) return true;
            if(!this.Parameters.get("activecondition",()=>false)) return true;
            var conds_ = this.Parameters.get("conditions", "");
            if (conds_.noContent()) return false;
            var conds = conds_.split();
            if(condition.Contains(",") || condition.Contains("|")){
                var condsets = condition.split(false, true, '|');
                foreach (var condset in condsets)
                {
                    var match = conds.containsAll(condset.split().ToArray());
                    if (match)
                    {
                        return true;
                    }
                }
                return false;    
            }
            var e = PythonPool.Get();
            var cond = condition;
            try{
                cond = condition.replace(@"[\w\d_]+", m =>{
                                                               if(m.Value.isIn("or","and","not")){
                                                                   return m.Value;
                                                               }
                                                               var c = m.Value;
                                                               if (conds.Contains(c)){
                                                                   return " True ";
                                                               }
                                                               return " False ";
                                                           }).Trim();
                var result = e.Execute<bool>(cond);
                return result;
            }
                catch(Exception){
                    throw new Exception("������ � "+cond);
                }
            finally{
                PythonPool.Release(e);
            }
            
            
        }
        

        /// <summary>
        /// ����������� ����� � �������
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="directDate"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IInputTemplate PrepareForPeriod(int year, int period, DateTime directDate, IZetaMainObject obj) {
            log.debug(() => "start prepare for period " + year + " " + period);
            int accomodatedPeriod = AccomodatePeriod(period, obj);
            IInputTemplate result = Clone();
            result.Year = year;
            result.Period = accomodatedPeriod;
            result.DirectDate = directDate;
            foreach (ColumnDesc value in result.GetAllColumns()) {
                value.ApplyPeriod(year, accomodatedPeriod, directDate);
            }
            foreach (var parameter in Parameters) {
                result.Parameters[parameter.Key] = parameter.Value;
            }

            return result;
        }


		/// <summary>
		/// ��������� ��� ��� MVC
		/// </summary>
		public string Controller { get; set; }

		/// <summary>
		/// ������� ������������� �������������� ����
		/// </summary>
		public bool IsCustomView {
            get { return CustomView.hasContent(); }
        }

		/// <summary>
		/// ��������������� ��������
		/// </summary>
		public string PeriodRedirect { get; set; }


		/// <summary>
		/// ������ �� �������� ������������ 
		/// </summary>
		public XPathNavigator SourceXmlConfiguration { get; set; }

		/// <summary>
		/// ������������� ���������������� ���
		/// </summary>
		public string CustomView { get; set; }

        /// <summary>
        /// �������� ������ �� ����������
        /// </summary>
        /// <returns></returns>
        public ScheduleState GetScheduleState() {
            if (this.IsOpen) {
                var periodstate = new PeriodStateManager {System = "Default"}.Get(this.Year, this.Period);
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
            return new ScheduleState { Date = DateExtensions.Begin, Overtime = ScheduleOvertime.None };
        }


		/// <summary>
		/// ������������� ��� �����������
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
		/// ��� �����
		/// </summary>
        public string Code { get; set; }
		/// <summary>
		/// �������� �����
		/// </summary>
        public string Name { get; set; }


		/// <summary>
		/// �������� ����������
		/// </summary>
		public int ScheduleDelta { get; set; }

	
		// IList<InputField> Fields {
       //     get { return fields; }
       // }

        /// <summary>
        /// ��������� �����
        /// </summary>
        /// <returns></returns>
        public IInputTemplate Clone() {
            var result = new InputTemplate {ForPeriods = ForPeriods.Select(x => x).ToArray()};

            result.bindfrom(this, true);

        	result.Thema = this.Thema;

         //  foreach (var pair in Queries) {
          //      result.Queries[pair.Key] = pair.Value;
       //    }
        //    foreach (InputField field in Fields) {
       //         result.Fields.Add(field);
         //   }
          foreach (RowDescriptor row in Rows) {
                result.Rows.Add(row);
            }
            foreach (string code in FixedRowCodes) {
                result.FixedRowCodes.Add(code);
            }
            foreach (ColumnDesc value in Values) {
                var val = value.Clone();
                val.ConditionMatcher = this;
                result.Values.Add(val);
            }
            foreach (var parameter in parameters) {
	            var key = parameter.Key ?? "NULL";
	            result.Parameters[key] = parameter.Value;
            }

            foreach (IInputTemplate source in Sources) {
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
        /// �������� ������ ��������������
        /// </summary>
        /// <returns></returns>
        public AutoFill GetAutoFill() {
            return new AutoFill(this);
        }

        /// <summary>
        /// ���������� ������������ ��������������
        /// </summary>
        /// <returns></returns>
        public bool IsAutoFill() {
            AutoFill af = GetAutoFill();
            return af.IsExecutable;
        }
		/// <summary>
		/// ���������� ������������ ��������������
		/// </summary>
		/// <returns></returns>

        public bool IsAutoFill(IZetaMainObject obj) {
            if (!IsAutoFill()) {
                return false;
            }
            return GetState(obj, null) == "0ISOPEN" || myapp.roles.IsInRole(myapp.usr,"NOBLOCK",false);
        }

		/// <summary>
		/// ������������� ������� ����������
		/// </summary>
		public string CustomSave { get; set; }

        #endregion

        private string internalGetState(IZetaMainObject obj, IZetaDetailObject detail, IDictionary<string, object> statecache) {
            obj = FixedObject ?? obj;
            if(this.UnderwriteCode.noContent()) {
                return "0ISOPEN";
            }
            if(statecache!=null) {
                return statecache.get(this.UnderwriteCode + "_" + this.Period.ToString(), "0ISOPEN");
            }
            foreach (IStateCheckInterceptor interceptor in StateInterceptors) {
                string res = interceptor.GetState(this, obj, detail);
                if (res.hasContent()) {
                    return res;
                }
            }
            string dstate = DefaultState;
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
            string result = StateManager.DoGet(obj.Id, Year, Period, UnderwriteCode);
#endif
            //state_ = result;
            stateCache[obj.Id] = result;
            return result;
        }

        private IEnumerable<ColumnDesc> getAllColumns() {
            foreach (IInputTemplate r in Sources) {
                if (null != r) {
                    foreach (ColumnDesc c in r.Values) {
                        yield return c.Clone();
                    }
                }//TODO check errors
            }
            foreach (ColumnDesc c in Values) {
                yield return c;
            }
        }

        /// <summary>
        /// �������� DRS ������ ������� - ����� �����!!
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public DataRowSet GetStateDrs(IZetaMainObject obj, IZetaDetailObject detail) {
            DataRowSet drs = new DataRowSet()
                .Row
                .SetCode(UnderwriteCode)
                .Column
                .SetCode("0CONSTSTR")
                .Periods
                .Set(PeriodDefinition.Choose(Year, Period, DirectDate));

            if (null == detail) {
                drs.Object.SetId(obj.Id);
            }
            else {
                if (IsForDetail && !DetailSplit) {
                    drs.DetailObject.SetId(detail.Id);
                }
                else {
                    drs.Object.SetId((detail).Object.Id);
                }
            }
            return drs;
        }

        private int processStateRow(IZetaDetailObject detail, IZetaMainObject obj, int period, string state,
                                    string comment, int parent) {
            int result = StateManager.DoSet(obj.Id, Year, period, UnderwriteCode, this.Code, myapp.usrName, state, comment, parent);
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
            IDbConnection conn = myapp.ioc.getSession().Connection;
            var parameters = new Dictionary<string, object>();

            parameters["row"] = UnderwriteCode;
            parameters["obj"] = obj.Id();
            parameters["year"] = Year;
            parameters["period"] = period;
            parameters["newstate"] = state;
	        parameters["template"] = this.Code;
            if (Rows.Count > 0) {
                parameters["trow"] = Rows[0].Code;
            }
            else {
                parameters["trow"] = "none";
            }
            IDbCommand command =
                conn.CreateCommand(
                    "exec usm.state_after_change @row=@row,@obj=@obj,@year=@year,@period=@period, @newstate=@newstate,@trow=@trow, @template = @template",
                    parameters);
            myapp.ioc.getSession().Transaction.Enlist(command);
            command.ExecuteNonQuery();
			if(state=="0ISCHECKED") {
				Evaluator.DefaultCache.Clear();	
			}
			
			
#if OLDSTATES
                s.Commit();
            }
#endif

            return result;
        }

        private void AssumeExistsState(string code) {
            StorageWrapper<IZetaRow> s = myapp.storage.Get<IZetaRow>();
            IZetaRow row = s.Load(code);
            if (null == row) {
                IZetaRow parentrow = s.Load("0STATE");
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

        private IZetaMainObject _fixedobj;
		/// <summary>
		/// ������������� ������
		/// </summary>
        public IZetaMainObject FixedObject {
            get {
                if(_fixedobj==null && FixedObjectCode.hasContent()) {
                    _fixedobj = myapp.storage.Get<IZetaMainObject>().Load(FixedObjectCode);
                }
                return _fixedobj;
            }
        }

        /// <summary>
        /// �������� ������������� ������������ �������
        /// </summary>
        public bool NeedPreloadScript { get; set; }

        /// <summary>
        /// ��������� ���������� ������
        /// </summary>
        /// <param name="period"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
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
            int newp = RedirectPeriodMap.get("0_" + objperiodtype,()=> period,false,period);
            
            newp = RedirectPeriodMap.get(period + "_" + objperiodtype,()=> newp, false, newp);
            return newp;
        }
		/// <summary>
		/// �������� ���� ��������� ��������� ������� - ����� ����� �����!!!
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="detail"></param>
		/// <returns></returns>
        public DateTime GetLastStateUpdateTime(IZetaMainObject obj, IZetaDetailObject detail) {
            DataRowSet drs = new DataRowSet()
                .Row
                .SetCode(UnderwriteCode)
                .Column
                .SetCode("0CONSTSTR")
                .Periods
                .Set(PeriodDefinition.Choose(Year, Period, DirectDate));
            return drs.GetVersion();
        }

        /// <summary>
        ///   Determines whether the specified obj is match.
        /// </summary>
        /// <param name = "obj">The obj.</param>
        /// <returns>
        ///   <c>true</c> if the specified obj is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(IZetaMainObject obj) {
        	return GroupFilterHelper.IsMatch(obj, ForGroup);
        }

        private IEnumerable<IZetaCell> BindCells(NameValueCollection _parameters) {
            foreach (InputTarget target in targets) {
                IZetaCell result = target.Bind(_parameters);
                if (null != result) {
                    yield return result;
                }
            }
        }
    }
}