#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ISerializableSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Threading.Tasks;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// 	Сериализуемая сессия
	/// </summary>
	public interface ISerializableSession : ISession {
		/// <summary>
		/// 	Сериальная синхронизация
		/// </summary>
		object SerialSync { get; }

		/// <summary>
		/// 	Задача для выполнения в асинхронном режиме из сериализованного доступа
		/// </summary>
		Task<QueryResult> SerialTask { get; set; }
	}
}