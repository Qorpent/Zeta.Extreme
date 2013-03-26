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
// PROJECT ORIGIN: Zeta.Extreme.FrontEnd/SingleAttachmentActionBase.cs
#endregion
using System;
using System.Linq;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd.Actions.Attachments {
	/// <summary>
	/// 	Базовый класс действий, ориентированных на работу с конкретным присоединенным файлом
	/// </summary>
	public class SingleAttachmentActionBase : FormSessionActionBase {
		/// <summary>
		/// 	First phase of execution - override if need special input parameter's processing
		/// </summary>
		protected override void Initialize() {
			base.Initialize();
			_myattach = MySession.GetAttachedFiles().FirstOrDefault(_ => _.Uid == uid);
		}

		/// <summary>
		/// 	Second phase - validate INPUT/REQUEST parameters here - it called before PREPARE so do not try validate
		/// 	second-level internal state and authorization - only INPUT PARAMETERS must be validated
		/// </summary>
		protected override void Validate() {
			base.Validate();
			if (null == _myattach) {
				throw new Exception("cannot remove attach, not attached to current session");
			}
		}

		private FormAttachment _myattach;

		/// <summary>
		/// 	Идентификатор существующего файла
		/// </summary>
		[Bind(Required = true, ValidatePattern = @"^[\d\w\-_]+$")] protected string uid;
	}
}