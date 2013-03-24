using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Model.MetaCaches {
	/// <summary>
	/// Extensions over meta cache, that provides additional meta-awared methods, usefull for querying
	/// </summary>
	public static class MetaExtensions {
		/// <summary>
		/// Retrievs array of zeta object's IDs by zone alias
		/// </summary>
		/// <param name="metacache"></param>
		/// <param name="alias"></param>
		/// <returns></returns>
		/// <exception cref="FormatException"></exception>
		public static IEnumerable<int> ResolveZoneAliasToObjectIds(this IMetaCache cache, string alias) {
			int intval;
			if ("0" == alias) yield break;
			if (0 != (intval = alias.ToInt())) {
				yield return intval;
				yield break;
			}
			var split = alias.Split('_');
			if (2 != split.Length) {
				throw new FormatException("given alias cannot be treated as obj zone def " + alias);
			}
			if (split[0] == "obj") {
				if (0 != (intval = alias.ToInt()))
				{
					yield return intval;
				}
			}
			else if (split[0] == "div") {
				var div = cache.Get<Division>(split[1]);
				if (null == div || null==div.MainObjects) {
					yield break;
				}
				foreach (var mainObject in div.MainObjects) {
					yield return mainObject.Id;
				}
			}else if (split[0] == "grp") {
				var ids = ObjCache.ObjById.Where(_ => _.Value.GroupCache.Contains("/" + split[1] + "/")).Select(_ => _.Value.Id);
				foreach (var id in ids) {
					yield return id;
				}
			}
			else {
				throw new FormatException("given alias cannot be treated as obj zone def "+alias);
			}
		}
	}
}