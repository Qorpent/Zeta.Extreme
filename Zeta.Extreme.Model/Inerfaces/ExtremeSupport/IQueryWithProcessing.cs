#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQueryWithProcessing.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Описатель запроса с поддержкой обработки
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		/// <summary>
		/// 	Обеспечивает возврат результата запроса
		/// </summary>
		/// <param name="timeout"> </param>
		/// <returns> </returns>
		/// <exception cref="Exception"></exception>
		QueryResult GetResult(int timeout = -1);

		/// <summary>
		/// 	Sign that primary was not set
		/// </summary>
		bool HavePrimary { get; set; }

		/// <summary>
		/// 	Back-reference to preparation tasks
		/// </summary>
		Task PrepareTask { get; set; }

		/// <summary>
		/// 	Client processed mark
		/// </summary>
		bool Processed { get; set; }

		/// <summary>
		/// 	Статус по подготовке
		/// </summary>
		PrepareState PrepareState { get; set; }

		/// <summary>
		/// 	Позволяет синхронизировать запросы в подсессиях
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitPrepare(int timeout = -1);
	}
}