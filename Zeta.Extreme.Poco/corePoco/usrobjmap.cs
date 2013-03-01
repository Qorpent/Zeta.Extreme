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

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class usrobjmap : IUserObjectMap{
        [Ref(ClassName = typeof (obj))]
        public virtual IZetaMainObject Org { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        #region IUserObjectMap Members
        [Map]
        public virtual string Tag { get; set; }

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual string Name { get; set; }

        [Map]
        public virtual string Code { get; set; }

        [Map]
        public virtual string Comment { get; set; }


        [Map]
        public virtual string Domain { get; set; }

        [Map]
        public virtual string Role { get; set; }


        [Map]
        public virtual DateTime Version { get; set; }


        public virtual IZetaMainObject Object{
            get { return Org; }
            set { Org = value; }
        }

        [Map]
        public virtual bool AllOrgs { get; set; }

        #endregion
    }
}