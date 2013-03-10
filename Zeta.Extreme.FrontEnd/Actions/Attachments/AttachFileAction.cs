#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : AttachFileAction.cs
// Project: Zeta.Extreme.FrontEnd
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Web;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	///<summary>
	///	Вызывает сохранение данных
	///</summary>
	[Action("zefs.attachfile")]
	public class AttachFileAction : FormSessionActionBase {
		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize() {
			base.Initialize();
			_datafile = (HttpPostedFileBase) Context.GetFile("datafile");
		}

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			base.Validate();
			if (null == _datafile) {
				throw new Exception("not file provided");
			}
		}

		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return MySession.AttachFile(_datafile, filename, type, uid);
		}

		private HttpPostedFileBase _datafile;

		/// <summary>
		/// 	Пользовательское имя файла
		/// </summary>
		[Bind] protected string filename;

		/// <summary>
		/// 	Тип привязываемого документа
		/// </summary>
		[Bind(Required = true)] protected string type;

		/// <summary>
		/// 	Идентификатор существующего файла
		/// </summary>
		[Bind] protected string uid;
	}
}