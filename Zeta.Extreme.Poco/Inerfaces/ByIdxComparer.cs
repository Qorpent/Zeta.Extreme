#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ByIdxComparer.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Poco.Inerfaces {
	public class ByIdxComparer<T> : IComparer<T> where T : IWithCode, IWithIdx {
		public int Compare(T x, T y) {
			if (x.Idx != y.Idx) {
				if (0 == x.Idx) {
					return 1;
				}
				if (0 == y.Idx) {
					return -1;
				}
				return x.Idx.CompareTo(x.Idx);
			}
			return x.Code.CompareTo(y.Code);
		}
	}
}