using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Zeta.Extreme.Developer.Scripting {
	/// <summary>
	/// Фабрика команд
	/// </summary>
	public  class ScriptCommandFactory {
		/// <summary>
		/// Мапинг элементов в классы
		/// </summary>
		public static IDictionary<string, Type> KnownScriptMap = new Dictionary<string, Type> {
			{"clean",typeof(CleanCommand)},
			{"generate-form",typeof(GenerateFormCommand)},
			{"generate-dict",typeof(GenerateDictCommand)},
			{"generate-periods",typeof(GeneratePeriods)},
			{"generate-bizprocesses",typeof(GenerateBizProcesses)},
			{"generate-columns",typeof(GenerateColumns)},
			{"generate-objtypes",typeof(GenerateObjTypes)},
			{"generate-objdivs",typeof(GenerateObjDivs)},
			{"generate-themastructure",typeof(GenerateThemaStructure)},
			{"generate-transfer",typeof(GenerateTransferScript)},
		};
		/// <summary>
		/// Фабричный метод парсинга команд из XML
		/// </summary>
		/// <param name="definition"></param>
		/// <returns></returns>
		public static IEnumerable<IScriptCommand> GenerateCommands(XElement definition) {
			foreach (var e in definition.Elements()) {
				if (KnownScriptMap.ContainsKey(e.Name.LocalName)) {
					var command = Activator.CreateInstance(KnownScriptMap[e.Name.LocalName]) as IScriptCommand;
					command.Initialize(e);
					yield return command;
				}
			}
		}
	}
}