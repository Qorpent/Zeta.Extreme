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
// PROJECT ORIGIN: Zeta.Extreme.Model/ObjectGroup.cs
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Model {
	public partial class ObjectGroup : IZetaObjectGroup {
		public virtual string Tag { get; set; }

		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual string Code { get; set; }

		public virtual string Comment { get; set; }

		public virtual DateTime Version { get; set; }

		public virtual IList<IZetaMainObject> MainObjects {
			get {
				return _objcache ??
				       (_objcache =
				        new NativeZetaReader().ReadObjects("from ENTITY x where GroupCache like '%/'+" + Code + "+'/%'").OfType
					        <IZetaMainObject>().ToList()
				       )
					;
			}
			set {
				if (null == value) {
					_objcache = null;
				}
			}
		}

		/// <summary>
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }

		private IList<IZetaMainObject> _objcache;
	}
}