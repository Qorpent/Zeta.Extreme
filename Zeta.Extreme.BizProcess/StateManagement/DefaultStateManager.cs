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
using Qorpent.Security;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	///     —тандартна€ реализаци€ менеджера статусов
	/// </summary>
	public class DefaultStateManager : ServiceBase, IFormStateManager {
		/// <summary>
		/// </summary>
		public DefaultStateManager() {
			StateAvailabilityCheckers = new List<IFormStateAvailabilityChecker>();
		}

		/// <summary>
		///     –епозиторий дл€ пр€мого получени€ статусов и сохранени€ его
		/// </summary>
		[Inject] public IFormStateRepository Repository { get; set; }

		/// <summary>
		///     –асширени€ дл€ работы с проверкой возможности установки статуса
		/// </summary>
		[Inject] public IList<IFormStateAvailabilityChecker> StateAvailabilityCheckers { get; private set; }

		/// <summary>
		/// Ѕыстрое получение текущего типа статуса формы
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public FormStateType GetCurrentState(IFormSession session) {
			return
				(FormStateType)
				Array.IndexOf(DefaultFormStateRepository.StateStrings, Repository.GetFormRecord(session).CurrentState);
		}

		/// <summary>
		///     ”становить поставщика исходной информации по статусам
		/// </summary>
		/// <param name="repository"></param>
		public void SetStateRepository(IFormStateRepository repository) {
			Repository = repository;
		}

		/// <summary>
		///     ƒобавл€ет расширение дл€ контрол€ возможности установки статуса
		/// </summary>
		/// <param name="checker"></param>
		public void RegisterCanSetChecker(IFormStateAvailabilityChecker checker) {
			if (!StateAvailabilityCheckers.Contains(checker)) {
				StateAvailabilityCheckers.Add(checker);
				_checkersSorted = false;
			}
		}

		/// <summary>
		///     ѕроверка возможности установки статуса
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
				var context = new StateValidationContext(this, form, formRecord, lastState, newStateType,Container, Container.Get<IRoleResolver>());
				if (context.OldState == context.NewState) {
					//если статус не мен€етс€ значит и нельз€ выполнить эту операцию
					return new FormStateOperationResult
						{
							Allow = false,
							Reason = new FormStateOperationDenyReason {Type = FormStateOperationDenyReasonType.AlreadySet}
						};
				}
				foreach (var checker in StateAvailabilityCheckers) {
					var checkerResult = checker.GetCanSet(context);
					
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
		///     ”станавливает новый статус дл€ формы
		/// </summary>
		/// <param name="form"></param>
		/// <param name="newStateType"></param>
		/// <param name="comment"></param>
		/// <param name="parentId"></param>
		/// <param name="skipValidation">режим без проверки валидности, пр€мое выставление статуса</param>
		/// <returns>
		/// </returns>
		public FormStateOperationResult SetState(IFormSession form, FormStateType newStateType, string comment = "", int parentId = 0, bool skipValidation =false)
		{
			try {
				FormStateOperationResult canSetState =null;
				if (skipValidation || (canSetState = GetCanSet(form, newStateType)).Allow)
				{
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