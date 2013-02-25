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

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class usr : IZetaUnderwriter{
        private string _slotList;
        private IList<string> _slots;

        [Ref(ClassName = typeof (obj))]
        public virtual IZetaMainObject Org { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        #region IZetaUnderwriter Members
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

		/// <summary>
		/// Free list of documents,where basis for security provided
		/// </summary>
		[Map]
	    public virtual string Documents { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        public virtual IZetaMainObject Object{
            get { return Org; }
            set { Org = value; }
        }

        [Map]
        public virtual bool Boss { get; set; }

        [Map]
        public virtual bool Worker { get; set; }

        [Map]
        public virtual string Dolzh { get; set; }

        [Map]
        public virtual string Contact { get; set; }

        [Map]
        public virtual string Roles { get; set; }
        [Map]
        public virtual string Login { get; set; }
        [Map]
        public virtual bool Active { get; set; }

		[Map]
		public virtual string Login2 { get; set; }

    	[Map]
        public virtual string SlotList{
            get { return _slotList; }
            set{
                _slotList = value;
                _slots = null;
            }
        }

        public virtual IList<string> Slots{
            get { return _slots ?? (_slots = SlotList.split()); }
        }

        public virtual bool IsFor(string slot){
            return Slots.Contains(slot);
        }

        #endregion

        public virtual bool IsInRole(string role){
            return Roles.split().Contains(role);
        }
    }
}