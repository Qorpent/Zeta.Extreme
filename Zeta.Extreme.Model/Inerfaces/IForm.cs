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
// PROJECT ORIGIN: Zeta.Extreme.Model/IForm.cs

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	///     Record about form session
	/// </summary>
	public interface IForm : IWithId, IWithCode, IWithVersion {
		/// <summary>
		///     <see cref="Year" /> of input form
		/// </summary>
		int Year { get; set; }

		/// <summary>
		///     Period of input form
		/// </summary>
		int Period { get; set; }

		/// <summary>
		///     Template code or input form
		/// </summary>
		string BizCaseCode { get; set; }

		/// <summary>
		///     History of states
		/// </summary>
		IList<IFormState> States { get; set; }

		/// <summary>
		///     Current state value
		/// </summary>
		string CurrentState { get; set; }

		/// <summary>
		///     Target object (ZetaObject) of form
		/// </summary>
		IZetaMainObject Object { get; set; }
	}
}