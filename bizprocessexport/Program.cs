using System;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Form.Themas;

namespace bizprocessexport
{
	class Program
	{
		static void Main(string[] args) {
			const string commandTemplate = "exec zetai.refinebizprocessreg '{0}','{1}','{2}','{3}'";
			var factory = new ExtremeFormProvider("c:\\apps\\eco\\tmp\\compiled_themas").Factory;
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

		private static string PrepareCommand(IThema thema, string commandTemplate) {
			var code = thema.Code;
			var name = thema.Name.Replace("'","''");
			var role = thema.Role;
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
					rows = string.Join(",", thema.GetForm("A").Rows.Select(_ => _.Code));
				}
			}
			var cmdtext = string.Format(commandTemplate, code, name, role, rows);
			return cmdtext;
		}


		private static XElement GetProcessesXml() {
			throw new NotFiniteNumberException();
		}
	}
}
