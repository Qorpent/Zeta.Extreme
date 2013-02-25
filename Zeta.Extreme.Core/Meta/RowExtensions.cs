// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
#define NEWMODEL

using Comdiv.Zeta.Model;
using Comdiv.Extensions;
using Qorpent.Utils;

#if NEWMODEL

#endif

namespace Zeta.Extreme.Meta{
	/// <summary>
	/// Расширения для строк
	/// </summary>
    public static class RowExtensions{
		internal static bool HasColChanger(this IZetaRow row, IZetaColumn col)
		{

			var linkcol = TagHelper.Value(col.Tag, "linkcol");
			if (linkcol.noContent()) return false;
			var sourcelink = row.ResolveTag("sourcelink");
			var complexhelper = ComplexStringHelper.CreateComplexStringParser();
			complexhelper.ValueDelimiter = "=";
			var dict = complexhelper.Parse(sourcelink);
			return dict.ContainsKey(linkcol);


		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public static IZetaRow ResolveExRef(this IZetaRow row, IZetaColumn column)
		{

			if (null == row)
			{
				return null;
			}
			
			if (column == null) return row;
			if (!column.IsMarkSeted("DOEXREF")) return row;
			while (row.ExRefTo != null)
			{
				if (row.HasColChanger(column)) break;
				row = row.ExRefTo;

			}
			return row;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public static bool HasChildren(this IZetaRow row) {
			return null != row.NativeChildren && 0 != row.NativeChildren.Count;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public static string GetSortKey(this IZetaRow row){
            return string.Format("{0:00000}_{1}_{2}", row.Idx, row.OuterCode ?? "" , row.Code);
        }
    }
}