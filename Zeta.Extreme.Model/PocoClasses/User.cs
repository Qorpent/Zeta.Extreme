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
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Zeta user implementation
	/// </summary>
	public partial class User : Entity, IZetaUser {
		/// <summary>
		/// reference to container object
		/// </summary>
		public virtual IZetaMainObject Object { get; set; }

		/// <summary>
		/// Main login
		/// </summary>
		public virtual string Login { get; set; }

		/// <summary>
		/// 	True - объект активен
		/// </summary>
		public virtual bool Active { get; set; }

		/// <summary>
		/// marks that user is local admin
		/// </summary>
		public virtual bool IsLocalAdmin { get; set; }


		/// <summary>
		/// Occupation of user
		/// </summary>
		public virtual string Occupation { get; set; }
		/// <summary>
		/// Contact info
		/// </summary>
		public virtual string Contact { get; set; }


		/// <summary>
		///     Free list of documents,where basis for security provided
		/// </summary>
		public virtual string Documents { get; set; }


		/// <summary>
		/// s-list of roles given to user
		/// </summary>
		public virtual string Roles { get; set; }

		/// <summary>
		/// Alt login
		/// </summary>
		public virtual string Login2 { get; set; }

		/// <summary>
		/// s-list of slots of underwrite docxs
		/// </summary>
		public virtual string SlotList {
			get { return _slotList; }
			set {
				_slotList = value;
				_slots = null;
			}
		}

		/// <summary>
		/// Normalized lost of slots
		/// </summary>
		public virtual IList<string> Slots {
			get { return _slots ?? (_slots = SlotList.SmartSplit()); }
		}

		private string _slotList;
		private IList<string> _slots;

		/// <summary>
		/// 	Дата начала
		/// </summary>
		public DateTime Start { get; set; }

		/// <summary>
		/// 	Дата окончания
		/// </summary>
		public DateTime Finish { get; set; }
	}
}