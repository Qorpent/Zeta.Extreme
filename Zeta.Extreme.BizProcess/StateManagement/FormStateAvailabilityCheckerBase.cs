using System;
using System.Linq;
using Qorpent;
using Qorpent.Serialization;
using Zeta.Extreme.BizProcess.StateManagement.Attributes;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// ������� ����� ���������� ������� ����������� ��������� �������
	/// </summary>
	[Serialize]
	public abstract class FormStateAvailabilityCheckerBase : ServiceBase,IFormStateAvailabilityChecker {
		/// <summary>
		/// ������ ����������� ����������� ������ ������ �� ����������� ���������
		/// </summary>
		protected FormStateAvailabilityCheckerBase() {
			var attributes = GetType().GetCustomAttributes(typeof (StateAttributeBase), true).OfType<StateAttributeBase>().ToArray();
			foreach (var attribute in attributes) {
				var errorMessage = attribute as ErrorMessageAttribute;
				if (null != errorMessage) {
					Message = errorMessage.Message;
					continue;
				}
				var targetState = attribute as TargetStateAttribute;
				if (null != targetState)
				{
					TargetFormState = targetState.Type;
					continue;
				}
				var sourceState = attribute as SourceStateAttribute;
				if (null != sourceState)
				{
					SourceFormState = sourceState.Type;
					continue;
				}
				var checkIndex = attribute as CheckIndexAttribute;
				if (null != checkIndex)
				{
					Index = checkIndex.Index;
					continue;
				}

				var reasonType = attribute as ReasonTypeAttribute;
				if (null != reasonType)
				{
					ReasonType = reasonType.Type;
					continue;
				}
				var skipRole = attribute as SkipRoleAttribute;
				if (null != skipRole)
				{
					SkipRole = skipRole.Role;
					SkipRoleExact = skipRole.Exact;
					continue;
				}

				var defaultAllow = attribute as DefaultResultAttribte;
				if (null != defaultAllow) {
					UseDefaultResult = true;
					DefaultResult = defaultAllow.Allow;
					continue;

				}

				var regCode = attribute as ReglamentCodeAttribute;
				if (null != regCode) {
					ReglamentCode = regCode.Code;
					ReglamentDescription = regCode.Description;
					continue;
				}

				var defMessage = attribute as DefaultMessageAttribute;
				if (null != defMessage)
				{
					DefaultMessage = defMessage.Message;
					continue;
				}

			}

		}
		/// <summary>
		/// ��������� ��������� ��� ������������� �������
		/// </summary>
		[SerializeNotNullOnly]
		public string DefaultMessage { get; set; }

		/// <summary>
		/// ��������, ����� ������ ����������
		/// </summary>
		[Serialize]
		public string ReglamentDescription { get; set; }

		/// <summary>
		/// ��� ������� � ����������
		/// </summary>
		[Serialize]
		public string ReglamentCode { get; set; }

		/// <summary>
		/// ��������� �� ���������
		/// </summary>
		[Serialize] public bool DefaultResult { get; set; }

		/// <summary>
		/// ������� ������������� ���������� �� ���������
		/// </summary>
		[Serialize] public bool UseDefaultResult { get; set; }

		/// <summary>
		/// ������� ���� ��� <see cref="SkipRole"/> ������ ���� ���� ��������� � ������������
		/// </summary>
		[Serialize] public bool SkipRoleExact { get; set; }

		/// <summary>
		/// ��������� ����, ������������ ������ ��������
		/// </summary>
		[Serialize]
		public string SkipRole { get; set; }

		/// <summary>
		/// User-defined order index
		/// </summary>
		[Serialize]
		public int Index { get; set; }


		/// <summary>
		/// ��������� ���������� ��� ������������� ��������� �� ������
		/// </summary>
		[Serialize]
		public FormStateOperationDenyReasonType ReasonType { get; set; }

		/// <summary>
		/// ��������� �� ������
		/// </summary>
		[Serialize]
		public string Message { get; set; }

		/// <summary>
		/// ������ ��� ����������
		/// </summary>
		[IgnoreSerialize]
		protected Exception Error { get; set; }

		/// <summary>
		/// ������ ��������� ���������� ��� ������
		/// </summary>
		/// <returns></returns>
		protected FormStateOperationResult GetErrorResult() {
			return new FormStateOperationResult{Allow = false,  Reason = new FormStateOperationDenyReason {Type=ReasonType,Message = Message,Error = Error,ReglamentCode = ReglamentCode}};
		}

		/// <summary>
		/// ������� � ������, ���� ���������� ������������� ��� �������� ������ ����������� �������
		/// </summary>
		[Serialize]
		public FormStateType TargetFormState { get; set; }

		/// <summary>
		/// ������� � ������, ���� ���������� ������������� ��� �������� ������ ����������� �������� �������
		/// </summary>
		[Serialize]
		public FormStateType SourceFormState { get; set; }

		/// <summary>
		///     �������� ����������� ��������� �������
		/// </summary>
		/// <param name="stateValidationContext"></param>
		/// <returns>
		/// </returns>
		public FormStateOperationResult GetCanSet(StateValidationContext stateValidationContext) {
			Context = stateValidationContext;
			if (null != Component) {
				var basename = Component.Name.Replace(".state.checker", "");
				var ignoreparameter = basename + "_ignore";
				if (Context.Options.ContainsKey(ignoreparameter) && Context.Options[ignoreparameter] == "1") return GetValidResult();
			}
			if (FormStateType.None != TargetFormState) {
				if (Context.NewState != TargetFormState) return GetValidResult();
			}
			if (FormStateType.None != SourceFormState)
			{
				if (Context.OldState != SourceFormState) return GetValidResult();
			}
			if (!string.IsNullOrWhiteSpace(SkipRole)) {
				var isInSkipRole = Context.RoleResolver.IsInRole(Context.Form.Usr, SkipRole, SkipRoleExact);
				if (isInSkipRole) {
					return GetValidResult();
				}
			}
			var allow = UseDefaultResult ? DefaultResult : InternalIsValid();
			if (allow) {
				return GetValidResult();
			}
			return GetErrorResult();
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected FormStateOperationResult GetValidResult() {
			if (string.IsNullOrWhiteSpace(DefaultMessage)) return null;
			return new FormStateOperationResult {Allow = true, DefaultMessageForState = DefaultMessage};
		}

		/// <summary>
		/// ��������� ������������ ���������
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalIsValid() {
			return false;
		}


		/// <summary>
		/// ������� �������� ���������
		/// </summary>
		[IgnoreSerialize]
		protected StateValidationContext Context { get; set; }
	}
}