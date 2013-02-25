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
using Comdiv.Olap.Model;
using Comdiv.Zeta.Model;

using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco{
    public partial class col : IZetaColumn, IZetaQueryDimension {
        #region IZetaColumn Members

        public virtual IList<IMarkLinkBase> GetMarkLinks(){
            if (null == _markLinks){
                if (0 != Id && (null != myapp.storage.Get<col>(false))){
                    myapp.storage.Get<col>().Refresh(this);
                }
            }
            if (null == _markLinks){
                _markLinks = MarkLinks;
            }
            return MarkLinks.OfType<IMarkLinkBase>().ToList();
        }

        public virtual void RemoveMark(IMark mark){
            var todel = MarkLinks.FirstOrDefault(i => i.Mark.Id == mark.Id);
            if (null != todel){
                MarkLinks.Remove(todel);
            }
        }

        public virtual bool IsMarkSeted(string code){
            return WithMarksExtension.IsMarkSeted(this, code);
        }

        public virtual string GetStaticMeasure(string format){
            if (IsDynamicMeasure){
                return "";
            }
            if (Measure.hasContent()){
                if (format.hasContent()){
                    return string.Format(format, Measure);
                }
                return Measure;
            }
            return "";
        }

        public virtual string GetDynamicMeasure(IZetaRow source, string format){
            if (!IsDynamicMeasure){
                return "";
            }
            if (source.Measure.hasContent()){
                if (format.hasContent()){
                    return string.Format(format, source.Measure);
                }
                return source.Measure;
            }
            return GetStaticMeasure(format);
        }

        #endregion
    }
}