#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjCache.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Poco.NativeSqlBind {
	/// <summary>
	/// 	Кэш данных по старшим объектам
	/// </summary>
	public static class ObjCache {
		private static readonly IDictionary<int, IZetaMainObject> _objById = new Dictionary<int, IZetaMainObject>();
		private static readonly IDictionary<string, IZetaMainObject> _objByCode = new Dictionary<string, IZetaMainObject>();
		private static readonly IDictionary<int, IMainObjectGroup> _divById = new Dictionary<int, IMainObjectGroup>();
		private static readonly IDictionary<string, IMainObjectGroup> _divByCode = new Dictionary<string, IMainObjectGroup>();
		private static readonly object startsync = new object();
		private static bool _instart;

		/// <summary>
		/// 	Словарь ID->Obj
		/// </summary>
		public static IDictionary<int, IZetaMainObject> ObjById {
			get { return _objById; }
		}

		/// <summary>
		/// 	Словарь Code->Obj
		/// </summary>
		public static IDictionary<string, IZetaMainObject> ObjByCode {
			get { return _objByCode; }
		}

		/// <summary>
		/// 	Словарь Id->Div
		/// </summary>
		public static IDictionary<int, IMainObjectGroup> DivById {
			get { return _divById; }
		}

		/// <summary>
		/// 	Словарь Code->Div
		/// </summary>
		public static IDictionary<string, IMainObjectGroup> DivByCode {
			get { return _divByCode; }
		}

		/// <summary>
		/// 	Получение объекта по ключу
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		public static IZetaMainObject Get(object key) {
			if (_instart) {
				lock (startsync) {}
			}
			if (key is int) {
				var id = (int) key;
				return ResolveObjectById(id);
			}
			var code = key as string;
			if (code != null) {
				return ResolveObjectByCode(code);
			}
			return null;
		}

		/// <summary>
		/// 	Получить дивизион по ключу
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		public static IMainObjectGroup GetDiv(object key) {
			if (_instart) {
				lock (startsync) {}
			}
			if (key is int) {
				var id = (int) key;
				return ResolveDivById(id);
			}
			var code = key as string;
			if (code != null) {
				return ResolveDivByCode(code);
			}
			return null;
		}

		private static IMainObjectGroup ResolveDivByCode(string code) {
			if (string.IsNullOrWhiteSpace(code)) {
				return null;
			}
			if (_divByCode.ContainsKey(code)) {
				return _divByCode[code];
			}
			var native = new NativeZetaReader().ReadDivisions("Code = " + code).FirstOrDefault();
			if (null != native) {
				RegisterDiv(native);
				return native;
			}
			return null;
		}

		private static IZetaMainObject ResolveObjectByCode(string code) {
			if (string.IsNullOrWhiteSpace(code)) {
				return null;
			}
			if (_objByCode.ContainsKey(code)) {
				return _objByCode[code];
			}
			var native = new NativeZetaReader().ReadObjects("Code = " + code).FirstOrDefault();
			if (null != native) {
				RegisterObject(native);
				return native;
			}
			return null;
		}

		private static IMainObjectGroup ResolveDivById(int id) {
			if (0 == id) {
				return null;
			}
			if (_divById.ContainsKey(id)) {
				return _divById[id];
			}
			var native = new NativeZetaReader().ReadDivisions("Id = " + id).FirstOrDefault();
			if (null != native) {
				RegisterDiv(native);
				return native;
			}
			return null;
		}

		private static IZetaMainObject ResolveObjectById(int id) {
			if (0 == id) {
				return null;
			}
			if (_objById.ContainsKey(id)) {
				return _objById[id];
			}
			var native = new NativeZetaReader().ReadObjects("Id = " + id).FirstOrDefault();
			if (null != native) {
				RegisterObject(native);
				return native;
			}
			return null;
		}


		/// <summary>
		/// 	Инициализация кэша
		/// </summary>
		/// <param name="divs"> </param>
		/// <param name="objs"> </param>
		public static void Start(IMainObjectGroup[] divs = null, IZetaMainObject[] objs = null) {
			lock (startsync) {
				_instart = true;
				try {
					_divById.Clear();
					_divById.Clear();
					_objByCode.Clear();
					_divById.Clear();
					divs = divs ?? GetAllDivs();
					objs = objs ?? GetAllObjs();
					foreach (var div in divs) {
						RegisterDiv(div);
					}
					foreach (var obj in objs) {
						RegisterObject(obj);
					}
				}
				finally {
					_instart = false;
				}
			}
		}

		private static void RegisterDiv(IMainObjectGroup div) {
			_divById[div.Id] = div;
			_divByCode[div.Code] = div;
			div.MainObjects = new List<IZetaMainObject>();
		}

		private static void RegisterObject(IZetaMainObject obj) {
			_objById[obj.Id] = obj;
			_objByCode[obj.Code] = obj;
			if (obj.DivId.HasValue && 0 != obj.DivId.Value) {
				if (_divById.ContainsKey(obj.DivId.Value)) {
					obj.Group = _divById[obj.DivId.Value];
					_divById[obj.DivId.Value].MainObjects.Add(obj);
				}
			}
		}

		private static IZetaMainObject[] GetAllObjs() {
			return new NativeZetaReader().ReadObjects("Main = 1").OfType<IZetaMainObject>().ToArray();
		}

		private static IMainObjectGroup[] GetAllDivs() {
			return new NativeZetaReader().ReadDivisions().OfType<IMainObjectGroup>().ToArray();
		}
	}
}