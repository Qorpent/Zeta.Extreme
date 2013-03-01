namespace Zeta.Extreme.Primary {
	/// <summary>
	/// Интерфейс генератора скриптов
	/// </summary>
	public interface IScriptGenerator {
		/// <summary>
		/// Строит SQL запрос с учетом прототипа, а по сути "хинтов" запроса
		/// </summary>
		/// <param name="queries"></param>
		/// <param name="prototype"></param>
		/// <returns></returns>
		string Generate(Query[] queries, PrimaryQueryPrototype prototype);
	}
}