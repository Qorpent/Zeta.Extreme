namespace Zeta.Extreme.Core {
	/// <summary>
	/// Базовый класс для запросов и условий, отвечает за целостность кэш-строки
	/// </summary>
	public abstract class CacheKeyGeneratorBase {
		private string _cacheKey; //cached value of key

		/// <summary>
		/// Возвращает кэш-строку запроса
		/// </summary>
		/// <returns></returns>
		public string GetCacheKey(bool save = true) {
			if(null!=_cacheKey) {
				return _cacheKey;
			}
			if(save) {
				return _cacheKey ?? (_cacheKey = EvalCacheKey());
			}
			return EvalCacheKey();
		}

		/// <summary>
		/// Возвращает строку, которая представляет текущий объект.
		/// </summary>
		/// <returns>
		/// Строка, представляющая текущий объект.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			//должен вернуть кэш строку, но не сохранять ее!!!
			return GetCacheKey(false);
		}

		/// <summary>
		/// Сбрасывает кэш-строку
		/// </summary>
		public void InvalidateCacheKey() {
			_cacheKey = null;
		}
		/// <summary>
		/// Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns></returns>
		protected abstract string EvalCacheKey();
	}
}