#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : WithMarksExtension.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Linq;
using Comdiv.Application;
using Comdiv.Model;
using Qorpent.Model;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.Poco.Inerfaces {
	public static class WithMarksExtension {
		public static bool UseMarkCaching = true;

		public static bool IsMarkSeted(this IWithMarksBase obj, string code) {
			if (UseMarkCaching && (obj is IWithMarkCache)) {
				var mc = (IWithMarkCache) obj;
				if (mc.MarkCache == null) {
					return false;
				}
				return mc.MarkCache.Contains("/" + code + "/");
			}
			return null != obj.GetMarkLinks().FirstOrDefault(l => l.MarkLinkMark.Code == code);
		}

		public static void SetMark<T>(this T obj, string code) where T : IWithMarksBase {
			if (!IsMarkSeted(obj, code)) {
				if (UseMarkCaching && (obj is IWithMarkCache)) {
					var mc = ((IWithMarkCache) obj);
					var storage = myapp.storage.Get<T>();
					var copy = (IWithMarkCache) storage.Load<T>(((IWithId)obj).Id);
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

		public static void SetMark(this IWithMarksBase obj, IMark mark) {
			if (obj.IsMarkSeted(mark.Code)) {
				return;
			}

			var linkType = GetLinkType(obj);
			var storage = myapp.storage.Get(linkType);
			var link = (IMarkLinkBase) storage.New(linkType, "");
			link.MarkLinkMark = mark;
			link.MarkLinkTarget = (IEntity) obj;
			storage.Save(link);
			storage.Refresh(obj);
		}

		public static Type GetLinkType(this IWithMarksBase obj) {
			var typeMark =
				obj.GetType().GetInterfaces().First(
					t => t.IsGenericType && typeof (IWithMarks<,>).IsAssignableFrom(t.GetGenericTypeDefinition()));
			return typeMark.GetGenericArguments()[1];
		}

		public static void UnSetMark<T>(this T obj, string code) where T : IWithMarksBase {
			if (IsMarkSeted(obj, code)) {
				if (UseMarkCaching && (obj is IWithMarkCache)) {
					var mc = ((IWithMarkCache) obj);
					var storage = myapp.storage.Get<T>();
					var copy = (IWithMarkCache) storage.Load<T>(((IWithId)obj).Id);
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

		public static void UnSetMark(this IWithMarksBase obj, IMark mark) {
			if (obj.IsMarkSeted(mark.Code)) {
				var todelete = obj.GetMarkLinks().FirstOrDefault(l => l.MarkLinkMark.Code == mark.Code);
				
				obj.RemoveMark(mark);
				var storage = myapp.storage.Get<IMark>();
				if (null != todelete)
				{
					storage.Delete(todelete.GetType(), ((IWithId)todelete).Id, null);
				}
				
				// storage.Refresh(obj);
			}
		}

		public static void DropAllMarks(this IWithMarksBase obj) {
			foreach (var link in obj.GetMarkLinks()) {
				myapp.storage.Get(link.GetType()).Delete(link);
			}
			myapp.storage.Get(obj.GetType()).Refresh(obj);
		}
	}
}