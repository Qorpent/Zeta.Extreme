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
// PROJECT ORIGIN: Zeta.Extreme.Model/BizTran.cs

#endregion

using Qorpent;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     Biz transaction definition implementation
	/// </summary>
	public sealed partial class BizTran : Entity, IBizTran {
		/// <summary>
		/// </summary>
		public BizTran() {
			Name = QorpentConst.NODEF;
			FirstType = QorpentConst.NODEF;
			FirstRole = QorpentConst.NODEF;
			FirstForm = QorpentConst.NODEF;
			SecondType = QorpentConst.NODEF;
			SecondRole = QorpentConst.NODEF;
			SecondForm = QorpentConst.NODEF;
		}

		/// <summary>
		///     Account type of first contragent
		/// </summary>
		public string FirstType { get; set; }

		/// <summary>
		///     Role of first contragent to second
		/// </summary>
		public string FirstRole { get; set; }

		/// <summary>
		///     Input form code of first contragent
		/// </summary>
		public string FirstForm { get; set; }

		/// <summary>
		///     Account type of second contragent
		/// </summary>
		public string SecondType { get; set; }

		/// <summary>
		///     Role of second contragent to first
		/// </summary>
		public string SecondRole { get; set; }

		/// <summary>
		///     Input form code of second contragent (inverse)
		/// </summary>
		public string SecondForm { get; set; }
	}
}