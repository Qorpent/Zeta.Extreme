using System;
using System.Linq;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	/// <summary>
	/// Базовый класс действий, ориентированных на работу с конкретным присоединенным файлом
	/// </summary>
	public class SingleAttachmentActionBase : FormSessionActionBase {
		/// <summary>
		/// Идентификатор существующего файла
		/// </summary>
		[Bind(Required = true,ValidatePattern = @"^[\d\w\-\_]+$")]protected string uid;

		private FormAttachment _myattach;

		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
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
	}
}