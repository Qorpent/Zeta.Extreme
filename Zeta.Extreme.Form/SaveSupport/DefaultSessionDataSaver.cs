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
// PROJECT ORIGIN: Zeta.Extreme.Form/DefaultSessionDataSaver.cs
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
insert usm.insertdata(year,period,obj,row,col,decimalvalue,stringvalue,valuta,usr,op)
values ({0},{1},{2},{3},{4},{5},{5},'{6}','{7}','=')
",
			                     cell.linkedcell.query.Time.Year,
			                     cell.linkedcell.query.Time.Period,
			                     cell.linkedcell.query.Obj.Id,
			                     cell.linkedcell.query.Row.Id,
			                     cell.linkedcell.query.Col.Id,
			                     cell.v,
								 cell.linkedcell.query.Obj.ObjRef.Currency,
								 user.Identity.Name
				);
		}
	}
}