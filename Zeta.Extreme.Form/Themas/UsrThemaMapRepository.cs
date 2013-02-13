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
using Comdiv.Inversion;
using Comdiv.Persistence;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.Themas{
    public class UsrThemaMapRepository : IUsrThemaMapRepository{
        private IInversionContainer _container;
        private StorageWrapper<IUsrThemaMap> _storage;

        public StorageWrapper<IUsrThemaMap> Storage{
            get{
                if (null == _storage){
                    _storage = myapp.storage.Get<IUsrThemaMap>();
                }
                return _storage;
            }
            set { _storage = value; }
        }


        public IInversionContainer Container{
            get{
                if (_container.invalid()){
                    lock (this){
                        if (_container.invalid()){
                            Container = myapp.Container;
                        }
                    }
                }
                return _container;
            }
            set { _container = value; }
        }

        #region IUsrThemaMapRepository Members

        public IUsrThemaMap GetResponsibility(string thema, string system, IZetaMainObject obj){
            lock (this){
                return Storage.First("from ENTITY x where x.Thema=? and x.System=? and x.Object=?", thema, system, obj);
            }
        }

        public IUsrThemaMap GetResponsibility2(string thema, string system, IZetaMainObject obj){
            lock (this){
                return Storage.First("from ENTITY x where x.Thema=? and x.System=? and x.Object=?", thema + "_2", system,
                                     obj);
            }
        }


        public void SetResponsibility(string thema, string system, IZetaMainObject obj, IZetaUnderwriter usr){
            lock (this){
                var existed = GetResponsibility(thema, system, obj);

                if (null == usr){
                    if (null != existed){
                        Storage.Delete(existed);
                    }
                    return;
                }

                if (null == existed){
                    existed = Storage.New();
                    existed.Thema = thema;
                    existed.System = system;
                    existed.Object = obj;
                }
                existed.Usr = usr;
                Storage.Save(existed);
            }
        }

        public void SetResponsibility2(string thema, string system, IZetaMainObject obj, IZetaUnderwriter usr){
            lock (this){
                var existed = GetResponsibility2(thema, system, obj);

                if (null == usr){
                    if (null != existed){
                        Storage.Delete(existed);
                    }
                    return;
                }

                if (null == existed){
                    existed = Storage.New();
                    existed.Thema = thema + "_2";
                    existed.System = system;
                    existed.Object = obj;
                }
                existed.Usr = usr;
                Storage.Save(existed);
            }
        }

        #endregion
    }
}