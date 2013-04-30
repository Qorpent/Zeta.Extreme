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
// PROJECT ORIGIN: LockStateInfo.cs

#endregion
using  Qorpent.Serialization;
namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     Информация о статусе закрытия формы
	/// </summary>
	public class LockStateInfo {
		/// <summary>
		///     Возможность блокировки формы
		/// </summary>
		public bool canblock;

		/// <summary>
		///     Доступна кнопка "утвердить"
		/// </summary>
		public bool cancheck;

		/// <summary>
		///     Доступна кнопка "открыть"
		/// </summary>
		public bool canopen;

		/// <summary>
		///     Признак возможности сохранения
		/// </summary>
		public bool cansave;

		/// <summary>
		///     Признак возможности сохранения поверх блокировки
		/// </summary>
		public bool cansaveoverblock;

		/// <summary>
		///     Признак несошедшихся контрольных точек
		/// </summary>
		public bool hasbadcontrolpoints;

		/// <summary>
		///     Наличие роли на утверждение
		/// </summary>
		public bool hasholdlockrole;

		/// <summary>
		///     Наличие роли на блокировку
		/// </summary>
		public bool haslockrole;

		/// <summary>
		///     Роль на блокировку поверх контрольных точек
		/// </summary>
		public bool hasnocontrolpoointsrole;

		/// <summary>
		///     Зарезервированное понятия открытой формы
		/// </summary>
		public bool isopen;

		/// <summary>
		///     Сообщение об ошибке сохранения
		/// </summary>
		public string message;

		/// <summary>
		///     Текущий статус
		/// </summary>
		public string state;

		/// <summary>
		/// Признак того что применяется сохранение поверх статуса периода
		/// </summary>
		public bool periodstateoverride;

		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public FormStateOperationResult canblockresult;
		/// <summary>
		/// 
		/// </summary>
		public bool newstates;

		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public FormStateOperationResult cancheckresult;
		
		/// <summary>
		/// 
		/// </summary>
		[SerializeNotNullOnly]
		public FormStateOperationResult canopenresult;
	}
}