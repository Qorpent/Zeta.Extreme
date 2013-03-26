using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.Themas;

namespace bizprocessexport
{
	class Program
	{
		private static IDictionary<string, List<string>> _dep;

		/// <summary>dependency</summary>
		public const string DEPENDENCY = "dependency";

		/// <summary>primaryform</summary>
		public const string PRIMARYFORM = "primaryform";

		/// <summary>c:\apps\eco\tmp\compiled_themas</summary>
		public const string THEMAROOT = @"c:\apps\eco\tmp\compiled_themas";

		static void Main(string[] args) {
			_dep = PrepareThemaDependency();
			const string commandTemplate = "exec zetai.refinebizprocessreg '{0}','{1}','{2}','{3}','{4}'";
			var factory = new ExtremeFormProvider(THEMAROOT).Factory;
			var themas = factory.GetAll().Where(_=>!_.Code.EndsWith("lib"));
			using (var c = new SqlConnection("Data Source=assoibdx;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=bizprocessexport"))
			{
				c.Open();
				foreach (var thema in themas) {
					var cmdtext = PrepareCommand(thema, commandTemplate);
					var cmd = c.CreateCommand();
					cmd.CommandText = cmdtext;
					cmd.ExecuteNonQuery();
					Console.WriteLine(cmdtext);
				}	
			}
			
		}

		private static IDictionary<string,List<string>>  PrepareThemaDependency() {
			var _ = new Dictionary<string, List<string>>();
			var xml = XElement.Load(Directory.GetFiles(THEMAROOT,"ZEXT_*.xml").First());
			foreach (var e in xml.Elements()) {
				if (e.Name.LocalName == DEPENDENCY || e.Name.LocalName == PRIMARYFORM) {
					var src = ToThemaCode(e.Attribute("source").Value);
					var trg = "";
					if (e.Name.LocalName == DEPENDENCY) {
						trg = ToThemaCode(e.Attribute("target").Value);
					}
					else {
						trg = ToThemaCode(e.Attribute("result").Value);
					}
					if (!_.ContainsKey(trg)) {
						_[trg] = new List<string>();
					}
					if (!_[trg].Contains(src)) {
						_[trg].Add(src);
					}
				}
			}
			return _;
		}

		private static string ToThemaCode(string value) {
			var len = value.Length - 4;
			return value.Substring(0, len);
		}


		private static string PrepareCommand(IThema thema, string commandTemplate) {
			var code = thema.Code;
			var name = thema.Name.Replace("'","''");
			var role = thema.Role;
			var dep = GetThemaDependency(thema);
			var roleprefix = thema.GetParameter("roleprefix", "");
			if (!string.IsNullOrWhiteSpace(roleprefix)) {
				if (string.IsNullOrWhiteSpace(role)) {
					role = roleprefix;
				}
				else {
					role = role + " (" + roleprefix + ")";
				}
			}
			var rows = "";
			if (string.IsNullOrWhiteSpace(rows = thema.GetParameter("rootrow", ""))) {
				if (null != thema.GetForm("A")) {
					rows = string.Join(", ", thema.GetForm("A").Rows.Select(_ => _.Code));
				}
			}
			var cmdtext = string.Format(commandTemplate, code, name, role, rows, dep);
			return cmdtext;
		}

		private static object GetThemaDependency(IThema thema) {
			if (_dep.ContainsKey(thema.Code)) {
				return string.Join(", ", _dep[thema.Code]);
			}
			return "";
		}
	}
}
