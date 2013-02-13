#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultMetaCache.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Application;
using Comdiv.Model.Interfaces;
using Comdiv.Zeta.Data.Minimal;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	����������� ����������, �������� ��� � ���������, ��� � ��������� �����������
	/// </summary>
	public class MetaCache : IMetaCache {

		/// <summary>
		///��������� �� ���������
		/// </summary>
		public readonly static IMetaCache Default = new MetaCache();
		/// <summary>
		/// 	�������� ������ �� ���������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		public T Get<T>(object id) where T : class, IEntityDataPattern {
			lock (this) {
				T result = null;
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
		/// If false - first error - no more native, true - will try again and again
		/// </summary>
		public bool OptimisticOnNativeError;

		/// <summary>
		/// ������� ���� ��� � ������� ���-�� �� ���
		/// </summary>
		protected bool NativeIsError;
		/// <summary>
		/// 	��������� ������ � ���������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		public IMetaCache Set<T>(T item) where T : class, IEntityDataPattern {
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
			if(typeof(IZetaRow).IsAssignableFrom(type)) return typeof (IZetaRow);
			if (typeof(IZetaColumn).IsAssignableFrom(type)) return typeof(IZetaColumn);
			if (typeof(IZetaMainObject).IsAssignableFrom(type)) return typeof(IZetaMainObject);
			return type;
		}

		private T GetNative<T>(object id) where T : class {
			try {
				if(NativeIsError && !OptimisticOnNativeError) return null;
				if (typeof (IZetaRow).IsAssignableFrom(typeof (T))) {
					return (T) RowCache.get(id);
				}
				if (typeof (IZetaColumn).IsAssignableFrom(typeof (T))) {
					return (T) ColumnCache.get(id);
				}
				return myapp.storage.Get<T>().Load(id);
			}
			catch {
				NativeIsError = true;
				return null;
			}
		}

		private T GetById<T>(int id) where T : class {
			var type = NormalizeType(typeof(T));
			if (_byid.ContainsKey(type)) {
				if (_byid[type].ContainsKey(id))
				{
					return (T)_byid[type][id];
				}
			}
			return null;
		}

		private T GetByCode<T>(string code) where T : class {
			var type = NormalizeType(typeof(T));
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
	}
}