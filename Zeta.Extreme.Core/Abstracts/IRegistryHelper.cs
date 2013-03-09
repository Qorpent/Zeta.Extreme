#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IRegistryHelper.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

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
		IQuery Register(IQuery query, string uid);
	}
}