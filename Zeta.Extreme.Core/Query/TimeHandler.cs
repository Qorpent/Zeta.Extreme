#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : TimeHandler.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System.Linq;
using System.Text;

namespace Zeta.Extreme {
	/// <summary>
	/// 	�������� ������� �� �����
	/// </summary>
	public sealed class TimeHandler : CacheKeyGeneratorBase {
		private int _year;
		private int _period;
		private int[] _years;
		private int[] _periods;
		private int _baseYear;
		private int _basePeriod;

		/// <summary>
		/// ���
		/// </summary>
		public int Year {
			get { return _year; }
			set {
				if(value==_year) {
					return;
				}
				_year = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// ������
		/// </summary>
		public int Period {
			get { return _period; }
			set {
				if(_period == value) {
					return;
				}
				_period = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// ����� �� ���������� �����
		/// </summary>
		public int[] Years {
			get { return _years; }
			set {
				if(_years==value) {
					return;
				}
				_years = value;
				InvalidateCacheKey();
				
			}
		}

		/// <summary>
		/// ����� ��������
		/// </summary>
		public int[] Periods {
			get { return _periods; }
			set {
				if(value==_periods) {
					return;
				}
				_periods = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// ������� ��� (��� �������� ��������)
		/// </summary>
		public int BaseYear {
			get { return _baseYear; }
			set {
				if(value==_baseYear) {
					return;
				}
				_baseYear = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// ������� ������ (��� �������� ��������)
		/// </summary>
		public int BasePeriod {
			get { return _basePeriod; }
			set {
				if(value==_basePeriod) {
					return;
				}
				_basePeriod = value;
				InvalidateCacheKey();
			}
		}

		/// <summary>
		/// True ���� ������ �������� � ���������
		/// </summary>
		/// <returns></returns>
		public bool IsPeriodDefined() {
			return Period > 0;
		}

		/// <summary>
		/// True ���� ��� ��� �������� � ���������
		/// </summary>
		/// <returns></returns>
		public bool IsYearDefinied() {
			return Year > 1900 && Year < 3000;
		}

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected override string EvalCacheKey() {
			var sb = new StringBuilder();
			sb.Append("TIME:");
			if(null!=_years && 0!=_years.Length) {
				_years = _years.Distinct().OrderBy(_ => _).ToArray();
				sb.Append("YEARS=");
				sb.Append(string.Join(",", _years));
				
			}else {
				if(IsYearDefinied()) {
					sb.Append("YEAR=");
					sb.Append(Year);
				}else {
					sb.Append("UYEAR=");
					sb.Append(BaseYear);
					sb.Append('~');
					sb.Append(Year);
				}
			}
			sb.Append(';');
			if (null != _periods && 0 != _periods.Length)
			{
				_periods = _periods.Distinct().OrderBy(_ => _).ToArray();
				sb.Append("PERIODS=");
				sb.Append(string.Join(",", _periods));

			}
			else
			{
				if (IsPeriodDefined())
				{
					sb.Append("PERIOD=");
					sb.Append(Period);
				}
				else
				{
					sb.Append("UPERIOD=");
					sb.Append(BasePeriod);
					sb.Append('~');
					sb.Append(Period);
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// ������� ����� ������� �� �����
		/// </summary>
		/// <returns></returns>
		public TimeHandler Copy()
		{
			return MemberwiseClone() as TimeHandler;
		}
	}
}