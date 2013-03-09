#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : RowExtensions.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

#define NEWMODEL

using Qorpent.Utils;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Inerfaces;

#if NEWMODEL

#endif

namespace Zeta.Extreme.Meta {
	/// <summary>
	/// 	Расширения для строк
	/// </summary>
	public static class RowExtensions {
		internal static bool HasColChanger(this IZetaRow row, IZetaColumn col) {
			var linkcol = TagHelper.Value(col.Tag, "linkcol");
			if (linkcol.IsEmpty()) {
				return false;
			}
			var sourcelink = row.ResolveTag("sourcelink");
			var complexhelper = ComplexStringHelper.CreateComplexStringParser();
			complexhelper.ValueDelimiter = "=";
			var dict = complexhelper.Parse(sourcelink);
			return dict.ContainsKey(linkcol);
		}


		/// <summary>
		/// </summary>
		/// <param name="row"> </param>
		/// <param name="column"> </param>
		/// <returns> </returns>
		public static IZetaRow ResolveExRef(this IZetaRow row, IZetaColumn column) {
			if (null == row) {
				return null;
			}

			if (column == null) {
				return row;
			}
			if (!column.IsMarkSeted("DOEXREF")) {
				return row;
			}
			while (row.ExRefTo != null) {
				if (row.HasColChanger(column)) {
					break;
				}
				row = row.ExRefTo;
			}
			return row;
		}

		/// <summary>
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		public static bool HasChildren(this IZetaRow row) {
			return null != row.NativeChildren && 0 != row.NativeChildren.Count;
		}

		/// <summary>
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		public static string GetSortKey(this IZetaRow row) {
			return string.Format("{0:00000}_{1}_{2}", row.Idx, row.OuterCode ?? "", row.Code);
		}
	}
}