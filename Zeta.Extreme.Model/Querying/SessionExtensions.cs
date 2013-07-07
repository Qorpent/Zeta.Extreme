#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.Model/SessionExtensions.cs
#endregion
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Extensions;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Model.Querying {



	/// <summary>
	/// ������ ��� ������ � ��������
	/// </summary>
	public static class SessionExtensions {
		/// <summary>
		/// ����������������� ����� ����������� ����������� ������ ����������
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static bool IsCollectStatistics(this ISession session) {
			if(null==session) return false;
			var sessionWithCollect = session as IWithSessionStatistics;
			return null != sessionWithCollect && sessionWithCollect.CollectStatistics;
		}
		/// <summary>
		/// ��������������� �������� � ���������� ������
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static SessionStatistics GetStatistics(this ISession session) {
			if(!session.IsCollectStatistics()) throw new ArgumentException("this session cannot support statistics");
			return ((IWithSessionStatistics) session).Statistics;
		}
		/// <summary>
		/// null-safe and type safe accessor to global or session-bound metacache
		/// </summary>
		/// <param name="session"></param>
		/// <returns>resultat is guaranted from nulls</returns>
		public static IMetaCache GetMetaCache (this ISession session) {
			if(null==session) return MetaCache.Default;
			var sessionAsDataSession = session as IWithDataServices;
			if(null==sessionAsDataSession) return MetaCache.Default;
			return sessionAsDataSession.MetaCache ?? MetaCache.Default;
		}

		/// <summary>
		/// ����� ���������� �������� �������� ������
		/// </summary>
		/// <param name="query"></param>
		/// <param name="pseudocode"></param>
		/// <returns></returns>
		public static string ResolveRealCode(this IQuery query, string pseudocode) {
			if (null != query)
			{
			var session = query.Session;
			var rowhandler = query.Row;
			
				return ResolveRealCode(session, query, pseudocode);
			}
			throw new Exception("cannot resolve " + pseudocode + " from " + query);
		}
		/// <summary>
		/// ��������� �������� ��� ��� ����� � �������
		/// </summary>
		/// <param name="session"></param>
		/// <param name="rowhandler"></param>
		/// <param name="pseudocode"></param>
		/// <returns></returns>
		public static string ResolveRealCode(this ISession session, IQuery query, string pseudocode) {
			var rowhandler = query.Row;
		    var code = "";
            if (query.Obj.Native is IZetaMainObject)
            {
                var obj = query.Obj.ObjRef;
                if (null == obj.ObjType && 0 != obj.ObjTypeId)
                {
                    obj = new NativeZetaReader().ReadObjectsWithTypes("o.Id = " + obj.Id).First();
                }
                code = obj.ResolveTag(pseudocode);
                if (!string.IsNullOrWhiteSpace(code))
                {
                    return code;
                }
            }
			code = ResolveRealCode(session, rowhandler, pseudocode);
			if (!string.IsNullOrWhiteSpace(code)) {
				return code;
			}

			

			return null;
		}
		/// <summary>
		/// ��������� �������� ��� ��� ����� ������ - �������������
		/// </summary>
		/// <param name="session"></param>
		/// <param name="rowhandler"></param>
		/// <param name="pseudocode"></param>
		/// <returns></returns>
		private static string ResolveRealCode(ISession session, IRowHandler rowhandler, string pseudocode) {
			if (session != null && session.PropertySource != null) {
				var code = session.PropertySource.Get(pseudocode).ToStr();
				if (!string.IsNullOrWhiteSpace(code)) {
					return code;
				}
			}

			if (null != rowhandler && null != rowhandler.Native) {
				if (rowhandler.Native.TemporalParent != null) {
					var code = rowhandler.Native.TemporalParent.ResolveTag(pseudocode);
					if (!string.IsNullOrWhiteSpace(code)) {
						return code;
					}
				}
				else {
					var code = rowhandler.Native.ResolveTag(pseudocode);
					if (!string.IsNullOrWhiteSpace(code)) {
						return code;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// null-safe and type safe accessor to global or session-bound metacache
		/// </summary>
		/// <param name="session"></param>
		/// <returns>resultat is guaranted from nulls</returns>
		public static IPrimarySource GetPrimarySource(this ISession session)
		{
			if (null == session) throw new Exception("null session");
			var sessionAsDataSession = session as IWithDataServices;
			if (null == sessionAsDataSession) throw new Exception("not primary data session");
			return sessionAsDataSession.PrimarySource;
		}
		/// <summary>
		/// Wrapper for session to wait data
		/// </summary>
		/// <param name="session"></param>
		/// <param name="timeout">if session is not IWithDataServices returns emidiatly </param>
		public static void WaitForPrimary(this ISession session, int timeout=-1) {
			if(null==session)return;
			var sessionAsDataSession = session as IWithDataServices;
			if(null==sessionAsDataSession)return;
			sessionAsDataSession.WaitPrimarySource(timeout); 
		}
	

		/// <summary>
		/// �������� ������� ��������
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static ConcurrentDictionary<string,IQuery> GetRegistry(this ISession session) {
			if(null==session)throw new Exception("session is null");
			var sessionAsRegistry = session as IWithQueryRegistry;
			if (null == sessionAsRegistry) throw new Exception("session is not with registry support");
			return sessionAsRegistry.Registry;
		}

		/// <summary>
		/// �������� ������� �������� (��������������)
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static ConcurrentDictionary<string, IQuery> GetRegistryActiveSet(this ISession session)
		{
			if (null == session) throw new Exception("session is null");
			var sessionAsRegistry = session as IWithQueryRegistry;
			if (null == sessionAsRegistry) throw new Exception("session is not with registry support");
			return sessionAsRegistry.ActiveSet;
		}

		/// <summary>
		/// �������� ������� �������� (���������)
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static IDictionary<string, string> GetRegistryKeyMap(this ISession session)
		{
			if (null == session) throw new Exception("session is null");
			var sessionAsRegistry = session as IWithQueryRegistry;
			if (null == sessionAsRegistry) throw new Exception("session is not with registry support");
			return sessionAsRegistry.KeyMap;
		}

		
	}
}