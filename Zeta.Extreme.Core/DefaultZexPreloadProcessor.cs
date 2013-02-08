#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexPreloadProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme.Core {
	/// <summary>
	/// 	Препроцессор до загрузки по умолчанию
	/// </summary>
	public class DefaultZexPreloadProcessor : IZexPreloadProcessor {
		/// <summary>
		/// 	Конструктор по умолчнию в привязке к сессии
		/// </summary>
		/// <param name="zexSession"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public DefaultZexPreloadProcessor(ZexSession zexSession) {
			_session = zexSession;
		}

		/// <summary>
		/// 	выполняет препроцессинг
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public virtual ZexQuery Process(ZexQuery query) {
			return query;
		}

		/// <summary>
		/// 	обратная ссылка на сессию
		/// </summary>
		protected ZexSession _session;
	}
}