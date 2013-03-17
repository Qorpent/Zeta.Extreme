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


namespace Zeta.Extreme.Model.Inerfaces {
	
	public interface IZetaRow : IZetaQueryDimension,
		IZetaFormsSupport,
		IWithMarkCache,  IWithMeasure {
		IDictionary<string, object> LocalProperties { get; }
		 string ObjectGroups { get; set; }
		 string FormElementType { get; set; }
		 string Validator { get; set; }
		 string ColumnSubstitution { get; set; }
		 string FullName { get; set; }
		 string Role { get; set; }
		 string Valuta { get; set; }
		IZetaRow RefTo { get; set; }
		int? ParentId { get; set; }
		int? RefId { get; set; }
		int? ObjectId { get; set; }
		IList<IZetaRow> NativeChildren { get; }
		IZetaRow[] AllChildren { get; }
		string FullRole { get; }
		int Level { get; }

		IZetaRow ExRefTo { get; set; }

		int? ExRefToId { get; set; }
		 bool Active { get; set; }
		IZetaRow TemporalParent { get; set; }
		IZetaRow Parent { get; set; }
		IList<IZetaRow> Children { get; set; }
		 IZetaMainObject Object { get; set; }
		string Path { get; set; }
		string ResolveColumnCode(string incode);
		IZetaRow Copy(bool withchildren);
		void ResetAllChildren();
		void CleanupByChildren(IEnumerable<string> codes);
		void ApplyProperty(string property, object value, bool cascade = true);
		void ApplyPropertyIfNew(string property, object value, bool children = false);
		MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default");
		void ApplyPropertyByCondition(string prop, object value, bool applyUp, bool applyDown, Func<IZetaRow, bool> test);
		void PropagateGroupAsProperty(string groupname, bool applyUp = true, string propname = null);
		object GetLocal(string name);
		string ResolveTag(string name);

		string ResolveMeasure();
		bool IsActiveFor(IZetaMainObject obj);
		bool IsObsolete(int year);
		}
}