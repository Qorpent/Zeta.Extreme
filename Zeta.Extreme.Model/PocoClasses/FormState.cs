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
// PROJECT ORIGIN: Zeta.Extreme.Model/FormState.cs

#endregion

using System;
using Qorpent.Serialization;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     <see cref="FormState" /> implementation
	/// </summary>
	[Serialize]
	public class FormState : IFormState {
		/// <summary>
		///     Идентификатор формы
		/// </summary>
		[IgnoreSerialize] public int FormId { get; set; }

		/// <summary>
		///     Идентификатор родительского статуса
		/// </summary>
		[SerializeNotNullOnly] public int ParentId { get; set; }

		/// <summary>
		///     Form reference
		/// </summary>
		[IgnoreSerialize] public virtual IForm Form { get; set; }

		/// <summary>
		///     Code of state
		/// </summary>
		public virtual string State { get; set; }

		/// <summary>
		/// </summary>
		public virtual string User { get; set; }

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IFormState.Parent" /> form state
		///     (for cascaded states)
		/// </summary>
		[SerializeNotNullOnly] public virtual IFormState Parent { get; set; }

		/// <summary>
		///     PK ID in database terms
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		///     User's or system's time stamp
		/// </summary>
		public virtual DateTime Version { get; set; }

		/// <summary>
		///     User's comment string
		/// </summary>
		[SerializeNotNullOnly] public virtual string Comment { get; set; }
	}
}