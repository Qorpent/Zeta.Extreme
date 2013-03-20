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
// PROJECT ORIGIN: Zeta.Extreme.Form.Tests/BenchMarkTools.cs
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using NUnit.Framework;

namespace Zeta.Extreme.Form.Tests.ZC371TestBench {
	[TestFixture(Description = "тесты - инструменты")]
	public class BenchMarkTools {
		/// <summary>
		/// моя стандартная директория для тем АССОИ (sfo)
		/// </summary>
		public const string ROOTDIR = @"c:\apps\eco\tmp\compiled_themas";
		[Explicit]
		[Test(Description = @"Это на деле не тест! Это именно туловина для подготовки C# для тестирования кондици
		грузит из тем все уникальные кондиции и оборачивает их ")]
		public void SetupConditionsListTook() {
			if(!CheckRootDirectory())return;
			string[] files; if (!GetFileNames(out files)) return;
			var conditions = CollectConditions(files);
			WriteOutConditionBasedCases(conditions);
		}

		private static void WriteOutConditionBasedCases(List<string> conditions) {
			string[] excludes = new[] {"and", "or", "not"};
			foreach (var condition in conditions) {
				var terms = Regex.Matches(condition, @"[\d\w_]+", RegexOptions.Compiled)
					.OfType<Match>()
					.Select(_ => _.Value)
					.Where(_ => -1 == Array.IndexOf(excludes, _))
					.Distinct()
					.ToArray()
					;
				var termlist = string.Join(",",terms);
				Console.WriteLine(@"[TestCase(""" + condition.Replace("\"", @"\""") + @""","""+termlist+@""")]");
			}
		}

		private static List<string> CollectConditions(string[] files) {
			var conditions = new List<string>();
			foreach (var file in files) {
				var xml = XElement.Load(file);
				var elementsWithConditions = xml.XPathSelectElements("//col[@condition]");
				foreach (var elementWithCondition in elementsWithConditions) {
					var condition = elementWithCondition.Attribute("condition").Value;
					if(!condition.Contains(" "))continue; //trivial conditions without any logic
					if (!conditions.Contains(condition)) {
						conditions.Add(condition);
					}
				}
			}
			return conditions;
		}

		private static bool GetFileNames(out string[] files) {
			files = Directory.GetFiles(ROOTDIR, "*.xml");
			if (0 == files.Length) {
				Assert.Ignore("No files with themas in ROOTDIR");
				return false;
			}
			return true;
		}

		private static bool CheckRootDirectory() {
			if (!Directory.Exists(ROOTDIR)) {
				Assert.Ignore("No environment prepared - ROOTDIR needed");
				return false;
			}
			return true;
		}
	}
}