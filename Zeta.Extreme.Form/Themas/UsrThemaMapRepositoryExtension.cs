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
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.Themas{
    public static class UsrThemaMapRepositoryExtension{
        public static IUsrThemaMap GetResponsibility(this IUsrThemaMapRepository repos, string thema,
                                                     IZetaMainObject obj){
            return repos.GetResponsibility(thema, myapp.System, obj);
        }

        public static IUsrThemaMap GetResponsibility2(this IUsrThemaMapRepository repos, string thema,
                                                      IZetaMainObject obj){
            return repos.GetResponsibility2(thema, myapp.System, obj);
        }


        public static void SetResponsibility(this IUsrThemaMapRepository repos, string thema, IZetaUnderwriter usr){
            SetResponsibility(repos, thema, usr.Object, usr);
        }

        public static void SetResponsibility(this IUsrThemaMapRepository repos, string thema, IZetaMainObject obj,
                                             IZetaUnderwriter usr){
            repos.SetResponsibility(thema, myapp.System, obj, usr);
        }

        public static void SetResponsibility2(this IUsrThemaMapRepository repos, string thema, IZetaUnderwriter usr){
            SetResponsibility2(repos, thema, usr.Object, usr);
        }

        public static void SetResponsibility2(this IUsrThemaMapRepository repos, string thema, IZetaMainObject obj,
                                              IZetaUnderwriter usr){
            repos.SetResponsibility2(thema, myapp.System, obj, usr);
        }
    }
}