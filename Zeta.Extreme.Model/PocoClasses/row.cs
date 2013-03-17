#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : row.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// 
	/// </summary>
	public partial class Row : IZetaRow {
		 public virtual string Grp { get; set; }

		 public virtual IZetaMainObject Org { get; set; }

		/// <summary>
		/// 	Режим использования формулы с Extreme
		/// </summary>
		public virtual int ExtremeFormulaMode { get; set; }

		 public virtual Guid Uid { get; set; }


		 public virtual bool Active { get; set; }

		public virtual int? ParentId { get; set; }
		public virtual int? RefId { get; set; }
		public virtual int? ObjectId { get; set; }

		public virtual void ApplyProperty(string property, object value, bool cascade = true) {
			LocalProperties[property] = value;
			if (cascade && null != NativeChildren) {
				foreach (var c in NativeChildren) {
					c.ApplyProperty(property, value, cascade);
				}
			}
		}

		public virtual object GetLocal(string name) {
			if (!LocalProperties.ContainsKey(name)) {
				return "";
			}
			return LocalProperties[name] ?? "";
		}

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

		public virtual string Tag { get; set; }

		 public virtual string Lookup { get; set; }

		public virtual IZetaRow TemporalParent { get; set; }


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

		 public virtual string Valuta { get; set; }

		 public virtual bool IsDynamicLookup { get; set; }

		 public virtual int Id { get; set; }

		 public virtual string Name { get; set; }

		 public virtual string Code { get; set; }

		 public virtual string Comment { get; set; }

		 public virtual DateTime Version { get; set; }

		/// <summary>
		/// 	Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		 public virtual bool IsFormula { get; set; }

		 public virtual string Formula { get; set; }



		 public virtual string Measure { get; set; }

		 public virtual bool IsDynamicMeasure { get; set; }


		 public virtual IZetaRow Parent { get; set; }


		public virtual IList<IZetaRow> NativeChildren {
			get { return _children; }
		}

		 public virtual IList<IZetaRow> Children {
			get { return _children; }
			set { _children = value; }
		}

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

		 public virtual string Path { get; set; }

		 public virtual IZetaRow RefTo { get; set; }


		public virtual int? ExRefToId { get; set; }

		 public virtual IZetaRow ExRefTo { get; set; }

		 public virtual int Idx { get; set; }

		 public virtual string OuterCode { get; set; }

		public virtual string Group {
			get { return Grp; }
			set { Grp = value; }
		}

		
		public virtual IZetaMainObject Object {
			get { return Org; }
			set { Org = value; }
		}

		 public virtual string MarkCache { get; set; }

		public virtual IDictionary<string, object> LocalProperties {
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		 public virtual string ObjectGroups { get; set; }
		 public virtual string FormElementType { get; set; }
		 public virtual string Validator { get; set; }

		 public virtual string ColumnSubstitution { get; set; }

		public virtual string ResolveColumnCode(string incode) {
			prepareColumnMap();
			if (columnmap.ContainsKey(incode)) {
				return columnmap[incode];
			}
			return incode;
		}


		public virtual string ResolveTag(string name) {
			if (TagHelper.Has(Tag, name)) {
				return TagHelper.Value(Tag, name) ?? "";
			}
			if (null != TemporalParent) {
				return TemporalParent.ResolveTag(name);
			}
			if (null == Parent) {
				return "";
			}
			return Parent.ResolveTag(name);
		}


		public virtual void PropagateGroupAsProperty(string groupname, bool applyUp = true, string propname = null) {
			propname = propname ?? groupname;
			Func<IZetaRow, bool> test = r => r.Group.SmartSplit(false, true, '/', ';').Any(x => x == groupname);
			ApplyPropertyByCondition(propname, true, applyUp, false, test);
		}

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

		public virtual void CleanupByChildren(IEnumerable<string> codes) {
			if (LocalProperties.ContainsKey("cleaned")) {
				return;
			}
			var directcodes = codes.Where(x => !x.StartsWith("GR:")).ToArray();
			var grpcodes = codes.Where(x => x.StartsWith("GR:")).Select(x => x.Substring(3)).ToArray();

			var result = false;

			if (grpcodes.Length != 0) {
				var groups = Group.SmartSplit(false, true, '/', ';');
				if (grpcodes.Any(x => groups.Contains(x))) {
					result = true;
				}
				else if (null !=
				         AllChildren.FirstOrDefault(x => x.Group.SmartSplit(false, true, ';', '/').Intersect(grpcodes).Count() != 0)) {
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

			foreach (var c in NativeChildren) {
				c.CleanupByChildren(codes);
			}
		}


		public virtual void ApplyPropertyIfNew(string property, object value, bool children = false) {
			if (!LocalProperties.ContainsKey(property)) {
				LocalProperties[property] = value;
			}
			if (children) {
				foreach (var nativeChild in NativeChildren) {
					nativeChild.ApplyPropertyIfNew(property, value, children);
				}
			}
		}

		 public virtual string FullName { get; set; }
		 public virtual string Role { get; set; }

		public virtual string SortKey {
			get { return string.Format("{0:00000}_{1}_{2}", Idx == 0 ? 10000 : Idx, OuterCode ?? "", Code); }
		}

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

		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
            var query = new MetalinkRecord
                            {
                                Src = Code,
                                SrcType = "zeta.row",
                                TrgType = nodetype,
                                Type = linktype,
                                SubType = subtype
                            };
            return new MetalinkRepository().Search(query, system);
			 */
		}

		public virtual bool IsActiveFor(IZetaMainObject obj) {
			var intag = ResolveTag("pr_stobj");
			if (intag.IsEmpty()) {
				intag = ResolveTag("viewforgroup");
			}
			var extag = ResolveTag("pr_exobj");
			var ins = intag.ToUpper().SmartSplit();
			var exs = extag.ToUpper().SmartSplit();
			foreach (var ex in exs) {
				if (obj.IsMatchZoneAcronim(ex)) {
					return false;
				}
			}
			if (ins.Count > 0) {
				foreach (var i in ins) {
					if (obj.IsMatchZoneAcronim(i)) {
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

		private IList<IZetaRow> _children;
		private IDictionary<string, string> columnmap;
		private IDictionary<string, object> localProperties;
		private IZetaRow[] _allchildren;
		

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
				var result = (Row) MemberwiseClone();
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
			if (((Row) row)._children != null) {
				foreach (var zetaRow in row.Children.OrderBy(x => ((Row) x).GetSortKey())) {
					_register(zetaRow, result);
				}
			}
		}

		public virtual string GetParentedName() {
			var result = Name;
			if (null != Parent) {
				result = ((Row) Parent).GetParentedName() + " / " + result;
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
	}
}