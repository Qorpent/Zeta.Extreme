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
		public void Init(ZexQuery query) {
			Query = query;
			Mastersesion = Query.Session;
			if (null == Mastersesion) {
				Mastersesion = new ZexSession();
				Session = Mastersesion.AsSerial();
			}
			else {
				IsSubSession = true;
				Session = Mastersesion.GetSubSession();
			}
		}


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
				Mastersesion.Return(Session);
			}
		}

		/// <summary>
		/// 	Признак работы в под-сессии
		/// </summary>
		protected internal bool IsSubSession;

		/// <summary>
		/// 	Базовая сессия
		/// </summary>
		protected internal ZexSession Mastersesion;

		/// <summary>
		/// 	Исходный запрос
		/// </summary>
		protected internal ZexQuery Query;

		/// <summary>
		/// 	Рабочая сессия
		/// </summary>
		protected internal ISerialSession Session;
	}
}