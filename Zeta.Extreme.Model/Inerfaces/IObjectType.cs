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
// PROJECT ORIGIN: Zeta.Extreme.Model/IObjectType.cs

#endregion

using System;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	///     Low level of zeta obj type hierarchy
	/// </summary>
	public interface IObjectType :
		IZetaObject,
		ICanResolveTag,
		IWithDetailObjects,
		IEntity {
		/// <summary>
		///     Reference to container obj class
		/// </summary>
		[Obsolete("ZC-416 must be replaced")] IZetaObjectClass Class { get; set; }

	    /// <summary>
	    ///     ID (FK) of <see cref="Zeta.Extreme.Model.Obj.ObjType" /> that current is
	    ///     attached to
	    /// </summary>
	    /// <remarks>
	    ///     Intended to use with ORM/SQL scenario
	    /// </remarks>
	    /// <exception cref="Exception">
	    ///     cannot setup <see cref="Zeta.Extreme.Model.Obj.ObjTypeId" /> when ObjType
	    ///     is attached
	    /// </exception>
#pragma warning disable 612,618
	    int? ClassId { get; set; }
	}
}