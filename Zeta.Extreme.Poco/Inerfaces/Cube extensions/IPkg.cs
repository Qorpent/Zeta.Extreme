#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IPkg.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model {
	public interface IPkg : IEntityDataPattern,
	                        IWithType<IPkgType>,
	                        IWithIerarchy<IPkg>,
	                        IWithCells<IZetaCell, IZetaRow, IZetaColumn, IZetaMainObject, IZetaDetailObject>,
	                        IWithMainObject<IZetaMainObject>,
	                        IWithDetailObject<IZetaDetailObject, IZetaMainObject>,
	                        IWithPkgState,
	                        IWithDocumentNumber {
		DateTime CreateTime { get; set; }
		string Usr { get; set; }

		[global::Zeta.Extreme.Poco.Deprecated.Map(Title = "Дата")] DateTime Date { get; set; }

		bool IsClosed();
	                        }
}