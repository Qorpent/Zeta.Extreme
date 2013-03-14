#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaRow.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.PocoClasses;


namespace Zeta.Extreme.Model.Inerfaces {
	[ForSearch("Строка, признак")]
	public interface IZetaRow : IZetaQueryDimension,
		IZetaFormsSupport,
		IWithMarkCache,  IWithMeasure {
		IDictionary<string, object> LocalProperties { get; }
		[Map] string ObjectGroups { get; set; }
		[Map] string FormElementType { get; set; }
		[Map] string Validator { get; set; }
		[Map] string ColumnSubstitution { get; set; }
		[Map] string FullName { get; set; }
		[Map] string Role { get; set; }
		[Map] string Valuta { get; set; }
		IZetaRow RefTo { get; set; }
		int? ParentId { get; set; }
		int? RefId { get; set; }
		int? ObjectId { get; set; }
		IList<IZetaRow> NativeChildren { get; }
		IZetaRow[] AllChildren { get; }
		string FullRole { get; }
		int Level { get; }

		[Ref(ClassName = typeof (IZetaRow))] IZetaRow ExRefTo { get; set; }

		int? ExRefToId { get; set; }
		[Map] bool Active { get; set; }
		IZetaRow TemporalParent { get; set; }
		IZetaRow Parent { get; set; }
		IList<IZetaRow> Children { get; set; }
		[Classic("Org")] IZetaMainObject Object { get; set; }
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