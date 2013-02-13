#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : XmlGeneratorBase.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Xml.Linq;
using Comdiv.Extensions;
using Comdiv.Xml;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Абстрактный генератор XML
	/// </summary>
	public abstract class XmlGeneratorBase : IXmlGenerator {
		/// <summary>
		/// 	Внутренний генератор ИД
		/// </summary>
		public static int id;

		/// <summary>
		/// 	Словарь условий
		/// </summary>
		protected IDictionary<string, string> Conditionmap { get; set; }

		/// <summary>
		/// 	Элемент для включения
		/// </summary>
		protected XElement Include { get; set; }

		/// <summary>
		/// 	Фильтрующее условие
		/// </summary>
		public string FilterCondition { get; set; }

		/// <summary>
		/// 	Основной метод генерации XML - контента
		/// </summary>
		/// <param name="call"> </param>
		/// <returns> </returns>
		public object[] Generate(XElement call) {
			Prepare(call);

			return InternalGenerate();
		}

		/// <summary>
		/// 	Возвращает расчитанное значение условия
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		protected virtual string GetCondition(string code) {
			var self = GetSelfCondition(code);

			if (!Conditionmap.ContainsKey(code)) {
				return self;
			}
			var mask = Conditionmap[code];
			if (mask.Contains("$SELF")) {
				return mask.Replace("$SELF", self);
			}
			return mask + "," + self;
		}

		/// <summary>
		/// 	Возвращает условие на себя
		/// </summary>
		/// <param name="code"> </param>
		/// <returns> </returns>
		protected virtual string GetSelfCondition(string code) {
			//stub
			return "";
		}

		/// <summary>
		/// 	Подготовка к генерации
		/// </summary>
		/// <param name="call"> </param>
		protected virtual void Prepare(XElement call) {
			var x = call.Element("filter");
			if (null != x) {
				FilterCondition = x.attr("id", "");
			}
			Include = call.Element("include");
			Conditionmap = new Dictionary<string, string>();
			ProcessIncludes();
			ProcessConditions(call);
		}

		/// <summary>
		/// 	Необходимый при перекрытии метод внутренней генарции
		/// </summary>
		/// <returns> </returns>
		protected abstract object[] InternalGenerate();

		private void ProcessConditions(XElement call) {
			foreach (var element in call.Elements("condition")) {
				var cond = element.attr("id");

				var list = element.Value.split();
				if (element.Value.noContent()) {
					list = GetTargetCodes();
				}

				foreach (var code in list) {
					var existed = Conditionmap.get(code, "");
					if (existed.hasContent()) {
						existed += ",";
					}
					existed += cond;
					Conditionmap[code] = existed;
				}
			}
		}

		/// <summary>
		/// 	Вернуть список целевых кодов
		/// </summary>
		/// <returns> </returns>
		protected virtual IList<string> GetTargetCodes() {
			//NOTE:  что это вообще за метод и зачем
			return new string[] {};
		}

		/// <summary>
		/// 	Применение дополнительных включений
		/// </summary>
		protected virtual void ProcessIncludes() {
			//заготовка
		}
	}
}