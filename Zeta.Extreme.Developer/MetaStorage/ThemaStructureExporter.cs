using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Qorpent.Applications;
using Qorpent.BSharp;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.Config;

namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Формирует структуру тем
    /// </summary>
    public class ThemaStructureExporter :  IDataToBSharpExporter {
        private const string SUBSYSTEM_ELEMENT_NAME = "subsystem";
        private const string BLOCK_ELEMENT_NAME = "block";
        private const string ROOT_ELEMENT_NAME = "root";
        private const string THEMA_ELEMENT_NAME = "thema";

        /// <summary>
        /// Формирует BSharp со структурой
        /// </summary>
        /// <returns></returns>
        public string Generate() {
            var xml = GenerateXml();
            return RenderXml(xml);
        }
        private MD5 hasher = MD5.Create();
        private string RenderXml(XElement xml) {
            var builder = new BSharpCodeBuilder();
            var hash = Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(hashsrc)));
            builder.WriteCommentBlock("Экспорт структуры форм",new{hash, BlocksOnly});
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName);
            builder.WriteClassElement(SUBSYSTEM_ELEMENT_NAME);
            builder.WriteClassElement(BLOCK_ELEMENT_NAME);
            builder.WriteClassElement(ROOT_ELEMENT_NAME);
            builder.WriteClassElement(THEMA_ELEMENT_NAME);
            foreach (var s in xml.Elements(SUBSYSTEM_ELEMENT_NAME)) {
                if (!s.HasElements) continue;
                builder.StartElement(SUBSYSTEM_ELEMENT_NAME,s.GetCode(),s.GetName());
                foreach (var b in s.Elements(BLOCK_ELEMENT_NAME )) {
                    builder.StartElement(BLOCK_ELEMENT_NAME,b.GetCode(),inlineattributes: new{oldcode=b.Attr("oldcode"),doubled=b.Attr("doubled")});
                    if (!BlocksOnly) {
                        foreach (var r in b.Elements(ROOT_ELEMENT_NAME)) {
                            builder.StartElement(ROOT_ELEMENT_NAME, r.GetCode(),
                                                 inlineattributes:
                                                     new {oldcode = r.Attr("oldcode"), doubled = r.Attr("doubled")});
                            foreach (var t in r.Elements(THEMA_ELEMENT_NAME)) {
                                builder.WriteElement(THEMA_ELEMENT_NAME, t.GetCode(), t.GetName(),
                                                     inlineattributes: new {hasform = r.Attr("hasform")});
                            }
                            builder.EndElement();
                        }
                    }
                    builder.EndElement();
                }
                builder.EndElement();
            }
            return builder.ToString();
        }

        private string hashsrc = "";
        private XElement GenerateXml() {
            
            var resultXml = new XElement("themaTree");
            var compiledFolder = Application.Current.Container.Get<IDeveloperConfig>().ThemaComiledFolder;
            foreach (var f in Directory.GetFiles(compiledFolder, "*.xml").OrderBy(_=>Path.GetFileNameWithoutExtension(_).Substring(5))) {
                var txt = File.ReadAllText(f);
                
                var fx = XElement.Parse(txt);
                bool processed = ProcessThema(fx, resultXml);
                if (processed) {
                    hashsrc += Convert.ToBase64String(hasher.ComputeHash(Encoding.UTF8.GetBytes(txt)));
                }
            }
            resultXml = AccomodateDoublers(resultXml);
            return resultXml;
        }

        private XElement AccomodateDoublers(XElement resultXml) {
            foreach (var e in resultXml.Descendants()) {
                if (null == e.Attribute("doubled") && e.Attribute("oldcode") != null) {
                    e.SetAttributeValue("code",e.Attr("oldcode"));
                    e.SetAttributeValue("oldcode",null);
                }
            }
            return resultXml;
        }

        string GetParam(XElement th, string name, string def = null) {
            var el = th.Elements("param").Where(_ => _.Attr("id") == name).FirstOrDefault();
            string result = "";
            if (null != el) {
               
                result = el.Attr("value");
            }
            var at = th.Attribute(name.Escape(EscapingType.XmlName));
            if (null != at) {
                result = at.Value;
            }
            if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(def)) {
                return def;
            }
            return result;
        }
        private bool ProcessThema(XElement th, XElement res) {
            var code = th.Attr("code");

            if (code.Contains("lib")) {
                return false;
            }
            var isgroup = GetParam(th,".isgroup").ToBool();
            if (isgroup) {
                EmitSubsystem(res, code, th.GetName());
            }
            else {
                var form = th.Elements("form").Where(_ => _.Attr("code") == "A").FirstOrDefault();
                if (!CheckThemaAttributes(th)) return false;
                bool hasform = false;
                if (null != form)
                {
                    if (form.Attr("active").ToBool())
                    {
                        if (form.Attr("visible") == "" ||form.Attr("visible").ToBool())
                        {
                            hasform = true;
                        }
                    }
                }
                if (!hasform) return false;
                var grp = GetParam(th, ".group","NOGROUP");
                var grpelement = EmitSubsystem(res, grp);
                var roleprefix = GetParam(th, "roleprefix", "NOROLE");
                var block = EmitBlock(res, grpelement, roleprefix);
                var rootrow = GetParam(th, "rootrow", "NOROOT");
                var excludeRoots = ExcludeRoots.SmartSplit();
                
                if (excludeRoots.Contains(rootrow)) return false;

                var root = EmitRoot(res, block, rootrow);
               
                var name = th.Attr("name");
                EmitThema(root, code, name);
            }


            return true;
        }

        private bool CheckThemaAttributes(XElement th) {
            if (!DisableStatusFilter) {
                if (GetParam(th, "thematype") != "in") return false;
                var status = GetParam(th, "status");
                if (!("" == status || "test" == status)) return false;

                if (GetParam(th, ".role") == "ADMIN") return false;
            }
            return true;
        }

        private XElement EmitThema(XElement root, string code, string name) {
            return root.AddElement(THEMA_ELEMENT_NAME, attributes: new {code, name});
        }

        private XElement EmitRoot(XElement res, XElement block, string code) {
            var fullcode = block.GetCode() + "." + code;
            var existed = block.Elements(ROOT_ELEMENT_NAME).FirstOrDefault(_ => _.Attr("oldcode") == code);
            if (null == existed)
            {
                var doublers = res.Descendants(ROOT_ELEMENT_NAME).Where(_ => _.Attr("oldcode") == code).ToArray();

                existed = block.AddElement(ROOT_ELEMENT_NAME, attributes: new {code=fullcode,oldcode= code });
                if (0 != doublers.Length)
                {
                    existed.SetAttributeValue("doubled", true);
                    foreach (var element in doublers)
                    {
                        element.SetAttributeValue("doubled", true);
                    }
                }
            }
            return existed;
        }

        private XElement EmitBlock(XElement res, XElement grpelement, string code) {
            var fullcode = grpelement.GetCode() + "." + code;
            var existed = grpelement.Elements(BLOCK_ELEMENT_NAME).FirstOrDefault(_ => _.Attr("oldcode") == code);
            if (null == existed) {
                var doublers = res.Descendants(BLOCK_ELEMENT_NAME).Where(_ => _.Attr("oldcode") == code).ToArray();
                
                existed = grpelement.AddElement(BLOCK_ELEMENT_NAME, attributes: new {code=fullcode,oldcode= code });
                if (0 != doublers.Length) {
                    existed.SetAttributeValue("doubled",true);
                    foreach (var element in doublers) {
                        element.SetAttributeValue("doubled",true);
                    }
                }
            }
            return existed;
        }

        private XElement EmitSubsystem(XElement res, string code, string name = null) {
            var aliases = SubsystemAliases.SmartSplit();
            foreach (var alias in aliases) {
                var from = alias.Split('=')[0];
                var to = alias.Split('=')[1];
                if (code == from) {
                    code = to;
                    name = "";
                    break;
                }
            }
            var existed = res.Elements(SUBSYSTEM_ELEMENT_NAME).FirstOrDefault(_ => _.Attr("code")==code);
            if (null == existed) {
                existed =res.AddElement(SUBSYSTEM_ELEMENT_NAME, attributes: new {code, name});
            }
            else {
                if (!string.IsNullOrWhiteSpace(name)) {
                    existed.SetAttributeValue("name",name);
                }
            }
            return existed;
        }

        /// <summary>
        /// Имя класса
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Пространство имен
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Вывод до уровня блоков
        /// </summary>
        public bool BlocksOnly { get; set; }

        /// <summary>
        /// Псевдонимы подсистем
        /// </summary>
        public string SubsystemAliases { get; set; }

        /// <summary>
        /// Параметр исключения части рутов
        /// </summary>
        public string ExcludeRoots { get; set; }

        /// <summary>
        /// Опция отключения фильтра по статусам темы
        /// </summary>
        public bool DisableStatusFilter { get; set; }

       
    }
}