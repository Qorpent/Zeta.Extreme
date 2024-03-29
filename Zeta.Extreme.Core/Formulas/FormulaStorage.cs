﻿#region LICENSE
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
// PROJECT ORIGIN: Zeta.Extreme.Core/FormulaStorage.cs
#endregion
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Хранилище формул
	/// </summary>
	/// <remarks>
	/// 	Новое хранилище формул работает в асинхронном режиме и старается 
	/// 	при этом как можно больше распаралелить и ускорить компиляцию и уменьшить
	/// 	кол-во сборок
	/// </remarks>
	public class FormulaStorage : IFormulaStorage {
		private static IFormulaStorage _default;


		/// <summary>
		/// 	Конструктор по умолчанию, также формирует простой препроцессор
		/// </summary>
		public FormulaStorage() {
			AddPreprocessor(new DefaultDeltaPreprocessor());
			AddPreprocessor(new BooConverter());
			AutoBatchCompile = true;
			_cache = new FormulaAssemblyCache();
		}
		/// <summary>
		/// Возвращает запрос по коду
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public FormulaRequest GetRequest(string code) {
			if (_registry.ContainsKey(code)) return _registry[code];
			return null;
		}
		

		/// <summary>
		/// 	Статическое хранилище формул по умолчанию
		/// </summary>
		public static IFormulaStorage Default {
			get { return _default ?? (_default = new FormulaStorage()); }
			set { _default = value; }
		}

		/// <summary>
		/// </summary>
		/// <param name="request"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public string Register(FormulaRequest request) {
			//STUB FOR NOW
			lock (_register_lock)
				lock (_compile_lock) //нельзя во время регистрации еще и компилировать
				{
					if (null != request.PreparedType) {
						_registry[request.Key] = request;
						return request.Key;
					}
					if (String.IsNullOrWhiteSpace(request.Key)) {
						request.Key = request.Formula.Trim();
					}
					if (_registry.ContainsKey(request.Key)) {
						var existed = _registry[request.Key];
						if (null == existed.PreparedType && null != request.PreparedType) {
							existed.PreparedType = request.PreparedType;
						}
						if (existed.Formula != request.Formula) {
							// обслуживаем обновление формул
							existed.PreparedType = request.PreparedType ?? existed.PreparedType;
							existed.PreprocessedFormula = request.PreprocessedFormula;
							existed.Cache.Clear();
							if (null == existed.PreparedType && String.IsNullOrWhiteSpace(existed.PreprocessedFormula)) {
								Preprocess(existed);
							}
						}
					}
					else {
						_registry[request.Key] = request;
						if (TagHelper.Value(request.Tags, FormulaParserConstants.IgnoreFormulaTag).ToBool()) {
							request.PreparedType = typeof (NoExtremeFormulaStub);
						}
						else {
							TryResolveFromCache(request);
							if (null == request.PreparedType && String.IsNullOrWhiteSpace(request.PreprocessedFormula)) {
								Preprocess(request);
							}
						}
					}


					if (null == request.PreparedType) {
						var waitbatchsize =
							_registry.Values.Count(_ => null == _.PreparedType && null == _.FormulaCompilationTask);
						if (AutoBatchCompile && BatchSize <= waitbatchsize) {
							StartAsyncCompilation();
						}
					}
					return request.Key;
				}
		}

	

		/// <summary>
		/// 	Регистрирует препроцессор в хранилище
		/// </summary>
		/// <param name="preprocessor"> </param>
		/// <returns> </returns>
		public void AddPreprocessor(IFormulaPreprocessor preprocessor) {
			_preprocessors.Add(preprocessor);
			_preprocessors.Sort((f, s) => f.Idx.CompareTo(s.Idx));
		}


		/// <summary>
		/// 	Возвращает экземпляр формулы по ключу
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="throwErrorOnNotFound"> false если надо возвращать NULL при отсутствии формулы </param>
		/// <returns> </returns>
		public IFormula GetFormula(string key, bool throwErrorOnNotFound = true) {
			lock (_register_lock) {
				lock (_get_lock) {
					if (!_registry.ContainsKey(key)) {
						if (throwErrorOnNotFound) {
							throw new Exception("formula with key " + key + " not registered");
						}
						return null;
					}
					var request = _registry[key];
					IFormula result;
					if (request.Cache.TryPop(out result)) {
						return result; //try get from cache
					}
					if (request.PreparedType == null) {
						ForceCompilation(request);
					}
					var instance = Activator.CreateInstance(request.PreparedType) as IFormula;
					instance.SetContext(request);
					return instance;
				}
			}
		}

		/// <summary>
		/// 	Возвращает формулу обратно хранилищу, может использовать для реализации кэша формул
		/// </summary>
		/// <param name="key"> </param>
		/// <param name="formula"> </param>
		public void Return(string key, IFormula formula) {
			lock (_register_lock) {
				if (!_registry.ContainsKey(key)) {
					return; //nowhere to store
				}
				_registry[key].Cache.Push(formula);
			}
		}

		/// <summary>
		/// 	Асинхронно выполняет полную компиляцию формул
		/// </summary>
		public void StartAsyncCompilation() {
			lock (_compile_lock) {
				var batch = _registry.Values.Where(_ => null == _.PreparedType && null == _.FormulaCompilationTask).ToArray();
				var t = Task.Run(() => DoCompile(batch, null));
				foreach (var f in batch) {
					f.FormulaCompilationTask = t;
				}
			}
		}

		/// <summary>
		/// 	Последняя ошибка компиляции
		/// </summary>
		public Exception LastCompileError { get; set; }

		/// <summary>
		/// 	True - включен режим автоматического батча
		/// </summary>
		public bool AutoBatchCompile { get; set; }

		/// <summary>
		/// 	Компилирует все формы в стеке
		/// </summary>
		public void CompileAll(string savepath) {
			lock (_compile_lock) {
				var batch = _registry.Values.Where(_ => null == _.PreparedType && null == _.FormulaCompilationTask).ToArray();
				DoCompile(batch, savepath);
			}
		}

		/// <summary>
		/// 	Очистка кэша
		/// </summary>
		public void Clear() {
			LastCompileError = null;
			_registry.Clear();
		}

		/// <summary>
		/// 	Проверяет наличие формулы в хранилище
		/// </summary>
		/// <param name="key"> </param>
		/// <returns> </returns>
		public bool Exists(string key) {
			return _registry.ContainsKey(key);
		}

		/// <summary>
		/// 	Количество формул
		/// </summary>
		public int Count {
			get { return _registry.Count; }
		}

		private void TryResolveFromCache(FormulaRequest request) {
			var key = request.Key;
			if(_cache.ContainsKey(key)) {
				if(_cache.GetVersion(key)==request.Version) {
					request.PreparedType = _cache.GetFormulaType(key);
				}
			}
		}

		/// <summary>
		/// 	Обертка над вызовом компилятора с корректной обработкой ошибок компиляции
		/// </summary>
		/// <param name="batch"> </param>
		/// <param name="savepath"> </param>
		protected internal void DoCompile(FormulaRequest[] batch, string savepath) {
			try {
				new FormulaCompiler().Compile(batch, savepath);
			}
			catch (Exception e) {
				LastCompileError = e;
				foreach (var formulaRequest in batch) {
					formulaRequest.ErrorInCompilation = e;
					formulaRequest.PreparedType = typeof (CompileErrorFormulaStub);
				}
			}
		}

		/// <summary>
		/// 	Позволяет подготовить формулу к компиляции
		/// </summary>
		/// <param name="request"> </param>
		public void Preprocess(FormulaRequest request) {
			string result = _preprocessors.Aggregate(request.Formula, (current, p) => p.Preprocess(current, request));
			request.PreprocessedFormula = result;
		}

		private void ForceCompilation(FormulaRequest request) {
			lock (_compile_lock) {
				if (null != request.FormulaCompilationTask) {
					//сначала прверяем - вдруг формула уже на компиляции, просто дожидаемся
					request.FormulaCompilationTask.Wait();
					return;
				}
				// все, значит мы синхронно должны закомпилить это дело
				DoCompile(new[] {request}, null);
			}
		}


		private readonly object _compile_lock = new object(); //синхронизатор компилятора
		private readonly object _get_lock = new object(); //синхронизатор получения формулы
		private readonly List<IFormulaPreprocessor> _preprocessors = new List<IFormulaPreprocessor>();
		private readonly object _register_lock = new object(); //синхронизатор регистрации

		/// <summary>
		/// 	коллекция запросов
		/// </summary>
		private readonly IDictionary<string, FormulaRequest> _registry = new ConcurrentDictionary<string, FormulaRequest>();

		/// <summary>
		/// 	Размер батча для асинхронной компиляции
		/// </summary>
		public int BatchSize = 5;


		private FormulaAssemblyCache _cache;

		/// <summary>
		/// Загружает формулы по умолчанию из кжша, с использованием указанной папки готовых DLL
		/// </summary>
		/// <param name="rootDirectory"></param>
		public  void LoadDefaultFormulas(string rootDirectory) {
			if(rootDirectory.IsNotEmpty()) {
				_cache.Rebuild(rootDirectory);
			}
			var oldrowformulas = RowCache.Formulas.Where(
				_ => _.Version < DateTime.Today
				).ToArray();

			var newrowformulas = RowCache.Formulas.Where(
				_ => _.Version >= DateTime.Today
				).ToArray();


			var oldcolformulas = (
				                     from c in ColumnCache.Byid.Values
				                     //myapp.storage.AsQueryable<col>()
				                     where c.IsFormula
				                           && c.FormulaType == "boo" && !String.IsNullOrEmpty(c.Formula)
				                           && c.Version < DateTime.Today
				                     select new {c = c.Code, f = c.Formula, tag = c.Tag, version = c.Version}
			                     ).ToArray();

			var newcolformulas = (
				                     from c in ColumnCache.Byid.Values
				                     //myapp.storage.AsQueryable<col>()
				                     where c.IsFormula
				                           && c.FormulaType == "boo" && !String.IsNullOrEmpty(c.Formula)
				                           && c.Version >= DateTime.Today
				                     select new {c = c.Code, f = c.Formula, tag = c.Tag, version = c.Version}
			                     ).ToArray();


			foreach (var f in oldrowformulas) {
				var req = new FormulaRequest
					{
						Key = "row:" + f.Code,
						Formula = f.Formula,
						Language = f.FormulaType,
						Version = f.Version.ToString(CultureInfo.InvariantCulture)
					};
				Register(req);
			}

			foreach (var c in oldcolformulas) {
				var req = new FormulaRequest
					{
						Key = "col:" + c.c,
						Formula = c.f,
						Language = "boo",
						Tags = c.tag,
						Version = c.version.ToString(CultureInfo.InvariantCulture)
					};
				Register(req);
			}
			CompileAll(rootDirectory);


			foreach (var f in newrowformulas) {
				var req = new FormulaRequest
					{
						Key = "row:" + f.Code,
						Formula = f.Formula,
						Language = f.FormulaType,
						Version = f.Version.ToString(CultureInfo.InvariantCulture)
					};
				Register(req);
			}

			foreach (var c in newcolformulas) {
				var req = new FormulaRequest
					{
						Key = "col:" + c.c,
						Formula = c.f,
						Language = "boo",
						Tags = c.tag,
						Version = c.version.ToString(CultureInfo.InvariantCulture)
					};
				Register(req);
			}
			CompileAll(rootDirectory);
		}
	}
}