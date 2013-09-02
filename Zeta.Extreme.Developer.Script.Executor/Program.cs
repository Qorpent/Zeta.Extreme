using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Qorpent.Applications;
using Qorpent.IO.Resources;

namespace Zeta.Extreme.Developer.Script.Executor
{
	/// <summary>
	/// 
	/// </summary>
	public class Program {
		/// <summary>
		/// Выполняет скрипт ZDEV
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static int Main(string[] args) {
			var rs =Application.Current.Resources.GetString(
				"https://127.0.0.1/zdev/zdev/exportdependencydot.string.qweb?root=a111", 
				new ResourceConfig {
					AcceptAllCeritficates = true,
					Credentials = new NetworkCredential("comdiv","zaq1!QAZ"),
					UseQwebAuthentication = true,
				});
			Console.WriteLine(rs);
			return 0;
		}
	}
}
