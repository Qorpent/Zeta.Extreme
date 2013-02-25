﻿// // Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
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
using Comdiv.Olap.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class TreeMark : IZetaRowMark{
        public virtual int MarkId { get; set; }
        public virtual int RowId { get; set; }
        public virtual string Code { get; set; }

        #region IZetaRowMark Members

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        [Ref(ClassName = typeof (IZetaRow))]
        public virtual IZetaRow Target { get; set; }

        [Ref(ClassName = typeof (Mark))]
        public virtual IMark Mark { get; set; }

        #endregion
    }
}