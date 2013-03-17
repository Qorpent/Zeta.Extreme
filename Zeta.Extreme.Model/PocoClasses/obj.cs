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
// PROJECT ORIGIN: Zeta.Extreme.Model/Obj.cs

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	/// Default implementation or ZetaObject entity of Zeta model
	/// </summary>
	public sealed partial class Obj : IZetaMainObject {
		/// <summary>
		/// </summary>
		public Obj() {
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
			// Properties = new Dictionary<string, object>();
		}

		/// <summary>
		/// Division of current ZetaObject
		/// </summary>
		public IMainObjectGroup Division { get; set; }


		/// <summary>
		/// Point of ZetaObject's location
		/// </summary>
		public IZetaPoint Point { get; set; }


		/// <summary>
		/// Helper code that maps any foreign coding system
		/// </summary>
		public string OuterCode { get; set; }

		/// <summary>
		/// ID (FK) of parent <see cref="Obj"/>
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		/// <exception cref="Exception">cannot setup ParentId when Parent is attached</exception>
		public int? ParentId {
			get {
				if (null != Parent) return Parent.Id;
				return _parentId;
			}
			set {
				if (null != Parent) {
					throw new Exception("cannot setup ParentId when Parent is attached");
				}
				_parentId = value;
			}
		}
		/// <summary>
		/// ID (FK) of <see cref="Point"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		/// <exception cref="Exception">cannot setup PointId when Point is attached</exception>
		public int? PointId {
			get {
				if (null != Point) return Point.Id;
				return _pointId;
			}
			set {
				if (null != Parent){
					throw new Exception("cannot setup PointId when Point is attached");
				}
				_pointId = value;
			}
		}

		/// <summary>
		/// ID (FK) of <see cref="Department"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		/// <exception cref="Exception">cannot setup DepartmentId when Department is attached</exception>
		public int? DepartmentId
		{
			get
			{
				if (null != Department) return Department.Id;
				return _departmentId;
			}
			set
			{
				if (null != Department)
				{
					throw new Exception("cannot setup DepartmentId when Department is attached");
				}
				_departmentId = value;
			}
		}

		/// <summary>
		/// ID (FK) of <see cref="ObjType"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		public int? ObjTypeId
		{
			get
			{
				if (null != ObjType) return ObjType.Id;
				return _objtypeId;
			}
			set
			{
				if (null != ObjType)
				{
					throw new Exception("cannot setup ObjTypeId when ObjType is attached");
				}
				_objtypeId = value;
			}
		}


		
		/// <summary>
		/// Department of current ZetaObject (<c>ru</c>: <c>отрасль</c>)
		/// </summary>
		public IMainObjectRole Department { get; set; }

		/// <summary>
		/// Словарь локальных (временных) свойств
		/// </summary>
		public IDictionary<string, object> LocalProperties {
			get { return localProperties ?? (localProperties = new Dictionary<string, object>()); }
			set { localProperties = value; }
		}

		public int? DivisionId { get; set; }
		public string Path { get; set; }

		public string Tag { get; set; }
		public string Currency { get; set; }

		public string FullName { get; set; }
		public string ShortName { get; set; }

		/// <summary>
		///     Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		public bool IsFormula {
			get { return !string.IsNullOrWhiteSpace(Formula); }
			set { _isFormula = value; }
		}

		public string Formula { get; set; }


		public bool ShowOnStartPage { get; set; }


		public IList<IZetaDetailObject> DetailObjects { get; set; }


		public int Id { get; set; }

		public string Name { get; set; }


		public string GroupCache { get; set; }

		public string Code { get; set; }

		public string Comment { get; set; }

		public DateTime Version { get; set; }


		public IEnumerable<IZetaMainObject> AllChildren(int level, string typefiler) {
			if (0 == level) {
				yield break;
			}
			foreach (var child in Children.OrderBy(x => x.Idx*100000 + x.Id)) {
				if (matchTypeFilter(child, typefiler)) {
					yield return child;
				}
				foreach (var nest in child.AllChildren(level - 1, typefiler)) {
					yield return nest;
				}
			}
		}


		public IList<IUsrThemaMap> UserBizCaseMaps { get; set; }

		public string[] GetConfiguredBizCaseCodes() {
			return UserBizCaseMaps.Select(x => x.ThemaCode).Distinct().ToArray();
		}

		public IZetaUser[] GetConfiguredUsers() {
			return UserBizCaseMaps.Select(x => x.Usr).Distinct().ToArray();
		}

		public IUsrThemaMap GetUserMap(string themacode, bool plan) {
			return UserBizCaseMaps.FirstOrDefault(x => x.ThemaCode == themacode && x.IsPlan == plan);
		}

		public string[] GetConfiguredThemas(IZetaUser usr, bool plan) {
			return
				UserBizCaseMaps.Where(x => x.Usr.Id == usr.Id && x.IsPlan == plan).Select(x => x.ThemaCode).Distinct().ToArray();
		}

		public IList<IZetaUser> Users { get; set; }


		//public virtual IList<IDocumentOfCorrections> Documents { get; set; }


		public int Idx { get; set; }
		public DateTime Start { get; set; }
		public DateTime Finish { get; set; }

		public IObjectType ObjType { get; set; }

		IObjectType IWithDetailObjectType.Type {
			get { return ObjType; }
			set { ObjType = value; }
		}

		public IZetaMainObject Parent { get; set; }
		public IList<IZetaMainObject> Children { get; set; }

		public IDictionary<string, object> Properties {
			get { return properties ?? (properties = new Dictionary<string, object>()); }
			protected set { properties = value; }
		}

		public MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null,
		                                         string system = "Default") {
			//TODO: implement!!! 
			throw new NotImplementedException();
		}

		public string ResolveTag(string name) {
			var tag = TagHelper.Value(Tag, name);
			if (tag.IsEmpty() && null != ObjType) {
				tag = ObjType.ResolveTag(name);
			}
			if (tag.IsEmpty() && null != Parent) {
				tag = Parent.ResolveTag(name);
			}
			return tag ?? "";
		}

		public bool IsMatchZoneAcronym(string s) {
			s = s.ToUpper();
			if (!s.Contains("_")) {
				if (Regex.IsMatch(s, @"^\d+$")) {
					return s == Id.ToString();
				}
				return GroupCache.ToUpper().Contains("/" + s + "/");
			}
			if (s.StartsWith("OBJ_")) {
				return s.Substring(4) == Id.ToString();
			}
			if (s.StartsWith("GRP_") || s.StartsWith("OG_")) {
				var grp = s.Split('_')[1];
				return GroupCache.ToUpper().Contains("/" + grp + "/");
			}
			if (s.StartsWith("DIV_")) {
				if (null == Division) {
					return false;
				}
				return Division.Code.ToUpper() == s.Substring(4);
			}
			if (s.StartsWith("OTR_")) {
				if (null == Department) {
					return false;
				}
				return Department.Code.ToUpper() == s.Substring(4);
			}
			return GroupCache.ToUpper().Contains("/" + s + "/");
		}

		public IEnumerable<IZetaMainObject> AllChildren() {
			return AllChildren(10, null);
		}

		private bool matchTypeFilter(IZetaMainObject child, string typefiler) {
			if (typefiler.IsEmpty()) {
				return true;
			}
			if (null == child.ObjType) {
				return false;
			}
			var s = "/" + child.ObjType.Class.Code + "/" + child.ObjType.Code + "/";
			return Regex.IsMatch(s, typefiler);
		}

		public bool Equals(Obj org) {
			if (org == null) {
				return false;
			}
			if (Id != org.Id) {
				return false;
			}
			if (!Equals(Name, org.Name)) {
				return false;
			}
			if (!Equals(Code, org.Code)) {
				return false;
			}

			return true;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return Equals(obj as Obj);
		}

		public override int GetHashCode() {
			var result = Id;
			result = 29*result + (Code != null ? Code.GetHashCode() : 0);
			result = 29*result + (Name != null ? Name.GetHashCode() : 0);

			return result;
		}

		public Detail[] FindOwnSubparts() {
			return DetailObjects.Cast<Detail>().ToArray();
		}


		public int CountOwnSubparts() {
			return DetailObjects.Count;
		}

		private IList<IZetaObjectGroup> _groups;
		private bool _isFormula;
		private IDictionary<string, object> localProperties;
		private IDictionary<string, object> properties;
		private int? _parentId;
		private int? _pointId;
		private int? _departmentId;
		private int? _objtypeId;
	}
}