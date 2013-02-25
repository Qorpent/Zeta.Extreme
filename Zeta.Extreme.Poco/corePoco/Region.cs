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
using Comdiv.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class region : IZetaRegion{
        private IList<IZetaDetailObject> _detailobjects;
        private IList<IZetaMainObject> _mainobjects;

        [Ref(ClassName = typeof (zone))]
        public virtual IZetaZone Country { get; set; }

        [Map]
        public virtual Guid Uid { get; set; }

        #region IZetaRegion Members

        public virtual string Tag { get; set; }

        [Many(ClassName = typeof (point))]
        public virtual IList<IZetaPoint> Points { get; set; }

        public virtual IZetaZone Zone{
            get { return Country; }
            set { Country = value; }
        }

        [Map]
        public virtual int Id { get; set; }

        [Map]
        public virtual string Name { get; set; }

        [Map]
        public virtual string Code { get; set; }

        [Map]
        public virtual string Comment { get; set; }

        [Map]
        public virtual DateTime Version { get; set; }

        public virtual int Idx { get; set; }

        public virtual IList<IZetaMainObject> MainObjects{
            get{
                if (null == _mainobjects){
                    var result = new List<IZetaMainObject>();
                    foreach (var town in Points){
                        foreach (var mainObject in town.MainObjects){
                            result.Add(mainObject);
                        }
                    }
                    _mainobjects = result;
                }
                return _mainobjects;
            }
            set { _mainobjects = value; }
        }

        public virtual IList<IZetaDetailObject> DetailObjects{
            get{
                if (null == _detailobjects){
                    var result = new List<IZetaDetailObject>();
                    foreach (var town in Points){
                        foreach (var mainObject in town.DetailObjects){
                            result.Add(mainObject);
                        }
                    }
                    _detailobjects = result;
                }
                return _detailobjects;
            }
            set { _detailobjects = value; }
        }

        #endregion
    }
}