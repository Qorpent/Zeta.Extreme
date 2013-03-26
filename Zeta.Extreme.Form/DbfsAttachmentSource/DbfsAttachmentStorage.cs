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
// PROJECT ORIGIN: Zeta.Extreme.Form/DbfsAttachmentStorage.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Qorpent.Applications;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.DbfsAttachmentSource
{
	/// <summary>
	/// Реализует хранилище файлов на базе DBFS
	/// По сути это заглушка для работы исключительно с файлами форм
	/// </summary>
	public class DbfsAttachmentStorage: BufferedAttachmentStorageBase
	{
	
		/// <summary>
		/// Имя строки соединения
		/// </summary>
		public string ConnectionName { get; set; }
		/// <summary>
		/// Имя базы данных
		/// </summary>
		public string DatabaseName { get; set; }

		private string GetConnectionName() {
			if(null==ConnectionName) return "Default";
			return ConnectionName;
		}

		private string GetDatabaseName() {
			if(string.IsNullOrWhiteSpace(DatabaseName) && "Default"==GetConnectionName()) {
				return "dbfs";
			}
			
			return DatabaseName ?? "dbfs";

		}

		IDbConnection OpenConnection() {
			var connection = Application.Current.DatabaseConnections.GetConnection(GetConnectionName());
			connection.Open();
			connection.ChangeDatabase(GetDatabaseName());
			
			return connection;
		}
		
		/// <summary>
		/// Осуществляет поиск аттачментов с указанной маской поиска
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public override IEnumerable<Attachment> Find(Attachment query) {
		

			var script = @"select 
			code, name, comment,version, usr, revision, mime, hash, size,
			 _form as templatecode, _year as year, _period as period, _obj as objid, _doctype as type,extension
			 from [comdiv_dbfs].[usr_view] where 
				(('{4}'='' and _form='{0}' and _year={1} and _period={2} and _obj={3}) or ('{4}'!='' and code='{4}')) and deleted!=1";
			// данный дефолтный поисковик ищет только ПО МЕТАДАННЫМ И ТОЛЬКО ПО ФОРМАМ!!!
			var realattach = new FormAttachment(null, query, AttachedFileType.None, true);
			script = string.Format(script, realattach.TemplateCode, realattach.Year,
			 realattach.Period, realattach.ObjId,realattach.Uid);
			var result = new List<Attachment>();
			using (var c = OpenConnection()) {
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				using (var r = cmd.ExecuteReader()) {
					while (r.Read()) {
						result.Add(ConvertReaderToAttachment(r));	
					}
					
				}
			}
			return result.ToArray();
		}

		private Attachment ConvertReaderToAttachment(IDataReader r) {
			/*
		 * code, name, comment,version, usr, revision, mime, hash, size,
			 _form as templatecode, _year as year, _period as period, _obj as objid, _doctype as type,extension
		 * 
		 */
			// это общее меню полей
			var result = new FormAttachment
				{
					Uid = r.GetString(0),
					Name = r.GetString(1),
					Comment = r.GetString(2),
					Version = r.GetDateTime(3),
					User = r.GetString(4),
					Revision = r.GetInt32(5),
					MimeType = r.GetString(6),
					Hash = r.GetString(7),
					Size = r.GetInt64(8),
					TemplateCode = r.GetString(9),
					Year =Convert.ToInt32(r.GetString(10)),
					Period = Convert.ToInt32(r.GetString(11)),
					ObjId = Convert.ToInt32(r.GetString(12)),
					Type = r.GetString(13),
					Extension = r.GetString(14)
				};
			return result;
		}

		/// <summary>
		/// Сохраняет аттачмент в хранилище
		/// </summary>
		/// <param name="attachment"></param>
		public override void Save(Attachment attachment) {
			var script = @"insert [comdiv_dbfs].[fileoperatonview] (code,name,comment,usr,mime,tag,extension)
			values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
			var code = attachment.Uid;
			if(string.IsNullOrWhiteSpace(code)) {
				code = Guid.NewGuid().ToString();
				attachment.Uid = code;
			}
			var name = attachment.Name;
			if(string.IsNullOrWhiteSpace(name)) {
				name = "noname";
			}
			var comment = attachment.Comment ?? "";
			var usr = attachment.User ?? Application.Current.Principal.CurrentUser.Identity.Name;
			var mime = attachment.MimeType ?? "unknown/bin";
			// производим при создании метаданных замену ключа "template" на принятый в Dbfs "form"
			var tag = TagHelper.ToString(attachment.Metadata.ToDictionary(_ => _.Key =="template"?"form":_.Key, _ => _.Value.ToString()));
			tag = TagHelper.Merge(tag, "/doctype:" + attachment.Type + "/");
			var extension = attachment.Extension;
			script = string.Format(script, code, name, comment, usr, mime, tag,extension);
			using (var c = OpenConnection()) {
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				cmd.ExecuteNonQuery();
			}

		}

		/// <summary>
		/// Выполняет реальную загрузку массива байтов из хранилища
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		protected override byte[] DoRealLoadData(Attachment attachment) {
			var script = string.Format("select bindata from [comdiv_dbfs].[usr_view] where code = '{0}'", attachment.Uid);
			using (var c = OpenConnection()) {
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				using(var r = cmd.ExecuteReader()) {
					if(r.Read()) {
						return (byte[]) r[0];
					}
					return new byte[]{};
				}
			}
		}

		/// <summary>
		/// Удаляет атачмент
		/// </summary>
		/// <param name="attachment"></param>
		public override void Delete(Attachment attachment)
		{
			var script = "insert [comdiv_dbfs].[fileoperatonview] (code,deleted) values (@code, 1)";
			using (var c = OpenConnection())
			{
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				cmd.Parameters.Add(new SqlParameter { DbType = DbType.String, ParameterName = "@code", Value = attachment.Uid });
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Выполняет реальное сохранение потока данных в БД
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="data"></param>
		protected override void PerformDataUpdate(string uid, byte[] data) {
			var script = "insert [comdiv_dbfs].[fileoperatonview] (code,bindata) values (@code, @bindata)";
			using (var c = OpenConnection()) {
				var cmd = c.CreateCommand();
				cmd.CommandText = script;
				cmd.Parameters.Add(new SqlParameter {DbType = DbType.String, ParameterName = "@code", Value = uid});
				cmd.Parameters.Add(new SqlParameter { DbType = DbType.Binary, ParameterName = "@bindata", Value = data });
				cmd.ExecuteNonQuery();
			}
		}
	}
}
