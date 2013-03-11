#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQueryPreparator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Описывает исполнителя фазы подготовки запроса
	/// </summary>
	public interface IQueryPreparator {
		/// <summary>
		/// 	Выполняет подготовку запроса к выполнению
		/// 	выполняется после препроцессора и проверок
		/// </summary>
		/// <param name="query"> </param>
		void Prepare(IQuery query);
	}
}