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
using System.Collections;
using System.Linq;
using Comdiv.Application;
using Comdiv.Persistence;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class InputTemplateStorageStep : StorageQueryStep<InputTemplateStorageStep>{
        private static readonly IInputTemplateCache cache = new InputTemplateCache();
        private static bool configured;

        private bool wasLoaded;

        public InputTemplateStorageStep(){
            Syncronized = true;
            if (!configured){
                myapp.OnReload += (s, a) => cache.Reload();
                configured = true;
            }

            //Priority = (int)myapp.storage.PriorityClass.Action;

            //try{
            //    cache.Reload();
            //    wasLoaded = true;
            //}
            //catch (Exception){}
        }

        public string Path{
            get { return cache.Path; }
            set { cache.Path = value; }
        }

        protected override bool getSupport(){
            return true;
        }

        protected override bool internalIsApplyable(StorageQuery query){
            lock (this){
                return typeof (IInputTemplate).IsAssignableFrom(MyQuery.TargetType);
            }
        }

        protected override object getLoad(){
            lock (this){
                if (!wasLoaded){
                    cache.Reload();
                    wasLoaded = true;
                }


                return cache[MyQuery.Key.ToString()];
            }
        }

        protected override IEnumerable getQuery(){
            lock (this){
                if (!wasLoaded){
                    cache.Reload();
                    wasLoaded = true;
                }

                return cache.Values.ToArray();
            }
        }
    }
}