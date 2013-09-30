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
// PROJECT ORIGIN: Zeta.Extreme.Model/Region.cs

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Deprecated {
	/// <summary>
	/// Middle level of old-style geo
	/// </summary>
	[Obsolete("ZC-417")]
	public partial class Region : Entity,IZetaRegion {
		/// <summary>
		///     Reference to container Zone
		/// </summary>
		public virtual IZetaZone Zone { get; set; }


		/// <summary>
		///     Collection of nested points
		/// </summary>
		public virtual IList<IZetaPoint> Points { get; set; }


		/// <summary>
		///     Collection of subordinated main objects
		/// </summary>
		public virtual IList<IZetaMainObject> MainObjects {
			get {
				if (null == _mainobjects) {
					var result = new List<IZetaMainObject>();
					foreach (var town in Points) {
						foreach (var mainObject in town.MainObjects) {
							result.Add(mainObject);
						}
					}
					_mainobjects = result;
				}
				return _mainobjects;
			}
			set { _mainobjects = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        public int? ZoneId {
            get {
                if (null != Zone) return Zone.Id;
                return _zoneId;
            }
            set {
                if (null != Zone) {
                    throw new Exception("cannot set ZoneId if Zone set");
                }
                _zoneId = value;
            }
        }
        private int? _zoneId;

	    private IList<IZetaMainObject> _mainobjects;
	    
	}
}