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
// PROJECT ORIGIN: Zeta.Extreme.Model/IZetaRow.cs

#endregion

using System;
using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	///     Over-weighted <see langword="interface" /> of main attribute dimension part
	///     - ROW
	/// </summary>
	public interface IZetaRow : IZetaQueryDimension,
	                            IZetaFormsSupport,
	                            IWithMarkCache, IWithMeasure, IWithCurrency, IContextEntity {
		/// <summary>
		///     s-list of ZetaObject group codes, that is actual for row
		/// </summary>
		[Obsolete("ZC-419 metalink must be used")] string ObjectGroups { get; set; }

		/// <summary>
		///     Ui level validation code
		/// </summary>
		[Obsolete("mixin ui")] string Validator { get; set; }

		/// <summary>
		///     NEED INVESTIGATION
		/// </summary>
		[Obsolete("ZC-418")] string ColumnSubstitution { get; set; }

		/// <summary>
		///     Full name of row
		/// </summary>
		string FullName { get; set; }

		/// <summary>
		///     <see cref="Role" /> to access row
		/// </summary>
		string Role { get; set; }

		/// <summary>
		///     Referenced row
		/// </summary>
		IZetaRow RefTo { get; set; }

		/// <summary>
		///     ID (FK) of parent row
		/// </summary>
		int? ParentId { get; set; }

		/// <summary>
		///     ID (FK) of referenced row
		/// </summary>
		int? RefId { get; set; }

		/// <summary>
		///     ID (FK) of container obj row
		/// </summary>
		[Obsolete("ZC-419")] int? ObjectId { get; set; }

		/// <summary>
		///     Collection for ORM of children to load
		/// </summary>
		[Obsolete("ZC-420 NH - ancestor")] IList<IZetaRow> NativeChildren { get; }

		/// <summary>
		///     Full collection of all children down
		/// </summary>
		IZetaRow[] AllChildren { get; }

		/// <summary>
		///     <see cref="Level" /> of row in hierarchy
		/// </summary>
		int Level { get; }

		/// <summary>
		///     Extended reference to row
		/// </summary>
		IZetaRow ExRefTo { get; set; }

		/// <summary>
		///     ID (FK) of extended referenced row
		/// </summary>
		int? ExRefToId { get; set; }

		/// <summary>
		///     Reference to parent row
		/// </summary>
		IZetaRow Parent { get; set; }

		/// <summary>
		///     <see cref="Children" /> rows
		/// </summary>
		IList<IZetaRow> Children { get; set; }

		/// <summary>
		///     Container object
		/// </summary>
		[Obsolete("ZC-419")] IZetaMainObject Object { get; set; }

		/// <summary>
		///     <see cref="Path" /> to row over hierarchy
		/// </summary>
		string Path { get; set; }
		/// <summary>
		/// s-list of groups
		/// </summary>
		string GroupCache { get; set; }
		/// <summary>
		/// resolves role over hierarchy
		/// </summary>
		string FullRole { get; }

		/// <summary>
		///     NEED INVESTIGATION
		/// </summary>
		/// <param name="incode"></param>
		/// <returns>
		/// </returns>
		[Obsolete("ZC-418")]
		string ResolveColumnCode(string incode);

		/// <summary>
		///     Clone method to get full copy of row with descendants
		/// </summary>
		/// <param name="withchildren"></param>
		/// <returns>
		/// </returns>
		[Obsolete("ZC-421")]
		IZetaRow Copy(bool withchildren);

		/// <summary>
		///     Method for cleanup and rewind collection of
		///     <see cref="Zeta.Extreme.Model.Inerfaces.IZetaRow.AllChildren" />
		/// </summary>
		[Obsolete("ZC-421")]
		void ResetAllChildren();

		/// <summary>
		///     Old style visitor accessor to cleanup by code filter
		/// </summary>
		/// <param name="codes"></param>
		[Obsolete("ZC-421")]
		void CleanupByChildren(IEnumerable<string> codes);

		/// <summary>
		///     Applys local <paramref name="property" /> to it and descendants (???)
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <param name="cascade"></param>
		[Obsolete("ZC-421")]
		void ApplyProperty(string property, object value, bool cascade = true);

		/// <summary>
		///     Apply local <paramref name="property" /> to it and descendants if not yet
		///     setted (???)
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <param name="children"></param>
		[Obsolete("ZC-421")]
		void ApplyPropertyIfNew(string property, object value, bool children = false);

		/// <summary>
		///     Apply local property to it and other properties with up and down visitor
		///     patter
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="value"></param>
		/// <param name="applyUp"></param>
		/// <param name="applyDown"></param>
		/// <param name="test"></param>
		[Obsolete("ZC-421")]
		void ApplyPropertyByCondition(string prop, object value, bool applyUp, bool applyDown, Func<IZetaRow, bool> test);

		/// <summary>
		///     propagetes group definition as local property
		/// </summary>
		/// <param name="groupname"></param>
		/// <param name="applyUp"></param>
		/// <param name="propname"></param>
		[Obsolete("ZC-421")]
		void PropagateGroupAsProperty(string groupname, bool applyUp = true, string propname = null);

		/// <summary>
		///     Resolves local proprty over hierarchy
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// </returns>
		[Obsolete("ZC-421")]
		object GetLocal(string name);

		/// <summary>
		///     Resolves tag with parents and refs
		/// </summary>
		/// <param name="name"></param>
		/// <returns>
		/// </returns>
		string ResolveTag(string name);

		/// <summary>
		///     Resolves meausure with checking of dynamics
		/// </summary>
		/// <returns>
		/// </returns>
		string ResolveMeasure();

		/// <summary>
		///     Helper method to identify activity for object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>
		/// </returns>
		[Obsolete("ZC-419")]
		bool IsActiveFor(IZetaMainObject obj);

		/// <summary>
		///     Helper method to identify activity on period
		/// </summary>
		/// <param name="year"></param>
		/// <returns>
		/// </returns>
		[Obsolete("ZC-415")]
		bool IsObsolete(int year);
	}
}