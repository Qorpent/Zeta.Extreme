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
// PROJECT ORIGIN: Zeta.Extreme.Model/CellHistory.cs

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     Implements <see cref="ICellHistory" /> as poco
	/// </summary>
	public class CellHistory : ICellHistory {
		/// <summary>
		///     Type of action
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		///     Merged code of source <see cref="Cell" />
		/// </summary>
		public string PseudoCode { get; set; }

		/// <summary>
		///     ID of target <see cref="Cell" />
		/// </summary>
		public int CellId { get; set; }

		/// <summary>
		///     user of change
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///     Timestamp of change
		/// </summary>
		public DateTime Time { get; set; }

		/// <summary>
		///     New value
		/// </summary>
		public string Value { get; set; }
	}
}