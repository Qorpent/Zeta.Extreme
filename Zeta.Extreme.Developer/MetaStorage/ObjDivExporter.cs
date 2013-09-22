using System.Collections.Generic;
using System.Linq;
using Qorpent.BSharp;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Экспортер периодов
    /// </summary>
    public class ObjDivExporter : IDataToBSharpExporter
    {
        private const string ElementName = "objdiv";

        /// <summary>
        /// 
        /// </summary>
        public ObjDivExporter()
        {
            Namespace = "import";
            ClassName = "objdivs";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            var builder = new BSharpCodeBuilder();
            var divisions = new NativeZetaReader().ReadDivisions().ToArray();


            builder.WriteCommentBlock("ЭКСПОРТ ДИВИЗИОНОВ ИЗ БД ECO",
                                      new Dictionary<string, object> {
                                          {"Время последего обновления",divisions.Select(_=>_.Version).Max().ToString("yyyy-MM-dd HH:mm:ss")},
                                      });
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName, new { prototype = "meta" });
            builder.WriteElement(BSharpSyntax.ClassExportDefinition, ElementName);
            builder.WriteElement(BSharpSyntax.ClassElementDefinition, ElementName);


            foreach (var g in divisions)
            {
                
                Output(g, builder);

            }
          
            return builder.ToString();
        }
        /// <summary>
        /// Имя класса
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Пространство имен
        /// </summary>
        public string Namespace { get; set; }

        private void Output(Division period, BSharpCodeBuilder sb)
        {
            sb.WriteElement(ElementName,
                            period.Code,
                            period.Name,
                            inlineattributes:
                                new
                                {
                                    comment = period.Comment,
                                });
           

        }
    }
}