namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// Интерфейс резолюции дополнительных параметров сессии
	/// </summary>
	public interface ISessionPropertySource {
		/// <summary>
		/// Метод получения параметра по имени
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		object Get(string name);
	}
}