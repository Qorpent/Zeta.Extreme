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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Qorpent;
using Qorpent.Model;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model {
	/// <summary>
	///     Default implementation or ZetaObject entity of Zeta model
	/// </summary>
	public sealed partial class Obj : Entity, IZetaMainObject {
		/// <summary>
		/// </summary>
		public Obj() {
			Start = QorpentConst.Date.Begin;
			Finish = QorpentConst.Date.End;
			// Properties = new Dictionary<string, object>();
		}

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Obj.Division" /> of current ZetaObject
		/// </summary>
		public IObjectDivision Division { get; set; }


		/// <summary>
		///     Point of ZetaObject's location
		/// </summary>
		[Obsolete("ZC-417")]
		public IZetaPoint Point { get; set; }


		/// <summary>
		///     Helper code that maps any foreign coding system
		/// </summary>
		public string OuterCode { get; set; }

		/// <summary>
		///     ID (FK) of parent <see cref="Obj" />
		/// </summary>
		/// <remarks>
		///     Intended to use with ORM/SQL scenario
		/// </remarks>
		/// <exception cref="Exception">
		///     cannot setup <see cref="Zeta.Extreme.Model.Obj.ParentId" /> when
		///     <see cref="Zeta.Extreme.Model.Obj.Parent" /> is attached
		/// </exception>
		public int? ParentId {
			get {
				if (null != Parent) {
					return Parent.Id;
				}
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
		///     ID (FK) of <see cref="Zeta.Extreme.Model.Obj.Point" /> that current is
		///     attached to
		/// </summary>
		/// <remarks>
		///     Intended to use with ORM/SQL scenario
		/// </remarks>
		/// <exception cref="Exception">
		///     cannot setup <see cref="Zeta.Extreme.Model.Obj.PointId" /> when Point is
		///     attached
		/// </exception>
		public int? PointId {
			get {
				if (null != Point) {
					return Point.Id;
				}
				return _pointId;
			}
			set {
				if (null != Parent) {
					throw new Exception("cannot setup PointId when Point is attached");
				}
				_pointId = value;
			}
		}

		/// <summary>
		///     ID (FK) of <see cref="Zeta.Extreme.Model.Obj.Department" /> that current
		///     is attached to
		/// </summary>
		/// <remarks>
		///     Intended to use with ORM/SQL scenario
		/// </remarks>
		/// <exception cref="Exception">
		///     cannot setup <see cref="Zeta.Extreme.Model.Obj.DepartmentId" /> when
		///     <see cref="Zeta.Extreme.Model.Obj.Department" /> is attached
		/// </exception>
		public int? DepartmentId {
			get {
				if (null != Department) {
					return Department.Id;
				}
				return _departmentId;
			}
			set {
				if (null != Department) {
					throw new Exception("cannot setup DepartmentId when Department is attached");
				}
				_departmentId = value;
			}
		}

		/// <summary>
		///     ID (FK) of <see cref="Zeta.Extreme.Model.Obj.ObjType" /> that current is
		///     attached to
		/// </summary>
		/// <remarks>
		///     Intended to use with ORM/SQL scenario
		/// </remarks>
		/// <exception cref="Exception">
		///     cannot setup <see cref="Zeta.Extreme.Model.Obj.ObjTypeId" /> when ObjType
		///     is attached
		/// </exception>
		public int? ObjTypeId {
			get {
				if (null != ObjType) {
					return ObjType.Id;
				}
				return _objtypeId;
			}
			set {
				if (null != ObjType) {
					throw new Exception("cannot setup ObjTypeId when ObjType is attached");
				}
				_objtypeId = value;
			}
		}


		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Obj.Department" /> of current ZetaObject (
		///     <c>ru</c> : <c>отрасль</c> )
		/// </summary>
		public IObjectDepartment Department { get; set; }


		/// <summary>
		///     ID (FK) of
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaMainObject.Division" /> that
		///     current is attached to
		/// </summary>
		/// <remarks>
		///     Intended to use with ORM/SQL scenario
		/// </remarks>
		public int? DivisionId { get; set; }

		/// <summary>
		///     Full hierarchy path of ZetaObject (see
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaMainObject.Parent" /> and
		///     Code)
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Obj.Currency" /> of entity
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		///     Full name of ZetaObject
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		///     Short display name of ZetaObject
		/// </summary>
		public string ShortName { get; set; }

		/// <summary>
		///     Тип формулы
		/// </summary>
		public string FormulaType { get; set; }

		/// <summary>
		///     Formula's activity flag
		/// </summary>
		public bool IsFormula {
			get { return !string.IsNullOrWhiteSpace(Formula); }
			set { _isFormula = value; }
		}

		/// <summary>
		///     Formula's definition
		/// </summary>
		public string Formula { get; set; }


		/// <summary>
		///     Flag that ZetaObject must be shown on start page of application
		/// </summary>
		public bool ShowOnStartPage { get; set; }


		/// <summary>
		///     list of attached details
		/// </summary>
		public IList<IZetaDetailObject> Details { get; set; }


		/// <summary>
		///     Slash-delimited list of groups that ZetaObject is attached to
		/// </summary>
		public string GroupCache { get; set; }

		/// <summary>
		///     Retrieves all children in hierarchy down
		/// </summary>
		/// <returns>
		/// </returns>
		public IEnumerable<IZetaMainObject> AllChildren(int level = 10, string typefiler = null) {
			if (0 == level) {
				yield break;
			}
			foreach (var child in Children.OrderBy(x => x.Index*100000 + x.Id)) {
				if (matchTypeFilter(child, typefiler)) {
					yield return child;
				}
				foreach (var nest in child.AllChildren(level - 1, typefiler)) {
					yield return nest;
				}
			}
		}


		/// <summary>
		///     <c>List</c> of mappings ZetaObject's users to themas
		/// </summary>
		public IList<IUserBizCaseMap> UserBizCaseMaps { get; set; }

		/// <summary>
		///     NEED INVESTIGATION!
		/// </summary>
		/// <returns>
		/// </returns>
		public string[] GetConfiguredBizCaseCodes() {
			return UserBizCaseMaps.Select(x => x.ThemaCode).Distinct().ToArray();
		}

		/// <summary>
		///     NEED INVESTIGATION!
		/// </summary>
		/// <returns>
		/// </returns>
		public IZetaUser[] GetConfiguredUsers() {
			return UserBizCaseMaps.Select(x => x.User).Distinct().ToArray();
		}

		/// <summary>
		///     NEED INVESTIGATION!
		/// </summary>
		/// <returns>
		/// </returns>
		public IUserBizCaseMap GetUserMap(string themacode, bool plan) {
			return UserBizCaseMaps.FirstOrDefault(x => x.ThemaCode == themacode && x.IsPlan == plan);
		}

		/// <summary>
		///     NEED INVESTIGATION!
		/// </summary>
		/// <returns>
		/// </returns>
		public string[] GetConfiguredThemas(IZetaUser usr, bool plan) {
			return
				UserBizCaseMaps.Where(x => x.User.Id == usr.Id && x.IsPlan == plan).Select(x => x.ThemaCode).Distinct().ToArray();
		}

		/// <summary>
		///     Registry of ZetaObject-attached users of application
		/// </summary>
		public IList<IZetaUser> Users { get; set; }


		/// <summary>
		///     <see cref="Zeta.Extreme.Model.Obj.Start" /> date of ZetaObject's activity
		/// </summary>
		public DateTime Start { get; set; }

		/// <summary>
		///     End date of ZetaObject's activity
		/// </summary>
		public DateTime Finish { get; set; }

		/// <summary>
		///     References definition of ZetaObject's type (in zeta terms)
		/// </summary>
		public IObjectType ObjType { get; set; }

		/// <summary>
		///     <para>
		///         <see cref="Zeta.Extreme.Model.Obj.Parent" /> <see cref="IZetaObject" /> ,
		///         can be null, this one will be appeared in
		///         <see cref="Zeta.Extreme.Model.Inerfaces.IZetaMainObject.Children" />
		///     </para>
		///     <para>collection</para>
		/// </summary>
		public IZetaMainObject Parent { get; set; }

		/// <summary>
		///     Sub-IZetaObjects, for which <c>this</c> one is a
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaMainObject.Parent" />
		/// </summary>
		public IList<IZetaMainObject> Children { get; set; }

		/// <summary>
		///     Temporary (local) properties collection
		/// </summary>
		public IDictionary<string, object> LocalProperties {
			get { return _localProperties ?? (_localProperties = new Dictionary<string, object>()); }
		}

		/// <summary>
		///     Resolves tag value by it's <c>name</c>
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// </returns>
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

		/// <summary>
		///     Checkout if current ZetaObject is match acronym of zone
		/// </summary>
		/// <param name="s"></param>
		/// <returns>
		/// </returns>
		public bool IsMatchZoneAcronym(string s) {
			s = s.ToUpper();
			if (!s.Contains("_")) {
				if (Regex.IsMatch(s, @"^\d+$")) {
					return s == Id.ToString(CultureInfo.InvariantCulture);
				}
				return GroupCache.ToUpper().Contains("/" + s + "/");
			}
			if (s.StartsWith("OBJ_")) {
				return s.Substring(4) == Id.ToString(CultureInfo.InvariantCulture);
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

		/// <summary>
		///     True - объект активен
		/// </summary>
		public bool Active { get; set; }

		/// <summary>
		/// </summary>
		/// <param name="child"></param>
		/// <param name="typefiler"></param>
		/// <returns>
		/// </returns>
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

		/// <summary>
		///     Checks equality with another obj
		/// </summary>
		/// <param name="org"></param>
		/// <returns>
		/// </returns>
		private bool Equals(Obj org) {
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

		/// <summary>
		///     Определяет, равен ли заданный объект <see cref="Object" /> текущему
		///     объекту <see cref="Object" /> .
		/// </summary>
		/// <param name="obj">
		///     Объект, который требуется сравнить с текущим объектом.
		/// </param>
		/// <returns>
		///     true, если заданный объект равен текущему объекту; в противном случае —
		///     false.
		/// </returns>
		/// <filterpriority>
		///     2
		/// </filterpriority>
		public override bool Equals(object obj) {
			if (ReferenceEquals(this, obj)) {
				return true;
			}
			return Equals(obj as Obj);
		}

		/// <summary>
		///     Играет роль хэш-функции для определенного типа.
		/// </summary>
		/// <returns>
		///     Хэш-код для текущего объекта <see cref="Object" /> .
		/// </returns>
		/// <filterpriority>
		///     2
		/// </filterpriority>
		public override int GetHashCode() {
			var result = Id;
			result = 29*result + (Code != null ? Code.GetHashCode() : 0);
			result = 29*result + (Name != null ? Name.GetHashCode() : 0);

			return result;
		}

		private int? _departmentId;


		private bool _isFormula;
		private IDictionary<string, object> _localProperties;
		private int? _objtypeId;
		private int? _parentId;
		private int? _pointId;
	}
}