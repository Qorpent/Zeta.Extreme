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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Comdiv.Extensions;

namespace Comdiv.Zeta.Web.Themas{
    public class ThemaConfiguration : IThemaConfiguration{
        public readonly IDictionary<string, CommandConfiguration> Commands =
            new Dictionary<string, CommandConfiguration>();

        public readonly IDictionary<string, DocumentConfiguration> Documents =
            new Dictionary<string, DocumentConfiguration>();

        public readonly IDictionary<string, InputConfiguration> Inputs = new Dictionary<string, InputConfiguration>();
        public readonly IDictionary<string, OutputConfiguration> Outputs = new Dictionary<string, OutputConfiguration>();

        public ThemaConfiguration(){
            Active = true;
            Visible = true;
            Parameters = new Dictionary<string, TypedParameter>();
            Imports = new List<IThemaConfiguration>();
        }

        public bool Abstract { get; set; }
        public string Parent { get; set; }
        public bool IsGroup { get; set; }
        public string Group { get; set; }
        public string ClassName { get; set; }
        public bool IsTemplate { get; set; }

        public bool AutoImport { get; set; }
        public bool ImportsProcessed { get; set; }
        public bool Visible { get; set; }
        public string Layout { get; set; }

        public string Id{
            get { return Code; }
        }


        public IDictionary<string, TypedParameter> Parameters { get; set; }


        public XElement SrcXml { get; set; }

        public string Role { get; set; }

        #region IThemaConfiguration Members

        public string Code { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Idx { get; set; }

        public string Evidence{
            get { return SrcXml.attr("evidence"); }
        }

        public TypedParameter ResolveParameter(string name, object  def = null){
            if (Parameters.ContainsKey(name)){
                return Parameters[name];
            }
            var prop = GetType().resolveProperty(name);
            if (null != prop){
                return new TypedParameter{
                                             Name = name,
                                             Type = prop.PropertyType,
                                             Value = prop.GetValue(this, null).toStr(),
                                         };
            }
            foreach (var import in Imports){
                var value = import.ResolveParameter(name);
                if (!typeof (Missing).Equals(value.Type)){
                    return value;
                }
            }

            return new TypedParameter{Value = def.toStr()};
        }

        public IList<IThemaConfiguration> Imports { get; set; }

        public string DefaultElementRole { get; set; }


        public IThema Configure(){

            var type = typeof (Thema);
            if (ClassName.hasContent()){
                type = Type.GetType(ClassName, true);
            }
            var result = type.create<Thema>();
            result.Configuration = this;
            result.Code = Code;
            result.Name = Name;
            result.Role = Role;
            result.Idx = Idx;
            result.IsGroup = IsGroup;
            result.Group = Group;
            result.Layout = Layout;
            result.Visible = Visible;
            result.IsTemplate = IsTemplate;
            result.Parent = Parent;


			try {

				foreach (var output in Outputs.Values) {
					if (!output.IsError) {
						var def = output.Configure();
						def.Thema = result;
						result.Reports[output.Code] = def;
					}
				}

				foreach (var input in Inputs.Values) {
					if (!input.IsError) {
						var def = input.Configure();
						def.Thema = result;
						result.Forms[input.Code] = def;
					}
				}

				foreach (var command in Commands.Values) {
					if (!command.IsError) {
						result.Commands[command.Code] = command.Configure();
					}
				}

				foreach (var doc in Documents.Values) {
					if (!doc.IsError) {
						result.Documents[doc.Code] = doc.Configure();
					}
				}

				foreach (var parameter in Parameters.Values) {
					//skips temporary and pseudo-property parameters
					if (!parameter.Name.Contains(".")) {
						result.Parameters[parameter.Name] = parameter.GetValue();
					}
				}
			}catch(Exception ex) {
				result.Error = ex;
			}
        	return result;
        }

        #endregion

        public override string ToString(){
            return Code + " " + Name;
        }

        public void ProcessImports(){
            if (ImportsProcessed){
                return;
            }
            var firstelement = SrcXml.Elements().FirstOrDefault();
            foreach (ThemaConfiguration import in Imports){
                import.ProcessImports();

                foreach (var element in import.SrcXml.Elements()){
                    XElement subelement = null;
                    //if(element.Name.LocalName=="param" && !element.Attribute("id").Value.Contains(".")){
                    //    subelement = new XElement(element);
                    //    subelement.Attribute("id").Value = import.Code + "." + subelement.Attribute("id").Value;
                    //}
                    if (firstelement != null){
                        firstelement.AddBeforeSelf(element);
                        if (subelement != null){
                            firstelement.AddBeforeSelf(subelement);
                        }
                    }
                    else{
                        SrcXml.Add(new XElement(element));
                        if (subelement != null){
                            SrcXml.Add(new XElement(subelement));
                        }
                    }
                }
            }
            embedEarlyParametersIntoXml(this, SrcXml);
            ImportsProcessed = true;
        }

        private void embedEarlyParametersIntoXml(ThemaConfiguration configuration, XElement x){
            foreach (XAttribute attr in ((IEnumerable) x.XPathEvaluate(".//@*"))){
                attr.Value = embedEarlyParameters(configuration, attr.Value);
                //escapes some custom constructions, that must use ${...} constructions further
                //attr.Value = attr.Value.Replace("#{", "${");
            }
            foreach (XText e in ((IEnumerable) x.XPathEvaluate(".//text()"))){
                e.Value = embedEarlyParameters(configuration, e.Value);
                //escapes some custom constructions, that must use ${...} constructions further
                //e.Value = e.Value.Replace("#{", "${");
            }
        }

        private string embedEarlyParameters(ThemaConfiguration configuration, string val){
            if (val.like(@"ZZ([\.\w]+)ZZ")){
                val = val.replace(@"ZZ([\.\w]+)ZZ", m =>{
                                                        var par =
                                                            configuration.SrcXml.XPathSelectElements("./param[@id='" +
                                                                                                     m.Groups[1].Value +
                                                                                                     "']").
                                                                LastOrDefault();
                                                        if (null == par){
                                                            return m.Value;
                                                        }
                                                        var v = par.attr("value");
                                                        if (v.noContent()){
                                                            v = par.Value;
                                                        }
                                                        return v;
                                                    });
            }
            return val;
        }
    }
}