using System.Collections.Generic;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.BizProcess.StateManagement {
	/// <summary>
	/// �������� ���������� ����������� ���������� �������
	/// </summary>
	public class StateValidationContext {
		private readonly IFormStateManager _manager;
		private readonly IFormSession _form;
		private readonly Form _savedFormData;
		private readonly FormState _savedLastState;
		private readonly FormStateType _newState;

		/// <summary>
		/// �������� �����������
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="form"></param>
		/// <param name="savedFormData"></param>
		/// <param name="savedLastState"></param>
		/// <param name="newState"></param>
		public StateValidationContext(IFormStateManager manager, IFormSession form, Form savedFormData, FormState savedLastState, FormStateType newState) {
			_manager = manager;
			_form = form;
			_savedFormData = savedFormData;
			_savedLastState = savedLastState;
			_newState = newState;
			Options = new Dictionary<string, string>();
		}

		/// <summary>
		/// ������ �� �������� ��������
		/// </summary>
		public IFormStateManager Manager {
			get { return _manager; }
		}
		/// <summary>
		/// ��������� ������ ����
		/// </summary>
		public IFormSession Form {
			get { return _form; }
		}

		/// <summary>
		/// ��������� �������� ������ �����
		/// </summary>
		public Form SavedFormData {
			get { return _savedFormData; }
		}
		/// <summary>
		/// ��������� ����������� ������
		/// </summary>
		public FormState SavedLastState {
			get { return _savedLastState; }
		}

		/// <summary>
		/// ����� ��������������� ������
		/// </summary>
		public FormStateType NewState {
			get { return _newState; }
		}


		/// <summary>
		/// �������������� ��������� ���������
		/// </summary>
		public  IDictionary<string,string> Options { get; private set; }
	}
}