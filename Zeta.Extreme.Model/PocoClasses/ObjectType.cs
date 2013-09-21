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
// PROJECT ORIGIN: Zeta.Extreme.Model/ObjType.cs

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Implementation of zeta obj type
	/// </summary>
	public partial class ObjectType : Entity,IObjectType {
		/// <summary>
		/// list of attached details
		/// </summary>
		public virtual IList<IZetaDetailObject> Details { get; set; }


		/// <summary>
		///     Reference to container obj class
		/// </summary>
		[Obsolete("ZC-416 must be replaced")] 
		public virtual IZetaObjectClass Class { get; set; }


		/// <summary>
		/// Resolves tag value by it's name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.IsEmpty()) {
				tag = Class.ResolveTag(name);
			}
			return tag ?? "";
		}

        private int? _classId;
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
        public int? ClassId
        {
            get
            {

                if (null != Class)

                {
                    return Class.Id;
                }
                return _classId;
            }
            set
            {
                if (null != Class)
                {
                    throw new Exception("cannot setup ClassId when Class is attached");
                }
                _classId = value;
            }
        }
#pragma warning restore 612,618
    }
}