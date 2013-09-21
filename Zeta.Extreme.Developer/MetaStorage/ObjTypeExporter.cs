using System.Collections.Generic;
using System.Linq;
using Qorpent.BSharp;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Экспорт таблицы BizProcess
    /// </summary>
    public class ObjTypeExporter : IDataToBSharpExporter
    {
        private const string TypeElementName = "type";
        private const string ClsElementName = "cls";
        /// <summary>
        /// Формирует таблицу BizProcess
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {

            var builder = new BSharpCodeBuilder();
            var classes = new NativeZetaReader().ReadObjClasses().ToDictionary(_=>_.Id,_=>_);
            var types = new NativeZetaReader().ReadObjTypes();
            foreach (var ot in types) {
                var cls = classes[ot.ClassId.Value];
                if (null == cls.Types) {
                    cls.Types = new List<IObjectType>();
                }
                cls.Types.Add(ot);
#pragma warning disable 612,618
                ot.Class = cls;
#pragma warning restore 612,618
            }
            
            builder.WriteCommentBlock("ЭКСПОРТ Типов объектов БД ECO",
                                      new Dictionary<string, object> {
                                          {"Время последего обновления классов",classes.Values.Select(_=>_.Version).Max().ToString("yyyy-MM-dd HH:mm:ss")},
                                          {"Время последего обновления типов",types.Select(_=>_.Version).Max().ToString("yyyy-MM-dd HH:mm:ss")},
                                      });
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName, new { prototype = "meta" });
            builder.WriteElement(BSharpSyntax.ClassExportDefinition, ClsElementName);
            builder.WriteElement(BSharpSyntax.ClassElementDefinition, ClsElementName);


            foreach (var p in classes.Values)
            {
                WriteClass(p, builder);
            }
            return builder.ToString();
        }

#pragma warning disable 612,618
        private static void WriteClass(ObjectClass p, BSharpCodeBuilder builder) {
#pragma warning restore 612,618
            var tag = p.Tag;
            string badtag = null;
            IDictionary<string, string> tags = null;
            try {
                tags = TagHelper.Parse(tag);
            }
            catch {
                badtag = tag;
            }
            builder.StartElement(ClsElementName, p.Code, p.Name,
                                 inlineattributes: new {
                                     comment = p.Comment,
                                     badtag,
                                 },
                                 linedattributes: tags
                );
            if (null != p.Types) {
                foreach (var t in p.Types) {
                    WriteType(builder, t, tag);
                }
            }
            builder.EndElement();
        }

        private static void WriteType(BSharpCodeBuilder builder, IObjectType t, string tag) {
            var ttag = t.Tag;
            string tbadtag = null;
            IDictionary<string, string> ttags = null;
            try {
                ttags = TagHelper.Parse(tag);
            }
            catch {
                tbadtag = ttag;
            }
            builder.WriteElement(TypeElementName, t.Code, t.Name,
                                 inlineattributes: new {
                                     comment = t.Comment,
                                     badtag = tbadtag
                                 },
                                 linedattributes: ttags
                );
        }

        /// <summary>
        /// Имя класса
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Пространство имен
        /// </summary>
        public string Namespace { get; set; }
    }
}