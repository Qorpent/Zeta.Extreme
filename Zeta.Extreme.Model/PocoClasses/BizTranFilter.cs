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
// PROJECT ORIGIN: Zeta.Extreme.Model/BizTranFilter.cs

#endregion

using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// </summary>
	public partial class BizTranFilter : IBizTranFilter {
		/// <summary>
		///     PK ID in database terms
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     <see cref="Action" /> for filter
		/// </summary>
		/// <remarks>
		///     <list type="number">
		///         <item>
		///             <description>
		///                 - disbled filter, 1 - allow filter, -1 - deny filter
		///             </description>
		///         </item>
		///     </list>
		/// </remarks>
		public int Action { get; set; }

		/// <summary>
		///     ID of main contragent
		/// </summary>
		public int MainId { get; set; }

		/// <summary>
		///     ID of second contragent
		/// </summary>
		public int ContragentId { get; set; }

		/// <summary>
		///     CODE of <see cref="BizTran" />
		/// </summary>
		public string TranCode { get; set; }

		/// <summary>
		///     <see cref="Role" /> of second contragent
		/// </summary>
		public string Role { get; set; }

		/// <summary>
		///     CODE of target row in link
		/// </summary>
		public string RowCode { get; set; }

		/// <summary>
		///     CODE of form of first contragent
		/// </summary>
		public string FirstForm { get; set; }

		/// <summary>
		///     CODE of bill of first contragent
		/// </summary>
		public string FirstType { get; set; }

		/// <summary>
		///     CODE of bill of second contragent
		/// </summary>
		public string SecondType { get; set; }

		/// <summary>
		///     CODE of form of second contragent
		/// </summary>
		public string SecondForm { get; set; }
	}
}