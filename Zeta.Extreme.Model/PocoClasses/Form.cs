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
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     Input form session record
	/// </summary>
	public class Form : IForm {
		/// <summary>
		///     Идентификатор объекта
		/// </summary>
		public virtual int ObjectId { get; set; }

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IForm.Year" /> of input form
		/// </summary>
		public virtual int Year { get; set; }

		/// <summary>
		///     Period of input form
		/// </summary>
		public virtual int Period { get; set; }

		/// <summary>
		///     Template code or input form
		/// </summary>
		public virtual string BizCaseCode { get; set; }

		/// <summary>
		///     History of states
		/// </summary>
		public virtual IList<IFormState> States { get; set; }

		/// <summary>
		///     Current state value
		/// </summary>
		public virtual string CurrentState { get; set; }

		/// <summary>
		///     PK ID in database terms
		/// </summary>
		public virtual int Id { get; set; }


		/// <summary>
		///     Unique memo-code
		/// </summary>
		public virtual string Code { get; set; }


		/// <summary>
		///     User's or system's time stamp
		/// </summary>
		public virtual DateTime Version { get; set; }


		/// <summary>
		///     Target object (ZetaObject) of form
		/// </summary>
		public virtual IZetaMainObject Object { get; set; }
	}
}