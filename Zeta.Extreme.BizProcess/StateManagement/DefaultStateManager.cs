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
// PROJECT ORIGIN: DefaultStateManager.cs

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Qorpent;
using Qorpent.IoC;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     Стандартная реализация менеджера статусов
	/// </summary>
	public class DefaultStateManager : ServiceBase, IFormStateManager {
		/// <summary>
		/// </summary>
		public DefaultStateManager() {
			StateAvailabilityCheckers = new List<IFormStateAvailabilityChecker>();
		}

		/// <summary>
		///     Репозиторий для прямого получения статусов и сохранения его
		/// </summary>
		[Inject] public IFormStateRepository Repository { get; set; }

		/// <summary>
		///     Расширения для работы с проверкой возможности установки статуса
		/// </summary>
		[Inject] public IList<IFormStateAvailabilityChecker> StateAvailabilityCheckers { get; private set; }

		/// <summary>
		/// Быстрое получение текущего типа статуса формы
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public FormStateType GetCurrentState(IFormSession session) {
			return
				(FormStateType)
				Array.IndexOf(DefaultFormStateRepository.StateStrings, Repository.GetFormRecord(session).CurrentState);
		}

		/// <summary>
		///     Установить поставщика исходной информации по статусам
		/// </summary>
		/// <param name="repository"></param>
		public void SetStateRepository(IFormStateRepository repository) {
			Repository = repository;
		}

		/// <summary>
		///     Добавляет расширение для контроля возможности установки статуса
		/// </summary>
		/// <param name="checker"></param>
		public void RegisterCanSetChecker(IFormStateAvailabilityChecker checker) {
			if (!StateAvailabilityCheckers.Contains(checker)) {
				StateAvailabilityCheckers.Add(checker);
				_checkersSorted = false;
			}
		}

		/// <summary>
		///     Проверка возможности установки статуса
		/// </summary>
		/// <param name="form"></param>
		/// <param name="newStateType"></param>
		/// <returns>
		/// </returns>
		public FormStateOperationResult GetCanSet(IFormSession form, FormStateType newStateType) {
			try {
				CheckCheckersOrder();
				var formRecord = Repository.GetFormRecord(form);
				var lastState = Repository.GetLastFormState(form);
				foreach (var checker in StateAvailabilityCheckers) {
					var checkerResult = checker.GetCanSet(this, form, formRecord, lastState, newStateType);
					
					if ( null!= checkerResult  && !checkerResult.Allow) {
						checkerResult.Reason = checkerResult.Reason ??
						                       new FormStateOperationDenyReason {Type = FormStateOperationDenyReasonType.Custom};
						checkerResult.Reason.Checker = checkerResult.Reason.Checker ?? checker;
						return checkerResult;
					}
				}
				return new FormStateOperationResult {Allow = true};
			}
			catch (Exception ex) {
				return ThrowCanSetResult(ex);
			}
		}

		/// <summary>
		///     Устанавливает новый статус для формы
		/// </summary>
		/// <param name="form"></param>
		/// <param name="newStateType"></param>
		/// <param name="comment"></param>
		/// <param name="parentId"></param>
		/// <returns>
		/// </returns>
		public FormStateOperationResult SetState(IFormSession form, FormStateType newStateType, string comment = "", int parentId = 0) {
			try {
				var canSetState = GetCanSet(form, newStateType);
				if (canSetState.Allow) {
					Repository.SetState(form, newStateType,comment,parentId);
				}
				return canSetState;
			}
			catch (Exception ex) {
				return ThrowCanSetResult(ex);
			}
		}

		private static FormStateOperationResult ThrowCanSetResult(Exception ex) {
			return new FormStateOperationResult
				{
					Allow = false,
					Reason = new FormStateOperationDenyReason {Type = FormStateOperationDenyReasonType.Exception, Error = ex}
				};
		}

		private void CheckCheckersOrder() {
			if (!_checkersSorted) {
				StateAvailabilityCheckers = StateAvailabilityCheckers.OrderBy(_ => _.Index).ToList();
				_checkersSorted = true;
			}
		}

		private bool _checkersSorted;
	}
}