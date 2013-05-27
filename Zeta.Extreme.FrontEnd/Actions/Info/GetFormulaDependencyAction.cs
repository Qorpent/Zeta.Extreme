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
		/// </summary>
		/// <exception cref="Exception"></exception>
		protected override void Validate()
		{
			base.Validate();
			if (null == row) {
				throw new Exception("Строка отсутствует в БД");
			}
		}

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return GetRowDependencies(row,false);
		}

		private object GetRowDependencies(IZetaRow r, bool child) {
			if (!r.IsFormula) {
				if (child) return null;
				return "Строка не является формулой";
			}
			var foreignRowCodes = Regex.Matches(r.Formula, @"\$([\w\d_]+)");
			IList<string> codes = new List<string>();
			foreach (Match codeMatches in foreignRowCodes) {
				var code = codeMatches.Groups[1].Value;
				if (!codes.Contains(code)) {
					codes.Add(code);
				}
			}
			var myForm = GetFormName(r);
			var depinfo = codes.Select(_ => MetaCache.Default.Get<IZetaRow>(_)).Select(ConvertToDependency);
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
					dependency = depinfo,
				};
		}

		private object ConvertToDependency(IZetaRow r) {
			return new
				{
					code = r.Code,
					name = r.Name,
					outercode = r.OuterCode,
					form = GetFormName(r),
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