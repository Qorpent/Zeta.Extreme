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

namespace Comdiv.Zeta.Model{
    public partial class Correction : IAcceptedDataCorrection{
        #region IAcceptedDataCorrection Members

        [Map]
        public virtual string Comment { get; set; }

        [Ref(ClassName = typeof (Document))]
        public virtual IDocumentOfCorrections Document { get; set; }

        [Map]
        public virtual int Id { get; set; }


        [Ref(ClassName = typeof (MainDataRow))]
        public virtual IZetaCell Cell { get; set; }

        public virtual decimal Value { get; set; }

        public virtual Guid Uid { get; set; }

        public virtual DateTime Version { get; set; }

        #endregion
    }
}