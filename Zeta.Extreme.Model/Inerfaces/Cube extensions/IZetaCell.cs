#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaCell.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Qorpent.Model;
using Zeta.Extreme.Poco.Deprecated;

namespace Zeta.Extreme.Poco.Inerfaces {
	[Classic("MainDataRow")]
	public interface IZetaCell :
		IWithComment,
		IWithData,
		IOlapCell<IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
		IWithTag,
		IWithAutoManualFlag,
		IWithUsr {
		string Valuta { get; set; }
		IZetaMainObject AltObj { get; set; }
		int AltObjId { get; set; }
		int RowId { get; set; }
		int ColumnId { get; set; }
		int ObjectId { get; set; }
		int DetailId { get; set; }
		}
}