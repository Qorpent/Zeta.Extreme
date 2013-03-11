#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ISession.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Асинхронная, Zeta.Extrem cecсия, базовый интерфейс
	/// </summary>
	public interface ISession {
		/// <summary>
		/// 	Локальный кэш объектных данных
		/// </summary>
		IMetaCache MetaCache { get; }

		/// <summary>
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		QueryResult Get(string key, int timeout = -1);

		/// <summary>
		/// 	Синхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query"> исходный запрос </param>
		/// <param name="uid"> позволяет явно указать словарный код для составления синхронизируемой коллекции запросов </param>
		/// <returns> запрос по итогам регистрации в сессии </returns>
		/// <remarks>
		/// 	При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// 	возвращается именно итоговый запрос
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		IQuery Register(IQuery query, string uid = null);

		/// <summary>
		/// 	Асинхронная регистрация запроса в сессии
		/// </summary>
		/// <param name="query"> исходный запрос </param>
		/// <param name="uid"> позволяет явно указать словарный код для составления синхронизируемой коллекции запросов </param>
		/// <returns> задачу, по результатам которой возвращается запрос по итогам регистрации в сессии </returns>
		/// <remarks>
		/// 	При регистрации запроса, он проходит дополнительную оптимизацию и проверку на дубляж,
		/// 	возвращается именно итоговый запрос
		/// </remarks>
		Task<IQuery> RegisterAsync(IQuery query, string uid = null);

		/// <summary>
		/// 	Выполняет синхронизацию и расчет значений в сессии
		/// </summary>
		/// <param name="timeout"> </param>
		void Execute(int timeout = -1);

		/// <summary>
		/// Ожидает завершения задач, связанных с первичными данными
		/// </summary>
		/// <param name="timeout"></param>
		void WaitPrimarySource(int timeout = -1);
	}
}