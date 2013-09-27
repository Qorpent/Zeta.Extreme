using System.Collections.Generic;
using System.Linq;
using Qorpent.BSharp;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Экспорт таблицы BizProcess
    /// </summary>
    public class BizProcessExporter : IDataToBSharpExporter {
        private const string ElementName = "bizprocess";
        /// <summary>
        /// Формирует таблицу BizProcess
        /// </summary>
        /// <returns></returns>
        public string Generate() {

            var builder = new BSharpCodeBuilder();
            var query = "";
            if (PrimaryOnly) {
                query = " Tag like '%primary%'";
            }
            var processes = new NativeZetaReader().ReadBizProcesses(query);
            var bizProcesses = processes as Extreme.Model.BizProcess[] ?? processes.ToArray();
            builder.WriteCommentBlock("ЭКСПОРТ BIZPROCESS БД ECO",
                                      new Dictionary<string, object> {
                                          {"Время последего обновления",bizProcesses.Select(_=>_.Version).Max().ToString("yyyy-MM-dd HH:mm:ss")},
                                      });
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName, new { prototype = "meta" });
            builder.WriteElement(BSharpSyntax.ClassExportDefinition, ElementName);
            builder.WriteElement(BSharpSyntax.ClassElementDefinition, ElementName);
            
            foreach (var p in bizProcesses) {
                var tag = p.Tag;
                string badtag = null;
                IDictionary<string, string> tags = null;
                try {
                    tags = TagHelper.Parse(tag);
                }
                catch {
                    badtag = tag;
                }
                builder.WriteElement(ElementName,p.Code,p.Name,
                    inlineattributes: new {
                        inprocess = p.InProcess,
                        isfinal = p.IsFinal,
                        process = p.Process,
                        role = p.Role,
                        rootrows = p.RootRows,
                        badtag = badtag
                    },
                    linedattributes: tags
                    );
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

        /// <summary>
        /// Выгон только первичных процессов
        /// </summary>
        public bool PrimaryOnly { get; set; }
    }
}