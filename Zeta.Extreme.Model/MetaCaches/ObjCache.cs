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
// PROJECT ORIGIN: Zeta.Extreme.Model/ObjCache.cs
#endregion
using System.Collections.Generic;
using System.Linq;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Model.MetaCaches {
	/// <summary>
	/// 	��� ������ �� ������� ��������
	/// </summary>
	public static class ObjCache {
		private static readonly IDictionary<int, IZetaMainObject> _objById = new Dictionary<int, IZetaMainObject>();
		private static readonly IDictionary<string, IZetaMainObject> _objByCode = new Dictionary<string, IZetaMainObject>();
		private static readonly IDictionary<int, IObjectDivision> _divById = new Dictionary<int, IObjectDivision>();
		private static readonly IDictionary<string, IObjectDivision> _divByCode = new Dictionary<string, IObjectDivision>();
		private static readonly object startsync = new object();
		private static bool _instart;

		/// <summary>
		/// 	������� ID->Obj
		/// </summary>
		public static IDictionary<int, IZetaMainObject> ObjById {
			get { return _objById; }
		}

		/// <summary>
		/// 	������� Code->Obj
		/// </summary>
		public static IDictionary<string, IZetaMainObject> ObjByCode {
			get { return _objByCode; }
		}

		/// <summary>
		/// 	������� Id->Div
		/// </summary>
		public static IDictionary<int, IObjectDivision> DivById {
			get { return _divById; }
		}

		/// <summary>
		/// 	������� Code->Div
		/// </summary>
		public static IDictionary<string, IObjectDivision> DivByCode {
			get { return _divByCode; }
		}

		/// <summary>
		/// 	��������� ������� �� �����
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
		/// 	�������� �������� �� �����
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		public static IObjectDivision GetDiv(object key) {
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

		private static IObjectDivision ResolveDivByCode(string code) {
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
			var native = new NativeZetaReader().ReadObjects("Code = '" + code.Replace("'","''")+"'").FirstOrDefault();
			if (null != native) {
				RegisterObject(native);
				return native;
			}
			return null;
		}

		private static IObjectDivision ResolveDivById(int id) {
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
		/// 	������������� ����
		/// </summary>
		/// <param name="divs"> </param>
		/// <param name="objs"> </param>
		public static void Start(IObjectDivision[] divs = null, IZetaMainObject[] objs = null) {
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

		private static void RegisterDiv(IObjectDivision div) {
			_divById[div.Id] = div;
			_divByCode[div.Code] = div;
			div.MainObjects = new List<IZetaMainObject>();
		}

		private static void RegisterObject(IZetaMainObject obj) {
			_objById[obj.Id] = obj;
			_objByCode[obj.Code] = obj;
			if (obj.DivisionId.HasValue && 0 != obj.DivisionId.Value) {
				if (_divById.ContainsKey(obj.DivisionId.Value)) {
					obj.Division = _divById[obj.DivisionId.Value];
					_divById[obj.DivisionId.Value].MainObjects.Add(obj);
				}
			}
		}

		private static IZetaMainObject[] GetAllObjs() {
			return new NativeZetaReader().ReadObjects("Main = 1").OfType<IZetaMainObject>().ToArray();
		}

		private static IObjectDivision[] GetAllDivs() {
			return new NativeZetaReader().ReadDivisions().OfType<IObjectDivision>().ToArray();
		}
	}
}