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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaMainObject.cs

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Zeta Main <c>Object</c> <c>interface</c>
	/// </summary>
	public interface IZetaMainObject : ICanResolveTag,
	                                   IWithObjType,
	                                   IZetaQueryDimension, IWithDetailObjects, IZetaObject,IWithOuterCode,IWithCurrency,IContextEntity {
		/// <summary>
		/// Slash-delimited list of groups that ZetaObject is attached to
		/// </summary>
		string GroupCache { get; set; }

		/// <summary>
		/// Full name of ZetaObject
		/// </summary>
		string FullName { get; set; }
		/// <summary>
		/// Short display name of ZetaObject
		/// </summary>
		string ShortName { get; set; }


		/// <summary>
		/// Flag that  ZetaObject must be shown on start page of application
		/// </summary>
		[Obsolete("Deprecated due to bad design of usage (merges model and UI concern)")]
		bool ShowOnStartPage { get; set; }
		/// <summary>
		/// Sub-IZetaObjects, for which <c>this</c> one is a
		/// <see cref="Zeta.Extreme.Model.Inerfaces.IZetaMainObject.Parent" />
		/// </summary>
		IList<IZetaMainObject> Children { get; set; }
		/// <summary>
		/// Parent <see cref="IZetaObject"/>, can be null, this one will be appeared in <see cref="Children"/> collection
		/// </summary>
		IZetaMainObject Parent { get; set; }

		/// <summary>
		/// <c>List</c> of mappings ZetaObject's users to themas
		/// </summary>
		[Obsolete("Due to ZC-408 must be moved to special extension")]
		IList<IUsrThemaMap> UserBizCaseMaps { get; set; }

		/// <summary>
		/// Full hierarchy path of ZetaObject (see <see cref="Parent"/> and Code)
		/// </summary>
		string Path { get; set; }
		/// <summary>
		/// Division of current ZetaObject
		/// </summary>
		IObjectDivision Division { get; set; }
		/// <summary>
		/// Department of current ZetaObject (<c>ru</c>: <c>отрасль</c>)
		/// </summary>
		IObjectDepartment Department { get; set; }
		/// <summary>
		/// Point of ZetaObject's location
		/// </summary>
		[Obsolete("ZC-417")]
		IZetaPoint Point { get; set; }
		/// <summary>
		/// Registry of ZetaObject-attached users of application
		/// </summary>
		IList<IZetaUser> Users { get; set; }

		/// <summary>
		/// ID (FK) of parent <see cref="Obj"/>
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		/// <exception cref="Exception">cannot setup ParentId when Parent is attached</exception>
		int? ParentId { get; set; }

		/// <summary>
		/// ID (FK) of <see cref="Point"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		/// <exception cref="Exception">cannot setup PointId when Point is attached</exception>
		int? PointId { get; set; }

		/// <summary>
		/// ID (FK) of <see cref="Department"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		/// <exception cref="Exception">cannot setup DepartmentId when Department is attached</exception>
		int? DepartmentId { get; set; }

		/// <summary>
		/// ID (FK) of <see cref="ObjectType"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		int? ObjTypeId { get; set; }
		/// <summary>
		/// ID (FK) of <see cref="Division"/> that current is attached to
		/// </summary>
		/// <remarks>Intended to use with ORM/SQL scenario</remarks>
		int? DivisionId { get; set; }


		/// <summary>
		/// NEED INVESTIGATION!
		/// </summary>
		/// <returns></returns>
		[Obsolete("Due to ZC-408 must be moved to special extension")]
		string[] GetConfiguredBizCaseCodes();
		/// <summary>
		/// NEED INVESTIGATION!
		/// </summary>
		/// <returns></returns>
		[Obsolete("Due to ZC-408 must be moved to special extension")]
		IUsrThemaMap GetUserMap(string themacode, bool plan);
		/// <summary>
		/// NEED INVESTIGATION!
		/// </summary>
		/// <returns></returns>
		[Obsolete("Due to ZC-408 must be moved to special extension")]
		IZetaUser[] GetConfiguredUsers();
		/// <summary>
		/// NEED INVESTIGATION!
		/// </summary>
		/// <returns></returns>
		[Obsolete("Due to ZC-408 must be moved to special extension")]
		string[] GetConfiguredThemas(IZetaUser usr, bool plan);


		/// <summary>
		/// Retrieves all children in  hierarchy down
		/// </summary>
		/// <returns></returns>
		IEnumerable<IZetaMainObject> AllChildren(int level =100, string typefilter = null);

		/// <summary>
		/// Checkout if current ZetaObject is match acronym of zone
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		[Obsolete("Due to ZC-410 must be moved to another implementation")]
		bool IsMatchZoneAcronym(string s);
	}
}