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
// PROJECT ORIGIN: Zeta.Extreme.Form/ThemaExtensions.cs
#endregion
using System.Collections.Generic;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Themas;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// 	Вспомогательные расширения тем
	/// </summary>
	public static class ThemaExtensions {
		/// <summary>
		/// 	Привязать родительские темы
		/// </summary>
		/// <param name="active"> </param>
		/// <typeparam name="T"> </typeparam>
		/// <returns> </returns>
		public static IEnumerable<T> BindParents<T>(this IEnumerable<T> active) where T : IThema {
			var full = active.UnGroup();
			foreach (var thema in full) {
				if (thema.Parent.IsNotEmpty()) {
					thema.ParentThema = full.FirstOrDefault(x => x.Code == thema.Parent);
					if (thema.ParentThema != null) {
						thema.ParentThema.Children.Add(thema);
					}
				}
			}
			return active;
		}

		/// <summary>
		/// 	Проверить избранное
		/// </summary>
		/// <param name="active"> </param>
		/// <typeparam name="T"> </typeparam>
		/// <returns> </returns>
		public static IEnumerable<T> CheckFavorities<T>(this IEnumerable<T> active) where T : class, IThema {
			var onlyfav = false;
			var src = active.UnGroup().ToArray();
			IDictionary<string, object> fav = null;//myapp.getProfile().GetAsDictionary("favoritethemas");
			foreach (var src1 in src) {
				src1.IsFavorite = false;
			}
			if (onlyfav) {
				return CheckFavorities(active, fav, src);
			}
			else {
				foreach (var src1 in src) {
					src1.IsFavorite = fav.SafeGet(src1.Code, false);
				}
			}
			return active;
		}

		private static IEnumerable<T> CheckFavorities<T>(IEnumerable<T> active, IDictionary<string, object> fav, T[] src) where T : class, IThema {
			if (fav.Count() == 0) {
				return new List<T>();
			}
			var toremove = new List<T>();


			foreach (var th in src) {
				if (th.IsGroup) {
					continue;
				}
				if (!fav.SafeGet(th.Code, false)) {
					toremove.Add(th);
				}
			}

			var result = new List<T>();

			foreach (var th in active) {
				if (th.IsGroup) {
					var newg = (th as Thema).Clone(true) as Thema;
					newg.GroupMembers.Clear();
					foreach (var thema in th.GetGroup()) {
						if (!toremove.Contains((T) thema)) {
							thema.IsFavorite = true;
							newg.GroupMembers.Add(thema);
						}
					}
					if (newg.GroupMembers.Count != 0) {
						result.Add(newg as T);
					}
					continue;
				}
				if (th.Group.IsEmpty()) {
					if (!toremove.Contains(th)) {
						th.IsFavorite = true;
						result.Add(th);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 	Разгруппировать темы
		/// </summary>
		/// <param name="active"> </param>
		/// <typeparam name="T"> </typeparam>
		/// <returns> </returns>
		public static IEnumerable<T> UnGroup<T>(this IEnumerable<T> active) where T : IThema {
			foreach (var t in active) {
				yield return t;
				if (t.IsGroup) {
					foreach (var t2 in t.GetGroup().UnGroup()) {
						yield return (T) t2;
					}
				}
			}
		}
	}
}