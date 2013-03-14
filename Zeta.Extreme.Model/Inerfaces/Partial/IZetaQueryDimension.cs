#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IZetaQueryDimension.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Collections.Generic;
using Qorpent.Model;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// Интерфейс для обработчиков формул
	/// </summary>
	public interface IZetaQueryDimension : IEntity,
		IWithFormula
	{
		
		/// <summary>
		/// Словарь локальных (временных) свойств
		/// </summary>
		IDictionary<string, object> LocalProperties { get; }
	}
}