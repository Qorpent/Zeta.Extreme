using System;
using System.Linq;
using System.Web;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.attachfile")]
	public class AttachFileAction : FormSessionActionBase {
		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			_datafile =(HttpPostedFile) Context.GetFile("datafile");
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
		/// Тип привязываемого документа
		/// </summary>
		[Bind(Required = true)] protected string type;
		/// <summary>
		/// Пользовательское имя файла
		/// </summary>
		[Bind] protected string filename;

		/// <summary>
		/// Идентификатор существующего файла
		/// </summary>
		[Bind]
		protected string uid;

		private HttpPostedFile _datafile;
	}


	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.deleteattach",Role = "ADMIN")]
	public class DeleteAttachAction : FormSessionActionBase
	{
		protected override void Initialize()
		{
			base.Initialize();
			_myattach = MySession.GetAttachedFiles().FirstOrDefault(_ => _.Uid == uid);
		}

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate()
		{
			base.Validate();
			if(null==_myattach) {
				throw new Exception("cannot remove attach, not attached to current session");
			}
			
		}
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			
		}
		

		/// <summary>
		/// Идентификатор существующего файла
		/// </summary>
		[Bind(Required = true,ValidatePattern = @"^[\d\w\-\_]+$")]protected string uid;

		private FormAttachment _myattach;
	}
}