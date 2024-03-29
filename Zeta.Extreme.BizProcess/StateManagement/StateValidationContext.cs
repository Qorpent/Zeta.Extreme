using System;
using System.Collections.Generic;
using Qorpent.IoC;
using Qorpent.Security;
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
		private readonly IContainer _container;

		/// <summary>
		/// �������� �����������
		/// </summary>
		/// <param name="manager"></param>
		/// <param name="form"></param>
		/// <param name="savedFormData"></param>
		/// <param name="savedLastState"></param>
		/// <param name="newState"></param>
		/// <param name="container"></param>
		/// <param name="roleResolver"></param>
		public StateValidationContext(IFormStateManager manager, IFormSession form, Form savedFormData, FormState savedLastState, FormStateType newState,IContainer container, IRoleResolver roleResolver) {
			_manager = manager;
			_form = form;
			_savedFormData = savedFormData;
			_savedLastState = savedLastState;
			_newState = newState;
			_container = container;
			RoleResolver = roleResolver;
			
			if (null == roleResolver && null!=container) {
				RoleResolver = container.Get<IRoleResolver>();
			}
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
		/// ������ �� ��������� ��������
		/// </summary>
		public IContainer Container
		{
			get { return _container; }
		}

		/// <summary>
		/// ������� ������
		/// </summary>
		public FormStateType OldState {
			
			get {
				if (null == SavedLastState) {
					return FormStateType.Open;
				}
				return (FormStateType)Array.IndexOf(DefaultFormStateRepository.StateStrings, SavedLastState.State);
			}
		}

		/// <summary>
		/// �������������� ��������� ���������
		/// </summary>
		public  IDictionary<string,string> Options { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public IRoleResolver RoleResolver { get;  private set; }
	}
}