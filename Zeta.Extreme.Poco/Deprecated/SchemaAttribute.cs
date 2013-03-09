using System;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Deprecated {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class SchemaAttribute : MappingAttribute,IWithName {
		public SchemaAttribute(string name) {
			this.Name = name;
		}

		public string Name { get;  set; }
	}
}