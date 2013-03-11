#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithCacheKey.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	Сущность, обладающая уникальной описательной строкой
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