using System;
using System.Data.SqlClient;
using Qorpent.Applications;
using Qorpent.Data;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Developer
{
	/// <summary>
	/// Инициирует загрузку среды Developer
	/// </summary>
	public class DeveloperStartUp : IApplicationStartup
	{
		/// <summary>
		/// User-defined order index
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// 	Executes some startup logic against given application
		/// </summary>
		/// <param name="application"> </param>
		public void Execute(IApplication application) {
			if (!application.DatabaseConnections.Exists("Default")) {
				application.DatabaseConnections.Register(
					new ConnectionDescriptor {
						ConnectionString = "Data Source=192.168.26.137;Initial Catalog=eco;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Application Name=zefs_" + Environment.MachineName,
						Name = "Default",
						ConnectionType = typeof(SqlConnection),
						Evidence= "directstartup"
					},false
					);
			}

			Periods.Get(12);
			RowCache.start();
			ColumnCache.Start();
			ObjCache.Start();

		}
	}
}
