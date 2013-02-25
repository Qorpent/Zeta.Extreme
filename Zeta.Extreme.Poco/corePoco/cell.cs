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
using System.Collections.Generic;
using Comdiv.Model;
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class cell : IZetaCell{
        private StandardRowData rowData;

       // [Ref(ClassName = typeof (DataVersion))]
       // public virtual IDataVersion Ver { get; set; }


        public virtual FixRuleResult? FixStatus { get; set; }

        [Ref(ClassName = typeof (IZetaRow))]
        public virtual IZetaRow MainDataTree { get; set; }

        
        [Ref(ClassName = typeof (col))]
        public virtual IZetaColumn ValueType { get; set; }

        [Ref(ClassName = typeof (obj))]
        public virtual IZetaMainObject Org { get; set; }

        [Ref(ClassName = typeof (detail))]
        public virtual IZetaDetailObject Subpart { get; set; }

        public virtual string Path { get; set; }

        #region IZetaCell Members
        [Map]
        public virtual string Valuta { get; set; }

        [Map]
        public virtual string Comment { get; set; }

        [Map]
        public virtual string Data { get; set; }

        [Map]
        public virtual bool Finished { get; set; }

    //    [Many(ClassName = typeof (DataVersion))]
     //   public virtual IList<IDataVersion> Versions { get; set; }

    //    [Many(ClassName = typeof (Correction))]
      // public virtual IList<IAcceptedDataCorrection> Corrections { get; set; }

        //public virtual IDataVersion DataVersion{
        //    get { return Ver; }
        //    set { Ver = value; }
        //}

     //   [Ref(ClassName = typeof (Scenario))]
       // public virtual IScenario Scenario { get; set; }

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        [Nest]
        public virtual StandardRowData RowData{
            get { return rowData ?? (rowData = new StandardRowData()); }
            set { rowData = value; }
        }

        public virtual object Value{
            get { return RowData.GetValue(Column); }
            set { RowData.SetValue(Column, value); }
        }

        public virtual IZetaRow Row{
            get { return MainDataTree; }
            set { MainDataTree = value; }
        }

        [Map]
        public virtual int Year { get; set; }

        [Map("Kvart")]
        public virtual int Period { get; set; }

        [Map]
        public virtual DateTime DirectDate { get; set; }

        public virtual IZetaColumn Column{
            get { return ValueType; }
            set { ValueType = value; }
        }

        public virtual IZetaMainObject Object{
            get { return Org; }
            set { Org = value; }
        }

        public virtual IZetaMainObject AltObj { get; set; }
        public virtual int AltObjId { get; set; }
        public virtual int RowId { get; set; }
        public virtual int ColumnId { get; set; }
        public virtual int ObjectId { get; set; }
        public virtual int DetailId { get; set; }

        

        public virtual IZetaDetailObject DetailObject{
            get { return Subpart; }
            set { Subpart = value; }
        }
        
        public virtual object Tag { get; set; }

        public virtual IPkg Pkg { get; set; }
        [Map]
        public virtual bool IsAuto { get; set; }
        [Map]
        public virtual string Usr { get; set; }

        #endregion
    }
}