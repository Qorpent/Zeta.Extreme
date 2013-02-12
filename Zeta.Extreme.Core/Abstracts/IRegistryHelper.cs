#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IZexRegistryHelper.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Вспомогательный интерфейс класса для
	/// 	регистрации запроса в сессии
	/// </summary>
	public interface IRegistryHelper {
		/// <summary>
		/// 	Выполняет регистрацию запроса
		/// 	возвращает запрос, в итоге зарегистрированный в системе
		/// </summary>
		/// <param name="query"> исзодный запрос </param>
		/// <param name="uid"> </param>
		/// <returns> итоговый запрос после регистрации </returns>
		Query Register(Query query, string uid);
	}
}