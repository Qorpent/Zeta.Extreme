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
using Zeta.Extreme.Poco.Deprecated;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class usr : IZetaUnderwriter {
		[Ref(ClassName = typeof (obj))] public virtual IZetaMainObject Org { get; set; }

		[Map] public virtual Guid Uid { get; set; }
		[Map] public virtual string Login { get; set; }
		[Map] public virtual bool Active { get; set; }

		[Map] public virtual string Tag { get; set; }

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		/// <summary>
		/// 	Free list of documents,where basis for security provided
		/// </summary>
		[Map] public virtual string Documents { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		[Map] public virtual bool Boss { get; set; }

		[Map] public virtual bool Worker { get; set; }

		[Map] public virtual string Dolzh { get; set; }

		[Map] public virtual string Contact { get; set; }

		[Map] public virtual string Roles { get; set; }

		[Map] public virtual string Login2 { get; set; }

		[Map] public virtual string SlotList {
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