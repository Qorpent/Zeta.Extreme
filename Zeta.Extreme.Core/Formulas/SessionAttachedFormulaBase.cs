#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : SessionAttachedFormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовая абстракция формул с поддержой сессий
	/// </summary>
	public abstract class SessionAttachedFormulaBase : IFormula {
		/// <summary>
		/// 	Настраивает формулу на конкретный переданный запрос
		/// </summary>
		/// <param name="query"> </param>
		public void Init(Query query) {
			Query = query;
			Mastersession = Query.Session;
			//if (null == Mastersession) {
			//	Mastersession = new ZexSession();
			//	Session = Mastersession.AsSerial();
			//}
			//else {
			//	IsSubSession = true;
			//	Session = Mastersession.GetSubSession();
			//}
		}

		/// <summary>
		/// Вызывается в фазе подготовки, имитирует вызов функции, но без вычисления значений
		/// </summary>
		/// <param name="query"> </param>
		public abstract void Playback(Query query);


		/// <summary>
		/// 	Команда вычисления результата
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	В принципе кроме вычисления результата формула не должна ничего уметь
		/// </remarks>
		public abstract QueryResult Eval();

		/// <summary>
		/// 	Выполняет очистку ресурсов формулы после использования
		/// </summary>
		public void CleanUp() {
			if (IsSubSession) {
				Mastersession.Return(Session);
			}
		}

		/// <summary>
		/// 	Признак работы в под-сессии
		/// </summary>
		protected internal bool IsSubSession;

		/// <summary>
		/// 	Базовая сессия
		/// </summary>
		protected internal Session Mastersession;

		/// <summary>
		/// 	Исходный запрос
		/// </summary>
		protected internal Query Query;

		/// <summary>
		/// 	Рабочая сессия
		/// </summary>
		protected internal ISerialSession Session;
	}
}