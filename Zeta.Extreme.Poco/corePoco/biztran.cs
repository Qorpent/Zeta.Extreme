﻿#region LICENSE

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

		[Map] public virtual int Id { get; set; }

		[Map] public virtual string Tag { get; set; }

		[Map] public virtual string Name { get; set; }

		[Map] public virtual string Code { get; set; }

		[Map] public virtual string Comment { get; set; }

		[Map] public virtual DateTime Version { get; set; }

		[Map] public virtual string FirstType { get; set; }

		[Map] public virtual string FirstRole { get; set; }

		[Map] public virtual string FirstForm { get; set; }

		[Map] public virtual string SecondType { get; set; }

		[Map] public virtual string SecondRole { get; set; }

		[Map] public virtual string SecondForm { get; set; }
	}
}