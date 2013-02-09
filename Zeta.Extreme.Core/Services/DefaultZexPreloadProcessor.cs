#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : DefaultZexPreloadProcessor.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;

namespace Zeta.Extreme {
	/// <summary>
	/// 	ѕрепроцессор до загрузки по умолчанию
	/// </summary>
	public class DefaultZexPreloadProcessor : IZexPreloadProcessor {
		/// <summary>
		/// 	 онструктор по умолчнию в прив€зке к сессии
		/// </summary>
		/// <param name="zexSession"> </param>
		/// <exception cref="NotImplementedException"></exception>
		public DefaultZexPreloadProcessor(ZexSession zexSession) {
			_session = zexSession;
		}

		/// <summary>
		/// 	выполн€ет препроцессинг
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public virtual ZexQuery Process(ZexQuery query) {
			var internalquery = query.Copy(true);
			// внутри сессии работаем только с копи€ми
			// ибо иначе отконтроллировать изменени€ препроцессора по сути невозможно

			//сначала вызываем стандартную процедуру нормализации запроса
			internalquery.Normalize();

			return internalquery;
		}


		/// <summary>
		/// 	обратна€ ссылка на сессию
		/// </summary>
		protected ZexSession _session;
	}
}