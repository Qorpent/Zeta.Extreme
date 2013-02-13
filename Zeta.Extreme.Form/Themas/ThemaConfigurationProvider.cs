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
using Comdiv.Booxml;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.IO;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Data.Minimal;
using Qorpent.Bxl;

namespace Comdiv.Zeta.Web.Themas{
	/// <summary>
	/// Провайдер конфигураций
	/// </summary>
    public class ThemaConfigurationProvider : IThemaConfigurationProvider{
        private readonly IDictionary<string, string> globals = new Dictionary<string, string>();

    
        private readonly IList<string> lateglobals = new List<string>();
        private IInversionContainer _container;

        private IFilePathResolver _pathResolver;
        private XslCompiledTransform xsltcompiler;
    	private DateTime cfgVersion;

    	/// <summary>
    	/// Папка с откомпилированными файлами
    	/// </summary>
    	public string CompileFolder { get; set; }
		/// <summary>
		/// Создает стандартный конфигуратор
		/// </summary>
        public ThemaConfigurationProvider(){
            ThemaConfigurationFile = "data/root.xml";
            this.CompileFolder = "compiled_themas";
            this.xsltcompiler = new XslCompiledTransform();
            this.xsltcompiler.Load(myapp.files.Resolve("~/sys/themaxmlcompiler.xslt"),XsltSettings.TrustedXslt,new XmlUrlResolver());
        }
		/// <summary>
		/// Обратная ссылка на контейнер
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
		/// Резольвер файлов
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
		/// Файл конфигурации
		/// </summary>
        public string ThemaConfigurationFile { get; set; }

        /// <summary>
        /// Прямой XML
        /// </summary>
        public string DirectXml { get; set; }


		/// <summary>
		/// Установить значение параметра для конкретной темы
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
		/// Получить конфигурацию
		/// </summary>
		/// <returns></returns>
		public IThemaFactoryConfiguration Get() {
        	this.cfgVersion = DateTime.Now;
            XElement compiledxml = null;

            if (needCompilation()){
                compiledxml = CompileXml();
            }else{
                compiledxml = LoadCompiled();
            }

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

        	result.Version = cfgVersion;

            return result;
        }

        public bool RecompileAlways { get; set; }

        private bool needCompilation(){
#if TC
            return RecompileAlways;
#else
            if(! myapp.files.Exists("~/tmp/compiled_themas/.compilestamp")) return true;
            var cdate = myapp.files.LastWriteTime("~/tmp/compiled_themas/", "*.xml");
            var sdate = myapp.files.LastWriteTime("data", "*.bxl");
            if (sdate > cdate){
                return true;
            }
            return false;
#endif
        }


