#define USEROWCACHE

// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
#define _USEROWCACHE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comdiv.Application;
using Comdiv.Common;
using Comdiv.Extensions;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;
using Zeta.Extreme.Poco;

namespace Zeta.Extreme.Meta{
	/// <summary>
	/// Метаданные строк для Zeta
	/// </summary>
	public static class RowCache{
        private static object locker = new object();

		private static readonly IDictionary<string, IZetaRow> bycode = new Dictionary<string, IZetaRow>();
		private static readonly IDictionary<int, IZetaRow> byid = new Dictionary<int, IZetaRow>();

		private static readonly IDictionary<string, List<IZetaRow>> bygroup = new Dictionary<string, List<IZetaRow>> ();
		/// <summary>
		/// Режим без разрешения линков на файлах
		/// </summary>
		public static bool NoResolveLinksOnFiles { get; set; }
		private static StorageWrapper<IZetaRow> storage;
		/// <summary>
		/// Получить строку по ключу
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static IZetaRow get(object key){
            if (null == key) return null;
            if (key.Equals(0) || key.Equals("0")) return null;
	
#if USEROWCACHE
            //lock (locker)
            {
                if (key is int){
                    if (byid.ContainsKey((int) key)){
						var result = byid[(int)key];
						if (result.LocalProperties.ContainsKey("dyn"))
						{
							return get("file:" + result.LocalProperties["file"]);
						}
                    	return result;
                    }
                    else{
						if (null == storage)
						{
							storage = myapp.storage.Get<IZetaRow>(false);

						}
						if (storage != null)
						{
							var result = storage.Load(key);
							
							if(null!=result) {
								processIds(result);
								processRefs(result);
							}
							
							return result;
						}
						else
						{
							return null;
						}
                    }
                }
                else {
                    var sk = ((string) key).ToUpper();
                    if (bycode.ContainsKey(sk)){
                        var result = bycode[sk];
						if(result.LocalProperties.ContainsKey("dyn")) {
							return get("file:"+result.LocalProperties["file"]);
						}
                    	return result;
                    }
                    else{
						if(null==storage) {
							storage =myapp.storage.Get<IZetaRow>(false);
							
						}
                        if (storage != null) {
                            var result =  storage.Load(key);
							if(null!=result) {
								processIds(result);
								processRefs(result);
							}
	                        return result;
                        }
                        else{
                            return null;
                        }
                    }
                }
            }
#else
            if (key.Equals(0) || key.Equals("0")) return null;
             var s = myapp.storage.Get<IZetaRow>(false);
                        if (s != null) {
                            return s.Load(key);
                        }
                        else{
                            return null;
                        }
#endif
        }


		/// <summary>
		/// Корневые строки синхронизации
		/// </summary>
		public static string[] rootсodes;
		/// <summary>
		/// Перезагрузка кэша
		/// </summary>
		/// <param name="_roots"></param>
		public static void start(params string[] _roots){
            myapp.OnReload += myapp_OnReload;
#if USEROWCACHE
			rootсodes = _roots;
            reloadCache();
            #endif
        }

        private static void myapp_OnReload(object sender, EventWithDataArgs<int> args){
            #if USEROWCACHE

            reloadCache();
            #endif
        }

		private static IList<IZetaRow> formulas = new List<IZetaRow>();

		static readonly IList<Task> _formularegistrators = new List<Task>();	
        private static void reloadCache(){
            lock (locker) {
				_formularegistrators.Clear();
				formulas.Clear();
                bycode.Clear();
                byid.Clear();
				bygroup.Clear();
				IEnumerable<IZetaRow> roots;
	            if(null==rootсodes || 0==rootсodes.Length) {
		            roots = new NativeZetaReader().ReadRows().ToArray(); //myapp.storage.Get<IZetaRow>().All();
	            }else {
		            var _r = new List<IZetaRow>();
		            foreach (var rootсode in rootсodes) {
			            foreach (var entity in 
							myapp.storage.AsQueryable<IZetaRow>()
							.Where(_=>_.Path.Contains("/"+rootсode+"/"))
							
							) {
							_r.Add(entity);	
			            }
		            }
		            roots = _r;
	            }
	            
	            foreach (var row in roots) {
                    processIds(row);
                }
                foreach (var row in roots)
                {
                    processRefs(row);
                }
				var _sumh = new StrongSumProvider();
				foreach (var row in roots)
				{
					if (row.IsFormula && row.FormulaEvaluator == "boo" && !string.IsNullOrWhiteSpace(row.Formula))
					{
						if (row.ResolveTag("extreme") != "1") continue;
						if (_sumh.IsSum(row)) continue;
						formulas.Add(row);
					}
				}
	      //      Task.WaitAll(_formularegistrators.ToArray());
            }
        }

	

		
		/// <summary>
		/// Доступ к словарю по коду
		/// </summary>
		public static IDictionary<string, IZetaRow> Bycode {
			get { return bycode; }
		}
		/// <summary>
		/// Доступ к словарю по ид
		/// </summary>
		
		public static IDictionary<int, IZetaRow> Byid {
			get { return byid; }
		}
		/// <summary>
		/// Доступ к словарю по группе
		/// </summary>
		
		public static IDictionary<string, List<IZetaRow>> Bygroup {
			get { return bygroup; }
		}

		/// <summary>
		/// Реестр "честных" формул
		/// </summary>
		public static IList<IZetaRow> Formulas {
			get { return formulas; }
			set { formulas = value; }
		}


		private static void processRefs(IZetaRow row) {
            if(row.ParentId.HasValue) {
				if(byid.ContainsKey(row.ParentId.Value)) {
					row.Parent = byid[row.ParentId.Value];
					row.Parent.NativeChildren.Add(row);
				}

            }else {
                row.Parent = null;
            }
            if(row.RefId.HasValue) {
				try {
					row.RefTo = byid[row.RefId.Value];
				}catch {
					throw new Exception("cannot resolve ref to "+row.RefId);
				}
            }else {
                row.RefTo = null;
            }
            if(row.ExRefToId.HasValue) {
                row.ExRefTo = byid[row.ExRefToId.Value];
            }else {
                row.ExRefTo = null;
            }
            if(row.ObjectId.HasValue) {
	            row.Object = new obj {Id = row.ObjectId.Value};
	            //  row.Object = row.Object;
	            //  row.Object.Tag = row.Object.Tag; //forces object to reload


            }else {
                row.Object = null;
            }
        }

        private static void processIds(IZetaRow row){
            bycode[row.Code.ToUpper()] = row;
            byid[row.Id] = row;
        	row.Tag = row.Tag;
            row.Children = new List<IZetaRow>();
			if(!string.IsNullOrWhiteSpace(row.Group)) {
				if(!row.IsMarkSeted("0SA")) {
					var groups = row.Group.split(false, true, ';', '/').Distinct();
					foreach(var g in groups) {
						if(!bygroup.ContainsKey(g)) {
							bygroup[g] = new List<IZetaRow>();
						}
						if(!bygroup[g].Contains(row)) {
							bygroup[g].Add(row);
						}
					}
				}
			}
        }
		/// <summary>
		/// Доступ к прямой регистрации строки
		/// </summary>
		/// <param name="row"></param>
		public static void RegisterCustom(IZetaRow row) {
			processIds(row);
			processRefs(row);
		}
	}
}