#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IScriptGenerator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	Интерфейс генератора скриптов
	/// </summary>
	public interface IScriptGenerator {
		/// <summary>
		/// 	Строит SQL запрос с учетом прототипа, а по сути "хинтов" запроса
		/// </summary>
		/// <param name="queries"> </param>
		/// <param name="prototype"> </param>
		/// <returns> </returns>
		string Generate(IQuery[] queries, PrimaryQueryPrototype prototype);
	}
}