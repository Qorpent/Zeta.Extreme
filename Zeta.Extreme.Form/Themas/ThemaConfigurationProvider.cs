//#define OLDPARSER -- was tested with XDiff
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
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.IO;
using Comdiv.Zeta.Data;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// ��������� ������������
	/// </summary>
    public class ThemaConfigurationProvider : IThemaConfigurationProvider{
		private IInversionContainer _container;

        private IFilePathResolver _pathResolver;
        private readonly XslCompiledTransform _xsltcompiler;
    	private DateTime _cfgVersion;

    	/// <summary>
    	/// ����� � ������������������ �������
    	/// </summary>
    	public string CompileFolder { get; set; }
		/// <summary>
		/// ������� ����������� ������������
		/// </summary>
        public ThemaConfigurationProvider(){
            ThemaConfigurationFile = "data/root.xml";
            this.CompileFolder = "compiled_themas";
            this._xsltcompiler = new XslCompiledTransform();
            this._xsltcompiler.Load(myapp.files.Resolve("~/sys/themaxmlcompiler.xslt"),XsltSettings.TrustedXslt,new XmlUrlResolver());
        }
		/// <summary>
		/// �������� ������ �� ���������
		/// </summary>
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
		/// <summary>
		/// ��������� ������
		/// </summary>
        public IFilePathResolver PathResolver{
            get{
                lock (this){
                    if (null == _pathResolver){
                        _pathResolver = myapp.files;
                    }
                    return _pathResolver;
                }
            }
            set { _pathResolver = value; }
        }
		/// <summary>
		/// ���� ������������
		/// </summary>
        public string ThemaConfigurationFile { get; set; }

        /// <summary>
        /// ������ XML
        /// </summary>
        public string DirectXml { get; set; }


		/// <summary>
		/// ���������� �������� ��������� ��� ���������� ����
		/// </summary>
		/// <param name="themacode"></param>
		/// <param name="parameter"></param>
		/// <param name="value"></param>
		public void Set(string themacode, string parameter, object value){
            lock (this){
                value = value ?? "";
                var type = ReflectionExtensions.ResolveWellKnownName(value.GetType());
                var overfile = PathResolver.Resolve("data/override.xml");
                if (null == overfile){
                    overfile = PathResolver.Resolve("~/usr/data/override.xml", false);
                    PathResolver.Write(overfile, "<root/>");
                }
                var content = XElement.Parse(PathResolver.Read(overfile));
                var themaelement = content.XPathSelectElement("./thema[@id='" + themacode + "']");
                if (themaelement == null){
                    themaelement = new XElement("thema", new XAttribute("id", themacode));
                    content.Add(themaelement);
                }
                var param = themaelement.XPathSelectElement("./param[@id='" + parameter + "']");
                if (param == null){
                    param = new XElement("param", new XAttribute("id", parameter));
                    themaelement.Add(param);
                }
                param.SetAttributeValue("type", type);
                param.SetValue(value);
                content.Save(overfile);
            }
        }


		/// <summary>
		/// �������� ������������
		/// </summary>
		/// <returns></returns>
		public IThemaFactoryConfiguration Get() {
        	this._cfgVersion = DateTime.Now;
            XElement compiledxml = null;
			compiledxml = LoadCompiled();
         

            IDictionary<string, ThemaConfiguration> configurations2 = new Dictionary<string, ThemaConfiguration>();
            prepareEmpty(configurations2, compiledxml);
            readParameters(configurations2, compiledxml);
            readAll(configurations2,compiledxml);
            applyRenames(configurations2);
            var result = new ThemaFactoryConfiguration();
            foreach (var themaConfiguration in configurations2.Values){
                result.Configurations.Add(themaConfiguration);
            }
            result.SrcXml = compiledxml;

        	result.Version = _cfgVersion;

            return result;
        }
		

        /// <summary>
        /// ��������� ����������������� ����
        /// </summary>
        /// <returns></returns>
        public XElement LoadCompiled() {
        	var filters = LoadCompileFilters.split();
            var result = new XElement("root");
			_cfgVersion = new DateTime();
            foreach (var f in myapp.files.ResolveAll("~/tmp/"+CompileFolder, "*.xml").OrderBy(x=>Path.GetFileNameWithoutExtension(x))){
				if (File.GetLastWriteTime(f) > _cfgVersion) _cfgVersion = File.GetLastWriteTime(f);
				if (filters.Count == 0) {
					result.Add(XElement.Load(f));
				}else {
					if(null!=filters.FirstOrDefault(x=>f.like(x))) {
						result.Add(XElement.Load(f));	
					}
				}
            }
            return result;
        }


		private void readParameters(IDictionary<string, ThemaConfiguration> configurations2, XElement compiledxml){
            foreach (var cfg in configurations2.Values){
                var parameters = cfg.SrcXml.Elements("param");
                int idx = 0;
                foreach (var p in parameters){
                    var id = p.attr("id");
                    var val = p.attr("value");
                    if (val.noContent()){
                        val = p.Value;
                    }
                    idx += 10;
                    cfg.Parameters[id] = new TypedParameter{Type= typeof(string), Name = id, Value = val, Idx =  idx};
                }
            }
        }


        private void applyRenames(IDictionary<string, ThemaConfiguration> configurations){
            foreach (var configuration in configurations.Values){
                foreach (var rreport in configuration.SrcXml.XPathSelectElements("./renamereport")){
                    var code = configuration.Code + rreport.attr("code") + ".out";
                    var newv = configuration.Code + rreport.attr("name") + ".out";
                    var r = configuration.Outputs.get(code);
                    if (null != r){
                        r.Code = newv;
                        configuration.Outputs.Remove(code);
                        configuration.Outputs[newv] = r;
                    }
                }
                foreach (var rform in configuration.SrcXml.XPathSelectElements("./renameform")){
                    var code = configuration.Code + rform.attr("code") + ".in";
                    var newv = configuration.Code + rform.attr("name") + ".in";
                    var r = configuration.Inputs.get(code);
                    if (null != r){
                        r.Code = newv;
                        configuration.Inputs.Remove(code);
                        configuration.Inputs[newv] = r;
                    }
                }
            }
        }


		private void readAll(IDictionary<string, ThemaConfiguration> configurations, XElement descriptor){
            foreach (var configuration in configurations.Values){
                configuration.Name = configuration.SrcXml.attr("name", configuration.Code);
                applyPseudoProperties(configuration);
                readCommands(configuration);
                readDocuments(configuration);
                readInputs(configuration);
                readOutputs(configuration);
            }
        }

        private void applyPseudoProperties(ThemaConfiguration configuration){
            foreach (var parameter in configuration.Parameters){
                if (parameter.Key.StartsWith(".")){
                    var name = parameter.Key.Substring(1);
                    configuration.setPropertySafe(name, parameter.Value.GetValue());
                }
            }
        }

        private void readCommands(ThemaConfiguration configuration){
            foreach (var element in configuration.SrcXml.Elements("command")){
                var cmd = new CommandConfiguration();
                cmd.Thema = configuration;
                cmd.Code = configuration.Code + element.attr("id", "") + ".cmd";
                cmd.Name = element.attr("name", cmd.Code);
                cmd.Url = element.attr("url", "");
                cmd.Role = element.attr("role", configuration.DefaultElementRole);
                element.update(cmd, "id", "name", "code", "role", "url");
                if (!cmd.Active){
                    continue;
                }
                configuration.Commands[cmd.Code] = cmd;
            }
        }

        private void readDocuments(ThemaConfiguration configuration){
            foreach (var element in configuration.SrcXml.Elements("doc")){
                var doc = new DocumentConfiguration();
                doc.Thema = configuration;
                doc.Code = configuration.Code + element.idorcode() + ".doc";
                doc.Name = element.attr("name", doc.Code);
                doc.Url = element.attr("url", "");
                doc.Role = element.attr("role", configuration.DefaultElementRole);
                doc.Type = element.attr("type", "link");
                element.update(doc, "id", "name", "code", "role", "type");
                if (!doc.Active){
                    continue;
                }
                configuration.Documents[doc.Code] = doc;
            }
        }

        private void readInputs(ThemaConfiguration configuration){
            var inputs = configuration.SrcXml.Elements("in").Union(configuration.SrcXml.Elements("form")).ToArray();
            foreach (var element in inputs) {
                var id = element.idorcode();
                if (id.isIn("A", "B", "C")){
                    if (configuration.Parameters.ContainsKey("active" + id)){
                        var active = configuration.ResolveParameter("active" + id).Value.toBool();
                        if (!active){
                            element.Remove();
                            continue;
                        }
                    }
                }
                var input = new InputConfiguration();
                foreach (var parameter in configuration.Parameters){
                    if (parameter.Key.StartsWith("input.")){
                        var name = parameter.Key.Substring(6);
                        input.setPropertySafe(name, parameter.Value.GetValue());
                    }
                }
                
                input.Thema = configuration;
                input.Code = configuration.Code + element.idorcode() + ".in";
                input.Name = element.attr("name", input.Name ?? input.Code);
                input.Role = element.attr("role", input.Role ?? configuration.DefaultElementRole);
                input.Template = element.attr("template", input.Template ?? input.Code);
                input.SqlOptimization = element.attr("sqloptimization", "");
                if (!input.Template.EndsWith(".in")){
                    input.Template += ".in";
                }

                element.update(input, "id", "name", "code", "role", "template");
            	input.IgnorePeriodState = element.attr("ignoreperiodstate", false);
            	input.IsObjectDependent = element.attr("isobjectdependent", false);
                input.UseFormMatrix = element.elementOrAttr("useformmatrix", false);
                input.MatrixExRows = element.elementOrAttr("matrixexrows", "");
                input.MatrixExSqlHint = element.elementOrAttr("matrixexsqlhint", "");
                input.FixedObj = element.elementOrAttr("fixedobj", "");
			
            	input.Biztran = element.attr("biztran","");
                input.NeedPreloadScript = element.elementOrAttr("needpreloadscript", false);
                input.UseQuickUpdate = element.elementOrAttr("usequickupdate", false);
                input.DocumentRoot = element.attr("docroot", "");
                if (input.DocumentRoot.noContent())
                {
                    input.DocumentRoot = element.Elements("docroot").LastOrDefault().attr("code", "");
                }


                if (!input.Active){
                    continue;
                }

                 var exfile = PathResolver.ResolveAll("data", input.Template + ".xml", true).FirstOrDefault();
                 if (null == exfile){
                     input.Template = "empty.in";
                 }

                if (input.Template == "empty.in"){
                    input.TemplateXml = XElement.Parse("<input/>");
                }
                else{
                    var templateFile = PathResolver.ResolveAll("data", input.Template + ".xml", true).FirstOrDefault();
                    if (null == templateFile){
                        input.IsError = true;
                        input.Warrnings.Add("����������� ������ � ����� " + input.Template);
                    }
                    else{
                        input.TemplateXml =
                            XElement.Parse(PathResolver.ReadXml(templateFile, null,
                                                                new ReadXmlOptions
                                                                {UseIncludes = true, IncludeRoot = "data/include/"}));
#if ADAPT_TO_OLD_INPUT_TEMPLATES
                        foreach (var info in input.GetType().GetProperties()){
                            var name = "form." + info.Name.ToLower();
                            configuration.Parameters[name] = new TypedParameter{
                                                                                   Name = name,
                                                                                   Type = info.PropertyType,
                                                                                   Value =
                                                                                       input.getPropertySafe<string>(
                                                                                           info.Name)
                                                                               };
                        }

#endif
                        embedParametersIntoXml(configuration, input.TemplateXml);
                    }
                }
                input.Sources = element.Elements("uselib").Select(x => x.attr("id")).Where(x=>x.hasContent()).ToArray();
                input.ColumnDefinitions = element.XPathSelectElements("./col").ToArray();
                input.RowDefinitions = element.XPathSelectElements("./row").ToArray();
                var root = element.XPathSelectElement("./root");
                if (root != null){
                    input.RootCode = root.attr("code", "");
                }


                var parameters = element.XPathSelectElements("./param");
                foreach (var parameter in parameters){
                    var param = readParameter(parameter);
                    input.Parameters.Add(param);
                }

                var docs = element.XPathSelectElements("./doc");
                foreach (var x in docs)
                {
                    input.Documents[x.attr("code")] = x.attr("name");
                }




                configuration.Inputs[input.Code] = input;
            }
        }

        private void readOutputs(ThemaConfiguration configuration){
            var outs = configuration.SrcXml.Elements("out").ToArray();
            foreach (var element in outs){
                var id = element.attr("code", "");
                if (id.isIn("Aa", "Ab", "Ba","Bb","Ca","Cb")){
                    if (configuration.Parameters.ContainsKey("active" + id.Substring(0, 1))){
                        var active = configuration.ResolveParameter("active" + id.Substring(0, 1)).Value.toBool();
                        if (!active){
                            element.Remove();
                            continue;
                        }
                    }
                }


                var output = new OutputConfiguration();

                foreach (var parameter in configuration.Parameters){
                    if (parameter.Key.StartsWith("out.")){
                        var name = parameter.Key.Substring(4);
                        output.setPropertySafe(name, parameter.Value.GetValue());
                    }
                }

                output.Thema = configuration;

                output.Code = configuration.Code + element.idorcode() + ".out";
                output.Name = element.attr("name", output.Code);
                output.Role = element.attr("role", output.Role ?? configuration.DefaultElementRole);
                output.Type = element.attr("type", "call");
                output.Template = element.attr("template", output.Template ?? output.Code);
                output.Sources = element.Elements("uselib").Select(x => x.idorcode()).Where(x=>x.hasContent()).ToArray();
                output.UseFormMatrix = element.elementOrAttr("useformmatrix", false);
                output.MatrixExRows = element.elementOrAttr("matrixexrows", "");
                output.MatrixExSqlHint = element.elementOrAttr("matrixexsqlhint", "");
                if (!output.Template.EndsWith(".out")){
                    output.Template += ".out";
                }
                 var exfile = PathResolver.ResolveAll("data", output.Template + ".xml", true).FirstOrDefault();
                 if (null == exfile){
                     output.Template = "empty.out";
                 }

                element.update(output, "id", "name", "code", "role", "type", "template");


                if (!output.Active){
                    continue;
                }
                if (output.Template == "empty.out"){
                    output.TemplateXml = XElement.Parse("<report/>");
                    foreach (var e in element.Elements()){
                        output.TemplateXml.Add(e);
                    }
                }
                else{
                    var templateFile = PathResolver.ResolveAll("data", output.Template + ".xml", true).FirstOrDefault();
                    if (null == templateFile){
                        output.IsError = true;
                        output.Warrnings.Add("����������� ������ � ����� " + output.Template);
                    }
                    else{
                        output.TemplateXml =
                            XElement.Parse(PathResolver.ReadXml(templateFile, null,
                                                                new ReadXmlOptions
                                                                {UseIncludes = true, IncludeRoot = "data/include/"}));
#if ADAPT_TO_OLD_INPUT_TEMPLATES
                        foreach (var info in output.GetType().GetProperties()){
                            var name = "report." + info.Name.ToLower();
                            configuration.Parameters[name] = new TypedParameter{
                                                                                   Name = name,
                                                                                   Type = info.PropertyType,
                                                                                   Value =
                                                                                       output.getPropertySafe<string>(
                                                                                           info.Name)
                                                                               };
                        }

#endif
                        embedParametersIntoXml(configuration, output.TemplateXml);

                        var parameters = element.XPathSelectElements("./param");

                        foreach (var parameter in parameters){
                            var param = readParameter(parameter);
                            output.Parameters.Add(param);
                        }
                    }
                }


                configuration.Outputs[output.Code] = output;
            }
        }

        //class processedannotation {}
        //static processedannotation _defpa = new processedannotation();
    	private void embedParametersIntoXml(ThemaConfiguration configuration, XElement x){
    	    foreach (var e in x.DescendantsAndSelf()) {
                //if (null != e.Annotations<processedannotation>().FirstOrDefault()) continue;
    	        
    	        foreach (var a in e.Attributes()) {
                    if (-1 != a.Value.IndexOf('$'))
                    {
                        a.Value = embedParameters(configuration, a.Value);
                    }
                    if (-1 != a.Value.IndexOf('#')) {
                        //escapes some custom constructions, that must use ${...} constructions further
                        a.Value = a.Value.Replace("#{", "${");
                    }
    	        }
    	        foreach (var n_ in e.Nodes()) {
    	            if(n_ is XText) {
    	                var a = n_ as XText;
                        if (-1 != a.Value.IndexOf('$'))
                        {
                            a.Value = embedParameters(configuration, a.Value);
                        }
                        if (-1 != a.Value.IndexOf('#'))
                        {
                            //escapes some custom constructions, that must use ${...} constructions further
                            a.Value = a.Value.Replace("#{", "${");
                        }
    	            }
    	        }
               // e.AddAnnotation(_defpa);
    	    }
           
        }


		private string embedParameters(ThemaConfiguration configuration, string val){
           return val.replace(@"\$\{([\.\w]+)\}", m => configuration.ResolveParameter(m.Groups[1].Value).Value);
        }


		private TypedParameter readParameter(XElement parameter){
            var p = new TypedParameter
	            {
		            Type = ReflectionExtensions.ResolveTypeByWellKnownName(parameter.attr("type", "str")),
		            Value = parameter.attr("value", null) ?? parameter.Value,
		            Name = parameter.idorcode(),
		            Mode = parameter.attr("mode", "static")
	            };
			return p;
        }

        private void prepareEmpty(IDictionary<string, ThemaConfiguration> configurations, XElement descriptor){
            var themas = descriptor.XPathSelectElements("./thema");
            foreach (var thema in themas){
                var desc = new ThemaConfiguration{
                                                     Code = thema.idorcode("default"),
                                                     Name = thema.attr("name", thema.attr("code", "default")),
                                                     Active = thema.attr("active", true),
                                                     AutoImport = thema.attr("autoimport", false),
                                                     Layout = thema.attr("layout", "default"),
                                                     Abstract = thema.attr("abst", false),
                                                     Idx = thema.attr("idx", 0),
                                                     SrcXml = thema,
                                                 };
                thema.update(desc, "id", "name", "code", "active", "autoimport", "layout", "abst", "idx");
                if (desc.Abstract){
                    desc.Active = false;
                }
                configurations[desc.Code] = desc;
            }
        }

		/// <summary>
		/// ����������� ������� ����������
		/// </summary>
		public string LoadCompileFilters { get; set; }
    }


}