#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : TimeHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using System.Text;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	Описание условия на время
	/// </summary>
	public sealed class TimeHandler : CacheKeyGeneratorBase, ITimeHandler {
		/// <summary>
		/// 	Год
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
		/// 	Период
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
		/// 	Набор из нескольких годов
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
		/// 	Набор периодов
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
		/// 	Базовый год (для формовых запросов)
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
		/// 	Базовый период (для формовых запросов)
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
		/// 	True если период приведен к константе
		/// </summary>
		/// <returns> </returns>
		public bool IsPeriodDefined() {
			return Period > 0 || (null != _periods && 0 != _periods.Length);
		}

		/// <summary>
		/// 	True если год уже приведен к константе
		/// </summary>
		/// <returns> </returns>
		public bool IsYearDefinied() {
			return (Year > 1900 && Year < 3000) || (null != _years && 0 != _years.Length);
		}

		/// <summary>
		/// 	Простая копия условия на время
		/// </summary>
		/// <returns> </returns>
		public ITimeHandler Copy() {
			return MemberwiseClone() as TimeHandler;
		}

		/// <summary>
		/// 	Нормализует формульные года и периоды
		/// </summary>
		/// <param name="session"> </param>
		public void Normalize(ISession session = null) {
			if (!IsYearDefinied()) {
				ResolveYear();
			}
			if (!IsPeriodDefined()) {
				ResolvePeriod(session);
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
		/// 	Функция непосредственного вычисления кэшевой строки
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
			//TODO: fix to real logic, должен вызывать функцию
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
			//применяет дельту к году
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