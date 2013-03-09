#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DefaultSessionDataSaver.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Xml.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.SaveSupport {
	/// <summary>
	/// 	—ервис сохранени€ данных формы по умолчанию
	/// </summary>
	public class DefaultSessionDataSaver : DataSaverBase {
		/// <summary>
		/// 	¬ыполн€ет операции после сохранени€ основных €чеек
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		/// <param name="user"> </param>
		protected override void AfterSave(IFormSession session, XElement savedata, SaveResult result, IPrincipal user) {
			using (var c = GetConnection()) {
				c.Open();
				c.ExecuteNonQuery(
					@"
					exec usm.after_save_trigger 
                            @form=@form,
                            @obj=@obj,
                            @year=@year,
                            @period=@period,
                            @usr=@usr",
					new Dictionary<string, object>
						{
							{"@form", session.Template.Code},
							{"@obj", session.Object.Id},
							{"@year", session.Year},
							{"@period", session.Period},
							{"@usr", Application.Principal.CurrentUser.Identity.Name}
						}, 5000);
				result.AfterSaveCalled = true;
			}
		}

		/// <summary>
		/// 	¬ыполн€ет основное сохранение в хранилище
		/// </summary>
		/// <param name="session"> </param>
		/// <param name="savedata"> </param>
		/// <param name="result"> </param>
		/// <param name="user"> </param>
		protected override void Save(IFormSession session, XElement savedata, SaveResult result, IPrincipal user) {
			var script = "";
			foreach (var cell in result.SaveCells) {
				script += GenerateSaveSql(cell, user);
			}
			result.SaveSqlScript = script;
			using (var c = GetConnection()) {
				c.Open();
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				cmd.ExecuteNonQuery();
			}
		}

		private IDbConnection GetConnection() {
			return Application.DatabaseConnections.GetConnection("Default") ;
		}

		private string GenerateSaveSql(OutCell cell, IPrincipal user) {
			if (cell.linkedcell.c != 0) {
				return string.Format("\r\nupdate cell set decimalvalue= {0}, usr= '{1}' where id= {2}",
				                     cell.v,
				                     user.Identity.Name,
				                     cell.linkedcell.c);
			}
			return string.Format(@"
insert usm.insertdata(year,period,obj,row,col,decimalvalue,stringvalue,usr,op)
values ({0},{1},{2},{3},{4},{5},{5},'{6}','=')
",
			                     cell.linkedcell.query.Time.Year,
			                     cell.linkedcell.query.Time.Period,
			                     cell.linkedcell.query.Obj.Id,
			                     cell.linkedcell.query.Row.Id,
			                     cell.linkedcell.query.Col.Id,
			                     cell.v,
								 user.Identity.Name
				);
		}
	}
}