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
// PROJECT ORIGIN: Zeta.Extreme.Model/MetaCache.cs
#endregion
using System;
using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// 	Стандартная реализация, работает как с временным, так и реальными хранилищами
	/// </summary>
	public class MetaCache : IMetaCache {
		///<summary>
		///	Экземпляр по умолчанию
		///</summary>
		public static readonly IMetaCache Default = new MetaCache();

		/// <summary>
		/// 	Родительский кэш
		/// </summary>
		public IMetaCache Parent { get; set; }

		/// <summary>
		/// 	Получить объект из хранилища
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		public T Get<T>(object id) where T : class, IEntity {
			lock (this) {
				T result = null;
				if (null != Parent) //сначала опрашивается родитель и это основной кейс, 
					//так как кастом-коды редкость, плюс не надо чтобы кастом 
					//коды перекрывали штатные
				{
					result = Parent.Get<T>(id);
				}
				if (null != result) {
					return result;
				}
				if (id is string) {
					result = GetByCode<T>((string) id);
				}
				else {
					result = GetById<T>((int) id);
				}
				if (null == result) {
					result = GetNative<T>(id);
				}

				return result;
			}
		}

		/// <summary>
		/// 	Сохранить объект в хранилище
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		public IMetaCache Set<T>(T item) where T : class, IEntity {
			lock (this) {
				var type = NormalizeType(item.GetType());
				if (!_byid.ContainsKey(type)) {
					_byid[type] = new Dictionary<int, object>();
					_bycode[type] = new Dictionary<string, object>();
				}
				_byid[type][item.Id] = item;
				_bycode[type][item.Code] = item;

				return this;
			}
		}


		private Type NormalizeType(Type type) {
			if (typeof (IZetaRow).IsAssignableFrom(type)) {
				return typeof (IZetaRow);
			}
			if (typeof (IZetaColumn).IsAssignableFrom(type)) {
				return typeof (IZetaColumn);
			}
			if (typeof (IZetaMainObject).IsAssignableFrom(type)) {
				return typeof (IZetaMainObject);
			}
			if (typeof (IObjectDivision).IsAssignableFrom(type)) {
				return typeof (IObjectDivision);
			}
			return type;
		}

		private T GetNative<T>(object id) where T : class {
			try {
				if (NativeIsError && !OptimisticOnNativeError) {
					return null;
				}
				if (typeof (IZetaRow).IsAssignableFrom(typeof (T))) {
					return (T) RowCache.get(id);
				}
				if (typeof (IZetaColumn).IsAssignableFrom(typeof (T))) {
					return (T) ColumnCache.get(id);
				}
				if (typeof (IZetaMainObject).IsAssignableFrom(typeof (T))) {
					return (T) ObjCache.Get(id);
				}
				if (typeof (IObjectDivision).IsAssignableFrom(typeof (T))) {
					return (T) ObjCache.GetDiv(id);
				}

				return null;
			}
			catch {
				NativeIsError = true;
				return null;
			}
		}

		private T GetById<T>(int id) where T : class {
			var type = NormalizeType(typeof (T));
			if (_byid.ContainsKey(type)) {
				if (_byid[type].ContainsKey(id)) {
					return (T) _byid[type][id];
				}
			}
			return null;
		}

		private T GetByCode<T>(string code) where T : class {
			var type = NormalizeType(typeof (T));
			if (_bycode.ContainsKey(type)) {
				if (_bycode[type].ContainsKey(code)) {
					return (T) _bycode[type][code];
				}
			}
			return null;
		}

		private readonly IDictionary<Type, IDictionary<string, object>> _bycode =
			new Dictionary<Type, IDictionary<string, object>>();

		private readonly IDictionary<Type, IDictionary<int, object>> _byid = new Dictionary<Type, IDictionary<int, object>>();

		/// <summary>
		/// 	Признак того что с нативом что-то не так
		/// </summary>
		protected bool NativeIsError;

		/// <summary>
		/// 	If false - first error - no more native, true - will try again and again
		/// </summary>
		public bool OptimisticOnNativeError = true;
	}
}