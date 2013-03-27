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
// PROJECT ORIGIN: Zeta.Extreme.Model/IBizTranFilter.cs
#endregion
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Filter for <see cref="BizTran"/> at implementation level
	/// </summary>
	public interface IBizTranFilter : IWithId {
		 /// <summary>
		 /// Action for filter
		 /// </summary>
		 /// <remarks>0 - disbled filter, 1 - allow filter, -1 - deny filter</remarks>
		 int Action { get; set; }

		 /// <summary>
		 /// ID of main contragent
		 /// </summary>
		 int MainId { get; set; }

		 /// <summary>
		 ///ID of second contragent 
		 /// </summary>
		 int ContragentId { get; set; }

		 /// <summary>
		 ///CODE of <see cref="BizTran"/>
		 /// </summary>
		 string TranCode { get; set; }

		 /// <summary>
		 /// Role of second contragent
		 /// </summary>
		 string Role { get; set; }

		 /// <summary>
		 /// CODE of target row in link
		 /// </summary>
		 string RowCode { get; set; }

		 /// <summary>
		 /// CODE of form of first contragent
		 /// </summary>
		 string FirstForm { get; set; }

		 /// <summary>
		 /// CODE of bill of first contragent
		 /// </summary>
		 string FirstType { get; set; }

		 /// <summary>
		 /// CODE of bill of second contragent
		 /// </summary>
		 string SecondType { get; set; }


		 /// <summary>
		 /// CODE of form of second contragent
		 /// </summary>
		 string SecondForm { get; set; }
	}
}