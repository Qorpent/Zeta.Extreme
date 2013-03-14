#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : WithMarksExtension.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.Extensions {
	public static class WithMarksExtension {
		public static bool UseMarkCaching = true;

		public static bool IsMarkSeted(this IWithMarkCache obj, string code) {
			if (obj.MarkCache == null) {
				return false;
			}
			return obj.MarkCache.Contains("/" + code + "/");
		}
	}
}