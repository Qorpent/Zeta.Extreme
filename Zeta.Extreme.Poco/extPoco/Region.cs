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
using Comdiv.Application;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class region : IZetaRegion{
        #region IZetaRegion Members

        public virtual int CountObjects(){
            return myapp.storage.Get<obj>().First<IZetaMainObject, int>(
                "select count(x.Id) from Org x where x.Location.Region=" + Id);
        }

        #endregion
/*
        public virtual int CountMunicipals(){
            var crit = DetachedCriteria.For(typeof (IZetaPoint))
                .SetProjection(Projections.Count("Id"))
                .Add(Restrictions.Eq("Region", this));
            return myapp.storage.Get<point>().First<IZetaPoint, int>(crit);
        }
 */
    }
}