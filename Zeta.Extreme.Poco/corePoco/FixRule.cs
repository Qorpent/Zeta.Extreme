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
using Comdiv.Model;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class fixrule : IFixRule{
        private DateTime ___DirectDate = Qorpent.QorpentConst.Date.Begin;

        [Ref(ClassName = typeof (IZetaRow))]
        public virtual IZetaRow MainDataTree { get; set; }

        public virtual int Kvart { get; set; }

        [Ref(ClassName = typeof (col))]
        public virtual IZetaColumn ValueType { get; set; }

        [Ref(ClassName = typeof (detail))]
        public virtual IZetaDetailObject Subpart { get; set; }

        #region IFixRule Members

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        public virtual IZetaRow Row{
            get { return MainDataTree; }
            set { MainDataTree = value; }
        }

        [Map]
        public virtual int Year { get; set; }
        [Map]
        public virtual int Period{
            get { return Kvart; }
            set { Kvart = value; }
        }

        [Map]
        public virtual DateTime DirectDate{
            get { return ___DirectDate; }
            set { ___DirectDate = value; }
        }

        public virtual IZetaColumn Column{
            get { return ValueType; }
            set { ValueType = value; }
        }


        public virtual IZetaMainObject Object { get; set; }

        public virtual IZetaDetailObject DetailObject{
            get { return Subpart; }
            set { Subpart = value; }
        }

        [Map]
        public virtual string Comment { get; set; }

        [Map]
        public virtual string Category { get; set; }

        [Map]
        public virtual bool Active { get; set; }

        [Map(CustomType = typeof(int))]
        public virtual FixRulePriority Priority { get; set; }

        [Map(CustomType = typeof(int))]
        public virtual FixRuleResult Result { get; set; }

        [Map(Formula = "usm.GetFixRuleSalience(Id)")]
        public virtual int AdvancedWeight { get; set; }

        #endregion
    }
}