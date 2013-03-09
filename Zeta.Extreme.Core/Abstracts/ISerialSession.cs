#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ISerialSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	ќписывает самый простой, синхронизированный вариант доступа к сессии
	/// 	все запросы должны четко следовать один за другим
	/// </summary>
	public interface ISerialSession {
		/// <summary>
		/// 	√арантирует синхронный, последовательный доступ к сессии, вычисл€ет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		QueryResult Eval(IQuery query, int timeout = -1);

		/// <summary>
		/// 	√арантирует синхронный, последовательный доступ к сессии, вычисл€ет значение
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		Task<QueryResult> EvalAsync(IQuery query);

		/// <summary>
		/// 	¬озвращает ссылку на реальную сессию
		/// </summary>
		/// <returns> </returns>
		ISession GetUnderlinedSession();
	}
}