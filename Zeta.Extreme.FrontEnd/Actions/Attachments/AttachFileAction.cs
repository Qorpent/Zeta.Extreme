using System;
using System.Web;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd {
	///<summary>
	///	�������� ���������� ������
	///</summary>
	[Action("zefs.attachfile")]
	public class AttachFileAction : FormSessionActionBase {
		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			_datafile =(HttpPostedFileBase) Context.GetFile("datafile");
		}

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate()
		{
			base.Validate();
			if(null==_datafile) {
				throw new Exception("not file provided");
			}
		}
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.AttachFile(_datafile, filename, type,uid);
		}
		/// <summary>
		/// ��� �������������� ���������
		/// </summary>
		[Bind(Required = true)] protected string type;
		/// <summary>
		/// ���������������� ��� �����
		/// </summary>
		[Bind] protected string filename;

		/// <summary>
		/// ������������� ������������� �����
		/// </summary>
		[Bind]
		protected string uid;

		private HttpPostedFileBase _datafile;
	}
}