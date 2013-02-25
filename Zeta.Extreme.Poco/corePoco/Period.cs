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
using Comdiv.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public class period : IPeriod{
        public virtual Guid Uid { get; set; }

        #region IPeriod Members
        [Map]
        public virtual string Tag { get; set; }

        [Map(Title = "Старый Ид")]
        public virtual int ClassicId { get; set; }

        [Map(Title = "Категория")]
        public virtual string Category { get; set; }

        [Map]
        public virtual string ShortName { get; set; }

        [Map(Title = "Для дней")]
        public virtual bool IsDayPeriod { get; set; }

        [Map(Title = "Начальная дата")]
        public virtual DateTime StartDate { get; set; }

        [Map(Title = "Конечная дата")]
        public virtual DateTime EndDate { get; set; }

        [Map(Title = "Число месяцев")]
        public virtual int MonthCount { get; set; }

        [Map(Title = "Явл. формулой")]
        public virtual bool IsFormula { get; set; }

        [Map(Title = "Формула")]
        public virtual string Formula { get; set; }
        [Map]
        public virtual string FormulaEvaluator { get; set; }
        
        public virtual string ParsedFormula { get; set; }


        public virtual string Code { get; set; }

        public virtual string Comment { get; set; }

        public virtual int Id { get; set; }

        [Map(Title = "Индекс")]
        public virtual int Idx { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime Version { get; set; }

        #endregion
    }
}