using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
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
			row = MetaCache.Default.Get<IZetaRow>(Code);
		}

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

		private object GetExRefDependency(IZetaRow r, bool child)
		{
			IList<string> codes = new List<string>();
			codes.Add(r.ExRefTo.Code);
			var myForm = GetFormName(r);
			var depinfo = codes.Select(_ => MetaCache.Default.Get<IZetaRow>(_)).Select(_ => ConvertToDependency(_, "exref"));
			if (child)
			{
				return depinfo;
			}
			return new
			{
				code = r.Code,
				name = r.Name,
				formula = r.Formula,
				form = myForm,
				outercode = r.OuterCode,
				type = "exref",
				dependency = depinfo,
				
			};
		}

		private object GetRefDependency(IZetaRow r, bool child)
		{
			IList<string> codes = new List<string>();
			codes.Add(r.RefTo.Code);
			var myForm = GetFormName(r);
			var depinfo = codes.Select(_ => MetaCache.Default.Get<IZetaRow>(_)).Select(_ => ConvertToDependency(_, "ref"));
			if (child)
			{
				return depinfo;
			}
			return new
			{
				code = r.Code,
				name = r.Name,
				formula = r.Formula,
				form = myForm,
				outercode = r.OuterCode,
				type = "ref",
				dependency = depinfo,
				
			};
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
			var depinfo = codes.Select(_ => MetaCache.Default.Get<IZetaRow>(_)).Select(_=>ConvertToDependency(_,"formula"));
			if (child) {
				return depinfo;
			}
			return new
				{
					code = r.Code,
					name = r.Name,
					formula = r.Formula,
					form = myForm,
					outercode = r.OuterCode,
					type = "formula",
					dependency = depinfo,
					
				};
		}

		private object ConvertToDependency(IZetaRow r,string type) {
			return new
				{
					code = r.Code,
					name = r.Name,
					outercode = r.OuterCode,
					form = GetFormName(r), type,
					dependency = GetRowDependencies(r,true)
				};

		}

		private const string formroot = "/0INPUTROOT/";
		private string GetFormName(IZetaRow row) {
			if (row.MarkCache.Contains(formroot)) return row.Name;
			if (null == row.Parent) return "UNKNOWN";
			return GetFormName(row.Parent);
		}
	}
}