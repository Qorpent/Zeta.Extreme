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
using System.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Model;
using Comdiv.Model.Interfaces;
using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model{
    public static class WithMarksExtension{
        public static bool UseMarkCaching = true;

        public static bool IsMarkSeted(this IWithMarksBase obj, string code){
            if (UseMarkCaching && (obj is IWithMarkCache)){
                var mc = (IWithMarkCache) obj;
                if (mc.MarkCache == null){
                    return false;
                }
                return mc.MarkCache.Contains("/" + code + "/");
            }
            return null != obj.GetMarkLinks().FirstOrDefault(l => l.MarkLinkMark.Code == code);
        }

		public static void SetMark<T>(this T obj, string code) where T : IWithMarksBase
		{
			if (!IsMarkSeted(obj, code)) {
				if (UseMarkCaching && (obj is IWithMarkCache)) {
					var mc = ((IWithMarkCache) obj);
					var storage = myapp.storage.Get<T>();
					var copy = (IWithMarkCache) storage.Load<T>(obj.Id());
					var existed = copy.MarkCache;
					copy.MarkCache = SlashListHelper.SetMark(existed, code);
					storage.Save(copy);
					storage.Refresh(mc);
				}
				else {
					var mark = myapp.storage.Get<IMark>().Load(code);
					obj.SetMark(mark);
				}
			}
        }

        public static void SetMark(this IWithMarksBase obj, IMark mark){

            if (obj.IsMarkSeted(mark.Code)){
                return;
            }

            var linkType = GetLinkType(obj);
            var storage = myapp.storage.Get(linkType);
            var link = (IMarkLinkBase) storage.New(linkType, "");
            link.MarkLinkMark = mark;
            link.MarkLinkTarget = (IEntityDataPattern) obj;
            storage.Save(link);
            storage.Refresh(obj);
        }

        public static Type GetLinkType(this IWithMarksBase obj){
            var typeMark =
                obj.GetType().GetInterfaces().First(
                    t => t.IsGenericType && typeof (IWithMarks<,>).IsAssignableFrom(t.GetGenericTypeDefinition()));
            return typeMark.GetGenericArguments()[1];
        }

        public static void UnSetMark<T>(this T obj, string code) where T : IWithMarksBase{
			if (IsMarkSeted(obj, code)) {
				if (UseMarkCaching && (obj is IWithMarkCache)) {
					var mc = ((IWithMarkCache) obj);
					var storage = myapp.storage.Get<T>();
					var copy = (IWithMarkCache) storage.Load<T>(obj.Id());
					var existed = copy.MarkCache;
					copy.MarkCache = SlashListHelper.RemoveMark(existed, code);
					storage.Save(copy);
					storage.Refresh(mc);
				}
				else {
					var mark = myapp.storage.Get<IMark>().Load(code);
					obj.UnSetMark(mark);
				}
			}
        }

        public static void UnSetMark(this IWithMarksBase obj, IMark mark){
			
				if (obj.IsMarkSeted(mark.Code)) {
					var todelete = obj.GetMarkLinks().FirstOrDefault(l => l.MarkLinkMark.Code == mark.Code);
					obj.RemoveMark(mark);
					var storage = myapp.storage.Get<IMark>();
					storage.Delete(todelete.GetType(), todelete.Id(), null);
					// storage.Refresh(obj);
				}
			
        }

        public static void DropAllMarks(this IWithMarksBase obj){
            foreach (var link in obj.GetMarkLinks()){
                myapp.storage.Get(link.GetType()).Delete(link);
            }
            myapp.storage.Get(obj.GetType()).Refresh(obj);
        }
    }
}