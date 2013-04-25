using System;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.BizProcess.Tests.StateManagement {
	/// <summary>
	/// </summary>
	public class FakeStateChecker : IFormStateAvailabilityChecker {
		public int Index { get; set; }
		string GetStrState(FormStateType type) {
			return DefaultFormStateRepository.StateStrings[(int) type];
		}
		FormStateType FromString(string type) {
			return (FormStateType) Array.IndexOf(DefaultFormStateRepository.StateStrings, type);
		}
		public FormStateOperationResult GetCanSet(StateValidationContext stateValidationContext) {
			var oldState = FromString(stateValidationContext.SavedFormData.CurrentState);
			if (oldState == stateValidationContext.NewState) {
				return new FormStateOperationResult
					{
						Allow = false,
						Reason =
							new FormStateOperationDenyReason
								{
									Type = FormStateOperationDenyReasonType.InvalidBaseState,
									Message = "already set"
								}
					};
			}
			if (stateValidationContext.NewState == FormStateType.Open) {
				if (oldState != FormStateType.Closed) {
					return new FormStateOperationResult
						{
							Allow = false,
							Reason =
								new FormStateOperationDenyReason
									{
										Type = FormStateOperationDenyReasonType.InvalidBaseState,
										Message = "cannot open "+oldState
									}
						};
				}
			}

			if (stateValidationContext.NewState == FormStateType.Closed)
			{
				if (oldState != FormStateType.Open)
				{
					return new FormStateOperationResult
						{
							Allow = false,
							Reason =
								new FormStateOperationDenyReason
									{
										Type = FormStateOperationDenyReasonType.InvalidBaseState,
										Message = "cannot close " + oldState
									}
						};
				}
			}

			if (stateValidationContext.NewState == FormStateType.Checked)
			{
				if (oldState != FormStateType.Closed)
				{
					return new FormStateOperationResult
						{
							Allow = false,
							Reason =
								new FormStateOperationDenyReason
									{
										Type = FormStateOperationDenyReasonType.InvalidBaseState,
										Message = "cannot check " + oldState
									}
						};
				}
			}
			return null;
		}
	}
}