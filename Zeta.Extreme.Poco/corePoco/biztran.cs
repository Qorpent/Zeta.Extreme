#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : biztran.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;

namespace Zeta.Extreme.Poco {
	public partial class biztran : IBizTran {
		private const string nodef = "Не определено";

		public biztran() {
			Name = nodef;
			FirstType = nodef;
			FirstRole = nodef;
			FirstForm = nodef;
			SecondType = nodef;
			SecondRole = nodef;
			SecondForm = nodef;
		}

		[Deprecated.Map] public virtual int Id { get; set; }

		[Deprecated.Map] public virtual string Tag { get; set; }

		[Deprecated.Map] public virtual string Name { get; set; }

		[Deprecated.Map] public virtual string Code { get; set; }

		[Deprecated.Map] public virtual string Comment { get; set; }

		[Deprecated.Map] public virtual DateTime Version { get; set; }

		[Deprecated.Map] public virtual string FirstType { get; set; }

		[Deprecated.Map] public virtual string FirstRole { get; set; }

		[Deprecated.Map] public virtual string FirstForm { get; set; }

		[Deprecated.Map] public virtual string SecondType { get; set; }

		[Deprecated.Map] public virtual string SecondRole { get; set; }

		[Deprecated.Map] public virtual string SecondForm { get; set; }

		/// <summary>
		/// 	An index of object
		/// </summary>
		public virtual int Idx { get; set; }
	}
}