#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : DateRange.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections;
using Qorpent;

namespace Zeta.Extreme.Poco {
	/// <summary>
	/// 	Для обеспечения работы расширения... тут сложный момент с избавлением 
	/// 	Comdiv.Common от зависимостей при
	/// </summary>
	public class DateRange {
		private const string deltaPatternRegex =
			@"(?ix)^
			(?<delta>-|\+)?
			(?<year>\d{1,4})-
			(?<month>\d{1,2})-
			(?<day>\d{1,2})(\s
			(?<hour>\d{1,2}):
			(?<min>\d{1,2}))?
			$
			";

		public DateRange() {}

		public DateRange(DateTime startDate, DateTime endDate) {
			Start = startDate;
			Finish = endDate;
		}

		public DateRange(DateTime keydate) {
			if (keydate < DateTime.Now) {
				Start = keydate;
			}
			if (keydate > DateTime.Now) {
				Finish = keydate;
			}
		}

		public virtual DateTime Start {
			get { return start; }
			set { start = value; }
		}

		public virtual DateTime Finish {
			get { return finish; }
			set { finish = value; }
		}

		public DateRange GetNotLimited() {
			return new DateRange(new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(3000, 1, 1, 0, 0, 0, 0));
		}

		public static DateRange GetPeriod(DateTime startDelta, DateTime endDelta, string s_context, string e_context) {
			var start = GetCorrected(startDelta, s_context);
			var end = GetCorrected(endDelta, e_context);
			return new DateRange(start, end);
		}

		public bool IsInRange(DateTime d) {
			return d >= Start && d <= Finish;
		}

		public static DateTime GetCorrected(DateTime d, string context) {
			var res = d;
			var delta = 1;
			if (context.IndexOf("G") != -1) {
				return res;
			}
			if (context.IndexOf("L") != -1) {
				delta = 2;
			}
			if (context.IndexOf("D") != -1) {
				res = res - new TimeSpan(0, res.Hour, res.Minute, res.Second, res.Millisecond);
				res = res.AddDays(-(delta - 1));
			}
			if (context.IndexOf("W") != -1) {
				res = res.AddDays(-(int) d.DayOfWeek + 1);
				res = res.AddDays(-(delta - 1)*7);
			}
			if (context.IndexOf("M") != -1) {
				res = res.AddDays(-d.Day + 1);
				res = res.AddMonths(-(delta - 1));
			}
			if (context.IndexOf("Y") != -1) {
				return new DateTime(d.Year, 1, 1, 0, 0, 0, 0);
			}
			return res;
		}


		public static DateRange[] Merge(IEnumerable periods) {
			var basis = new ArrayList();
			foreach (var p_ in periods) {
				var p = p_ as DateRange;
				if (p == null) {
					continue;
				}
				var merged = false;
				foreach (DateRange b in basis) {
					if (b.Appart(p)) {
						continue;
					}
					b.Merge(p);
					merged = true;
					break;
				}
				if (merged) {
					continue;
				}
				basis.Add(p.Clone());
			}
			return basis.ToArray(typeof (DateRange)) as DateRange[];
		}

		public object Clone() {
			return new DateRange(Start, Finish);
		}

		public bool Appart(DateRange p) {
			if (Start > p.Finish) {
				return true;
			}
			if (Finish < p.Start) {
				return true;
			}
			return false;
		}

		public void Merge(DateRange p) {
			if (Appart(p)) {
				return;
			}
			Start = Start > p.Start ? p.Start : Start;
			Finish = Finish < p.Finish ? p.Finish : Finish;
		}

		public static DateRange Infinite() {
			return new DateRange(QorpentConst.Date.Begin, QorpentConst.Date.End);
		}

		private DateTime finish = DateTime.Now;
		private DateTime start = DateTime.Now;
	}
}