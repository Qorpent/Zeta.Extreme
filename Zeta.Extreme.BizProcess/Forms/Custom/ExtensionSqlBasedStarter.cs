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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/FormAttachment.cs
#endregion
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Qorpent.Applications;

namespace Zeta.Extreme.BizProcess.Forms.Custom {
	/// <summary>
	///     Базовые класс для стартеров библиотек расширений Zeta.Extreme
	/// </summary>
	public abstract class ExtensionSqlBasedStarter : IApplicationStartup {
		/// <summary>
		///     User-defined order index
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		///     Executes some startup logic against given <paramref name="application" />
		/// </summary>
		/// <param name="application"></param>
		public void Execute(IApplication application) {
			var connectionString = PrepareConnectionString();
			ExecuteScripts(connectionString);
		}

		/// <summary>
		///     Подготавливает адресную строку
		/// </summary>
		/// <returns>
		/// </returns>
		protected abstract string PrepareConnectionString();

		/// <summary>
		///     Выполняет SQL запросы относительно заданной в конфигурации строки
		///     подключения
		/// </summary>
		/// <param name="connectionString"></param>
		protected void ExecuteScripts(string connectionString) {
			var myassembly = GetType().Assembly;
			var resourceNames = myassembly.GetManifestResourceNames().Where(_ => _.EndsWith(".sql"));
			using (var c = new SqlConnection(connectionString)) {
				c.Open();
				foreach (var resourceName in resourceNames) {
					using (var sreader = new StreamReader(
						myassembly.GetManifestResourceStream(resourceName), Encoding.UTF8)
						) {
						var commands = sreader.ReadToEnd().Replace(" GO ", "~").Split('~');
						foreach (var command in commands) {
							var cmd = c.CreateCommand();
							cmd.CommandText = command;
							try {
								cmd.ExecuteNonQuery();
							}
							catch (SqlException e) {
								//hide recreate object errors
								if (!e.Message.Contains("There is already")) {
									throw;
								}
							}
						}
					}
				}
			}
		}
	}
}