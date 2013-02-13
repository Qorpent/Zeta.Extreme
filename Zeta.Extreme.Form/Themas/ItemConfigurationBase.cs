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
using System.Collections.Generic;
using System.Xml.Linq;

namespace Comdiv.Zeta.Web.Themas{
    public abstract class ItemConfigurationBase<T> : IConfiguration<T>{
        public readonly IList<TypedParameter> Parameters = new List<TypedParameter>();
        public readonly IList<string> Warrnings = new List<string>();
        public XElement TemplateXml;
        private bool _isError;

        public ItemConfigurationBase(){
            Active = true;
        }

        public bool Active { get; set; }
        public string Value { get; set; }

        #region IConfiguration<T> Members

        public IThemaConfiguration Thema { get; set; }
        public abstract T Configure();
        public string Role { get; set; }

        public string Type { get; set; }

        public string Code { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Template { get; set; }

        public bool IsError{
            get { return _isError || getErrorInternal(); }
            set { _isError = value; }
        }

        #endregion

        protected virtual bool getErrorInternal(){
            return false;
        }
    }
}