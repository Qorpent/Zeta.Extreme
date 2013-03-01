using System;
using System.Linq;
using System.Security;
using Comdiv.Zeta.Model;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Form.InputTemplates;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������� �������� ��� ���������� ������
	/// </summary>
	public class SessionStartBase : FormServerActionBase {
		/// <summary>
		/// �������� ������
		/// </summary>
		protected IInputTemplate _realform;
		/// <summary>
		/// ������� ������
		/// </summary>
		protected IZetaMainObject _realobj;
		/// <summary>
		/// ��� �����
		/// </summary>
		[Bind(Required = true)] protected string form = "";
		/// <summary>
		/// ��� �������
		/// </summary>
		[Bind(Required = true)] protected int obj = 0;
		/// <summary>
		/// ������
		/// </summary>
		[Bind(Required = true)] protected int period = 0;
		/// <summary>
		/// ���
		/// </summary>
		[Bind(Required = true)] protected int year = 0;

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			MyFormServer.ReadyToServeForms.Wait();
			if (!FormServer.Default.IsOk) {
				throw new Exception("Application not loaded properly!");
			}
			base.Validate();
			_realform = FormServer.Default.FormProvider.Get(form);
			if (null == _realform) {
				throw new Exception("form not found");
			}
			_realobj = MetaCache.Default.Get<IZetaMainObject>(obj);
			if (null == _realobj) {
				throw new Exception("obj not found");
			}
		}
		/// <summary>
		/// ���������� ����������� � �����
		/// </summary>
		protected override void Authorize()
		{
			base.Authorize();
			var acessobject = new AccessibleObjectsHelper().GetAccessibleObjects();
			if (!acessobject.objs.Any(_ => _.id == _realobj.Id))
			{
				throw new SecurityException("try access not allowed object");
			}
			if(!Application.Roles.IsInRole(Application.Principal.CurrentUser, _realform.Role)) {
				throw new SecurityException("try access not allowed form");
			}
		}
	}
}