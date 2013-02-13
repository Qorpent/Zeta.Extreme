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
using Comdiv.Extensions;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class UsefulDetailFilter : IDetailFilter{
        public string[] Values { get; set; }
        public string[] Classes { get; set; }
        public string[] Types { get; set; }
        public int Year { get; set; }
        public int Period { get; set; }

        #region IDetailFilter Members

        public void Configure(IInputTemplate template){
            Year = template.Year;
            Period = template.Period;
        }

        public IList<IZetaDetailObject> GetDetails(IEnumerable<IZetaDetailObject> allObjects){
            var res = new List<IZetaDetailObject>();
            foreach (var o in allObjects){
                if (Classes.yes()){
                    if (!o.Type.Class.Code.isIn(Classes)){
                        continue;
                    }
                }
                if (Types.yes()){
                    if (!o.Type.Code.isIn(Types)){
                        continue;
                    }
                }
                if (Values.yes()){
                    var valid = true;
                    foreach (var value in Values){
                        var q = value.Split(':');
                        var row = q[0];
                        var tests = q[1].split();
                        var test = Query.EvaluateConstant(o, row, Year, Period, false);
                        if (!tests.Contains(test)){
                            valid = false;
                            continue;
                        }
                    }
                    if (!valid){
                        continue;
                    }
                }
                res.Add(o);
            }
            return res;
        }

        #endregion

        public UsefulDetailFilter TestValues(params string[] values){
            Values = values;
            return this;
        }

        public UsefulDetailFilter TestClasses(params string[] classes){
            Classes = classes;
            return this;
        }

        public UsefulDetailFilter TestTypes(params string[] types){
            Types = types;
            return this;
        }
    }
}