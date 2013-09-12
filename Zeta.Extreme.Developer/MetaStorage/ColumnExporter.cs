using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage
{
    /// <summary>
    /// Экспортер колонок
    /// </summary>
    public class ColumnExporter
    {
        private StringBuilder buffer;

        /// <summary>
        /// Формирует класс, содержащий описания колонок
        /// </summary>
        /// <param name="ns">Целевое пространство имен</param>
        /// <param name="cls">Имя класса</param>
        /// <param name="dicname">Имя экспортируемого справочника</param>
        /// <returns></returns>
        public string GenerateBSharp(string ns = "import", string cls= "columns", string dicname = "columns") {
            buffer = new StringBuilder();
            if (string.IsNullOrWhiteSpace(ns)) {
                ns = "import";
            }
            if (string.IsNullOrWhiteSpace(cls)) {
                cls = "columns";

            }
            if (string.IsNullOrWhiteSpace(dicname)) {
                dicname = "columns";
            }
            var columns = new NativeZetaReader().ReadColumns().OrderBy(_=>_.IsFormula).ThenBy(_=>_.Code).ToArray();
            InternalGenerateColumns(columns, ns, cls, dicname);
            return buffer.ToString();
        }

        private void InternalGenerateColumns(IEnumerable<Column> columns, string ns, string cls, string dicname) {
            WriteClassComment(columns);
            WriteClassHeader(ns, cls, dicname);
            foreach (var c in columns) {
                WriteColumn(c);
            }
        }

        private void WriteColumn(Column column) {
            buffer.AppendFormat("\t\t\tcolumn {0} '{1}'",column.Code,column.Name);
            if (!string.IsNullOrWhiteSpace(column.Measure)) {
                buffer.Append(" measure=" + column.Measure);
            }
            if (!string.IsNullOrWhiteSpace(column.Currency) && "NONE"!=column.Currency)
            {
                buffer.Append(" currency=" + column.Currency);
            }
            if (!string.IsNullOrWhiteSpace(column.MarkCache)) {
                foreach (var m in column.MarkCache.SmartSplit()) {
                    buffer.Append(" " + m.ToLower());
                }
            }
            if (!string.IsNullOrWhiteSpace(column.Tag)) {
                var dict = TagHelper.Parse(column.Tag);
                foreach (var d in dict) {
                    if (string.IsNullOrEmpty(d.Value)) {
                        buffer.Append(" " + d.Key);
                    }
                    else if (d.Value.All(char.IsLetterOrDigit)) {
                        buffer.AppendFormat(" {0}={1}", d.Key, d.Value);
                    }
                    else {
                        buffer.AppendFormat(" {0}=\"\"\"{1}\"\"\"", d.Key, d.Value);
                    }
                }
            }
            if (column.IsFormula) {
                if (column.FormulaType != "boo") {
                    if (column.FormulaType.Contains(",")) {
                        buffer.AppendFormat(" formulatype='ERROR:{0}'", column.Formula);
                    }
                    else {
                        buffer.AppendFormat(" formulatype={0}", column.Formula);
                    }
                }
                buffer.AppendLine(" formula = (");
                buffer.Append("\t\t\t\t");
                buffer.Append(column.Formula);
                buffer.AppendLine();
                buffer.Append("\t\t\t)");
            }
            buffer.AppendLine();
        }

        private void WriteClassHeader(string ns, string cls, string dicname) {
            buffer.AppendLine("namespace " + ns );
            buffer.AppendLine("\tclass "+ cls+" prototype='meta'");
            buffer.AppendLine("\t\texport " + dicname);
            buffer.AppendLine("\t\telement column");
        }

        private void WriteClassComment(IEnumerable<Column> columns) {
            buffer.AppendLine("######################################################################################");
            buffer.AppendLine("#### Экспорт колонок из БД");
            buffer.AppendLine("#### Последняя версия: " + columns.Max(_ => _.Version).ToString("yyyy-MM-dd HH:mm:ss"));
            buffer.AppendLine("######################################################################################");
        }
    }
}
