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
using Comdiv.Model;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class cell : IZetaCell{
#if ADAPTMODEL
        public virtual IZetaRow Meta{
            get { return Row; }
            set { Row = value; }
        }

        public virtual int Month { get; set; }

        private FixRuleResult _fixed = FixRuleResult.Open;

        public virtual FixRuleResult Fixed{
            get { return _fixed; }
            set { _fixed = value; }
        }

        public virtual int FixRuleId { get; set; }


#endif


        public override string ToString(){
            return string.Format("cell:{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}",
                                 Row.Code(), Column.Code(), Object.Code(), DetailObject.Code(), Year, Period,
                                 DirectDate.ToString("yyyy-MM-dd"), Pkg.Code(),
                                 Value
                );
        }
    }
}