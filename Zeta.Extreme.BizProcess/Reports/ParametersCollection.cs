// Copyright 2007-2010 Comdiv (F. Sadykov) - http://code.google.com/u/fagim.sadykov/
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// MODIFICATIONS HAVE BEEN MADE TO THIS FILE

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Logging;
using Qorpent.Dsl.XmlInclude;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.BizProcess.Reports
{
	/// <summary>
	/// Коллекция параметров отчета
	/// </summary>
    public class ParametersCollection:List<Parameter>
    {
		/// <summary>
		/// 
		/// </summary>
        public ParametersCollection()
        {
            Prefix = "tp.";
            Extensible = true;
        }
		/// <summary>
		/// Префикс параметра отчета
		/// </summary>
        public string Prefix { get; set; }
		/// <summary>
		/// Признак расширяемости коллекции
		/// </summary>
        public bool Extensible { get; set; }

        /// <summary>
        /// Коллекция сохраненных параметров
        /// </summary>
        public ISavedReport SavedReport
        {
            get; set;
        }
		/// <summary>
		/// Обратная ссылка на отчет
		/// </summary>
        public IReportDefinition Report { get; set; }

        /// <summary>
        /// Получение словаря из входной коллекции
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public IDictionary<string, object> Eval(NameValueCollection collection)
        {
            var dict = new Dictionary<string, string>();
            collection.AllKeys.DoForEach(x => dict[x] = collection[x]);
            return Eval(dict);
        }

       
        new void Add(Parameter parameter){
            cachedall = null;
            var existed = this.FirstOrDefault(x => x.Code == parameter.Code);
            if (null != existed)
            {
                this.Remove(existed);
            }
            base.Add(parameter);
        }

        private ILog log = logger.get("zreport");
        private IEnumerable<Parameter> cachedall = null;
    	private bool cacheAble;
		/// <summary>
		/// Получить все параметры, включая сохраненные
		/// </summary>
		/// <returns></returns>
    	public IEnumerable<Parameter> AllParameters(){
            
            if (cachedall == null || !cacheAble){
                var skipped = new List<Parameter>();
                var result = new List<Parameter>();
            	foreach (var p in this) {
            		processSelfParameter(p,skipped,result);
            	}


                if (this.Report != null) {
                    var hiidx = 5;
                    foreach (var rd in this.Report.Sources.Reverse()) {
                        var idx = 1;
                        foreach (var p in rd.TemplateParameters.AllParameters()){
                            if(p.Idx==0) {
                                p.Idx = hiidx*100 + idx;
                            }
                            Parameter existed = null;
                            if (null == (existed = result.FirstOrDefault(x => x.Code == p.Code))){
                                result.Add(p.Clone().MarkAsFromLib(rd.Code));
                            }
                            else{
                                if(existed.Idx==0) {
                                    existed.Idx = p.Idx;
                                }
								skipped.Add(p.Clone().MarkAsFromLib(rd.Code));
                            }
                            idx++;
                        }
                        hiidx--;

                    }
                }
                normalizeParameter(result, skipped,  "viewname");
                normalizeParameter(result, skipped,  "generatorname");

                cachedall = result.Where(x=>x.Authorize(myapp.usr)).OrderBy(x=>x.Idx).ToArray();
            }
            return cachedall;
        }

    	private void processSelfParameter(Parameter parameter,IList<Parameter> target1,IList<Parameter> target2  ) {
    		if(parameter.Type=="file") {
    			this.cacheAble = false;
    			var file = parameter.DefaultValue.ToString();
				if(!file.Contains("/")) {
					file = "data/" + file + ".bxl";
				}
    			var xml = Qorpent.Applications.Application.Current.Container.Get<IXmlIncludeProcessor>().Load(file);
    			var paramelements = xml.XPathSelectElements("//param").ToArray();
    			var parameters = paramelements.Select(_ => _.Deserialize<Parameter>("")).ToArray();
    			foreach (var p in parameters) {
					if(!p.IsHidden) {
						p.Static = false;
					}
    				target1.Add(p);
					target2.Add(p);
    			}
    		}else {
    			target1.Add(parameter);
				target2.Add(parameter);
    		}
    	}

    	private void normalizeParameter(List<Parameter> result, List<Parameter> skipped, string name){
            var otherparam = skipped.LastOrDefault(x => x.Code == name && x.DefaultValue.ToBool());
            if (null != otherparam){
                var myparam = result.FirstOrDefault(x => x.Code == name);
                if (null == myparam){
                    result.Add(otherparam);
                }else{
                    if (!myparam.DefaultValue.ToBool()){
                        result.Remove(myparam);
                        result.Add(otherparam);
                    }    
                }
                    
            }
        }

        /// <summary>
        /// Довычисление набора парамтеров
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDictionary<string ,object> Eval<T>(IDictionary<string ,T> collection)
        {
            lock (this)
            {
                log.debug(() => "start eval parameters");
                
                //prepare
                var result = new Dictionary<string, object>();
                AllParameters().DoForEach(x => x.RawValue = null);

                log.debug(() => "Initial parameters:\r\n" + this.Select(x => x.ToString()).ConcatString(";\r\n"));


                var outervalues = new Dictionary<string, object>();
                collection.Where(x => x.Key.StartsWith(Prefix))
                    .DoForEach(x => outervalues[x.Key.Substring(Prefix.Length)] = x.Value);
                // first - we need collect all targets from parameters and init rawvalues collection
                AllParameters().Select(x => x.RealTarget).Distinct().DoForEach(x => result[x] = null);
                if (Extensible)
                {
                    foreach (var pair in outervalues)
                    {
                        if (!result.ContainsKey(pair.Key))
                        {
                            result[pair.Key] = pair.Value;
                        }
                    }
                }

                log.debug(() => "Outer parameters:\r\n" + outervalues.Select(x => x.Key+" : "+x.Value).ConcatString(";\r\n"));

                // firstly we process STATIC parameters
                foreach (var v in result.Keys.ToArray())
                {

                    var parameters = AllParameters().Where(x => x.Static && x.RealTarget == v && x.Value != null).ToList();
                    if (parameters.Count == 0) continue;
                    if (parameters.Count == 1) result[v] = parameters[0].Value;
                    else
                    {
                        result[v] = parameters.Select(x => x.Value.ToStr()).ConcatString(",");
                    }
                }
                //next we must process template parameters that not shares targets with static except conditions
                var staticTargets = AllParameters().Where(x => x.Static).Select(x => x.RealTarget).ToList();
                var templateTargets = AllParameters().Where(x =>  !x.Static && (x.Target=="condition" || !staticTargets.Contains(x.RealTarget) ))
                    .Select(x => x.RealTarget)
                    .Distinct();
                var templateValues = new Dictionary<string, object>();


                this.AdvancedSavedReports = new List<ISavedReport>();
                foreach (var parameter in AllParameters().Where(x=>x.Code.StartsWith("param_template_")).OrderBy(x=>x.Code)) {
                	string value = "";
					if(collection.ContainsKey("tp."+parameter.Code)) {
						value = collection["tp."+parameter.Code].ToStr();
					}
					if (value.IsEmpty()) value = parameter.Value.ToStr();
                    if(value.IsNotEmpty()) {
                        var report = myapp.storage.Get<ISavedReport>().Load(value);
                        
                        this.AdvancedSavedReports.Add(report);
                    }
                }

                foreach (var target in templateTargets)
                {
                    var temps = AllParameters().Where(x => x.RealTarget == target).Select(x => new Parameter() { Code=x.Code, DefaultValue = x.DefaultValue, Type = x.Type, AltValue = x.AltValue}).ToList();
                    foreach (var list in temps){
                        if(outervalues.ContainsKey(list.Code)){
                            list.RawValue = outervalues[list.Code];
                        }
                    }

                    foreach (var advancedSavedReport in this.AdvancedSavedReports)
                    {
                        foreach (var t in temps)
                        {
                            var sr = advancedSavedReport.Parameters.FirstOrDefault(x => x.Name == t.Code);
                            if (null != sr)
                            {
                                t.RawValue = sr.Value;
                                //t.setdef(sr.Value);
                            }
                        }
                    }

                    if (null != SavedReport){
                        foreach (var t in temps){
                            var sr = SavedReport.Parameters.FirstOrDefault(x => x.Name == t.Code);
                            if(null!=sr){
                                t.RawValue = sr.Value;
                                //t.setdef(sr.Value);
                            }
                        }
                    }

                   
                    
                    var pseudo = new Parameter { RealType = temps[0].RealType };
                    if(target=="condition"){
                        pseudo.RealType = typeof (string);
                    }
                    templateValues[target] = null;
                    if(outervalues.ContainsKey(target))
                    {
                        pseudo.DefaultValue = outervalues[target];
                    }else
                    {

                        if (1 == temps.Count){
                            
                            pseudo.RawValue = temps[0].Value;
                        }
                        else
                        {    
                            pseudo.RawValue = temps.Select(x => x.Value).Where(v => null != v).Select(v => v.ToStr()).ConcatString(",");
                            
                        }
                    }
                    
                    templateValues[target] = pseudo.Value;
                }
    
                foreach (var v in templateValues)
                {
                    var val = v.Value;
                    if(v.Key=="condition"){
                        val = result["condition"].ToStr() + "," + val;
                    }
                    result[v.Key] = val;
                }


                var keys = result.Keys.ToArray();

                foreach (var key in keys)
                {

                    try
                    {
                        if (result[key] is string)
                        {
                            result[key] = Regex.Replace(
                                result[key].ToString(), @"\$([^\W\d]+)",
                                m => result[m.Groups[1].Value].ToString(),
                                RegexOptions.Compiled
                                );
                        }
                    }
                    catch (Exception)
                    {
                        logger.get("comdiv.sys").Error("ошибка подготовки {0}({1})", key, result[key]);
                        throw;
                    }
                }
                log.debug(() => "Result :\r\n" + result.Select(x => x.Key + " : " + x.Value).ConcatString(";\r\n"));

                if(result.ContainsKey("year")){
                    var y = result.SafeGet("year").ToInt();
                    if(y>-20  && y < 20){
                        result["year"] = DateTime.Today.Year + y;
                    }
                }


                return result;
            }
        }
		/// <summary>
		/// Дополнительные сохраненные отчеты
		/// </summary>
        protected List<ISavedReport> AdvancedSavedReports { get; set; }

		/// <summary>
		/// Получить список закладок
		/// </summary>
		/// <returns></returns>
        public IList<string> GetTemplateParametersTabs() {
            return
                AllParameters().Select(x => x.Tab.IsNotEmpty() ? x.Tab : "00. Основные").Distinct().OrderBy(
                    x => x).ToList();
        }
		/// <summary>
		/// Получить список групп для закладки
		/// </summary>
		/// <param name="tab"></param>
		/// <returns></returns>
        public IList<string> GetTemplateParametersGroups(string tab) {
            return
                AllParameters().Where(x => (x.Tab == tab || (x.Tab.IsEmpty() && tab == "00. Основные"))).Select(x => x.Group.IsNotEmpty() ? x.Group : "99. Прочие").Distinct().OrderBy(
                    x => x == "99. Прочие" ? "ЯЯЯЯ" : x).ToList();
        }

		/// <summary>
		/// Получить параметры для группы в закладке
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="group"></param>
		/// <returns></returns>
        public IList<Parameter> GetTemplateParameters(string tab,string group) {
            return
                this.GetTemplateParameters().Where(x =>(x.Tab==tab || (x.Tab.IsEmpty() && tab=="00. Основные") ) && ((x.Group == group) || (x.Group.IsEmpty() && group == "99. Прочие"))).OrderBy(x=>x.Idx==0?1000:x.Idx).ToList();
        }

        
		/// <summary>
		/// Получить все группы
		/// </summary>
		/// <returns></returns>
        public IList<string> GetTemplateParametersGroups(){
            return
                AllParameters().Select(x => x.Group.IsNotEmpty() ? x.Group : "99. Прочие").Distinct().OrderBy(
                    x => x == "99. Прочие" ? "ЯЯЯЯ" : x).ToList();
        }

		/// <summary>
		/// Получить параметры по группе
		/// </summary>
		/// <param name="group"></param>
		/// <returns></returns>
        public IList<Parameter> GetTemplateParameters(string group){
            return
                this.GetTemplateParameters().Where(x => (x.Group == group) || (x.Group.IsEmpty() && group == "99. Прочие")).OrderBy(x=>x.Idx).ToList();
        }
		/// <summary>
		/// Получить все параметры
		/// </summary>
		/// <returns></returns>
        public IList<Parameter> GetTemplateParameters()
        {
            return AllParameters().Where(x => !x.Static).OrderBy(x=>x.Idx).ToList();
        }
		/// <summary>
		/// Получить параметр по коду
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
        public Parameter Get(string code)
        {
            return this.FirstOrDefault(x => x.Code == code);
        }

    }
}