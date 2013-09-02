﻿using System;
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
			
			var cls = SetupScriptDefinition(projname);
			var script = SetupScript(cls);

			var cr = SetupCredentials(dict, cah);
			script.Run(cr);
			return 0;
		}

		private static Scripting.Script SetupScript(IBSharpClass cls) {
			IUserLog log = ConsoleLogWriter.CreateLog("main",LogLevel.All,"${Message}");
			var script = new Scripting.Script {Log = log};
			script.Initialize(new BSharpRuntimeClass {Definition = cls.Compiled});
			return script;
		}

		private static IBSharpClass SetupScriptDefinition(string projname) {
			var sources = Directory.GetFiles(Environment.CurrentDirectory, "*.zdev-script", SearchOption.AllDirectories);
			var bxlparser = new BxlParser();
			var compiler = new BSharpCompiler();
			compiler.Initialize(new BSharpConfig {SingleSource = true, UseInterpolation = true});
			var context = compiler.Compile(sources.Select(_ => bxlparser.Parse(null, _)));
			var cls = context.Get(projname);
			if (null == cls) {
				throw new Exception("cannot find project " + projname);
			}
			return cls;
		}

		private static ICredentials SetupCredentials(IDictionary<string, string> dict, ConsoleArgumentHelper cah) {
			string user = "";
			string pass = "";
			if (!dict.ContainsKey("user")) {
				Console.WriteLine();
				Console.Write("Logon: ");
				user = Console.ReadLine();
			}
			else {
				user = dict["user"];
			}
			if (!dict.ContainsKey("password")) {
				pass = cah.ReadLineSafety("Password: ");
			}
			else {
				pass = dict["password"];
			}
			ICredentials cr = null;
			if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass)) {
				cr = new NetworkCredential(user, pass);
			}
			return cr;
		}
	}
}