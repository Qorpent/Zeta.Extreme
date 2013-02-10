#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexSqlBuilder.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Стандартный, пока только для простых режимов построитель SQL-запросов
	/// </summary>
	public class DefaultZexSqlBuilder : IZexSqlBuilder {
		/// <summary>
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public DefaultZexSqlBuilder(ZexSession session) {
			_session = session;
		}

		private ZexSession _session;

		/// <summary>
		/// Формирует строку вызова для запроса
		/// </summary>
		/// <param name="query"></param>
		public void PrepareSqlRequest(ZexQuery query) {
			query.SqlRequest = 
				string.Format("select {0}, Id, DecimalValue from cell where row = {1} and col= {2} and obj ={3} and year={4} and period={5} ",
					query.UID,query.Row.Id,query.Col.Id,query.Obj.Id,query.Time.Year,query.Time.Period
				);
		}
	}
}