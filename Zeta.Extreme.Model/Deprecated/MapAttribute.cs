#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : MapAttribute.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Model.Deprecated {
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class MapAttribute : MappingAttribute {
		public MapAttribute(string colname = "", bool notnull = false, string type = "", string title = "") {
			Title = title;
			ColumnName = colname;
			NotNull = notnull;
		}

		public string Title { get; set; }
		public string ColumnName { get; set; }
		public bool NotNull { get; set; }

		public bool ReadOnly { get; set; }

		public Type CustomType { get; set; }

		public string Formula { get; set; }

		public bool Cascade { get; set; }

		public bool UseMaxLength { get; set; }

		public bool NoLazy { get; set; }
	}
}