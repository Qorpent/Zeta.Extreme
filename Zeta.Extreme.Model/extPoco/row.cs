#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : row.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Poco {
	public partial class
		row : IZetaRow {
		public virtual string SortKey {
			get { return string.Format("{0:00000}_{1}_{2}", Idx == 0 ? 10000 : Idx, OuterCode ?? "", Code); }
		}

		public virtual IZetaRow[] AllChildren {
			get {
				lock (this) {
					if (null == _allchildren) {
						var result = new List<IZetaRow>();
						foreach (var row in Children.OrderBy(x => ((row) x).GetSortKey())) {
							_register(row, result);
						}
						_allchildren = result.ToArray();
					}
					return _allchildren;
				}
			}
		}

		/// <summary>
		/// 	Проверяет, что строка не устарела
		/// </summary>
		/// <param name="year"> </param>
		/// <returns> </returns>
		public virtual bool IsObsolete(int year) {
			var obs = ResolveTag("obsolete").ToInt();
			if (0 == obs) {
				return false;
			}
			if (obs > year) {
				return false;
			}
			return true;
		}

		public virtual IZetaRow Copy(bool withchildren) {
			lock (this) {
				var result = (row) MemberwiseClone();
				result._children = new List<IZetaRow>();
				result._allchildren = null;
				result.LocalProperties = new Dictionary<string, object>(LocalProperties);
				if (withchildren) {
					foreach (var row in _children) {
						var r_ = row.Copy(withchildren);
						r_.Parent = result;
						result._children.Add(r_);
					}
				}
				if (null != result.RefTo) {
					result.RefTo = result.RefTo.Copy(withchildren);
				}
				if (null != result.ExRefTo) {
					result.ExRefTo = result.ExRefTo.Copy(withchildren);
				}
				return result;
			}
		}

		public virtual void ResetAllChildren() {
			_allchildren = null;
			if (_children != null) {
				foreach (var row in _children) {
					row.ResetAllChildren();
				}
			}
		}

	


		public virtual bool IsMarkSeted(string code) {
			if (code.ToUpper() == "ISFORMULA") {
				return IsFormula;
			}
			return WithMarksExtension.IsMarkSeted(this, code);
		}

		private string GetSortKey() {
			return string.Format("{0:00000}_{1}_{2}", Idx, OuterCode ?? "", Code);
		}

		private void _register(IZetaRow row, List<IZetaRow> result) {
			result.Add(row);
			if (((row) row)._children != null) {
				foreach (var zetaRow in row.Children.OrderBy(x => ((row) x).GetSortKey())) {
					_register(zetaRow, result);
				}
			}
		}

		public virtual string GetParentedName() {
			var result = Name;
			if (null != Parent) {
				result = ((row) Parent).GetParentedName() + " / " + result;
			}
			return result;
		}

		private void prepareColumnMap() {
			if (columnmap == null) {
				columnmap = new Dictionary<string, string>();
				if (ColumnSubstitution.IsNotEmpty()) {
					var rules = ColumnSubstitution.SmartSplit();
					foreach (var rule in rules) {
						var pair = rule.SmartSplit(false, true, '=');
						columnmap[pair[0]] = pair[1];
					}
				}
			}
		}

		private IZetaRow[] _allchildren;
		}
}