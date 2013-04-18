using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;


namespace Zeta.Extreme.Model.MetaCaches {
	/// <summary>
	/// Extensions over meta cache, that provides additional meta-awared methods, usefull for querying
	/// </summary>
	public static class MetaExtensions {
		
		/// <summary>
		/// Проверяет, относится ли указанный объект к какому-либо групповому альясу
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="alias"></param>
		/// <returns></returns>
		public static bool IsMatchAliases(this IZetaMainObject obj, string alias) {
			var aliases = alias.SmartSplit();
			foreach (var a in aliases) {
				if (a.StartsWith("div_")) {
					var divcode = a.Substring(4);
					if (obj.Division != null && obj.Division.Code == divcode) return true;
				}
				else if (a.StartsWith("obj_"))
				{
					var objids = a.Substring(4);
					if (obj.Id.ToString() == objids) return true;
				}
				else if (a.StartsWith("grp_")) {
					var grpcode = a.Substring(4);
					if (!string.IsNullOrWhiteSpace(obj.GroupCache) && obj.GroupCache.Contains("/" + grpcode + "/")) return true;
				}else if (obj.Id.ToString() == a) {
					return true;
				}else if (!string.IsNullOrWhiteSpace(obj.GroupCache) && obj.GroupCache.Contains("/" + a + "/")) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Retrievs array of zeta object's IDs by zone alias
		/// </summary>
		/// <param name="metacache"></param>
		/// <param name="alias"></param>
		/// <returns></returns>
		/// <exception cref="FormatException"></exception>
		public static IEnumerable<int> ResolveZoneAliasToObjectIds(this IMetaCache cache, string alias,IZetaMainObject currentObject=null) {
			int intval;
			if ("0" == alias) yield break;
			if (0 != (intval = alias.ToInt())) {
				yield return intval;
				yield break;
			}
			var split = alias.Split('_');
			if (1 == split.Length) {
				var ids = ObjCache.ObjById.Where(_ => _.Value.GroupCache.Contains("/" + split[0] + "/")).Select(_ => _.Value.Id);
				foreach (var id in ids)
				{
					yield return id;
				}
				yield break;
			}
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
				if (split[1] == "CURRENT")
				{
					if (null == currentObject) {
						throw new Exception("cannot resolve without current object");
					}
					if (null == currentObject.Division) {
						throw new Exception("current obj have not division");
					}
					div = (Division) currentObject.Division;
				}
				
				
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