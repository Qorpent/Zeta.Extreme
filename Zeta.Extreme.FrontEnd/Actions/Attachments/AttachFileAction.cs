#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/AttachFileAction.cs
#endregion
using System;
using System.Web;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Form.DbfsAttachmentSource;

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
			try {
				var dbfsAttacher = new DbfsAttachmentStorage();
				var formattacher = new FormAttachmentSource {InternalStorage = dbfsAttacher};
				formattacher.AttachHttpFile(MySession, _datafile, filename, type, uid);
			}
			catch {
				
			}
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