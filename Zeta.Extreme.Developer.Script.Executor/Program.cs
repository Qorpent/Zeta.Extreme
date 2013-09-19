using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Qorpent.BSharp;
using Qorpent.BSharp.Runtime;
using Qorpent.Bxl;
using Qorpent.Log;
using Qorpent.Utils;

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
			var cah = new ConsoleArgumentHelper();
			var dict =cah.ParseDictionary(args);
			var projname = dict["arg1"];
			
			var cls = SetupScriptDefinition(projname,dict);
			var script = SetupScript(cls);

			
			script.Run(()=>SetupCredentials(dict, cah));
			return 0;
		}

		private static Scripting.Script SetupScript(IBSharpClass cls) {
			IUserLog log = ConsoleLogWriter.CreateLog("main",LogLevel.All,"${Message}");
			var script = new Scripting.Script {Log = log};
			script.Initialize(new BSharpRuntimeClass {Definition = cls.Compiled});
			return script;
		}

		private static IBSharpClass SetupScriptDefinition(string projname, IEnumerable<KeyValuePair<string, string>> dict) {
			var sources = Directory.GetFiles(Environment.CurrentDirectory, "*.zdev-script", SearchOption.AllDirectories);
			var bxlparser = new BxlParser();
			var compiler = new BSharpCompiler();
		    var config = new BSharpConfig {SingleSource = true, UseInterpolation = true};
            foreach (var a in dict) {
                if(null==config.Conditions)config.Conditions = new Dictionary<string, string>();
                config.Conditions[a.Key] = a.Value;
            }
			compiler.Initialize(config);
			var context = compiler.Compile(sources.Select(_ => bxlparser.Parse(null, _)));
			var cls = context.Get(projname);
			if (null == cls) {
				throw new Exception("cannot find project " + projname);
			}
			return cls;
		}

	    private static ICredentials _logoncredentials;
		private static ICredentials SetupCredentials(IDictionary<string, string> dict, ConsoleArgumentHelper cah) {
		    if (null == _logoncredentials) {
		        string user;
		        string pass;
		        if (!dict.ContainsKey("user")) {
		            Console.WriteLine();
		            Console.Write("Logon: ");
		            user = Console.ReadLine();
		        }
		        else {
		            user = dict["user"];
		        }
		        pass = !dict.ContainsKey("password") ? cah.ReadLineSafety("Password: ") : dict["password"];
		        ICredentials cr = null;
		        if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass)) {
		            cr = new NetworkCredential(user, pass);
		        }
		        _logoncredentials = cr;
		    }
		    return _logoncredentials;
		}
	}
}
