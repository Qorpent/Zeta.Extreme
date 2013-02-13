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
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Inversion;
using Comdiv.Zeta.Data;
using Comdiv.Zeta.Model;

namespace Comdiv.Zeta.Web.Themas{
    public class RowListGenerator : XmlGeneratorBase{
        private readonly IDictionary<string, string> rownames = new Dictionary<string, string>();
        private IRowRepository Repository;
        private IInversionContainer _container;
        public string Root { get; set; }
        public string Mark { get; set; }
		public bool UseParentName { get; set; }
		public bool UseCode { get; set; }
		public string Tag { get; set; }
        public IZetaRow[] Rows { get; set; }
        public string RowList { get; set; }

        public IInversionContainer Container{
            get{
                if (_container.invalid()){
                    lock (this){
                        if (_container.invalid()){
                            Container = ioc.Container;
                        }
                    }
                }
                return _container;
            }
            set { _container = value; }
        }

        public string Value { get; set; }

        protected string ParamGroup { get; set; }

        protected string GroupCode { get; set; }

        protected string GroupName { get; set; }

        public string GroupCustomView { get; set; }

        public bool Hidden { get; set; }

        public string Condition { get; set; }

        public string CustomView { get; set; }

        protected override void prepare(XElement call){
            base.prepare(call);
            Root = call.attr("root", "");
            Mark = call.attr("mark", "");
            Value = call.attr("value", "");
        	Tag = call.attr("tag", "");
            if (Value.noContent()){
                Value = "false";
            }
        	UseParentName = call.attr("useparentname", false);
            Hidden = call.attr("hidden", false);
            CustomView = call.attr("customview", "");
            GroupCustomView = call.attr("groupcustomview", "");
            GroupName = call.attr("groupname", "");
            GroupCode = call.attr("groupcode", "");
        	UseCode = call.attr("usecode", false);
            RowList = call.attr("rowlist", "");
            Tab = call.attr("tab", "Набор строк");
            ParamGroup = call.attr("paramgroup", "");
            Condition = call.attr("condition", "");
            Repository = Container.get<IRowRepository>();
            if (RowList.noContent()){
				if (Tag.noContent()) {

					Rows =
						Repository.GetByMark(Root, Mark).OrderBy(
							// x => string.Format("{0}{1}", 1000000 + x.Idx, x.OuterCode ?? "")
							x => x.Idx
							).ToArray();
				}else {
					Rows = myapp.storage.Get<IZetaRow>().Query("from ENTITY where Tag like '%/'+?+':1/%' order by Idx", Tag).ToArray();
				}
            }
            else{
                IList<IZetaRow> rows = new List<IZetaRow>();
                foreach (var s in RowList.split()){
                    var code = "";
                    var name = "";
                    if (s.Contains(":")){
                        code = s.Split(':')[0];
                        name = s.Split(':')[1];
                    }
                    else{
                        code = s;
                    }
                    rownames[code] = name;
                    var row = RowCache.get(code);
                    if (null == row){
                        var x = call;
                        var str = "";
                        while (x.Parent != null && x.Parent.Name != "root"){
                            x = x.Parent;
                            if (x != null){
                                str = x.ToString();
                            }
                        }
                        throw new Exception(
                            string.Format(
                                "Код зависит от строки с кодом '{0}', которая отсутствует в БД, контекст:<br/>{1}",
                                code, HttpUtility.HtmlEncode(str)
                                )
                            );
                    }
                    rows.Add(row);
                }
                Rows = rows.ToArray();
            }
        }

        public string Tab { get; set; }

        protected override object[] internalGenerate(){
            var result = new List<XElement>();
            foreach (var row in Rows){
                if (Root.hasContent() && row.Code == Root){
                    continue;
                }
                ;
                var code = row.Code;
                var name = row.Name;
				if(UseParentName) {
					name = name + " (" + row.Parent.Name + ")";
				}
				if(UseCode) {
					name = name + " [" + row.Code + "]";
				}
                var alias = rownames.get(code, "");
                if (alias.hasContent()){
                    name = alias;
                }
                var tcode = "row_" + row.Code;
                var self = tcode;
                if (Condition.hasContent()){
                    if (Condition.Contains("$SELF")){
                        tcode = Condition.Replace("$SELF", "(" + self + ")");
                    }
                    else{
                        tcode = "(" + Condition + ") and (" + self + ")";
                    }
                }

                if (GroupCode.hasContent()){
                    result.Add(new XElement("var",
                                            new XAttribute("code", GroupCode),
                                            new XAttribute("name", GroupName),
                                            new XAttribute("customview", GroupCustomView),
                                            new XAttribute("group", ParamGroup),
                                            new XAttribute("tab", Tab),
                                            new XAttribute("role", row.FullRole ?? "")
                                   ));
                }

                var condelement = new XElement("var",
                                               new XAttribute("code", self),
                                               new XAttribute("target", "condition"),
                                               new XAttribute("name", name),
                                               new XAttribute("type", "bool"),
                                               new XAttribute("altvalue", self),
                                               new XAttribute("defaultvalue", Value),
                                               new XAttribute("role", row.FullRole ?? ""),
                                               new XAttribute("ishidden", Hidden),
                                               new XAttribute("customview", CustomView),
                                               new XAttribute("group", ParamGroup),
                                               new XAttribute("tab", Tab)
                    );
                result.Add(condelement);
                var rowelement = new XElement("row",
                                              new XAttribute("code", code),
                                              new XAttribute("name", name),
                                              new XAttribute("condition", tcode)
                    );
                result.Add(rowelement);
            }
            return result.ToArray();
        }
    }
}