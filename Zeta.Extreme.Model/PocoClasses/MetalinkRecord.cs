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
// PROJECT ORIGIN: Zeta.Extreme.Model/MetalinkRecord.cs

#endregion

using System;
using Qorpent;
using Qorpent.Model;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     Metalink implementation
	/// </summary>
	public class MetalinkRecord : IWithId, IWithCode, IWithActive, IWithDateRange, IWithTag {
		/// <summary>
		/// </summary>
		public MetalinkRecord() {
			Active = true;
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
		}

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.MetalinkRecord.Source" /> object type
		/// </summary>
		public string SourceType { get; set; }

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.MetalinkRecord.Target" /> object type
		/// </summary>
		public string TargetType { get; set; }

		/// <summary>
		///     <see cref="Source" /> <c>object</c>
		/// </summary>
		public string Source { get; set; }

		/// <summary>
		///     <see cref="Target" /> <c>object</c>
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		///     Link type
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		///     Link sub-type
		/// </summary>
		public string SubType { get; set; }

		/// <summary>
		///     True - объект активен
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		///     Unique memo-code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		///     Дата начала
		/// </summary>
		public DateTime Start { get; set; }

		/// <summary>
		///     Дата окончания
		/// </summary>
		public DateTime Finish { get; set; }

		/// <summary>
		///     PK ID in database terms
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///     The TAG string
		/// </summary>
		public string Tag { get; set; }
	}
}