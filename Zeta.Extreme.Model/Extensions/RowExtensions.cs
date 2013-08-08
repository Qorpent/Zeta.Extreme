#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/RowExtensions.cs
#endregion
#define NEWMODEL

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Qorpent.Utils;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

#if NEWMODEL

#endif

namespace Zeta.Extreme.Model.Extensions {
	/// <summary>
	/// 	Расширения для строк
	/// </summary>
	public static class RowExtensions {
		public static bool HasColChanger(this IZetaRow row, IZetaColumn col) {
			string linkcol = "";
			linkcol= TagHelper.Value(col.Tag, "linkcol");
			if (linkcol.IsEmpty()) {
				return false;
			}
			var dict = GetSourceLinks(row);
			return dict.ContainsKey(linkcol);
		}

		public static bool GetIsPrimary(this IZetaRow r) {
			return !r.IsFormula 
				&& !r.IsMarkSeted("0SA") 
				&& 0 == r.Children.Count 
				&& null == r.RefTo 
				&& !r.LocalProperties.ContainsKey("readonly")
				&& !r.IsMarkSeted("0NOINPUT")
				;
		}

		public static string GetRedirectColCode(this IZetaRow row, IZetaColumn col) {
			string linkcol= "";
			linkcol= TagHelper.Value(col.Tag, "linkcol");
			if (linkcol.IsEmpty()) {
				return col.Code;
			}
			var dict = GetSourceLinks(row);
			if (dict.ContainsKey(linkcol)) {
				return dict[linkcol];
			}
			return col.Code;
		}

		public static IDictionary<string, string> GetSourceLinks(IZetaRow row) {
			var sourcelink = row.ResolveTag("sourcelink");
			if(string.IsNullOrWhiteSpace(sourcelink))return new Dictionary<string, string>();
			var complexhelper = ComplexStringHelper.CreateComplexStringParser();
			complexhelper.ValueDelimiter = "=";
			var dict = complexhelper.Parse(sourcelink);
			return dict;
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
			return row.HasChildren() && 0 != row.Children.Count;
		}

		/// <summary>
		/// </summary>
		/// <param name="row"> </param>
		/// <returns> </returns>
		public static string GetSortKey(this IZetaRow row) {
			return string.Format("{0:00000}_{1}_{2}", row.Index, ConvertToSortNumber(row.OuterCode) ?? "", row.Code);
		}

		private static string ConvertToSortNumber(string outerCode) {
			var s = outerCode.Trim().Replace(" ", "").Replace("\t", "");
			if (Regex.IsMatch(s, @"^(\d+\.?)+$")) {
				return Regex.Replace(s, @"((\d+)\.?)", _ => {
					var i = _.Groups[2].ToInt();
					return (1000 + i).ToString();
				});
			}
			return s;
		}
	}
}