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
// PROJECT ORIGIN: IFormStateRepository.cs

#endregion

using System.Security.Principal;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     Интерфейс низкоуровнего акцессора к данным статуса формы
	/// </summary>
	public interface IFormStateRepository {
		/// <summary>
		///     Возвращает хранимый объект формы
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		Form GetFormRecord(IInputTemplate form);

		/// <summary>
		///     Возвращает историю статусов для формы
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		FormState[] GetFormStateHistory(Form form);


		/// <summary>
		///     Возвращает последнюю запись о смене статуса
		/// </summary>
		/// <param name="form"></param>
		/// <returns>
		/// </returns>
		FormState GetLastFormState(Form form);

		/// <summary>
		///     Устанавливает новый статус для формы
		/// </summary>
		/// <param name="targetForm"></param>
		/// <param name="stateType"></param>
		/// <param name="principal">Учетная запись пользователя</param>
		void SetState(Form targetForm, FormStateType stateType, IPrincipal principal = null);
	}
}