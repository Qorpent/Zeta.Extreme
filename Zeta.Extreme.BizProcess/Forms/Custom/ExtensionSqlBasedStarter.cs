using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Qorpent.Applications;

namespace Zeta.Extreme.BizProcess.Forms.Custom {
	/// <summary>
	/// Базовые класс для стартеров библиотек расширений Zeta.Extreme
	/// </summary>
	public abstract class ExtensionSqlBasedStarter : IApplicationStartup {
		/// <summary>
		/// User-defined order index
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// 	Executes some startup logic against given application
		/// </summary>
		/// <param name="application"> </param>
		public void Execute(IApplication application) {
			var connectionString = PrepareConnectionString();
			ExecuteScripts(connectionString);
		}

		/// <summary>
		/// Подготавливает адресную строку
		/// </summary>
		/// <returns></returns>
		protected abstract string PrepareConnectionString();

		/// <summary>
		/// Выполняет SQL запросы относительно заданной в конфигурации строки подключения
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
						var commands = sreader.ReadToEnd().Replace(" GO ","~").Split('~');
						foreach (var command in commands) {
							var cmd = c.CreateCommand();
							cmd.CommandText = command;
							cmd.ExecuteNonQuery();	
						}
						
					}
				}
			}
		}
	}
}