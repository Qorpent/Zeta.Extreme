#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQueryWithProcessing.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Описатель запроса с поддержкой обработки
	/// </summary>
	public interface IQueryWithProcessing : IQuery {
		

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
		/// 	Дочерние запросы
		/// </summary>
		IList<IQuery> FormulaDependency { get; }

		/// <summary>
		/// 	Проверяет "первичность запроса"
		/// </summary>
		bool IsPrimary { get; }

		/// <summary>
		/// 	Зависимости для суммовых запросов
		/// </summary>
		IList<Tuple<decimal, IQuery>> SummaDependency { get; }

		/// <summary>
		/// 	Формула, которая присоединяется к запросу на фазе подготовки
		/// </summary>
		IFormula AssignedFormula { get; set; }

		/// <summary>
		/// 	Тип вычисления запроса
		/// </summary>
		QueryEvaluationType EvaluationType { get; set; }

		/// <summary>
		/// 	Позволяет синхронизировать запросы в подсессиях
		/// </summary>
		/// <param name="timeout"> </param>
		void WaitPrepare(int timeout = -1);
	}
}