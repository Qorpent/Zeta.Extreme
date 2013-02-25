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
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Persistence;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class objcls : IDetailObjectClass{
        [Map]
        public virtual Guid Uid { get; set; }


		public virtual MetalinkRecord[] GetLinks(string nodetype, string linktype, string subtype = null, string system = "Default")
		{
			//TODO: implement!!! 
			throw new NotImplementedException();
			/*
			var query = new MetalinkRecord
			{
				Src = this.Code,
				SrcType = "zeta.objcls",
				TrgType = nodetype,
				Type = linktype,
				SubType = subtype,
				Active = true,
			};
			return new MetalinkRepository().Search(query, system);
			 */
		}

        #region IDetailObjectClass Members

		[Map]
        public virtual string Tag { get; set; }

        [Many(ClassName = typeof (objtype))]
        public virtual IList<IDetailObjectType> Types { get; set; }

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual string Name { get; set; }

        [Map]
        public virtual string Code { get; set; }

        [Map]
        public virtual string Comment { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        public virtual int Idx { get; set; }

        #endregion

	    public virtual string ResolveTag(string name) {
		    return TagHelper.Value(this.Tag, name);
	    }
    }
}