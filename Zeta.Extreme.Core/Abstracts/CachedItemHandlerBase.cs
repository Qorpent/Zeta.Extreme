#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : CachedItemHandlerBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Text;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ����� ������� ���������
	/// </summary>
	/// <typeparam name="TItem"> </typeparam>
	public class CachedItemHandlerBase<TItem> : CacheKeyGeneratorBase
		where TItem : class, IWithCode, IWithId, IWithNewTags {
		/// <summary>
		/// 	����� ����� ��������
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
		/// 	������ �� �������� ������
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
		/// 	�� �������
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
		/// 	������������� ����� ���������������
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
		/// 	��� ��������
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
		/// 	������� �������
		/// </summary>
		public bool IsFormula {
			get {
				if (null != Native) {
					var isformula = Native as IWithFormula;
					if(null!=isformula) {
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
		/// 	���������������� �������
		/// </summary>
		public string Formula {
			get {
				if (null != Native) {
					var isformula = Native as IWithFormula;
					if (null != isformula)
					{
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

		/// <summary>
		/// 	��� �������
		/// </summary>
		public string FormulaType {
			get {
				if (null != Native) {
					var isformula = Native as IWithFormula;
					if (null != isformula)
					{
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

		/// <summary>
		/// 	��������� �������� �� �������� ��� ��������� �� Native
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
			if(null!=isformula) {
				_isFormula = isformula.IsFormula;
				_formula = isformula.Formula;
				_formulaType = isformula.FormulaEvaluator;
			}else {
				_isFormula = false;
				_formula = string.Empty;
				_formulaType = string.Empty;
			}
			_id = item.Id;
			_tag = item.Tag;
			InvalidateCacheKey();
		}


		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
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
		/// ���������� ��������������� ������ ������� �� ID|CODE
		/// </summary>
		/// <returns></returns>
		protected virtual string GetIdConditionString() {
			if (_ids != null && 0 != _ids.Length && null == Native) {
				//����� ID - ������ ���������
				return "IDS:" + string.Join(",", _ids);
			}
			if (_codes != null && 0 != _ids.Length && null == Native) //����� ����� �����
			{
				return "CODES:" + string.Join(",", _codes);
			}
			if (0 != Id) {
				//����� ��
				return "ID:" + Id;
			}
			if (!string.IsNullOrWhiteSpace(Code)) {
				return "CODE:" + Code;
			}
			return null;
		}

		private string _code;
		private string[] _codes;
		private string _formula;
		private string _formulaType;
		private int _id;
		private int[] _ids;
		private bool _isFormula;
		private TItem _native;
		private string _tag;
		}
}