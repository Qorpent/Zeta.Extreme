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
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Persistence;
using Comdiv.Reporting;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;
using NHibernate.Criterion;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class LockStateProvider : ILockStateProvider{
        private const string query =
            "exec usm.state_get_proc @row=@row, @obj=@obj, @detail = @detail, @year = @year, @period = @period";

        private const string last_query =
            "exec usm.state_last_proc @row=@row, @obj=@obj, @detail = @detail, @year = @year, @period = @period";

        private static readonly object sync = new object();

        private static IDbConnection connection;
        private readonly IDictionary<string, object> parameters = new Dictionary<string, object>();

        #region ILockStateProvider Members

        public string Get(object rowdefinition, object objdefinition, object detaildefinition, int year, int period){
            checkconnection();
            parameters["row"] = rowdefinition.asRow().Code;
            parameters["obj"] = objdefinition.asObject().Id;
            var d = detaildefinition.asDetail();
            parameters["detail"] = d == null ? DBNull.Value : (object) d.Id;
            parameters["year"] = year;
            parameters["period"] = period;
            lock (connection){
                return connection.ExecuteScalar<string>(query, parameters);
            }
        }

        public StateUser GetLastUser(object rowdefinition, object objdefinition, object detaildefinition, int year,
                                     int period){
            checkconnection();
            parameters["row"] = rowdefinition.asRow().Code;
            parameters["obj"] = objdefinition.asObject().Id;
            var d = detaildefinition.asDetail();
            parameters["detail"] = d == null ? DBNull.Value : (object) d.Id;
            parameters["year"] = year;
            parameters["period"] = period;
            lock (connection){
                var row = connection.ExecuteRow(last_query, parameters);
                if (null == row){
                    row = new object[]{"none", DateExtensions.Begin};
                }
                var login = row[0].toStr();
                var date = row[1].toDate();
            	var usr = Users.Get(login);
                if (null == usr){
                    usr = myapp.storage.Get<IZetaUnderwriter>().New();
                    usr.Name = login;
                    usr.Login = login;
                }
                return new StateUser{User = usr, Date = date};
            }
        }


        public void Contextualize(IReportDefinition definition){
        }

        #endregion

        private void checkconnection(){
            lock (sync){
                if (null == connection){
                    connection = myapp.Container.getConnection();
                }
            }
        }
    }
}