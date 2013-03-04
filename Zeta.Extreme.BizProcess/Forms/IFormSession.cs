#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IFormSession.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Comdiv.Zeta.Model;
using Qorpent.Serialization;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.Form.StateManagement;

namespace Zeta.Extreme.Form {
	/// <summary>
	/// 	Базовый интерфейс сессии
	/// </summary>
	public interface IFormSession {
		/// <summary>
		/// 	Идентификатор сессии
		/// </summary>
		string Uid { get; }

		/// <summary>
		/// 	Год
		/// </summary>
		int Year { get; }

		/// <summary>
		/// 	Период
		/// </summary>
		int Period { get; }

		/// <summary>
		/// 	Объект
		/// </summary>
		[IgnoreSerialize] IZetaMainObject Object { get; }

		/// <summary>
		/// 	Шаблон
		/// </summary>
		[IgnoreSerialize] IInputTemplate Template { get; }

		/// <summary>
		/// 	Пользователь
		/// </summary>
		string Usr { get; }

		/// <summary>
		/// 	Хранит уже подготовленные данные
		/// </summary>
		[IgnoreSerialize] List<OutCellBase> Data { get; }

		/// <summary>
		/// 	Возвращает статусную информацию по форме с поддержкой признака "доступа" блокировки
		/// </summary>
		/// <returns> </returns>
		LockStateInfo GetCurrentLockInfo();
	}
}