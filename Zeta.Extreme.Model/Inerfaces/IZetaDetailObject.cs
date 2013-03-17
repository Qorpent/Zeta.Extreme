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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaDetailObject.cs
#endregion
using System;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	
	/// <summary>
	/// Detail Zeta object
	/// </summary>
	public interface IZetaDetailObject : IZetaObject,
		ICanResolveTag,
		IWithObjType,
		IWithDetailObjects, IWithOuterCode, IEntity,IWithCurrency,IContextEntity {
		/// <summary>
		/// Parent <see cref="IZetaDetailObject"/>
		/// </summary>
		IZetaDetailObject Parent { get; set; }
		/// <summary>
		/// Geo location of detail
		/// </summary>
		[Obsolete("ZC-417")]
		IZetaPoint Point { get; set; }
		/// <summary>
		/// Full name of detail
		/// </summary>
		string FullName { get; set; }
		 /// <summary>
		 /// Path in hierarhy
		 /// </summary>
		 string Path { get; set; }
		 /// <summary>
		 /// Main obj of detail
		 /// </summary>
		 IZetaMainObject Object { get; set; }
		 /// <summary>
		 /// Second obj of detail
		 /// </summary>
		 IZetaMainObject AltObject { get; set; }
		
		}
}