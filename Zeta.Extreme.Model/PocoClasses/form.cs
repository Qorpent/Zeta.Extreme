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
// PROJECT ORIGIN: Zeta.Extreme.Model/Form.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	public class Form : IForm {
		public virtual string TemplateCode { get; set; }

		/// <summary>
		/// 	Идентификатор объекта
		/// </summary>
		public virtual int ObjectId { get; set; }

		public virtual int Year { get; set; }

		public virtual int Period { get; set; }

		public virtual string Template { get; set; }

		public virtual IList<IFormState> States { get; set; }

		public virtual string CurrentState { get; set; }

		public virtual int Id { get; set; }


		public virtual string Code { get; set; }


		public virtual DateTime Version { get; set; }


		public virtual IZetaMainObject Object { get; set; }


		public virtual IFormState GetLastState() {
			var result = States != null ? States.OrderBy(x => x.Version).LastOrDefault() : null;
			if (null == result) {
				result = new FormState();
				result.Form = this;
				result.State = "0ISOPEN";
				result.Usr = "система";
			}
			return result;
		}

		public virtual IFormState GetLastBlock() {
			var result = States != null ? States.Where(x => x.State == "0ISBLOCK").OrderBy(x => x.Version).LastOrDefault() : null;
			if (null == result) {
				result = new FormState();
				result.Form = this;
				result.State = "0ISOPEN";
				result.Usr = "система";
			}
			return result;
		}
	}
}