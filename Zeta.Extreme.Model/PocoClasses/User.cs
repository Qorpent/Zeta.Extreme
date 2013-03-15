#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : usr.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class User : IZetaUnderwriter {
		 public virtual IZetaMainObject Org { get; set; }

		 public virtual Guid Uid { get; set; }
		 public virtual string Login { get; set; }
		 public virtual bool Active { get; set; }

		 public virtual string Tag { get; set; }

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		/// <summary>
		/// 	Free list of documents,where basis for security provided
		/// </summary>
		 public virtual string Documents { get; set; }

		 public virtual DateTime Version { get; set; }

		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		 public virtual bool Boss { get; set; }

		 public virtual bool Worker { get; set; }

		 public virtual string Dolzh { get; set; }

		 public virtual string Contact { get; set; }

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
		/// 	An index of object
		/// </summary>
		public int Idx { get; set; }

		public virtual bool IsInRole(string role) {
			return Roles.SmartSplit().Contains(role);
		}

		private string _slotList;
		private IList<string> _slots;
	}
}