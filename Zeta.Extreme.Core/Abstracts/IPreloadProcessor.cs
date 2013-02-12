#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IZexPreloadProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Интерфейс службы, выполняющей работы по доводке запроса ДО входа в расчетчик
	/// 	и ДО определения кэша запроса
	/// </summary>
	public interface IPreloadProcessor {
		/// <summary>
		/// 	выполняет препроцессинг
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		Query Process(Query query);
	}
}