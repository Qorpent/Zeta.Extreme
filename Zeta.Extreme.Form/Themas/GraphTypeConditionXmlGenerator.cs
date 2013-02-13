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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.IO;
using Zeta.Extreme.Form.Themas;

namespace Comdiv.Zeta.Web.Themas{
    public class GraphTypeConditionXmlGenerator : XmlGeneratorBase{
        protected override string GetSelfCondition(string code){
            return "";
        }

        protected override object[] InternalGenerate(){
            var result = "";
            var filenames = myapp.files.ResolveAll("~/scripts/graph/charts", "*.swf");
            var types =
                filenames.Select(x => Path.GetFileNameWithoutExtension(x)).OrderBy(x => x.Replace("FCF_", "ZZZ_")).
                    ToArray();

            foreach (var type in types){
                var name = type.Replace("FCF_", "");

                name = rename(name);

                if (type.StartsWith("FCF_")){
                    name = name + " (free)";
                }
                else{
                    name = name + " (v3)";
                }

                if (result.hasContent()){
                    result += "|";
                }
                result += type + ":" + name;
            }
            return new object[]{new XText(result)};
        }

        private string rename(string name){
            var sname = name;
            var d3 = false;
            var ms = false;
            var st = false;
            if (sname.Contains("3D")){
                d3 = true;
                sname = sname.Replace("3D", "");
            }
            sname = sname.Replace("2D", "");
            if (sname.StartsWith("MS")){
                ms = true;
                sname = sname.Substring(2);
            }

            if (sname.Contains("Stacked")){
                st = true;
                sname = sname.Replace("Stacked", "");
            }
            var stype = sname;

            if (sname.Contains("Column")){
                stype = "колонки";
                sname = sname.Replace("Column", "");
            }
            else if (sname.Contains("Bar")){
                stype = "планки";
                sname = sname.Replace("Bar", "");
            }
            else if (sname.Contains("Pie")){
                stype = "пирогом";
                sname = sname.Replace("Pie", "");
            }
            else if (sname.Contains("Area")){
                stype = "область";
                sname = sname.Replace("Area", "");
            }
            else if (sname.Contains("Doughnut")){
                stype = "кольцом";
                sname = sname.Replace("Doughnut", "");
            }
            else if (sname.Contains("Line")){
                stype = "линии";
                sname = sname.Replace("Line", "");
            }

            var result = stype;
            if (sname.hasContent()){
                result += " (" + sname + ")";
            }
            if (ms){
                result += "; многорядный";
            }
            if (st){
                result += "; стековый";
            }
            if (d3){
                result += "; 3D";
            }
            return result;
        }
    }
}