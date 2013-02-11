using System;
using System.Collections.Generic;

namespace Zeta.Extreme
{
	/// <summary>
	/// Хранилище формул
	/// </summary>
	/// <remarks>
	/// Новое хранилище формул работает в асинхронном режиме и старается 
	/// при этом как можно больше распаралелить и ускорить компиляцию и уменьшить
	/// кол-во сборок
	/// </remarks>
	public class FormulaStorage : IFormulaStorage {
		private object _register_lock = new object(); //синхронизатор регистрации
		private object _get_lock = new object(); //синхронизатор получения формулы
		private object _compile_lock = new object(); //синхронизатор компилятора
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <exception cref="NotImplementedException"></exception>
		public string Register(FormulaRequest request) {
			//STUB FOR NOW
			lock (_register_lock)
			{
				if(string.IsNullOrWhiteSpace(request.Key)) {
					request.Key = request.Formula.Trim();
				}
				if(_registry.ContainsKey(request.Key)) {
					var existed = _registry[request.Key];
					if(null==existed.PreparedType && null!=request.PreparedType) {
						existed.PreparedType = request.PreparedType;
					}
				}else {
					_registry[request.Key] = request;
				}

				return request.Key;
			}
		}
		/// <summary>
		/// коллекция запросов
		/// </summary>
		private IDictionary<string, FormulaRequest> _registry = new Dictionary<string, FormulaRequest>();


		/// <summary>
		/// Возвращает экземпляр формулы по ключу 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="throwErrorOnNotFound">false если надо возвращать NULL при отсутствии формулы </param>
		/// <returns></returns>
		public IFormula GetFormula(string key, bool throwErrorOnNotFound = true)
		{
			lock(_register_lock) {
				lock(_get_lock) {
					if(!_registry.ContainsKey(key)) {
						if(throwErrorOnNotFound) {
							throw new Exception("formula with key " + key + " not registered");
						}
						return null;
					}
					var request = _registry[key];
					IFormula result;
					if(request.Cache.TryPop(out result)) return result; //try get from cache
					if(request.PreparedType==null) {
						ForceCompilation(request);
					}
					var instance = Activator.CreateInstance(request.PreparedType) as IFormula;
					return instance;
				}
			}
		}

		/// <summary>
		/// Возвращает формулу обратно хранилищу, может использовать для реализации кэша формул
		/// </summary>
		/// <param name="key"></param>
		/// <param name="formula"></param>
		public void Return(string key, IFormula formula) {
			lock(_register_lock) {
				if(!_registry.ContainsKey(key)) return;//nowhere to store
				_registry[key].Cache.Push(formula);	
			}
		}

		private void ForceCompilation(FormulaRequest request) {
			throw new NotImplementedException();
		}

		

		private static IFormulaStorage _default;
		/// <summary>
		/// Статическое хранилище формул по умолчанию
		/// </summary>
		public static IFormulaStorage Default {
			get { return _default ?? (_default = new FormulaStorage()); }
			set { _default = value; }
		}
	}
}
