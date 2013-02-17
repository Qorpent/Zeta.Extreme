#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CachedItemHandlerBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовый класс обертки измерения
	/// </summary>
	/// <typeparam name="TItem"> </typeparam>
	public class CachedItemHandlerBase<TItem> : CacheKeyGeneratorBase, IZetaQueryDimension
		where TItem : class, IWithCode, IWithId, IWithNewTags {
		/// <summary>
		/// 	Набор кодов элемента
		/// </summary>
		public string[] Codes {
			get { return _codes; }
			set {
				if (null != Native) {
					throw new Exception("cannot assign codes on natived condition");
				}
				if (value == _codes) {
					return;
				}
				_codes = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Ссылка на исходный объект
		/// </summary>
		public TItem Native {
			get { return _native; }
			set {
				if (value == _native) {
					return;
				}
				_native = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Множественный набор идентификаторов
		/// </summary>
		public int[] Ids {
			get { return _ids; }
			set {
				if (null != Native) {
					throw new Exception("cannot assign ids on natived condition");
				}
				if (value == _ids) {
					return;
				}
				_ids = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		public string FormulaType {
			get {
				if (null != Native) {
					var isformula = Native as IWithFormula;
					if (null != isformula) {
						return isformula.FormulaEvaluator;
					}
					return string.Empty;
				}
				return _formulaType;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot assign formulatype on natived condition");
				}
				if (value == _formulaType) {
					return;
				}
				_formulaType = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Ид объекта
		/// </summary>
		public int Id {
			get {
				if (null != Native) {
					return Native.Id;
				}
				return _id;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot assign id on natived condition");
				}
				if (value == _id) {
					return;
				}
				InvalidateCacheKey();
				_id = value;
			}
		}


		/// <summary>
		/// 	Код элемента
		/// </summary>
		public string Code {
			get {
				if (null != Native) {
					return Native.Code;
				}
				return _code;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot assign code on natived condition");
				}
				if (value == _code) {
					return;
				}
				_code = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Признак формулы
		/// </summary>
		public bool IsFormula {
			get {
				if (null != Native) {
					var isformula = Native as IWithFormula;
					if (null != isformula) {
						return isformula.IsFormula;
					}
					return false;
				}
				return _isFormula;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot assign isformula on natived condition");
				}
				if (value == _isFormula) {
					return;
				}
				_isFormula = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	Пользовательская формула
		/// </summary>
		public string Formula {
			get {
				if (null != Native) {
					var isformula = Native as IWithFormula;
					if (null != isformula) {
						return isformula.Formula;
					}
					return string.Empty;
				}
				return _formula;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot assign formula on natived condition");
				}
				if (value == _formula) {
					return;
				}
				_formula = value;
				if (!string.IsNullOrWhiteSpace(_formula)) {
					_isFormula = true;
				}
				InvalidateCacheKey();
			}
		}

		string IWithFormula.ParsedFormula { get; set; }

		string IWithFormula.FormulaEvaluator {
			get { return FormulaType; }
			set { FormulaType = value; }
		}


		/// <summary>
		/// </summary>
		public string Tag {
			get {
				if (null != Native) {
					return Native.Tag;
				}
				return _tag;
			}
			set {
				if (null != Native) {
					throw new Exception("cannot assign tag on natived condition");
				}
				if (value == _tag) {
					return;
				}
				_tag = value;
				InvalidateCacheKey();
			}
		}

		string IWithName.Name { get; set; }

		string IWithComment.Comment { get; set; }

		DateTime IWithVersion.Version {
			get { return _version; }
		}

		IDictionary<string, object> IZetaQueryDimension.LocalProperties {
			get {
				if (null != Native) {
					return ((IZetaQueryDimension) Native).LocalProperties;
				}
				return _localProperties ?? (_localProperties = new Dictionary<string, object>());
			}
		}

		/// <summary>
		/// 	Проверяет первичность элемента запроса
		/// </summary>
		/// <returns> </returns>
		public virtual bool IsPrimary() {
			return !IsFormula;
		}

		/// <summary>
		/// 	Применяет свойства от сущности без установки ее Native
		/// </summary>
		public virtual void Apply(TItem item) {
			if (null == item) {
				return;
			}
			if (null != Native) {
				throw new Exception("cannot assign item data while natived");
			}
			_code = item.Code;
			var isformula = item as IWithFormula;
			if (null != isformula) {
				_isFormula = isformula.IsFormula;
				_formula = isformula.Formula;
				_formulaType = isformula.FormulaEvaluator;
			}
			else {
				_isFormula = false;
				_formula = string.Empty;
				_formulaType = string.Empty;
			}
			_id = item.Id;
			_tag = item.Tag;
			InvalidateCacheKey();
		}


		/// <summary>
		/// 	Функция непосредственного вычисления кэшевой строки
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();
			sb.Append(typeof (TItem).Name);
			sb.Append('=');
			var idscache = GetIdConditionString();
			if (!string.IsNullOrWhiteSpace(idscache)) {
				sb.Append(idscache);
				sb.Append(';');
			}
			return sb.ToString();
		}

		/// <summary>
		/// 	Возвращает нормализованную строку условий по ID|CODE
		/// </summary>
		/// <returns> </returns>
		protected virtual string GetIdConditionString() {
			if (_codes != null && 0 != _codes.Length && null == Native) //затем набор кодов
			{
				_codes = _codes.Where(_ => null != _).Distinct().OrderBy(_ => _).ToArray();
				return "CODES:" + string.Join(",", _codes);
			}
			if (_ids != null && 0 != _ids.Length && null == Native) {
				//набор ID - высший приоритет
				_ids = _ids.Distinct().OrderBy(_ => _).ToArray();
				return "IDS:" + string.Join(",", _ids);
			}
			if (!string.IsNullOrWhiteSpace(Code)) {
				return "CODE:" + Code;
			}
			if (0 != Id) {
				//потом ид
				return "ID:" + Id;
			}

			return null;
		}

		/// <summary>
		/// 	Определяет что условие описывает одну, определенную инстанцию объекта
		/// 	еще без загрузки Native
		/// </summary>
		/// <returns> </returns>
		public virtual bool IsStandaloneSingletonDefinition(bool falseonnative = true) {
			if (null != Native && falseonnative) {
				return false;
			}
			if (null != Native) {
				return true; //falseonnative - false
			}
			if (null != _ids && 0 != _ids.Length) {
				return false;
			}
			if (null != _codes && 0 != _codes.Length) {
				return false;
			}
			if (0 == Id && string.IsNullOrWhiteSpace(Code)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// 	Возвращает нормализованный ID сущности
		/// </summary>
		/// <returns> </returns>
		public object GetEffectiveKey() {
			if (IsStandaloneSingletonDefinition(false)) {
				return 0 == Id ? (object) Code : Id;
			}
			return null;
		}

		private readonly DateTime _version = new DateTime();

		private string _code;
		private string[] _codes;
		private string _formula;
		private string _formulaType;
		private int _id;
		private int[] _ids;
		private bool _isFormula;
		private IDictionary<string, object> _localProperties;
		private TItem _native;
		private string _tag;
		}
}