// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// // Supported by Media Technology LTD 
// //  
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //  
// //      http://www.apache.org/licenses/LICENSE-2.0
// //  
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// // 
// // MODIFICATIONS HAVE BEEN MADE TO THIS FILE
using System;
using Comdiv.Extensions;
using Comdiv.Useful;
using Comdiv.Zeta.Data;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class EndtimeScheduleChecker : IScheduleChecker{
        #region IScheduleChecker Members

        public ScheduleState Evaluate(IInputTemplate template){
            var result = new ScheduleState{Overtime = ScheduleOvertime.None, Date = DateExtensions.End};

            var delta = template.ScheduleDelta;
            var date = DateExtensions.accomodateToYear(Periods.Get(template.Period).EndDate, template.Year);
            date = date.AddDays(delta);
            if (date.DayOfWeek == DayOfWeek.Saturday){
                date = date.AddDays(2);
            }
            if (date.DayOfWeek == DayOfWeek.Sunday){
                date = date.AddDays(1);
            }
            result.Date = date;
            if (!template.IsOpen){
                return result;
            }
            if (DateTime.Today > date){
                result.Overtime = ScheduleOvertime.Fail;
            }
            else if ((date - DateTime.Today).TotalDays <= 3){
                result.Overtime = ScheduleOvertime.Critical;
            }
            return result;
        }

        #endregion
    }
}