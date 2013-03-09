#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ForSearchAttribute.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comdiv.Extensions;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Poco.Deprecated {
	/// <summary>
	/// 	Marks interfaces to be found throug total search between distinct databases
	/// </summary>
	public class ForSearchAttribute : Attribute {
		public ForSearchAttribute() {}

		public ForSearchAttribute(string name) {
			Name = name;
		}

		public string Name { get; set; }

		public static Type[] Collect(params Assembly[] assemblies) {
			if (assemblies.IsEmpty()) {
				assemblies =
					AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.GetName().Name.StartsWith("System")).ToArray();
			}
			var result = new List<Type>();
			foreach (var assembly in assemblies) {
				var types =
					assembly.GetTypes().Where(x => x.GetCustomAttributes(typeof (ForSearchAttribute), false).Length != 0);
				foreach (var type in types) {
					result.Add(type);
				}
			}
			return result.ToArray();
		}

		public static string GetName(Type type) {
			var attr =
				type.GetCustomAttributes(typeof (ForSearchAttribute), false).OfType<ForSearchAttribute>().FirstOrDefault
					();
			if (null == attr) {
				return type.Name;
			}
			if (attr.Name.noContent()) {
				return type.Name;
			}
			return attr.Name;
		}
	}
}