#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : RefAttribute.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Poco.Deprecated {
	public class RefAttribute : MappingAttribute {
		public bool IsAutoComplete { get; set; }
		public string AutoComplete { get; set; }
		public string AutoCompleteType { get; set; }

		public string LookupData { get; set; }
		public Type ClassName { get; set; }

		public string Alias { get; set; }
		public string Title { get; set; }

		public bool Nullable { get; set; }
	}
}