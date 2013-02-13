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
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;

namespace Comdiv.Zeta.Web.Themas{
    public static class ThemaExtensions{
        public static IEnumerable<T> BindParents<T>(this IEnumerable<T> active) where T : IThema{
            var full = active.UnGroup();
            foreach (var thema in full){
                if (thema.Parent.hasContent()){
                    thema.ParentThema = full.FirstOrDefault(x => x.Code == thema.Parent);
                    if (thema.ParentThema != null){
                        thema.ParentThema.Children.Add(thema);
                    }
                }
            }
            return active;
        }

        public static IEnumerable<T> CheckFavorities<T>(this IEnumerable<T> active) where T : class, IThema{
            var onlyfav = myapp.getProfile().Get("show_only_favorite_themas", false);
            var src = active.UnGroup().ToArray();
            var fav = myapp.getProfile().GetAsDictionary("favoritethemas");
            foreach (var src1 in src){
                src1.IsFavorite = false;
            }
            if (onlyfav){
                if (fav.Count()==0){
                    return new List<T>();
                }
                var toremove = new List<T>();


                foreach (var th in src){
                    if (th.IsGroup){
                        continue;
                    }
                    if (!fav.get(th.Code,false)){
                        toremove.Add(th);
                    }
                }

                var result = new List<T>();

                foreach (var th in active){
                    if (th.IsGroup){
                        var newg = (th as Thema).Clone(true) as Thema;
                        newg.GroupMembers.Clear();
                        foreach (var thema in th.GetGroup()){
                            if (!toremove.Contains((T) thema)){
                                thema.IsFavorite = true;
                                newg.GroupMembers.Add(thema);
                            }
                        }
                        if (newg.GroupMembers.Count != 0){
                            result.Add(newg as T);
                        }
                        continue;
                    }
                    if (th.Group.noContent()){
                        if (!toremove.Contains(th)){
                            th.IsFavorite = true;
                            result.Add(th);
                        }
                    }
                }
                return result;
            }
            else{
                foreach (var src1 in src) {
                	src1.IsFavorite = fav.get(src1.Code, false);
                }
            }
            return active;
        }

        public static IEnumerable<T> UnGroup<T>(this IEnumerable<T> active) where T : IThema{
            foreach (var t in active){
                yield return t;
                if (t.IsGroup){
                    foreach (var t2 in t.GetGroup().UnGroup()){
                        yield return (T) t2;
                    }
                }
            }
        }
    }
}