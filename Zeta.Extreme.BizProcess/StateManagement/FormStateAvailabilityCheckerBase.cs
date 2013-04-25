using System;
using System.Linq;
using Qorpent;
using Zeta.Extreme.BizProcess.StateManagement.Attributes;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// ������� ����� ���������� ������� ����������� ��������� �������
	/// </summary>
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
			}

		}
		/// <summary>
		/// ��������� �� ���������
		/// </summary>
		protected bool DefaultResult { get; set; }

		/// <summary>
		/// ������� ������������� ���������� �� ���������
		/// </summary>
		protected bool UseDefaultResult { get; set; }

		/// <summary>
		/// ������� ���� ��� <see cref="SkipRole"/> ������ ���� ���� ��������� � ������������
		/// </summary>
		protected bool SkipRoleExact { get; set; }

		/// <summary>
		/// ��������� ����, ������������ ������ ��������
		/// </summary>
		public string SkipRole { get; set; }

		/// <summary>
		/// User-defined order index
		/// </summary>
		public int Index { get; set; }


		/// <summary>
		/// ��������� ���������� ��� ������������� ��������� �� ������
		/// </summary>
		public FormStateOperationDenyReasonType ReasonType { get; set; }

		/// <summary>
		/// ��������� �� ������
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// ������ ��� ����������
		/// </summary>
		protected Exception Error { get; set; }

		/// <summary>
		/// ������ ��������� ���������� ��� ������
		/// </summary>
		/// <returns></returns>
		protected FormStateOperationResult GetResult() {
			return new FormStateOperationResult{Allow = false, Reason = new FormStateOperationDenyReason {Type=ReasonType,Message = Message,Error = Error}};
		}

		/// <summary>
		/// ������� � ������, ���� ���������� ������������� ��� �������� ������ ����������� �������
		/// </summary>
		public FormStateType TargetFormState { get; set; }

		/// <summary>
		/// ������� � ������, ���� ���������� ������������� ��� �������� ������ ����������� �������� �������
		/// </summary>
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
				if (Context.Options.ContainsKey(ignoreparameter) && Context.Options[ignoreparameter] == "1") return null;
			}
			if (FormStateType.None != TargetFormState) {
				if (Context.NewState != TargetFormState) return null;
			}
			if (FormStateType.None != SourceFormState)
			{
				if (Context.OldState != SourceFormState) return null;
			}
			if (!string.IsNullOrWhiteSpace(SkipRole)) {
				var isInSkipRole = Context.RoleResolver.IsInRole(Context.Form.Usr, SkipRole, SkipRoleExact);
				if (isInSkipRole) {
					return null;
				}
			}
			var allow = UseDefaultResult ? DefaultResult : InternalIsValid();
			if (allow) {
				return null;
			}
			return GetResult();
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
		protected StateValidationContext Context { get; set; }
	}
}