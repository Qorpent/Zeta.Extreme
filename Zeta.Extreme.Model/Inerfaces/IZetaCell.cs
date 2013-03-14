#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaCell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Qorpent.Model;
using Zeta.Extreme.Model.Deprecated;

namespace Zeta.Extreme.Model.Inerfaces {
	[Classic("MainDataRow")]
	public interface IZetaCell :
		IWithComment,
		IWithTag,
		IWithUsr, IWithId, IWithVersion {
		string Valuta { get; set; }
		IZetaMainObject AltObj { get; set; }
		int AltObjId { get; set; }
		int RowId { get; set; }
		int ColumnId { get; set; }
		int ObjectId { get; set; }
		int DetailId { get; set; }
		[Classic("Subpart")] IZetaDetailObject DetailObject { get; set; }
		[Classic("Org")] IZetaMainObject Object { get; set; }
		DateTime DirectDate { get; set; }
		int Period { get; set; }
		int Year { get; set; }
		}
}