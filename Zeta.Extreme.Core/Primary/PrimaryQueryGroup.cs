namespace Zeta.Extreme.Primary {
	/// <summary>
	/// Группа первичных запросов
	/// </summary>
	public class PrimaryQueryGroup {
		/// <summary>
		/// Запросы в группе
		/// </summary>
		public Query[] Queries { get; set; }
		/// <summary>
		/// Прототип первичного запроса
		/// </summary>
		public PrimaryQueryPrototype Prototype { get; set; }

		/// <summary>
		/// Генератор скриптов
		/// </summary>
		public IScriptGenerator ScriptGenerator { get; set; }

		/// <summary>
		/// Строит SQL скрипт
		/// </summary>
		/// <returns></returns>
		public string GenerateSqlScript() {
			return ScriptGenerator.Generate(Queries, Prototype);
		}
	}
}