        public XElement LoadCompiled() {
        	var filters = LoadCompileFilters.split();
            var result = new XElement("root");
			cfgVersion = new DateTime();
            foreach (var f in myapp.files.ResolveAll("~/tmp/"+CompileFolder, "*.xml").OrderBy(x=>Path.GetFileNameWithoutExtension(x))){
				if (File.GetLastWriteTime(f) > cfgVersion) cfgVersion = File.GetLastWriteTime(f);
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



        private XElement CompileXml(){
            var compiledxml = new XElement("root");
            var descriptor = readBaseXml();
            var y = DateTime.Today.Year;

            ApplyGlobalsAndSubstitutionsStep.prepareGlobals(descriptor, globals, lateglobals);
            globals["ТЕКУЩИЙГОД"] = y.ToString();
            globals["CURRENTYEAR"] = globals["ТЕКУЩИЙГОД"];
            globals["СПИСОКГОДОВ"] = y.range(y - 4).concat("|") + "|--|" + 1990.range(y + 3).concat("|");
            globals["YEARLIST"] = globals["СПИСОКГОДОВ"];
            globals["ТЕКУЩИЙПЕРИОД"] = Periods.ForMonth(DateTime.Today.Month).ToString();
            globals["CURRENTPERIOD"] = globals["ТЕКУЩИЙПЕРИОД"];
            ApplyGlobalsAndSubstitutionsStep.applyGlobals(descriptor, globals);
            ApplyGlobalsAndSubstitutionsStep.applyGenerators(descriptor);
            ApplyGlobalsAndSubstitutionsStep.prepareLateGlobals(descriptor, globals, lateglobals);
            ApplyGlobalsAndSubstitutionsStep.applyGlobals(descriptor, globals);
            ApplyGlobalsAndSubstitutionsStep.resolveSubstitutions(descriptor);
            ApplyGlobalsAndSubstitutionsStep.resolveSubstitutions(descriptor);

            IDictionary<string, ThemaConfiguration> configurations = new Dictionary<string, ThemaConfiguration>();
            instantiateDescendants(descriptor);
            overrideFromOverrideFile(descriptor);
            //read list of themas with codes and parameters
            prepareEmpty(configurations, descriptor);


            //resolve import dependencies
            resolveImports(configurations, descriptor);
            //replace ${xxx} constructions in parameters
            
            prepareParameters(configurations, descriptor,false);
            cleanupParameters(configurations, descriptor);
            processEmbeds(configurations, descriptor);
            prepareParameters(configurations, descriptor,false);


            //read all other content of themas
            readAll(configurations, descriptor);
            applyRenames(configurations);

            foreach(var f in myapp.files.ResolveAll("~/tmp/"+CompileFolder+"/","*.*"))
            {
                File.Delete(f);
            }

            int idx = 10;
            foreach (var configuration in configurations.Values){
                if(configuration.Abstract)continue;
                var xml = configuration.SrcXml;
                var sw = new StringWriter();
                using (var xw = XmlWriter.Create(sw,new XmlWriterSettings{Indent = true,OmitXmlDeclaration = true})){
                    xsltcompiler.Transform(xml.CreateReader(),xw); 
                    xw.Flush();
                }
                var x = sw.ToString();
                myapp.files.Write("~/tmp/"+CompileFolder+"/"+idx.ToString("0000_")+configuration.Code+".xml",x);
                idx += 10;
                compiledxml.Add(XElement.Parse(x));
            }
            
            foreach (var e in descriptor.XPathSelectElements("//*[@preservexml]")){
                 myapp.files.Write("~/tmp/"+CompileFolder+"/"+idx.ToString("ZEXT_")+idx+".xml",e.ToString());
                idx++;
                compiledxml.Add(e);
            }


            myapp.files.Write("~/tmp/"+CompileFolder+"/.compilestamp",DateTime.Now.ToString());

            return compiledxml;
        }

        private void cleanupParameters(IDictionary<string, ThemaConfiguration> configurations, XElement descriptor) {
            foreach (var conf in configurations) {
                foreach (var p in conf.Value.Parameters.Keys.ToArray()) {
                    var par = conf.Value.Parameters[p];
                    if(string.IsNullOrEmpty(par.Value)||par.Value=="false") {
                        conf.Value.Parameters.Remove(p);
                    }
                }
            }
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


        private void processEmbeds(IDictionary<string, ThemaConfiguration> configurations, XElement descriptor){
            foreach (var configuration in configurations.Values){
                configuration.SrcXml = configuration.SrcXml.processIncludes("embed", descriptor);
            }
        }

        private void overrideFromOverrideFile(XElement descriptor){
            var over = PathResolver.Read("data/override.xml");
            if (over.hasContent()){
                var ox = XElement.Parse(over);
                var overridedthemas = ox.Elements("thema").ToArray();
                foreach (var element in overridedthemas){
                    var target = descriptor.XPathSelectElement("./thema[@id='" + element.attr("id") + "']");
                    if (null != target){
                        foreach (var e in element.Elements()){
                            target.Add(e);
                        }
                    }
                }
            }
        }

        public void instantiateDescendants(XElement descriptor){
            var absts = descriptor.XPathSelectElements("./thema[@abst='true' or @abst = '1']").ToArray();
            foreach (var element in absts){
                var id = element.attr("id");
                var childs = descriptor.XPathSelectElements("./" + id).ToArray();
                foreach (var child in childs){
                    var newelement = new XElement("thema");
                    newelement.Add(new XElement("import", new XAttribute("id", id)));
                    bindData(child, newelement);
                    child.ReplaceWith(newelement);
                }
            }
        }

        private void bindData(XElement from, XElement to){
            foreach (var e in from.Elements()){
                to.Add(e);
            }
            foreach (var a in from.Attributes()){
                if (a.Name == "imports"){
                    var imports = a.Value.split();
                    foreach (var import in imports){
                        to.Add(new XElement("import", new XAttribute("id", import)));
                    }
                    continue;
                }
                var selfprop = typeof (ThemaConfiguration).resolveProperty(a.Name.LocalName);
                if (selfprop != null || a.Name.LocalName == "id" || a.Name.LocalName == "code" || a.Name == "abst" ||
                    a.Name == "active"){
                    to.Add(a);
                }
                else{
                    var param = new XElement("param");
                    var name = a.Name.LocalName;
                    if (name.StartsWith("this.")){
                        name = name.Substring(4);
                    }

                    if (name.EndsWith(".bool")){
                        name = name.Substring(0, name.Length - 5);
                        param.SetAttributeValue("type", "bool");
                    }

                    if (name.EndsWith(".int")){
                        name = name.Substring(0, name.Length - 4);
                        param.SetAttributeValue("type", "int");
                    }

                    param.SetAttributeValue("id", name);
                    param.SetAttributeValue("value", a.Value);
                    to.Add(param);
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
                        input.Warrnings.Add("отсутствует шаблон с кодом " + input.Template);
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
                        output.Warrnings.Add("отсутствует шаблон с кодом " + output.Template);
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


        private void prepareParameters(IDictionary<string, ThemaConfiguration> configurations, XElement descriptor, bool embedonly){
            foreach (var configuration in configurations.Values){
                if (!embedonly) {
                    foreach (var parameter in GetValidOrder(configuration)) {
                        // parameter.Name = embedParameters(configuration, parameter.Name);
                        if (parameter.Value.IndexOf('$') != -1) {
                            parameter.Value = embedParameters(configuration, parameter.Value);
                        }
                    }
                }
                embedParametersIntoXml(configuration, configuration.SrcXml);
            }
        }


        private TypedParameter[] GetValidOrder(ThemaConfiguration configuration){
            var parameters = configuration.Parameters.Values.ToArray();
            var groupped = parameters.GroupBy(x => x.Name);
            var orderedGroups = groupped.OrderBy(x => x.Min(p => p.Idx)).ToArray();
            var result = orderedGroups.SelectMany(x => x.Select(p => p).OrderBy(p => p.Idx)).ToArray();
            return result;
        }

        private string embedParameters(ThemaConfiguration configuration, string val){
           return val.replace(@"\$\{([\.\w]+)\}", m => configuration.ResolveParameter(m.Groups[1].Value).Value);
        }


        private void resolveImports(IDictionary<string, ThemaConfiguration> configurations, XElement descriptor){
            foreach (var cfg in configurations){
                var imports = cfg.Value.SrcXml.Elements("import").Union(cfg.Value.SrcXml.Elements("imports")).ToArray();
                foreach (var import in imports){
                    var code = import.idorcode("default");
                    if(!configurations.ContainsKey(code)) {
                        throw new Exception(string.Format("невозможно произвести импорт темы {0} в тему {1} ({2})",code, cfg.Value.Code, cfg.Value.Evidence));
                    }
                    var thema = configurations[code];
                    cfg.Value.Imports.Add(thema);
                }
            }
            IList<ThemaConfiguration> autos = configurations.Values.Where(x => x.AutoImport).ToArray();
            foreach (var cfg in configurations.Values){
                foreach (var auto in autos){
                    if (cfg == auto){
                        continue;
                    }
                    if (null != cfg.Imports.OfType<ThemaConfiguration>().FirstOrDefault(x => x.Code == auto.Code)){
                        continue;
                    }
                    cfg.Imports.Add(auto);
                }
            }
            foreach (var cfg in configurations.Values){
                cfg.ProcessImports();
            }


            foreach (var cfg in configurations.Values){
                var idx = 1;
                var parameters = cfg.SrcXml.XPathSelectElements("./param");
                foreach (var parameter in parameters){
                    var p = readParameter(parameter);
                    p.Idx = idx;
                    if (cfg.Parameters.ContainsKey(p.Name)){
                        cfg.Parameters[p.Name].Value = p.Value;
                    }
                    else{
                        cfg.Parameters[p.Name] = p;
                    }
                    idx++;
                }
            }
        }

        private TypedParameter readParameter(XElement parameter){
            var p = new TypedParameter();
            p.Type = ReflectionExtensions.ResolveTypeByWellKnownName(parameter.attr("type", "str"));
            p.Value = parameter.attr("value", null) ?? parameter.Value;
            p.Name = parameter.idorcode();
            p.Mode = parameter.attr("mode", "static");
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

    	protected virtual XElement readBaseXml(){
#if OLDPARSER
            var booparser = new BooxmlParser();
#else
    		var booparser = new BxlParser();
#endif
            if (DirectXml.hasContent()){
                return XElement.Parse(DirectXml);
            }
            var result = new XElement("root");
            var allthemas = PathResolver.ResolveAll("data", "*.thema.xml");
            foreach (var thema in allthemas){
                var x = XElement.Parse(PathResolver.ReadXml(thema));
                foreach (var element in x.Elements()){
                    result.Add(element);
                }
            }
            allthemas =PathResolver.ResolveAll("data", "*.bxl");
            foreach (var thema in allthemas){
                if (thema.Contains(".bak")) continue;
                var name = Path.GetFileName(thema);
                IDictionary<string, string> defines = new Dictionary<string, string>();
                var y = DateTime.Today.Year;
                defines["__do_not_apply_globals"] = "";
                defines["ТЕКУЩИЙГОД"] = y.ToString();
                defines["CURRENTYEAR"] = y.ToString();
                defines["СПИСОКГОДОВ"] = 1990.range(y + 3).concat("|");
                defines["YEARLIST"] = 1990.range(y + 3).concat("|");
                defines["ТЕКУЩИЙПЕРИОД"] = Periods.ForMonth(DateTime.Today.Month).ToString();
                defines["CURRENTPERIOD"] = Periods.ForMonth(DateTime.Today.Month).ToString();
#if OLDPARSER
                var x = booparser.Parse(Path.GetFileNameWithoutExtension(thema), PathResolver.ReadXml(thema), defines);
#else
				var x = booparser.Parse( PathResolver.ReadXml(thema), Path.GetFileNameWithoutExtension(thema));
#endif
                foreach (var element in x.Elements()){
                    element.SetAttributeValue("evidence", name);
                    result.Add(element);
                }
            }

            var themas = result.Elements("thema").ToArray();
            foreach (var thema in themas){
                var rewrite = new XElement("thema");
                bindData(thema, rewrite);
                thema.ReplaceWith(rewrite);
            }

            result = result.processIncludes(PathResolver, "data/include/");
            return result;
        }

		public string LoadCompileFilters { get; set; }
    }


}