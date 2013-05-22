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
// PROJECT ORIGIN: Zeta.Extreme.Model/Row.cs

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     <see cref="Row" /> - is main tree-like part of attribute zeta dimension
	/// </summary>
	public partial class Row : Hierarchy<IZetaRow>, IZetaRow{
		/// <summary>
		///     Режим использования формулы с Extreme
		/// </summary>
		public virtual int ExtremeFormulaMode { get; set; }

		/// <summary>
		///     Helper property to simplify ordering
		/// </summary>
		public virtual string SortKey {
			get { return string.Format("{0:00000}_{1}_{2}", Index == 0 ? 10000 : Index, OuterCode ?? "", Code); }
		}

		/// <summary>
		///     Special pseudo hiearatchy to provide tag resolution in merged trees (in
		///     presentation for example)
		/// </summary>
		public IZetaRow TemporalParent { get; set; }

		/// <summary>
		/// Целевой объект смещения для запроса (используется в сценариях с динамическим деревом)
		/// </summary>
		public IZetaObject TargetObject { get; set; }

		/// <summary>
		///     s-list of groups
		/// </summary>
		public virtual string GroupCache { get; set; }

		/// <summary>
		///     Container object
		/// </summary>
		public virtual IZetaMainObject Object { get; set; }


		/// <summary>
		///     True - объект активен
		/// </summary>
		public virtual bool Active { get; set; }

		/// <summary>
		///     ID (FK) of referenced row
		/// </summary>
		public virtual int? RefId { get; set; }

		/// <summary>
		///     ID (FK) of contrainer obj
		/// </summary>
		public virtual int? ObjectId { get; set; }

		/// <summary>
		///     Applys local <paramref name="property" /> to it and descendants (???)
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <param name="cascade"></param>
		public virtual void ApplyProperty(string property, object value, bool cascade = true) {
			LocalProperties[property] = value;
			if (cascade && HasChildren()) {
				foreach (var c in Children) {
					c.ApplyProperty(property, value, cascade);
				}
			}
		}

		/// <summary>
		///     Resolves local proprty over hierarchy
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// </returns>
		public virtual object GetLocal(string name) {
			if (!LocalProperties.ContainsKey(name)) {
				return "";
			}
			return LocalProperties[name] ?? "";
		}

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaRow.Level" /> of row in
		///     hierarchy
		/// </summary>
		public virtual int Level {
			get {
				if (null == Path) {
					if (null == Parent) {
						return 0;
					}
					else {
						return Parent.Level + 1;
					}
				}
				return Regex.Matches(Path, @"[^/]+").Count;
			}
		}


		/// <summary>
		///     resolves role over hierarchy
		/// </summary>
		public virtual string FullRole {
			get {
				if (Role.IsNotEmpty()) {
					return Role;
				}
				if (Parent == null) {
					return "";
				}
				return Parent.FullRole;
			}
		}

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Row.Currency" /> of entity
		/// </summary>
		public virtual string Currency { get; set; }

		/// <summary>
		///     Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		/// <summary>
		///     Formula's activity flag
		/// </summary>
		public virtual bool IsFormula { get; set; }

		/// <summary>
		///     Formula's definition
		/// </summary>
		public virtual string Formula { get; set; }


		/// <summary>
		///     Type of measure <c>(ru : единица измерения)</c>
		/// </summary>
		public virtual string Measure { get; set; }

		/// <summary>
		///     Flag that measure must be setted up dynamically
		/// </summary>
		public virtual bool IsDynamicMeasure { get; set; }




		/// <summary>
		///     Resolves meausure with checking of dynamics
		/// </summary>
		/// <returns>
		/// </returns>
		public virtual string ResolveMeasure() {
			var mes = Measure;
			if (mes.IsEmpty() && null != RefTo) {
				mes = RefTo.Measure;
			}
			mes = mes ?? "";
			if (mes.Contains("dir")) {
				mes = MetaCache.Default.Get<IZetaRow>(mes.Replace("/", "")).Measure;
			}
			return mes;
		}


		/// <summary>
		///     Referenced row
		/// </summary>
		public virtual IZetaRow RefTo { get; set; }


		/// <summary>
		///     ID (FK) of extended referenced row
		/// </summary>
		public virtual int? ExRefToId { get; set; }

		/// <summary>
		///     Extended reference to row
		/// </summary>
		public virtual IZetaRow ExRefTo { get; set; }


		/// <summary>
		///     Helper code that maps any foreign coding system
		/// </summary>
		public virtual string OuterCode { get; set; }


		/// <summary>
		///     Slash-delimited list of mark codes
		/// </summary>
		public virtual string MarkCache { get; set; }

		/// <summary>
		///     Temporary (local) properties collection
		/// </summary>
		public virtual IDictionary<string, object> LocalProperties {
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		/// <summary>
		///     s-list of ZetaObject group codes, that is actual for row
		/// </summary>
		public virtual string ObjectGroups { get; set; }

		/// <summary>
		///     Ui level validation code
		/// </summary>
		public virtual string Validator { get; set; }

		/// <summary>
		///     NEED INVESTIGATION
		/// </summary>
		public virtual string ColumnSubstitution { get; set; }

		/// <summary>
		///     NEED INVESTIGATION
		/// </summary>
		/// <param name="incode"></param>
		/// <returns>
		/// </returns>
		public virtual string ResolveColumnCode(string incode) {
			prepareColumnMap();
			if (columnmap.ContainsKey(incode)) {
				return columnmap[incode];
			}
			return incode;
		}
		/// <summary>
		///     propagetes group definition as local property
		/// </summary>
		/// <param name="groupname"></param>
		/// <param name="applyUp"></param>
		/// <param name="propname"></param>
		public virtual void PropagateGroupAsProperty(string groupname, bool applyUp = true, string propname = null) {
			propname = propname ?? groupname;
			Func<IZetaRow, bool> test =
				r => ((IZetaFormsSupport) r).GroupCache.SmartSplit(false, true, '/', ';').Any(x => x == groupname);
			ApplyPropertyByCondition(propname, true, applyUp, false, test);
		}

		/// <summary>
		///     Apply local property to it and other properties with up and down visitor
		///     patter
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="value"></param>
		/// <param name="applyUp"></param>
		/// <param name="applyDown"></param>
		/// <param name="test"></param>
		public virtual void ApplyPropertyByCondition(string prop, object value, bool applyUp, bool applyDown,
		                                             Func<IZetaRow, bool> test) {
			foreach (var r in AllChildren) {
				if (r.LocalProperties.ContainsKey(prop) && Equals(r.LocalProperties[prop], value)) {
					continue;
				}
				if (test(r)) {
					r.LocalProperties[prop] = value;
					if (applyUp) {
						var current = r;
						while (null != (current = current.Parent)) {
							current.LocalProperties[prop] = value;
						}
					}
					if (applyDown) {
						foreach (var c in r.AllChildren) {
							c.LocalProperties[prop] = value;
						}
					}
				}
			}
		}

		/// <summary>
		///     Old style visitor accessor to cleanup by code filter
		/// </summary>
		/// <param name="codes"></param>
		public virtual void CleanupByChildren(IEnumerable<string> codes) {
			if (LocalProperties.ContainsKey("cleaned")) {
				return;
			}
			var directcodes = codes.Where(x => !x.StartsWith("GR:")).ToArray();
			var grpcodes = codes.Where(x => x.StartsWith("GR:")).Select(x => x.Substring(3)).ToArray();

			var result = false;

			if (grpcodes.Length != 0) {
				var groups = GroupCache.SmartSplit(false, true, '/', ';');
				if (grpcodes.Any(x => groups.Contains(x))) {
					result = true;
				}
				else if (null !=
				         AllChildren.FirstOrDefault(
					         x => ((IZetaFormsSupport) x).GroupCache.SmartSplit(false, true, ';', '/').Intersect(grpcodes).Count() != 0)) {
					result = true;
				}
			}

			if (directcodes.Length != 0) {
				if (directcodes.Contains(Code)) {
					result = true;
				}
				else if (null != AllChildren.FirstOrDefault(x => directcodes.Contains(x.Code))) {
					result = true;
				}
				else {
					result = false;
				}
			}

			ApplyPropertyIfNew("cleaned", result);

			foreach (var c in Children) {
				c.CleanupByChildren(codes);
			}
		}


		/// <summary>
		///     Apply local <paramref name="property" /> to it and descendants if not yet
		///     setted (???)
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <param name="children"></param>
		public virtual void ApplyPropertyIfNew(string property, object value, bool children = false) {
			if (!LocalProperties.ContainsKey(property)) {
				LocalProperties[property] = value;
			}
			if (children) {
				foreach (var nativeChild in Children) {
					nativeChild.ApplyPropertyIfNew(property, value, children);
				}
			}
		}

		/// <summary>
		///     Full name of row
		/// </summary>
		public virtual string FullName { get; set; }

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaRow.Role" /> to access row
		/// </summary>
		public virtual string Role { get; set; }

		/// <summary>
		///     Full collection of all children down
		/// </summary>
		public virtual IZetaRow[] AllChildren {
			get {
				lock (this) {
					if (null == _allchildren) {
						var result = new List<IZetaRow>();
						foreach (var row in Children.OrderBy(x => ((Row) x).GetSortKey())) {
							_register(row, result);
						}
						_allchildren = result.ToArray();
					}
					return _allchildren;
				}
			}
		}

		/// <summary>
		///     Helper method to identify activity for object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>
		/// </returns>
		public virtual bool IsActiveFor(IZetaMainObject obj) {
			var intag = this.ResolveTag("pr_stobj");
			if (intag.IsEmpty()) {
				intag = this.ResolveTag("viewforgroup");
			}
			var extag = this.ResolveTag("pr_exobj");
			var ins = intag.ToUpper().SmartSplit();
			var exs = extag.ToUpper().SmartSplit();
			foreach (var ex in exs) {
				if (obj.IsMatchZoneAcronym(ex)) {
					return false;
				}
			}
			if (ins.Count > 0) {
				foreach (var i in ins) {
					if (obj.IsMatchZoneAcronym(i)) {
						return true;
					}
				}
				return false;
			}

			if (null != RefTo) {
				return RefTo.IsActiveFor(obj);
			}

			return true;
		}


		/// <summary>
		///     Helper method to identify activity on period
		/// </summary>
		/// <param name="year"></param>
		/// <returns>
		/// </returns>
		public virtual bool IsObsolete(int year) {
			var obs = this.ResolveTag("obsolete").ToInt();
			if (0 == obs) {
				return false;
			}
			if (obs > year) {
				return false;
			}
			return true;
		}

		
		/// <summary>
		/// Prepares shallow non hierarchical copy of this node
		/// </summary>
		/// <returns></returns>
		protected override IZetaRow GetHierarchicalCopyBase() {
			var result = (Row)MemberwiseClone();
			result._children = new List<IZetaRow>();
			result._allchildren = null;
			result.LocalProperties = new Dictionary<string, object>(LocalProperties);
			if (null != _children) {
				foreach (var row in _children) {
					var r_ = row.GetCopyOfHierarchy();
					r_.Parent = result;
					result._children.Add((IZetaRow) r_);
				}
			}

			if (null != result.RefTo)
			{
				result.RefTo = (IZetaRow) result.RefTo.GetCopyOfHierarchy();
			}
			if (null != result.ExRefTo)
			{
				result.ExRefTo = (IZetaRow) result.ExRefTo.GetCopyOfHierarchy();
			}
			return result;
		}
		

		/// <summary>
		///     Method for cleanup and rewind collection of
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaRow.AllChildren" />
		/// </summary>
		public virtual void ResetAllChildren() {
			_allchildren = null;
			if (_children != null) {
				foreach (var row in _children) {
					row.ResetAllChildren();
				}
			}
		}

		/// <summary>
		///     Дата начала
		/// </summary>
		public DateTime Start { get; set; }

		/// <summary>
		///     Дата окончания
		/// </summary>
		public DateTime Finish { get; set; }

		/// <summary>
		///     <see cref="Uri.Check" /> that old-style mark is seted
		/// </summary>
		/// <param name="code"></param>
		/// <returns>
		/// </returns>
		public virtual bool IsMarkSeted(string code) {
			if (code.ToUpper() == "ISFORMULA") {
				return IsFormula;
			}
			return WithMarksExtension.IsMarkSeted(this, code);
		}

		private string GetSortKey() {
			return string.Format("{0:00000}_{1}_{2}", Index, OuterCode ?? "", Code);
		}

		private void _register(IZetaRow row, List<IZetaRow> result) {
			result.Add(row);
			if (((Row) row)._children != null) {
				foreach (var zetaRow in row.Children.OrderBy(x => ((Row) x).GetSortKey())) {
					_register(zetaRow, result);
				}
			}
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
		private ICollection<IZetaRow> _children;
		private IDictionary<string, string> columnmap;
		private IDictionary<string, object> localProperties;
	}
}