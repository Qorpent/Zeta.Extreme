#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : SchemaAttribute.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Deprecated {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class SchemaAttribute : MappingAttribute, IWithName {
		public SchemaAttribute(string name) {
			Name = name;
		}

		public string Name { get; set; }
	}
}