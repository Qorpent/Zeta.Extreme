#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : MetaCache.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme {
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
				if (typeof (IMainObjectGroup).IsAssignableFrom(typeof (T))) {
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
		public bool OptimisticOnNativeError;
	}
}