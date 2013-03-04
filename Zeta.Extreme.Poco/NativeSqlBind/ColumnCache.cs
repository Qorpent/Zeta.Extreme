﻿using System.Collections.Generic;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Meta {
	/// <summary>
	/// Кэш загруженных колонок
	/// </summary>
	public class ColumnCache {
		private static object locker = new object();

		private static readonly IDictionary<string, IZetaColumn> bycode = new Dictionary<string, IZetaColumn>();
		private static readonly IDictionary<int, IZetaColumn> byid = new Dictionary<int, IZetaColumn>();
/// <summary>
/// Прямой доступ к словарю по ID
/// </summary>
		public static IDictionary<int, IZetaColumn> Byid {
			get { return byid; }
		}
		/// <summary>
		/// Прямой доступ к словарю по объектам
		/// </summary>
		public static IDictionary<string, IZetaColumn> Bycode {
			get { return bycode; }
		}
		/// <summary>
		/// Получить колонку по ключу
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static IZetaColumn get(object key)
		{
			if (key.Equals(0) || key.Equals("0")) return null;
			if (key is int){
				if (byid.ContainsKey((int) key)){
					return byid[(int) key];
				}
			}
			else{
				if (Bycode.ContainsKey((key as string).ToUpper())) {
					return Bycode[(key as string).ToUpper()];
				}
			}
			return null;

		}
		/// <summary>
		/// Перегрузка кэшу
		/// </summary>
		public static void start()
		{
			lock (locker) {
				
			
			myapp.OnReload += myapp_OnReload;


			reloadCache(); 
			}
		}

		private static void myapp_OnReload(object sender, EventWithDataArgs<int> args)
		{


			reloadCache();

		}

		private static void reloadCache()
		{
			Bycode.Clear();
			byid.Clear();
			var cols = new NativeZetaReader().ReadColumns(); // myapp.storage.Get<IZetaColumn>().All();
			foreach (var col in cols)
			{
				process(col);
			}
		}

		private static void process(IZetaColumn col)
		{
			Bycode[col.Code.ToUpper()] = col;
			byid[col.Id] = col;
            
		}
	}
}