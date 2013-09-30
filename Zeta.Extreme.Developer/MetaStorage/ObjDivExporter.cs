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
        private const string ObjDivElementName = "objdiv";
        private const string DepartmentElementName = "objdep";

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
            var departments = new NativeZetaReader().ReadDepartments().ToArray();
            var ver = divisions.Select(_ => _.Version).Max();
            var ver2 = departments.Select(_ => _.Version).Max();
            if (ver2 > ver) {
                ver = ver2;
            }
            builder.WriteCommentBlock("ЭКСПОРТ ДИВИЗИОНОВ ИЗ БД ECO",
                                      new Dictionary<string, object> {
                                          {"Время последего обновления",ver.ToString("yyyy-MM-dd HH:mm:ss")},
                                      });
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName, new { prototype = "meta" });
            builder.WriteElement(BSharpSyntax.ClassExportDefinition, ObjDivElementName);
            builder.WriteElement(BSharpSyntax.ClassElementDefinition, ObjDivElementName);


            foreach (var g in divisions)
            {
                
                Output(g, builder);

            }
            foreach (var d in departments)
            {

                Output(d, builder);

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

        private void Output(Division div, BSharpCodeBuilder sb)
        {
            sb.WriteElement(ObjDivElementName,
                            div.Code,
                            div.Name,
                            inlineattributes:
                                new
                                {
                                    comment = div.Comment,
                                });
           

        }
        private void Output(Department dep, BSharpCodeBuilder sb)
        {
            sb.WriteElement(DepartmentElementName,
                            dep.Code,
                            dep.Name,
                            inlineattributes:
                                new
                                {
                                    comment = dep.Comment,
                                });


        }
    }
}