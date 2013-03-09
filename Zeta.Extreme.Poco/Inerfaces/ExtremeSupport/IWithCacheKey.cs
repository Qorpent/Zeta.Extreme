namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// Сущность, обладающая уникальной описательной строкой
	/// </summary>
	public interface IWithCacheKey {
		/// <summary>
		/// 	Возвращает кэш-строку запроса
		/// </summary>
		/// <returns> </returns>
		string GetCacheKey(bool save = true);

		/// <summary>
		/// 	Сбрасывает кэш-строку
		/// </summary>
		void InvalidateCacheKey();
	}
}