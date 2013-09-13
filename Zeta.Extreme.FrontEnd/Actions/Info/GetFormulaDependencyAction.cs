using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.FrontEnd.Helpers;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// Выводит перечень строк с зависимостями
	/// </summary>
	[Action("zeta.getrowformuladependency")]
	public class GetFormulaDependencyAction:FormServerActionBase {
		private IZetaRow row;

		/// <summary>
		/// Код строки
		/// </summary>
		[Bind]public string Code { get; set; }
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [Bind(Name = "obj")] public int ObjId { get; set; }
        /// <summary>
        /// Описание колсета для дополнитеьных данных
        /// </summary>
        [Bind(Name = "colset")]
        public string ColsetDesc { get; set; }

        /// <summary>
        /// Признак требования на дозагрузку данных в описание зависимостей
        /// </summary>
        public bool NeedData { get { return 0 != ObjId && !string.IsNullOrWhiteSpace(ColsetDesc); } }

        /// <summary>
        /// Колсет на отрисовку дополнительных данных
        /// </summary>
        public ColumnDesc[] Colset { get; set; }

        /// <summary>
        /// Целевой объект
        /// </summary>
        public IZetaMainObject MyObject { get; set; }


		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			allthemas = ((ExtremeFormProvider)MyFormServer.FormProvider).Factory.GetAll().ToDictionary(_=>_.Code);
			refforms.Clear();
			if (0 == rowthemamap.Count) {
				BuildRowThemaMap();
			}
			row = MetaCache.Default.Get<IZetaRow>(Code);
            if (NeedData) {
                MyObject = MetaCache.Default.Get<IZetaMainObject>(ObjId);
                var coldescs = ColsetDesc.Split(';');
                IList<ColumnDesc> cols = new List<ColumnDesc>();
                foreach (var coldesc in coldescs) {
                    var parsedcol = coldesc.SmartSplit(false,true,',');
	                var col = new ColumnDesc();
					foreach (var c in parsedcol) {
						if (c.All(char.IsDigit)) {
							var yp = Convert.ToInt32(c);
							if ((yp >= 1995) && (yp <= 2030)) {
								col.Year = yp;
							}
							else {
								col.Period = yp;
							}
						}
						else {
							col.Code = c;
						}
					}
                    cols.Add(col);
                }
                Colset = cols.ToArray();
                this.DataSession = new Session().AsSerial();
            }
		}
        /// <summary>
        /// Сессия для расчета дополнительных данных
        /// </summary>
	    protected ISerialSession DataSession { get; set; }

	    private void BuildRowThemaMap() {
			var allthemas = ((ExtremeFormProvider) MyFormServer.FormProvider).Factory.GetAll().ToArray();
			foreach (var t in allthemas) {
				if(t.GetParameter("thematype","")!="in")continue;
				
				var forms = t.GetAllForms().ToArray();
				if(0==forms.Length)continue;
				var root = forms[0].Rows.FirstOrDefault();
				if(null==root)continue;
				var code = root.Code;
				if (!rowthemamap.ContainsKey(code)) {
					rowthemamap[code] = new List<string>();
				}
				rowthemamap[code].Add(t.Code);
			}
		}

		IDictionary<string,IList<string>> rowthemamap = new Dictionary<string, IList<string>>();
		private Dictionary<string, IThema> allthemas =null;

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			if (null == row) return null;
			
			return GetRowDependencies(row,false);
		}

		private object GetRowDependencies(IZetaRow r, bool child) {
			if (!(r.IsFormula || null!=r.RefTo || null!=r.ExRefTo)) {
				if (child) return null;
				return "Строка не является формулой";
			}
			IList<Query> rootresults = new List<Query>();
		    if (null != Colset) {
		        foreach (var c in Colset) {
		            var q = new Query(r.Code, c.Code, ObjId, c.Year, c.Period);
		            q = (Query) DataSession.GetUnderlinedSession().Register(q);
		            q.GetResult();
		            rootresults.Add(q);
		        }
		    }
		    if (r.IsFormula) {

				return GetFormulaDependency(r, child,rootresults);
			}

			if (r.RefTo != null) {
				return GetRefDependency(r, child, rootresults);
			}

			if (r.ExRefTo != null)
			{
				return GetExRefDependency(r, child, rootresults);
			}

			return null;
		}
		AuthorizeHelper authhelper = new AuthorizeHelper();
		private IList<object> refforms = new List<object>();
		private object GetExRefDependency(IZetaRow r, bool child, IList<Query> rootresults)
		{
			IList<string> codes = new List<string>();
			codes.Add(r.ExRefTo.Code);
			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			
			
			
			var depinfo = GetDepList(codes, "exref",rootresults);

			if (child)
			{
				return depinfo;
			}
			foreach (var form in forms)
			{
				if (!refforms.Contains(form))
				{
					refforms.Add(form);
				}
			}
			var type = "exref";
			return ExRefDependency(r, child, myForm, forms, type, depinfo);
			
		}

		private object[] GetDepList(IList<string> codes, string type, IList<Query> rootresults) {
			var result = codes.Select(_ => MetaCache.Default.Get<IZetaRow>(_)).Select(_ => ConvertToDependency(_, type)).ToArray();
		    if (NeedData) {
		        foreach (var d in result) {
		            var vals = new List<string>();
					
		            foreach (var q in rootresults) {
			            var res = "";
			            var deps = q.FindDependencyResult(d.code).ToArray();
						if (0 == deps.Length) {
							res = "-";
						}
						else if (1 == deps.Length) {
							res = deps[0].GetResult().NumericResult.ToString("#,0.##");
						}
						else {
							foreach (var dv in deps) {
								res += string.Format("{0}({1},{2})<br/>", dv.GetResult().NumericResult.ToString("#,0.##"), dv.Col.Code, dv.Time.Period);
							}
						}
			            vals.Add(res);
		            }
		            d.values = vals.ToArray();
		        }
		    }
		    return result;
		}

		private object ExRefDependency(IZetaRow r, bool child, string[] myForm, object[] forms, string type, IEnumerable<object> depinfo) {
			return new
				{
					code = r.Code,
					name = r.Name,
					formula = r.Formula,
					formcode = myForm[0],
					form = myForm[1],
					forms = child ? forms : refforms.ToArray(),
					outercode = r.OuterCode,
					type,
					dependency = depinfo,
				};
		}

		private object[] getForms(string[] myForm) {
			return rowthemamap.ContainsKey(myForm[0]) ? rowthemamap[myForm[0]].Select(_=>ConvertToThemaInfo(_)).ToArray() : new object[]{};
		}

		private object ConvertToThemaInfo(string tcode) {
			var t = allthemas[tcode];
			return new {code = t.Code, name = t.Name, allow = authhelper.IsAllowed(t.GetAllForms().First())};
		}

		private object GetRefDependency(IZetaRow r, bool child, IList<Query> rootresults)
		{
			IList<string> codes = new List<string>();
			codes.Add(r.RefTo.Code);
			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			var depinfo = GetDepList(codes, "ref", rootresults);
			if (child)
			{
				return depinfo;
			}
			foreach (var form in forms)
			{
				if (!refforms.Contains(form))
				{
					refforms.Add(form);
				}
			}
			return ExRefDependency(r, child, myForm, forms, "ref", depinfo);
		}

		private object GetFormulaDependency(IZetaRow r, bool child, IList<Query> rootresults) {
			var foreignRowCodes = Regex.Matches(r.Formula, @"\$([\w\d_]+)");
			IList<string> codes = new List<string>();
			foreach (Match codeMatches in foreignRowCodes) {
				var code = codeMatches.Groups[1].Value;
				if (!codes.Contains(code)) {
					codes.Add(code);
				}
			}

			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			var depinfo = GetDepList(codes,"formula", rootresults);

			foreach (var form in forms)
			{
				if (!refforms.Contains(form))
				{
					refforms.Add(form);
				}
			}
			
			if (child) {
				return depinfo;
			}
			return ExRefDependency(r, child, myForm, forms, "formula", depinfo);
		}

		private DependencyDesc ConvertToDependency(IZetaRow r,string type) {
			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			foreach (var form in forms)
			{
				if (!refforms.Contains(form))
				{
					refforms.Add(form);
				}
			}
			return new DependencyDesc
				{
					code = r.Code,
					name = r.Name,
					outercode = r.OuterCode,
					formcode = myForm[0],
					form = myForm[1],
					forms = forms,
					type = type,
					dependency = GetRowDependencies(r,true)
				};
			
		}

		private const string formroot = "/0INPUTROOT/";
		private string[] GetFormName(IZetaRow row) {
            if(null==row)return new[] {"UNKNOWN", "UNKNOWN"};
			if (null!=row.MarkCache && row.MarkCache.Contains(formroot)) return new[]{row.Code, row.Name};
			if (null == row.Parent) return new[] {"UNKNOWN", "UNKNOWN"};
			return GetFormName(row.Parent);
		}
	}
}