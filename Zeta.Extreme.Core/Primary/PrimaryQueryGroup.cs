#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PrimaryQueryGroup.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	Группа первичных запросов
	/// </summary>
	public class PrimaryQueryGroup {
		/// <summary>
		/// 	Запросы в группе
		/// </summary>
		public IQuery[] Queries { get; set; }

		/// <summary>
		/// 	Прототип первичного запроса
		/// </summary>
		public PrimaryQueryPrototype Prototype { get; set; }

		/// <summary>
		/// 	Генератор скриптов
		/// </summary>
		public IScriptGenerator ScriptGenerator { get; set; }

		/// <summary>
		/// 	Строит SQL скрипт
		/// </summary>
		/// <returns> </returns>
		public string GenerateSqlScript() {
			return ScriptGenerator.Generate(Queries, Prototype);
		}
	}
}