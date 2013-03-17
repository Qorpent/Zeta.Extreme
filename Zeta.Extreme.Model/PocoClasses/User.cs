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
// PROJECT ORIGIN: Zeta.Extreme.Model/User.cs

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	public partial class User : IZetaUser {
		public virtual IZetaMainObject Org { get; set; }

		public virtual Guid Uid { get; set; }
		public virtual string Login { get; set; }
		public virtual bool Active { get; set; }
		public virtual bool Boss { get; set; }

		public virtual bool Worker { get; set; }

		public virtual string Dolzh { get; set; }

		public virtual string Contact { get; set; }

		public virtual string Tag { get; set; }

		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual string Code { get; set; }

		public virtual string Comment { get; set; }

		/// <summary>
		///     Free list of documents,where basis for security provided
		/// </summary>
		public virtual string Documents { get; set; }

		public virtual DateTime Version { get; set; }

		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		public virtual string Roles { get; set; }

		public virtual string Login2 { get; set; }

		public virtual string SlotList {
			get { return _slotList; }
			set {
				_slotList = value;
				_slots = null;
			}
		}

		public virtual IList<string> Slots {
			get { return _slots ?? (_slots = SlotList.SmartSplit()); }
		}

		public virtual bool IsFor(string slot) {
			return Slots.Contains(slot);
		}

		/// <summary>
		///     An index of object
		/// </summary>
		public int Index { get; set; }

		public virtual bool IsInRole(string role) {
			return Roles.SmartSplit().Contains(role);
		}

		private string _slotList;
		private IList<string> _slots;
	}
}