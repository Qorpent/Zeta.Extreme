﻿#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPrimarySource.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Threading.Tasks;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Абстракция акцессора первичных данных
	/// </summary>
	public interface IPrimarySource {
		/// <summary>
		/// 	Регистрирует целевой запрос
		/// </summary>
		/// <param name="query"> </param>
		void Register(IQuery query);

		/// <summary>
		/// 	Регистрирует заранее подготовленный SQL-запрос
		/// </summary>
		/// <param name="preparedQuery"> </param>
		void Register(string preparedQuery);

		/// <summary>
		/// 	Получает асинхронную задачу сбора текущих данных,
		/// 	завершение задачи означает окончание всех текущих запросов
		/// </summary>
		/// <returns> </returns>
		Task Collect();

		/// <summary>
		/// 	Выполняет все требуемые запросы в режиме ожидания
		/// </summary>
		void Wait(int timeout = -1);

		/// <summary>
		/// 	Журнал выполненных SQL
		/// </summary>
		IList<string> QueryLog { get; }
	}
}