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
// PROJECT ORIGIN: Zeta.Extreme.Model/Point.cs

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Deprecated {
	/// <summary>
	///     Lowest level of old-style geo implementation
	/// </summary>
	[Obsolete("ZC-417")]
	public partial class Point : Entity, IZetaPoint {
	
		/// <summary>
		///     Collection of subordinated main objects
		/// </summary>
		public virtual IList<IZetaMainObject> MainObjects { get; set; }

		/// <summary>
		///     Reference to container
		///     <see cref="IZetaPoint.Region" />
		/// </summary>
		public virtual IZetaRegion Region { get; set; }


		/// <summary>
		///     Nested detail objects of point
		/// </summary>
		public virtual IList<IZetaDetailObject> DetailObjects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? RegionId
        {
            get
            {
                if (null != Region) return Region.Id;
                return _regionId;
            }
            set
            {
                if (null != Region)
                {
                    throw new Exception("cannot set ZoneId if Zone set");
                }
                _regionId = value;
            }
        }
        private int? _regionId;
	}
}