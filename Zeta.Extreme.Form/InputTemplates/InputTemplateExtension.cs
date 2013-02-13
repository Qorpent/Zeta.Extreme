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
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using Castle.MonoRail.Framework;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.MVC;
using Comdiv.Security.Acl;
using Comdiv.Useful;
using Comdiv.Zeta.Data;

namespace Comdiv.Zeta.Web.InputTemplates{
    public static class InputTemplateExtension{
        public static InputTemplateRequest ExtractRequest(this IController controller){
            return controller.ExtractRequest("");
        }

        public static InputTemplateRequest ExtractRequest(this IController controller, string prefix){
            return ExtractRequest(controller, prefix, true);
        }

        public static InputTemplateRequest ExtractRequest(this IController controller, string prefix, bool renderRequest){
            prefix = GetNoramlPrefix(prefix);
            var c = (Controller) controller;

            InputTemplateRequest request = null;
            var current = myapp.conversation.Current;
            var existedRequest = current.Data.Values.OfType<InputTemplateRequest>().FirstOrDefault();
            if (existedRequest != null){
                request = existedRequest;
            }
            else{
                c.PropertyBag["requestKey"] = "";
                request = NewRequest(c, prefix);
                foreach (var param in c.Params.AllKeys){
                    if (param.StartsWith("fp.")){
                        var name = param.Substring(3);
                        request.Parameters[name] = c.Params[param];
                    }
                }
                current.Data["template.request"] = request;
            }
            c.PropertyBag["requestKey"] = current.Code;
            if (renderRequest){
                c.PropertyBag["request"] = request;
                c.PropertyBag["template"] = request.Template;
            }
            return request;
        }

        public static InputTemplateRequest NewRequest(this Controller c, string prefix){
            InputTemplateRequest request;
            request =
                new InputTemplateRequest{
                                            ObjectId = c.Params[prefix + "object"],
                                            TemplateCode = c.Params[prefix + "template"],
                                            Year = c.Params["year"].toInt().normalizeYear(),
                                            DetailId = c.ExtractInt(prefix + "detail"),
                                            Period = c.ExtractInt(prefix + "period"),
                                            Date = c.ExtractDate(prefix + "date")
                                        };

            return request;
        }

        private static string GetNoramlPrefix(string prefix){
            prefix = prefix.noContent() ? "" : prefix + ".";
            return prefix;
        }

        public static NameValueCollection PackToParameters(this InputTemplateRequest request){
            var result = new NameValueCollection();
            result["year"] = request.Year.ToString();
            result["period"] = request.Period.ToString();
            result["date"] = request.Date.ToShortDateString();
            result["object"] = request.Object == null ? "" : request.Object.Id.ToString();
            result["detail"] = request.Detail == null ? "" : request.Detail.Id.ToString();
            result["template"] = request.Template.Code;
            return result;
        }

        public static bool Authorize(this InputTemplateRequest request){
            return Authorize(request, myapp.usr);
        }

        public static bool Authorize(this InputTemplateRequest request, IPrincipal user){
            if (null != request.Detail && request.Detail.Object.Id != request.Object.Id){
                return false;
            }
            if (!request.Object.Authorize(user)){
                return false;
            }
            request.ReloadTemplate();
            return acl.get(request.Template);
            /// return typeof(InputTemplateExtension)._<IAuthorizeService>().Authorize("input-security", false, request.ToMvcContext().SetUser(user));
        }
    }
}