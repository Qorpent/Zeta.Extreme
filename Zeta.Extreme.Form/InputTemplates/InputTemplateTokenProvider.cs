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
using Comdiv.Extensions;
using Comdiv.Security.Acl;

namespace Comdiv.Zeta.Web.InputTemplates{
    public class InputTemplateTokenProvider : XmlBasedTokenProvider<IInputTemplate>{
        public InputTemplateTokenProvider(){
            FileName = "input.acl.token.config";
            DefaultPrefix = "input";
        }

        public override string suffix(IInputTemplate input){
            var result = "";
            if (input.Parameters.ContainsKey("_target_object")){
                result += "o" + input.Parameters["_target_object"].toStr() + "/";
            }
            if (input.Parameters.ContainsKey("_target_detail")){
                result += "s" + input.Parameters["_target_detail"].toStr() + "/";
            }
            if (input.Period != 0){
                result += "p" + input.Period + "/";
            }
            if (input.DirectDate.Year > 1900){
                result += string.Format("y{0}/m{1}/d{2}/", input.DirectDate.Year, input.DirectDate.Month,
                                        input.DirectDate.Day);
            }
            else{
                if (input.Year != 0){
                    result += "y" + input.Year + "/";
                }
            }
            return result;
        }
    }
}