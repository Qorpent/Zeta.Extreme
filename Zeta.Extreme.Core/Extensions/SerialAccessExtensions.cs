#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SerialAccessExtensions.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Расширения для последовательного API сессии
	/// </summary>
	public static class SerialAccessExtensions {
		/// <summary>
		/// 	Создает объект API последовательного синхронного доступа
		/// </summary>
		/// <param name="session"> </param>
		/// <returns> </returns>
		public static ISerialSession AsSerial(this ISession session) {
			return new SerialSession(session);
		}
	}
}