using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Qorpent.Applications;

namespace Zeta.Extreme.Developer.CodeMetrics
{
	/// <summary>
	/// Реализация сборщика метрик по основным свойствам файлов
	/// </summary>
	public class CommonFileDataMetricsProvider: MetricProviderBase {
		/// <summary>
		/// Метод помещения в хаб первичных (приватных метрик)
		/// </summary>
		/// <param name="hub"></param>
		public override void CollectHub(MetricHub hub) {
			CurrentHub = hub;
			CollectCommonFileData();
		}
		/// <summary>
		/// 
		/// </summary>
		protected class CommonFileMetric {
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fileName"></param>
			public CommonFileMetric(string fileName) {
				Name = fileName;
				CollectData();
			}

			private void CollectData() {
				var info = new FileInfo(Name);
				Size = (int)info.Length;
				var lines = File.ReadAllLines(Name);
				var text = File.ReadAllText(Name);
				var xml = Application.Current.Bxl.Parse(text);
				Lines = lines.Length;
				Symbols = text.Length;
				EmptyLines = lines.Count(string.IsNullOrWhiteSpace);
				var _allcomments = lines.Where(_ => _.Trim().StartsWith("#")).ToArray();
				CommentedCodeLines = _allcomments.Count(_ => Regex.IsMatch(_, @"^\#\s*[a-zA-Z_]+"));
				CommentedLines = _allcomments.Length - CommentedCodeLines;
				CodeLines = Lines - EmptyLines -  _allcomments.Length;
				Elements = xml.DescendantsAndSelf().Count();
				Attributes = ((IEnumerable<object>) xml.XPathEvaluate("//@*")).Count();
				Texts = xml.DescendantNodes().OfType<XText>().Count();
			}
			/// <summary>
			/// 
			/// </summary>
			public string Name { get; set; }
			/// <summary>
			/// 
			/// </summary>
			public int Size { get; set; }
			/// <summary>
			/// 
			/// </summary>
			public int Lines { get; set; }
			/// <summary>
			/// 
			/// </summary>
			public int Symbols { get; set; }
			/// <summary>
			/// 
			/// </summary>
			public int EmptyLines { get; set; }
			/// <summary>
			/// 
			/// </summary>
			public int CommentedLines { get; set; }
			/// <summary>
			/// Строк кода
			/// </summary>
			public int CodeLines { get; set; }
			/// <summary>
			/// Закомментированных строк кода
			/// </summary>
			public int CommentedCodeLines { get; set; }
			/// <summary>
			/// Количество элементов
			/// </summary>
			public int Elements { get; set; }
			/// <summary>
			/// Количество атрибутов
			/// </summary>
			public int Attributes { get; set; }
			/// <summary>
			/// Количество текстовых элементов
			/// </summary>
			public int Texts { get; set; }



		}

		private void CollectCommonFileData() {
			var files = CodeIndex.GetAllSources().Select(_ => new CommonFileMetric(_.FileName)).ToArray();
			SetupFileMetrics(files,"");
			SetupFileMetrics(files.Where(_=>_.Name.ToLower().Contains("\\sys\\")).ToArray(),"sys_");
			SetupFileMetrics(files.Where(_ => _.Name.ToLower().Contains("\\usr\\")).ToArray(), "usr_");
		}

	
		private void SetupFileMetrics(CommonFileMetric[] data, string prefix) {
			CurrentHub.Set(prefix+"filecount",data.Count());
			CollectMetric(data, prefix, "size", _=>_.Size);
			CollectMetric(data, prefix, "lines", _=>_.Lines);
			CollectMetric(data, prefix, "symbols", _=>_.Symbols);
			CollectMetric(data, prefix, "emptylines", _=>_.EmptyLines,"lines");
			CollectMetric(data, prefix, "codelines", _ => _.CodeLines, "lines");
			CollectMetric(data, prefix, "commentedlines", _ => _.CommentedLines, "lines");
			CollectMetric(data, prefix, "commentedcodelines", _ => _.CommentedCodeLines, "lines");
			CollectMetric(data, prefix, "elements", _ => _.Elements);
			CollectMetric(data, prefix, "attributes", _ => _.Attributes);
			CollectMetric(data, prefix, "texts", _ => _.Texts);

		}

		/// <summary>
		/// Метод собственно сбора итоговой статистики
		/// </summary>
		/// <param name="options"></param>
		/// <param name="hub"></param>
		/// <returns></returns>
		public override IEnumerable<MetricResult> Collect(MetricCollectOptions options, MetricHub hub) {
			foreach (var area in new[] {"", "sys_", "usr_"}) {
				foreach (var type in new[] {"", "total","perc", "avg", "max", "min"}) {
					foreach (var item in new[] { "filecount", "size", "lines", 
						"symbols", "emptylines", "commentedlines", "codelines", "commentedcodelines" ,
						"elements","attributes","texts"
					})
					{
						var name = area + type + item;
						var value = hub.Get(name, Int32.MinValue);
						if (value != Int32.MinValue) {
							var result = new MetricResult {
								Group = "files",
								Name = area + type + item,
								Value = value,
								SubGroup = area,
								Type = type,
								ItemName = item,
							};
							TuneResult(result, area,type,item);
							yield return result;
						}
					}
				}
			}

			

		}

		private void TuneResult(MetricResult metric, string area, string type, string item) {
			
		}
	}
}
