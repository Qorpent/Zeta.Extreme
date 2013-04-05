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
// PROJECT ORIGIN: Zeta.Extreme.Core/FormulaCompiler.cs
#endregion
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Компилятор формул на CSharp
	/// </summary>
	public class FormulaCompiler {
		/// <summary>
		/// 	Шаблон кодового файла для формул
		/// </summary>
		public const string MainTemplate = @"
using System;
using Zeta.Extreme;
namespace Zeta.Extreme.DyncamicFormulas {
	[Formula(""_KEY_"",""_VERSION_"")]
	public class Formula__CLASS_ : _BASE_ {
		protected override object EvaluateExpression(){
			return  (
				_EXP_
			);
		}
	}
}
	";


		/// <summary>
		/// 	Берет на вход массив формул и компилирует их
		/// 	полученные типы присваиваются формулам
		/// </summary>
		/// <param name="formulaRequests"> </param>
		/// <param name="savepath"> путь для сохранения сборки </param>
		public void Compile(FormulaRequest[] formulaRequests, string savepath = null) {
			if (0 == formulaRequests.Length) {
				return;
			}
			var codefiles = GetCodeFiles(formulaRequests).ToArray();
			var codeprovider = new CSharpCodeProvider();
			CompilerParameters parameters;
			if (string.IsNullOrWhiteSpace(savepath)) {
				parameters = new CompilerParameters
					{
						IncludeDebugInformation = false,
						GenerateInMemory = true,
						TreatWarningsAsErrors = false,
				
						//OutputAssembly = ((DateTime.Now - new DateTime(1979,1,23)).TotalMilliseconds).ToString(),
					};
			}
			else {
				parameters = new CompilerParameters
					{
						IncludeDebugInformation = false,
						GenerateInMemory = false,
						TreatWarningsAsErrors = false,
						OutputAssembly =
							Path.Combine(savepath,
							             DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "-formulas-" + Environment.TickCount + ".dll")
					};
			}
			parameters.ReferencedAssemblies.Add("mscorlib.dll");
			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add("System.Core.dll");
			parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(IWithId)).CodeBase.Replace("file:///", ""));
			parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof (IFormula)).CodeBase.Replace("file:///", ""));
			parameters.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(BackwardCompatibleFormulaBase)).CodeBase.Replace("file:///", ""));

			var result = codeprovider.CompileAssemblyFromSource(parameters, codefiles.ToArray());
			if (result.Errors.Count > 0) {
				var err = false;
				var sb = new StringBuilder();
				foreach (CompilerError error in result.Errors) {
					if (error.IsWarning) {
						continue;
					}
					err = true;
					sb.AppendLine(error.ErrorNumber + " " + error.FileName + " " + error.Line + " " + error.Column + " " +
					              error.ErrorText);
					sb.AppendLine("-------------------");
				}
				var message = sb.ToString();
				message = Regex.Replace(message, @"\.(\d+)\.cs", m =>
					{
						var idx = m.Groups[1].Value.ToInt();
						var fr = formulaRequests[idx].Key + "=" + formulaRequests[idx].Formula + "=" +
						         formulaRequests[idx].PreprocessedFormula;
						return m.Value + "(" + fr + ")";
					});
				if (err) {
					throw new Exception("Formula compilation error:\r\n " + message);
				}
			}

			Assembly assembly = savepath.IsEmpty() ? result.CompiledAssembly : Assembly.Load(File.ReadAllBytes(parameters.OutputAssembly));

			var types = assembly.GetTypes().Where(_ => typeof (IFormula).IsAssignableFrom(_));
			foreach (var t in types) {
				var attr = t.GetCustomAttributes(typeof (FormulaAttribute), true).OfType<FormulaAttribute>().First();
				var key = attr.Key;
				var target = formulaRequests.First(_ => _.Key == key);
				target.PreparedType = t;
			}
		}

		private  IEnumerable<string> GetCodeFiles(IEnumerable<FormulaRequest> formulaRequests) {
			foreach (var fr in formulaRequests) {
				var classname = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
				var basetype = typeof (BackwardCompatibleFormulaBase).FullName;
				if (null != fr.AssertedBaseType) {
					basetype = fr.AssertedBaseType.FullName;
				}
				var key = fr.Key.Replace("\\", "\\\\").Replace("\"", "\\\"");
				var expression = fr.PreprocessedFormula;
				yield return
					MainTemplate
						.Replace("_KEY_", key)
						.Replace("_CLASS_", classname)
						.Replace("_BASE_", basetype)
						.Replace("_EXP_", expression)
						.Replace("_VERSION_", fr.Version);
			}
		}
	}
}