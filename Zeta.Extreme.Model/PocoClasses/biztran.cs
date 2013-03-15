#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : biztran.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.PocoClasses {
	public partial class BizTran : IBizTran {
		private const string nodef = "Не определено";

		public BizTran() {
			Name = nodef;
			FirstType = nodef;
			FirstRole = nodef;
			FirstForm = nodef;
			SecondType = nodef;
			SecondRole = nodef;
			SecondForm = nodef;
		}

		 public virtual int Id { get; set; }

		 public virtual string Tag { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

		 public virtual string FirstType { get; set; }

		 public virtual string FirstRole { get; set; }

		 public virtual string FirstForm { get; set; }

		 public virtual string SecondType { get; set; }

		 public virtual string SecondRole { get; set; }

		 public virtual string SecondForm { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public virtual int Idx { get; set; }
	}
}