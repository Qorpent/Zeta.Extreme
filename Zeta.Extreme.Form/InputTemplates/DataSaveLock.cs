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
using System.Configuration;
using System.Threading;
using Comdiv.Extensions;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class DataSaveLock : IDisposable{
        private static readonly object sync = new object();


        private static readonly Mutex mutex;

        static DataSaveLock(){
            var mutexname = Guid.NewGuid().ToString();
            if (StringExtensions.hasContent(ConfigurationManager.AppSettings["savelock"])){
                mutexname = ConfigurationManager.AppSettings["savelock"];
            }
            mutex = new Mutex(false, mutexname);
        }

        private DataSaveLock(){
        }

        #region IDisposable Members

        public void Dispose(){
            mutex.ReleaseMutex();
        }

        #endregion

        public static DataSaveLock Get(){
            return Get(-1);
        }

        public static DataSaveLock Get(int timeout){
            lock (sync){
                var result = mutex.WaitOne(timeout, false);
                if (!result){
                    throw new TimeoutException("cannot get data save lock");
                }
                return new DataSaveLock();
            }
        }
    }
}