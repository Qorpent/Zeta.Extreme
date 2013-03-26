#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IFormSession.cs
#endregion
using System.Collections.Generic;
using Qorpent.Log;
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.BizProcess.Forms {
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
		[IgnoreSerialize] List<OutCell> Data { get; }

		/// <summary>
		/// Журнал
		/// </summary>
		IUserLog Logger { get; set; }

		/// <summary>
		/// 	Возвращает статусную информацию по форме с поддержкой признака "доступа" блокировки
		/// </summary>
		/// <returns> </returns>
		LockStateInfo GetCurrentLockInfo();
	}
}