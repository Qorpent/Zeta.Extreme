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
// PROJECT ORIGIN: Zeta.Extreme.Form/UserOrgDataMapper.cs
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Security.Principal;
using Qorpent.Applications;
using Qorpent.IoC;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.Form.Meta{
    /// <summary>
    /// 
    /// </summary>
    public static class UserOrgDataMapper{
        private static readonly object sync = new object();

        private static IContainer _container;


        /// <summary>
        /// 
        /// </summary>
        public static IPrincipal DefaultUser{
            get{
                lock (sync){
                    return Application.Current.Principal.CurrentUser;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IContainer Container{
            get { return _container ?? (_container = Application.Current.Container); }
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
                if (TagHelper.Value(obj.Tag, "public").ToBool()) return true;
                if (Application.Current.Roles.IsAdmin(user)){
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

		private static IDbConnection getConnection() {

			return Application.Current.DatabaseConnections.GetConnection("Default");
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
	            var domain = name.GetDomainNamePart();
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
            return GetAvailOrgs(Application.Current.Principal.CurrentUser, "", start);
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
	            var domain = name.GetDomainNamePart();
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