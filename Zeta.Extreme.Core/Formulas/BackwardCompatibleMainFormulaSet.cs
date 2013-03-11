#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : BackwardCompatibleMainFormulaSet.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;
using Zeta.Extreme.Poco.NativeSqlBind;

namespace Zeta.Extreme {
	/// <summary>
	/// 	���� ������ ������ ������, ����������� � ����� �������
	/// 	������ ����, ��� �����������
	/// 	������ ��� �����������, ������� �� ����������� � ����� ��
	/// </summary>
	public class BackwardCompatibleMainFormulaSet {
		/// <summary>
		/// 	������� ������ ������, �������������� � ����� �������
		/// </summary>
		/// <param name="host"> </param>
		public BackwardCompatibleMainFormulaSet(DeltaFormulaBase host) {
			_host = host;
		}

		/// <summary>
		/// 	������ ���������� ������� � �������
		/// </summary>
		/// <param name="query"> </param>
		/// <returns> </returns>
		public int monthCount(IQuery query) {
			return monthCount(query.Time.Period);
		}

		/// <summary>
		/// 	������ ������� � �������
		/// </summary>
		/// <param name="period"> </param>
		/// <returns> </returns>
		public int monthCount(int period) {
			var p = Periods.Get(period);
			return p.MonthCount;
		}

		/// <summary>
		/// 	���������� ��������� ������ � �������, ���� �� ������ ��-�������
		/// </summary>
		/// <param name="deltas"> </param>
		/// <remarks>
		/// 	��� ������������ ������ ���������������� ����� - ������� � ������� � ������
		/// </remarks>
		/// <returns> </returns>
		public decimal choose(params QueryDelta[] deltas) {
			return deltas.Select(d => _host.Eval(d)).FirstOrDefault(qr => 0 != qr);
		}

		/// <summary>
		/// 	��������� ������� - �������
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
		/// 	�������� ������� �������
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
		/// 	���������� ��������, ���� ������������� � 0 ���� �����-�� ����
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public object positive(object value) {
			var val = value.ToDecimal();
			if (val < 0) {
				return 0m;
			}
			return value;
		}

		/// <summary>
		/// 	���������� �������, ���� ���������� � 0 ���� ����
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public object negative(object value) {
			var val = value.ToDecimal();
			if (val > 0) {
				return 0m;
			}
			return value;
		}

		/// <summary>
		/// 	�������� �������������� ����������
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> </returns>
		public decimal round(decimal value) {
			return Math.Round(value, MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// 	�������� ����� �� �������� ��� ���������� �������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="periods"> </param>
		/// <param name="query"> </param>
		/// <param name="main"> </param>
		/// <param name="other"> </param>
		/// <returns> </returns>
		public object onperiods<T>(int[] periods, IQuery query, Func<T> main, Func<T> other) {
			if (periods.Contains(query.Time.Period)) {
				return main;
			}
			return other;
		}


		/// <summary>
		/// 	�������� ����� �� �������� ��� ���������� �������
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
			return other;
		}

		/// <summary>
		/// 	�������� ����� �� �������� ��� ���������� �������
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
			return other;
		}

		/// <summary>
		/// 	������������ ����� ������� � ��������
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