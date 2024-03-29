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
// PROJECT ORIGIN: Zeta.Extreme.Core/TimeHandler.cs
#endregion
using System;
using System.Linq;
using System.Text;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ������� �� �����
	/// </summary>
	public sealed class TimeHandler : CacheKeyGeneratorBase, ITimeHandler {
		/// <summary>
		/// 	���
		/// </summary>
		public int Year {
			get { return _year; }
			set {
				if (value == _year) {
					return;
				}
				_year = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	������
		/// </summary>
		public int Period {
			get { return _period; }
			set {
				if (_period == value) {
					return;
				}
				_period = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	����� �� ���������� �����
		/// </summary>
		public int[] Years {
			get { return _years; }
			set {
				if (_years == value) {
					return;
				}
				_years = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	����� ��������
		/// </summary>
		public int[] Periods {
			get { return _periods; }
			set {
				if (value == _periods) {
					return;
				}
				_periods = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	������� ��� (��� �������� ��������)
		/// </summary>
		public int BaseYear {
			get { return _baseYear; }
			set {
				if (value == _baseYear) {
					return;
				}
				_baseYear = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	������� ������ (��� �������� ��������)
		/// </summary>
		public int BasePeriod {
			get { return _basePeriod; }
			set {
				if (value == _basePeriod) {
					return;
				}
				_basePeriod = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// 	True ���� ������ �������� � ���������
		/// </summary>
		/// <returns> </returns>
		public bool IsPeriodDefined() {
			return Period > 0 || (null != _periods && 0 != _periods.Length);
		}

		/// <summary>
		/// 	True ���� ��� ��� �������� � ���������
		/// </summary>
		/// <returns> </returns>
		public bool IsYearDefinied() {
			return (Year > 1900 && Year < 3000) || (null != _years && 0 != _years.Length);
		}

		/// <summary>
		/// 	������� ����� ������� �� �����
		/// </summary>
		/// <returns> </returns>
		public ITimeHandler Copy() {
			return MemberwiseClone() as TimeHandler;
		}

		/// <summary>
		/// 	����������� ���������� ���� � �������
		/// </summary>
		public void Normalize(IQuery query) {
			if (!IsYearDefinied()) {
				ResolveYear();
			}
			if (!IsPeriodDefined()) {
				ResolvePeriod(query.Session);
			}
			if (null != _periods && 0 != _periods.Length) {
				Periods = _periods.Distinct().OrderBy(_ => _).ToArray();
				var intstr = string.Join("", Periods);
				if (intstr.Length <= 8) {
					Period = Convert.ToInt32(intstr);
				}
				else {
					var val = 0;
					for (var i = 0; i < intstr.Length; i += 8) {
						var chunk = intstr.Substring(i, Math.Min(8, intstr.Length - i));
						val += Convert.ToInt32(chunk);
					}
					Period = val;
				}
			}
			if (null != _years && 0 != _years.Length) {
				Years = _years.Distinct().OrderBy(_ => _).ToArray();
			}
		}

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();
			sb.Append("TIME:");
			if (null != _years && 0 != _years.Length) {
				_years = _years.Distinct().OrderBy(_ => _).ToArray();
				sb.Append("YEARS=");
				sb.Append(string.Join(",", _years));
			}
			else {
				if (IsYearDefinied()) {
					sb.Append("YEAR=");
					sb.Append(Year);
				}
				else {
					sb.Append("UYEAR=");
					sb.Append(BaseYear);
					sb.Append('~');
					sb.Append(Year);
				}
			}
			sb.Append(';');
			if (null != _periods && 0 != _periods.Length) {
				_periods = _periods.Distinct().OrderBy(_ => _).ToArray();
				sb.Append("PERIODS=");
				sb.Append(string.Join(",", _periods));
			}
			else {
				if (IsPeriodDefined()) {
					sb.Append("PERIOD=");
					sb.Append(Period);
				}
				else {
					sb.Append("UPERIOD=");
					sb.Append(BasePeriod);
					sb.Append('~');
					sb.Append(Period);
				}
			}
			return sb.ToString();
		}

		private void ResolvePeriod(ISession session) {
			//TODO: fix to real logic, ������ �������� �������
			if (0 == Period) {
				Period = BasePeriod;
			}
			var periodEvaluator  = new DefaultPeriodEvaluator();
			var result = periodEvaluator.Evaluate(BasePeriod, Period, Year);
			if (0 != result.Year && (null == _years || 0 == _years.Length)) {
				Year = Year;
			}
			if (null != result.Periods) {
				Periods = result.Periods;
			}
			else {
				Period = result.Period;
			}
		}

		private void ResolveYear() {
			//��������� ������ � ����
			Year = BaseYear + Year;
		}

		private int _basePeriod;
		private int _baseYear;
		private int _period;
		private int[] _periods;
		private int _year;
		private int[] _years;
		
	}
}