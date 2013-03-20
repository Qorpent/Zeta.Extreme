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
// PROJECT ORIGIN: Zeta.Extreme.Model/ColumnCache.cs
#endregion
using System.Collections.Generic;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Model.MetaCaches {
	/// <summary>
	/// 	Кэш загруженных колонок
	/// </summary>
	public class ColumnCache {
		private static readonly object locker = new object();

		private static readonly IDictionary<string, IZetaColumn> bycode = new Dictionary<string, IZetaColumn>();
		private static readonly IDictionary<int, IZetaColumn> byid = new Dictionary<int, IZetaColumn>();

		/// <summary>
		/// 	Прямой доступ к словарю по ID
		/// </summary>
		public static IDictionary<int, IZetaColumn> Byid {
			get { return byid; }
		}

		/// <summary>
		/// 	Прямой доступ к словарю по объектам
		/// </summary>
		public static IDictionary<string, IZetaColumn> Bycode {
			get { return bycode; }
		}

		/// <summary>
		/// 	Получить колонку по ключу
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		public static IZetaColumn get(object key) {
			if (key.Equals(0) || key.Equals("0")) {
				return null;
			}
			if (key is int) {
				if (byid.ContainsKey((int) key)) {
					return byid[(int) key];
				}
			}
			else {
				if (Bycode.ContainsKey((key as string).ToUpper())) {
					return Bycode[(key as string).ToUpper()];
				}
			}
			return null;
		}

		/// <summary>
		/// 	Перегрузка кэшу
		/// </summary>
		public static void Start() {
			lock (locker) {
				Bycode.Clear();
				byid.Clear();
				var cols = new NativeZetaReader().ReadColumns(); // myapp.storage.Get<IZetaColumn>().All();
				foreach (var col in cols) {
					process(col);
				}
			}
		}

		private static void process(IZetaColumn col) {
			Bycode[col.Code.ToUpper()] = col;
			byid[col.Id] = col;
		}
	}
}