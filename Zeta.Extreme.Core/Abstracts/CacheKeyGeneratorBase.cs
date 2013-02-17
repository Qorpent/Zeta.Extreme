#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CacheKeyGeneratorBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовый класс для запросов и условий, отвечает за целостность кэш-строки
	/// </summary>
	public abstract class CacheKeyGeneratorBase {
		/// <summary>
		/// 	Возвращает кэш-строку запроса
		/// </summary>
		/// <returns> </returns>
		public string GetCacheKey(bool save = true) {
			if (null != _cacheKey) {
				return _cacheKey;
			}
			if (save) {
				return _cacheKey ?? (_cacheKey = EvalCacheKey());
			}
			return EvalCacheKey();
		}

		/// <summary>
		/// 	Возвращает строку, которая представляет текущий объект.
		/// </summary>
		/// <returns> Строка, представляющая текущий объект. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			//должен вернуть кэш строку, но не сохранять ее!!!
			return GetCacheKey(false);
		}

		/// <summary>
		/// 	Сбрасывает кэш-строку
		/// </summary>
		public virtual void InvalidateCacheKey() {
			_cacheKey = null;
		}

		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected abstract string EvalCacheKey();

		private string _cacheKey; //cached value of key
	}
}