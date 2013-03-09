#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ClassicAttributeExtension.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Extensions;

namespace Zeta.Extreme.Poco.Deprecated {
	public static class ClassicAttributeExtension {
		public static string GetClassicName(this Type type, bool useClassic) {
			ClassicAttribute result = null;
			if (useClassic) {
				result = type.getFirstAttribute<ClassicAttribute>();
			}
			if (null == result) {
				return type.Name.Substring(1);
			}
			return result.Name;
		}

		public static string GetClassicName(this Type type, string propertyName, bool useClassic) {
			ClassicAttribute result = null;
			if (useClassic) {
				var property = type.resolveProperty(propertyName);
				result = property.getFirstAttribute<ClassicAttribute>();
			}
			if (null == result) {
				return propertyName;
			}
			return result.Name;
		}
	}
}