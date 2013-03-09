#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : row.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;
using Qorpent.Utils.Extensions;

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
		/// 	Возвращает копию строки, отфильтрованную по крыжу для предприятия по крыжу и (опционально) по филиалам
		/// </summary>
		/// <param name="objid"> ИД предприятия </param>
		/// <param name="selectorname"> Код крыжа </param>
		/// <param name="usefilialfilter"> Использовать фильтр по филиалам </param>
		/// <remarks>
		/// 	Зависит от БД!!!
		/// </remarks>
		/// <returns> </returns>
		public virtual IZetaRow FilterTree(int objid, string selectorname, bool usefilialfilter = false) {
			var viewname = "zetai.hard_m140_filter";
			if (usefilialfilter) {
				if (selectorname.hasContent()) {
					viewname = "zetai.full_m140_filter";
				}
				else {
					viewname = "zetai.light_m140_filter";
				}
			}
			var obj = myapp.storage.Get<IZetaMainObject>().Load(objid);
			var query = "select row from " + viewname +
			            " where (obj=@obj or srccode=@typecode or srccode=@clscode) and (type=@type or @type='') and path like @path";
			var typecode = "NOTYPE";
			var clscode = "NOCLSCODE";
			if (null != obj.ObjType) {
				typecode = obj.ObjType.Code;
				clscode = obj.ObjType.Class.Code;
			}
			var param = new Dictionary<string, object>
				{
					{"@obj", objid},
					{"@type", selectorname},
					{"@path", "%/" + Code + "/%"},
					{"@typecode", typecode},
					{"@clscode", clscode},
				};
			var codes = myapp.sql.ExecuteArray<string>(query, param);
			var result = Copy(true);
			result.ResetAllChildren();
			result.CleanupByChildren(codes);
			foreach (var r in result.AllChildren) {
				if (!r.LocalProperties["cleaned"].ToBool()) {
					r.Parent.NativeChildren.Remove(r);
				}
			}
			result.ResetAllChildren();
			return result;
		}

		/// <summary>
		/// 	Проверяет - является ли строка избранной для предприятия по указанному крыж-селектору
		/// </summary>
		/// <param name="objid"> ИД предприятия </param>
		/// <param name="selectorname"> </param>
		/// <remarks>
		/// 	Зависит от БД!!!
		/// </remarks>
		/// <returns> </returns>
		public virtual bool IsFavorite(int objid, string selectorname) {
			var query = "select count(*) from zetai.hard_m140_filter where obj = @obj and type=@type and row = @row";
			var param = new Dictionary<string, object> {{"@obj", objid}, {"@type", selectorname}, {"@row", Code}};
			var count = myapp.sql.ExecuteScalar<int>(query, param);
			return 0 != count;
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

		public virtual IList<IMarkLinkBase> GetMarkLinks() {
			return SlashListHelper.ReadList(MarkCache).Select(x => (IMarkLinkBase) new TreeMark
				{
					Target = this,
					Mark = myapp.storage.Get<IMark>().Load(x),
				}).ToList();
			/*

            if (null == MarkLinks){
                if (0 != Id && (null != myapp.storage.Get<row>(false))){
                    myapp.storage.Get<row>().Refresh(this);
                }
            }
            if (null == MarkLinks){
                MarkLinks = new List<IZetaRowMark>();
            }
            return MarkLinks.OfType<IMarkLinkBase>().ToList();
			 */
		}

		public virtual void RemoveMark(IMark mark) {
			var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
			if (null != todel) {
				MarkLinks.Remove(todel);
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
				if (ColumnSubstitution.hasContent()) {
					var rules = ColumnSubstitution.split();
					foreach (var rule in rules) {
						var pair = rule.split(false, true, '=');
						columnmap[pair[0]] = pair[1];
					}
				}
			}
		}

		private IZetaRow[] _allchildren;
		}
}