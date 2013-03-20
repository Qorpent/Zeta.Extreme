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
// PROJECT ORIGIN: Zeta.Extreme.Model/IBizTran.cs
#endregion
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Biz transaction definition
	/// </summary>
	public interface IBizTran : IEntity {
		/// <summary>
		/// Account type of first contragent
		/// </summary>
		 string FirstType { get; set; }
		 /// <summary>
		 /// Role of first contragent to second
		 /// </summary>
		 string FirstRole { get; set; }
		 /// <summary>
		 /// Input form code of first contragent
		 /// </summary>
		 string FirstForm { get; set; }
		 /// <summary>
		 /// Account type of second contragent
		 /// </summary>
		 string SecondType { get; set; }
		 /// <summary>
		 /// Role of second contragent to first
		 /// </summary>
		 string SecondRole { get; set; }
		 /// <summary>
		 /// Input form code of second contragent (inverse)
		 /// </summary>
		 string SecondForm { get; set; }
	}
}