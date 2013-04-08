#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Core/BackwardCompatibleMainFormulaSet.cs
#endregion
using System;
using System.Linq;
using System.Linq.Expressions;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.MetaCaches;
using Zeta.Extreme.Model.Querying;

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
			if (query.Time.Periods != null && 1 != query.Time.Periods.Length) {
				return query.Time.Periods.Length;
			}
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
		public T If<T>(Expression<Func<bool>> condition, Func<T> val1, Func<T> val2) {
			if (_host.IsInPlaybackMode) {
				var deltas = new QueryDeltaFindVisitor().CollectDeltas(condition).ToArray();
				if (0 != deltas.Length) {
					foreach (var queryDelta in deltas) {
						_host.Eval(queryDelta);
					}
					val1();
					return val2();
				}
				else {
					if (condition.Compile()()) {
						return val1();
					}
					return val2();
				}
			}
			_host.OptimizeDeltaFinding = false;
			try {
				if (condition.Compile()()) {
					return val1();
				}
				return val2();
			}
			finally {
				_host.OptimizeDeltaFinding = true;
			}
		}

		/// <summary>
		/// 	�������� ������� �������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="condition"> </param>
		/// <param name="val1"> </param>
		/// <returns> </returns>
		public T If<T>(Expression<Func<bool>> condition, Func<T> val1) {
			return If(condition, val1, () => default(T));
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