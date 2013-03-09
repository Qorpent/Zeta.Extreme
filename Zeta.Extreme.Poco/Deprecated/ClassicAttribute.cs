#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ClassicAttribute.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Poco.Deprecated {
	public class ClassicAttribute : Attribute {
		public ClassicAttribute(string name) {
			Name = name;
		}

		public string Name { get; set; }
	}
}