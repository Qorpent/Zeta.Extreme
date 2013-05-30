using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.Themas;
using Zeta.Extreme.FrontEnd.Helpers;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;

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
		}

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

			if (r.IsFormula) {

				return GetFormulaDependency(r, child);
			}

			if (r.RefTo != null) {
				return GetRefDependency(r, child);
			}

			if (r.ExRefTo != null)
			{
				return GetExRefDependency(r, child);
			}

			return null;
		}
		AuthorizeHelper authhelper = new AuthorizeHelper();
		private IList<object> refforms = new List<object>();
		private object GetExRefDependency(IZetaRow r, bool child)
		{
			IList<string> codes = new List<string>();
			codes.Add(r.ExRefTo.Code);
			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			var depinfo = GetDepList(codes,"exref");
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

		private object[] GetDepList(IList<string> codes,string type) {
			return codes.Select(_ => MetaCache.Default.Get<IZetaRow>(_)).Select(_ => ConvertToDependency(_, type)).ToArray();
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

		private object GetRefDependency(IZetaRow r, bool child)
		{
			IList<string> codes = new List<string>();
			codes.Add(r.RefTo.Code);
			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			var depinfo = GetDepList(codes, "ref");
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

		private object GetFormulaDependency(IZetaRow r, bool child) {
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
			var depinfo = GetDepList(codes,"formula");
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

		private object ConvertToDependency(IZetaRow r,string type) {
			var myForm = GetFormName(r);
			var forms = getForms(myForm);
			foreach (var form in forms)
			{
				if (!refforms.Contains(form))
				{
					refforms.Add(form);
				}
			}
			return new
				{
					code = r.Code,
					name = r.Name,
					outercode = r.OuterCode,
					formcode = myForm[0],
					form = myForm[1],
					forms,
					type,
					dependency = GetRowDependencies(r,true)
				};
			
		}

		private const string formroot = "/0INPUTROOT/";
		private string[] GetFormName(IZetaRow row) {
			if (row.MarkCache.Contains(formroot)) return new[]{row.Code, row.Name};
			if (null == row.Parent) return new[] {"UNKNOWN", "UNKNOWN"};
			return GetFormName(row.Parent);
		}
	}
}