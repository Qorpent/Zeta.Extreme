#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : FormulaStorage.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comdiv.Extensions;
using Qorpent.Utils.Extensions;

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
					if (string.IsNullOrWhiteSpace(request.Key)) {
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
							if (null == existed.PreparedType && string.IsNullOrWhiteSpace(existed.PreprocessedFormula)) {
								Preprocess(existed);
							}
						}
					}
					else {
						_registry[request.Key] = request;
						if(TagHelper.Value(request.Tags,FormulaParserConstants.IgnoreFormulaTag).ToBool()) {
							request.PreparedType = typeof (NoExtremeFormulaStub);
						}else {
							if (null == request.PreparedType && string.IsNullOrWhiteSpace(request.PreprocessedFormula)) {
								Preprocess(request);
							}
						}
					}

					

					var waitbatchsize = _registry.Values.Where(_ => null == _.PreparedType && null == _.FormulaCompilationTask).Count();
					if (AutoBatchCompile && BatchSize <= waitbatchsize) {
						StartAsyncCompilation();
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
		/// Асинхронно выполняет полную компиляцию формул
		/// </summary>
		public void StartAsyncCompilation() {
			lock (_compile_lock) {
				var batch = _registry.Values.Where(_ => null == _.PreparedType && null == _.FormulaCompilationTask).ToArray();
				var t = Task.Run(() => DoCompile(batch));
				foreach (var f in batch) {
					f.FormulaCompilationTask = t;
				}
			}
		}
		/// <summary>
		/// Обертка над вызовом компилятора с корректной обработкой ошибок компиляции
		/// </summary>
		/// <param name="batch"></param>
		protected internal  void DoCompile(FormulaRequest[] batch) {
			try {
				new FormulaCompiler().Compile(batch);
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
		/// Последняя ошибка компиляции
		/// </summary>
		public Exception LastCompileError { get; set; }

		/// <summary>
		/// True - включен режим автоматического батча
		/// </summary>
		public bool AutoBatchCompile { get; set; }

		/// <summary>
		/// Компилирует все формы в стеке
		/// </summary>
		public void CompileAll() {
			lock (_compile_lock) {
				var batch = _registry.Values.Where(_ => null == _.PreparedType && null == _.FormulaCompilationTask).ToArray();
				DoCompile(batch);
			}
		}
		/// <summary>
		/// Количество формул
		/// </summary>
		public int Count {
			get { return _registry.Count; }
		}

		/// <summary>
		/// 	Позволяет подготовить формулу к компиляции
		/// </summary>
		/// <param name="request"> </param>
		public void Preprocess(FormulaRequest request) {
			var result = request.Formula;
			foreach (var p in _preprocessors) {
				result = p.Preprocess(result, request);
			}
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
				DoCompile(new[] { request });
			}
		}

		private readonly object _compile_lock = new object(); //синхронизатор компилятора
		private readonly object _get_lock = new object(); //синхронизатор получения формулы
		private readonly List<IFormulaPreprocessor> _preprocessors = new List<IFormulaPreprocessor>();
		private readonly object _register_lock = new object(); //синхронизатор регистрации

		/// <summary>
		/// 	коллекция запросов
		/// </summary>
		private readonly IDictionary<string, FormulaRequest> _registry = new Dictionary<string, FormulaRequest>();

		/// <summary>
		/// 	Размер батча для асинхронной компиляции
		/// </summary>
		public int BatchSize = 5;
	}
}