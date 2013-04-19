using System.Collections.Generic;
using System.Security.Principal;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.BizProcess.StateManagement;
using Zeta.Extreme.Model;

namespace Zeta.Extreme.BizProcess.Tests.StateManagement {
	/// <summary>
	/// </summary>
	public class FakeStateRepository:IFormStateRepository {

		public FormStateType NextNewType =FormStateType.Open;
		
		IDictionary<IFormSession,Model.Form> sessionStorage = new Dictionary<IFormSession, Model.Form>();

		public Model.Form GetFormRecord(IFormSession form) {
			if (!sessionStorage.ContainsKey(form)) {
				sessionStorage[form] = new Model.Form
					{
						BizCaseCode = form.Template.Code,
						CurrentState = DefaultFormStateRepository.StateStrings[(int) NextNewType],
						Code = form.Template.UnderwriteCode,
						ObjectId = form.Object.Id,
						Year = form.Year,
						Period = form.Period
					};
			}
			return sessionStorage[form];
			
		}

		public FormState[] GetFormStateHistory(IFormSession form) {
			var form_ = GetFormRecord(form);
			return new []{new FormState{State = form_.CurrentState}};
		}

		public FormState GetLastFormState(IFormSession form) {
			var form_ = GetFormRecord(form);
			return  new FormState { State = form_.CurrentState } ;
		}

		public FormState SetState(IFormSession form, FormStateType stateType, string comment, int parentId, IPrincipal principal = null) {
			var form_ = GetFormRecord(form);
			form_.CurrentState = DefaultFormStateRepository.StateStrings[(int)stateType];
			return GetLastFormState(form);
		}
	}
}