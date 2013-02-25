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
    public partial class DataVersion : IDataVersion{
        [Ref(ClassName = typeof (Source))]
        public virtual IVersionSource Source { get; set; }

        [Ref(ClassName = typeof (DataClass))]
        public virtual IVersionClass DataClass { get; set; }

        [Ref(ClassName = typeof (MainDataRow))]
        public virtual IZetaCell MainDataRow { get; set; }

        [Map]
        public virtual string Val { get; set; }

        #region IDataVersion Members

        public virtual IVersionSource VersionSource{
            get { return Source; }
            set { Source = value; }
        }

        public virtual IVersionClass VersionClass{
            get { return DataClass; }
            set { DataClass = value; }
        }

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        public virtual IZetaCell Cell{
            get { return MainDataRow; }
            set { MainDataRow = value; }
        }

        public virtual string Value{
            get { return Val; }
            set { Val = value; }
        }

        #endregion
    }
}