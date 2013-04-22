using System;
using Qorpent;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// ������� ����� ���������� ������� ����������� ��������� �������
	/// </summary>
	public abstract class FormStateAvailabilityCheckerBase : ServiceBase,IFormStateAvailabilityChecker {
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
			return new FormStateOperationResult{Allow = false, Reason = {Type=ReasonType,Message = Message,Error = Error}};
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

			if (InternalIsValid()) {
				return null;
			}
			return GetResult();
		}

		/// <summary>
		/// ��������� ������������ ���������
		/// </summary>
		/// <returns></returns>
		protected abstract bool InternalIsValid();


		/// <summary>
		/// ������� �������� ���������
		/// </summary>
		protected StateValidationContext Context { get; set; }
	}
}