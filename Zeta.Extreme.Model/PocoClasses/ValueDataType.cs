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
// PROJECT ORIGIN: Zeta.Extreme.Model/ValueDataType.cs

#endregion

using System;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Available column data type
	/// </summary>
	[Flags]
	public enum ValueDataType {
		/// <summary>
		/// 
		/// </summary>
		Decimal = 0,
		/// <summary>
		/// 
		/// </summary>
		Int = 1,
		/// <summary>
		/// 
		/// </summary>
		Bool = 2,
		/// <summary>
		/// 
		/// </summary>
		Date = 3,
		/// <summary>
		/// 
		/// </summary>
		String = 4,
		/// <summary>
		/// 
		/// </summary>
		Dictionary = 5,
		/// <summary>
		/// type name
		/// </summary>
		Class = 6,
		/// <summary>
		/// lookup reference
		/// </summary>
		Lookup = 7,
		/// <summary>
		/// 
		/// </summary>
		Undefined = 8,
	}
}