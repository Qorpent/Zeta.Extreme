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
using System.Data;
using System.Linq;
using System.Security;
using System.Security.Principal;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Security;
using Comdiv.Zeta.Model;

namespace Zeta.Extreme.Meta{
    /// <summary>
    /// 
    /// </summary>
    public static class UserOrgDataMapper{
        private static readonly object sync = new object();

        private static IInversionContainer _container;


        /// <summary>
        /// 
        /// </summary>
        public static IPrincipal DefaultUser{
            get{
                lock (sync){
                    return myapp.usr;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IInversionContainer Container{
            get{
                if (_container.invalid()){
                    lock (typeof (UserOrgDataMapper)){
                        if (_container.invalid()){
                            Container = myapp.ioc;
                        }
                    }
                }
                return _container;
            }
            set { _container = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IZetaDetailObject Authorized(this IZetaDetailObject obj){
            lock (sync){
                obj.Object.Authorize(true);
                return obj;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IZetaMainObject Authorized(this IZetaMainObject obj){
            lock (sync){
                obj.Authorize(true);
                return obj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        /// <exception cref="SecurityException"></exception>
        public static bool Authorize(this IZetaMainObject obj, bool throwError){
            lock (sync){
                var result = Authorize(obj, DefaultUser);
                if (result || !throwError){
                    return result;
                }
                throw new SecurityException("Попытка доступа к данным чужого предприятия!");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Authorize(this IZetaMainObject obj){
            lock (sync){
                return Authorize(obj, DefaultUser);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Authorize(this IZetaMainObject obj, IPrincipal user){
            lock (sync){
				if (null == obj) return true;
                if (TagHelper.Value(obj.Tag, "public").toBool()) return true;
                if (myapp.roles.IsAdmin(user)){
                    return true;
                }
                if (HasAll(user)){
                    return true;
                }
                if (obj == null){
                    return false;
                }

                foreach (var org in GetAvailOrgs(user)){
                    if (org.Id == obj.Id){
                        return true;
                    }
                }


                return false;
            }
        }

		private static IDbConnection getConnection()
		{
			if (null == Qorpent.Applications.Application.Current.DatabaseConnections)
			{
				return myapp.getConnection("Default");
			}
			return Qorpent.Applications.Application.Current.DatabaseConnections.GetConnection("Default") ??
				   myapp.getConnection("Default");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool HasAll(){
            lock (sync){
                return HasAll(DefaultUser);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool HasAll(IPrincipal principal){
            lock (sync){
                if (Qorpent.Applications.Application.Current.Roles.IsInRole(principal,"SYS_ALLOBJECTS")){
                    return true;
                }
	            var name = principal.Identity.Name;
	            var domain = name.toDomain();
	            using (var c = getConnection()) {
		            c.Open();
		            var cmd = c.CreateCommand();
		            cmd.CommandText = "select top 1 allobj from zeta.usrobjmap where '" + name +
		                              "' like usrname or domain = '" + domain + "' and allobj = 1";
		            var exists = cmd.ExecuteScalar();
					if(null!=exists && !(exists is DBNull) && (int)exists!=0) {
						return true;
					}
	            }
	            return false;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IZetaMainObject[] GetAvailOrgs(){
            lock (sync){
                return GetAvailOrgs(DefaultUser, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static IZetaMainObject[] GetAvailOrgs(IPrincipal principal){
            lock (sync){
                return GetAvailOrgs(principal, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="like"></param>
        /// <returns></returns>
        public static IZetaMainObject[] GetAvailOrgs(IPrincipal principal, string like){
            return GetAvailOrgs(principal, like, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IZetaMainObject[] GetAvailOrgs(bool start){
            return GetAvailOrgs(myapp.usr, "", start);
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="principal"></param>
	    /// <param name="like"></param>
	    /// <param name="start"></param>
	    /// <returns></returns>
	    public static IZetaMainObject[] GetAvailOrgs(IPrincipal principal, string like, bool start) {
	        like = like ?? "";
            lock (sync){
				var name = principal.Identity.Name;
	            var domain = name.toDomain();
                if (HasAll(principal)){
					return (
						from o in  ObjCache.ObjById.Values // myapp.storage.AsQueryable<IZetaMainObject>()
						where (!start || o.ShowOnStartPage) && (like==""||o.Name.Contains(like))
						select o
					).ToArray();
                }

				IList<IZetaMainObject> objects = new List<IZetaMainObject>();
	            using (var c = getConnection()) {
					c.Open();
		            var cmd = c.CreateCommand();
		            cmd.CommandText = "select distinct obj from zeta.usrobjmap where '" + name +
		                              "' like usrname or domain = '" + domain + "' and allobj != 1 and obj!=0";

		            using (var r = cmd.ExecuteReader()) {
			            while (r.Read()) {
				            var id = r.GetInt32(0);
							if(0!=id) {
								objects.Add(MetaCache.Default.Get<IZetaMainObject>(id));
							}
			            }
		            }
	            }
                return objects.Where(x => x.Comment != "sys").ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetAvailOrgCount(){
            lock (sync){
                return GetAvailOrgCount(DefaultUser);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static int GetAvailOrgCount(IPrincipal principal){
            lock (sync){
                return GetAvailOrgs(principal).Count();
            }
        }
    }
}