#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : BackwardCompatibleMainFormulaSet.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Comdiv.Extensions;
using Periods = Zeta.Extreme.Poco.NativeSqlBind.Periods;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Порт набора старых формул, совместимый с новым движком
	/// 	ПРЯМОЙ ПОРТ, БЕЗ ПЕРЕРАБОТОК
	/// 	Убраны все конструкции, которые не встречаются в живой БД
	/// </summary>
	public class BackwardCompatibleMainFormulaSet {
		/// <summary>
		/// 	Создает модуль формул, присоединенный к хосту формулы
		/// </summary>
		/// <param name="host"> </param>
		public BackwardCompatibleMainFormulaSet(DeltaFormulaBase host) {
			_host = host;
		}

		/// <summary>
		/// Расчет количества месяцев в периоде
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public int monthCount(Query query)
		{
			return monthCount(query.Time.Period);
		}
		/// <summary>
		/// Расчет месяцев в периоде
		/// </summary>
		/// <param name="period"></param>
		/// <returns></returns>
		public int monthCount(int period)
		{
			var p = Periods.Get(period);
			return p.MonthCount;
		}
		/// <summary>
		/// 	Перебирает указанные дельты к запросу, пока не найдет не-нулевой
		/// </summary>
		/// <param name="deltas"> </param>
		/// <remarks>
		/// 	При портировании убрали неопределенности типов - привели к дельтам и числам
		/// </remarks>
		/// <returns> </returns>
		public decimal choose(params QueryDelta[] deltas) {
			foreach (var d in deltas) {
				var qr = _host.Eval(d);
				if (0 != qr) {
					return qr;
				}
			}
			return 0m;
		}

		/// <summary>
		/// 	Тернарный генерик - условие
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="condition"> </param>
		/// <param name="val1"> </param>
		/// <param name="val2"> </param>
		/// <returns> </returns>
		public T If<T>(bool condition, Func<T> val1, Func<T> val2) {
			if (condition) {
				return val1();
			}
			return val2();
		}

		/// <summary>
		/// 	Бинарный генерик условие
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="condition"> </param>
		/// <param name="val1"> </param>
		/// <returns> </returns>
		public T If<T>(bool condition, Func<T> val1) {
			if (condition) {
				return val1();
			}
			return default(T);
		}

		/// <summary>
		/// 	Возвращает значение, если положительное и 0 если какое-то иное
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public object positive(object value) {
			var val = value.toDecimal();
			if (val < 0) {
				return 0m;
			}
			return value;
		}

		/// <summary>
		/// 	Возвращает знаение, если негативное и 0 если иное
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public object negative(object value) {
			var val = value.toDecimal();
			if (val > 0) {
				return 0m;
			}
			return value;
		}

		/// <summary>
		/// 	Операция бухгалтерского округления
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public decimal round(decimal value) {
			return Math.Round(value, MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// 	Массовый выбор по периодам для указанного запроса
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="periods"> </param>
		/// <param name="query"> </param>
		/// <param name="main"> </param>
		/// <param name="other"> </param>
		/// <returns> </returns>
		public object onperiods<T>(int[] periods, Query query, Func<T> main, Func<T> other) {
			if (periods.Contains(query.Time.Period)) {
				return main;
			}
			else {
				return other;
			}
		}


		/// <summary>
		/// 	Массовый выбор по периодам для указанного запроса
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="periods"> </param>
		/// <param name="main"> </param>
		/// <param name="other"> </param>
		/// <returns> </returns>
		public object onperiods<T>(int[] periods, Func<T> main, Func<T> other) {
			if (periods.Contains(_host.Query.Time.Period)) {
				return main;
			}
			else {
				return other;
			}
		}

		/// <summary>
		/// 	Массовый выбор по периодам для указанного запроса
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="periods"> </param>
		/// <param name="delta"> </param>
		/// <param name="main"> </param>
		/// <param name="other"> </param>
		/// <returns> </returns>
		public object onperiods<T>(int[] periods, QueryDelta delta, Func<T> main, Func<T> other) {
			if (periods.Contains(delta.Apply(_host.Query).Time.Period)) {
				return main;
			}
			else {
				return other;
			}
		}

		/// <summary>
		/// 	Подсчитывает число месяцев в квартале
		/// </summary>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public int monthInKvart(int period) {
			var p = Periods.Get(period);
			var c = p.MonthCount%3;
			if (c == 0) {
				c = 3;
			}
			return c;
		}

		private readonly DeltaFormulaBase _host;
	}
}