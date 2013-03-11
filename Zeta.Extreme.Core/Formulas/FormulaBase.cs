#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : FormulaBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Базовая абстрактная формула
	/// </summary>
	public abstract class FormulaBase : IFormula, IDisposable {
		/// <summary>
		/// 	Выполняет определяемые приложением задачи, связанные с высвобождением или сбросом неуправляемых ресурсов.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			CleanUp();
		}

		/// <summary>
		/// 	Настраивает формулу на конкретный переданный запрос
		/// </summary>
		/// <param name="query"> </param>
		public virtual void Init(Query query) {
			Query = query;
			Session = query.Session;
		}

		/// <summary>
		/// 	Устанавливает контекст использования формулы
		/// </summary>
		/// <param name="request"> </param>
		public virtual void SetContext(FormulaRequest request) {
			Descriptor = request;
		}

		/// <summary>
		/// 	Вызывается в фазе подготовки, имитирует вызов функции, но без вычисления значений
		/// </summary>
		/// <param name="query"> </param>
		public void Playback(Query query) {
			IsInPlaybackMode = true;
			Init(query);
			InternalEval();
			CleanUp();
		}


		/// <summary>
		/// 	Команда вычисления результата
		/// </summary>
		/// <returns> </returns>
		/// <remarks>
		/// 	В принципе кроме вычисления результата формула не должна ничего уметь
		/// </remarks>
		public QueryResult Eval() {
			IsInPlaybackMode = false;
			return InternalEval();
		}

		/// <summary>
		/// 	Выполняет очистку ресурсов формулы после использования
		/// </summary>
		public virtual void CleanUp() {
			Query = null;
			Session = null;
		}

		/// <summary>
		/// 	Заготовка внутренней реализации вычисления
		/// </summary>
		/// <returns> </returns>
		protected abstract QueryResult InternalEval();

		/// <summary>
		/// 	Ссылка на контекст вызова и конструирования формулы
		/// </summary>
		protected FormulaRequest Descriptor;

		/// <summary>
		/// 	Флаг нахождения в режиме плейбэка
		/// </summary>
		protected bool IsInPlaybackMode;

		/// <summary>
		/// 	Ссылка на контекст текущего запроса
		/// </summary>
		protected internal Query Query;

		/// <summary>
		/// 	Базовая сессия
		/// </summary>
		protected internal ISession Session;
	}
}