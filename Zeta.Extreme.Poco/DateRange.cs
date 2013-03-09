// Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
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
// MODIFICATIONS HAVE BEEN MADE TO THIS FILE

using System;
using System.Collections;

namespace Zeta.Extreme.Poco{
    /// <summary>
    /// ��� ����������� ������ ����������... ��� ������� ������ � ����������� 
    /// Comdiv.Common �� ������������ ��� 
    /// </summary>
    /// 
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

        private DateTime finish = DateTime.Now;
        private DateTime start = DateTime.Now;

        public DateRange() {}

        public DateRange(DateTime startDate, DateTime endDate){
            Start = startDate;
            Finish = endDate;
        }

        public DateRange(DateTime keydate){
            if (keydate < DateTime.Now){
                Start = keydate;
            }
            if (keydate > DateTime.Now){
                Finish = keydate;
            }
        }

        #region IDateRange Members

        public virtual DateTime Start{
            get { return start; }
            set { start = value; }
        }

        public virtual DateTime Finish{
            get { return finish; }
            set { finish = value; }
        }

        #endregion

        public DateRange GetNotLimited(){
            return new DateRange(new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(3000, 1, 1, 0, 0, 0, 0));
        }

	    public static DateRange GetPeriod(DateTime startDelta, DateTime endDelta, string s_context, string e_context){
            var start = GetCorrected(startDelta, s_context);
            var end = GetCorrected(endDelta, e_context);
            return new DateRange(start, end);
        }
        public bool IsInRange(DateTime d)
        {
            return d >= Start && d <= Finish;
        }
        public static DateTime GetCorrected(DateTime d, string context){
            var res = d;
            var delta = 1;
            if (context.IndexOf("G") != -1){
                return res;
            }
            if (context.IndexOf("L") != -1){
                delta = 2;
            }
            if (context.IndexOf("D") != -1){
                res = res - new TimeSpan(0, res.Hour, res.Minute, res.Second, res.Millisecond);
                res = res.AddDays(-(delta - 1));
            }
            if (context.IndexOf("W") != -1){
                res = res.AddDays(-(int) d.DayOfWeek + 1);
                res = res.AddDays(-(delta - 1)*7);
            }
            if (context.IndexOf("M") != -1){
                res = res.AddDays(-d.Day + 1);
                res = res.AddMonths(-(delta - 1));
            }
            if (context.IndexOf("Y") != -1){
                return new DateTime(d.Year, 1, 1, 0, 0, 0, 0);
            }
            return res;
        }


	    public static DateRange[] Merge(IEnumerable periods){
            var basis = new ArrayList();
            foreach (var p_ in periods){
                var p = p_ as DateRange;
                if (p == null){
                    continue;
                }
                var merged = false;
                foreach (DateRange b in basis){
                    if (b.Appart(p)){
                        continue;
                    }
                    b.Merge(p);
                    merged = true;
                    break;
                }
                if (merged){
                    continue;
                }
                basis.Add(p.Clone());
            }
            return basis.ToArray(typeof (DateRange)) as DateRange[];
        }

        public object Clone(){
            return new DateRange(Start, Finish);
        }

        public bool Appart(DateRange p){
            if (Start > p.Finish){
                return true;
            }
            if (Finish < p.Start){
                return true;
            }
            return false;
        }

        public void Merge(DateRange p){
            if (Appart(p)){
                return;
            }
            Start = Start > p.Start ? p.Start : Start;
            Finish = Finish < p.Finish ? p.Finish : Finish;
        }

        public static DateRange Infinite(){
            return new DateRange(Qorpent.QorpentConst.Date.Begin, Qorpent.QorpentConst.Date.End);
        }
    }
}