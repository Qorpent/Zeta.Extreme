#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : ObjColQueryGeneratorStruct.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	Внутренняя конструкция для описания участка скрипта в терминах сочетания объкта-колонки
	/// </summary>
	internal struct ObjColQueryGeneratorStruct {
		/// <summary>
		/// 	Id колонки
		/// </summary>
		public int c;

		/// <summary>
		/// 	Тип детали
		/// </summary>
		public DetailMode m;

		/// <summary>
		/// 	Id объекта
		/// </summary>
		public int o;

		/// <summary>
		/// 	Тип объекта
		/// </summary>
		public ObjType t;
	}
}