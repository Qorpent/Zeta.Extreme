using System;
using System.Reflection;
using Comdiv.Application;
using Comdiv.Extensions;

namespace Comdiv.Olap.Model {
	public static class OlapCacheExtension{
		public static object GetValue(this IOlapCache cache){
			if (null == cache){
				return Missing.Value;
			}
			if (null == cache.ClassRef){
				return Missing.Value;
			}
			if (null == cache.Value){
				return null;
			}
			var type = cache.ClassRef.toType();
			return Convert.ChangeType(cache.Value, type);
		}

		public static IOlapCache SetValue(this IOlapCache cache, string key, object value){
			if (null == cache){
				cache = myapp.storage.Get<IOlapCache>().New();
				cache.Code = key;
			}

			if (null == value){
				cache.Value = null;
				cache.ClassRef = "System.Object, mscrorlib";
			}
			else{
				cache.Value = value.ToString();
				cache.ClassRef = value.GetType().FullName + ", mscorlib";
			}
			return cache;
		}
	}
}