using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Qorpent.BSharp;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage
{
    /// <summary>
	/// Экспортер периодов
	/// </summary>
	public class PeriodsExporter : IDataToBSharpExporter {
        private const string ElementName = "period";

        /// <summary>
		/// 
		/// </summary>
		public PeriodsExporter() {
			Namespace = "import";
			ClassName = "periods";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string Generate() {
		    var builder = new BSharpCodeBuilder();
			var srcperiods = new NativeZetaReader().ReadPeriods().ToArray();
			var primary = srcperiods.Where(_ =>! _.IsFormula).ToArray();
			var formulas = srcperiods.Where(_ => _.IsFormula).ToArray();
			var pgroups = primary.GroupBy(_ => _.Category);

            builder.WriteCommentBlock("ЭКСПОРТ ПЕРИОДОВ ИЗ БД ECO",
                new Dictionary<string,object> {
                    {"Время последего обновления",srcperiods.Select(_=>_.Version).Max().ToString("yyyy-MM-dd HH:mm:ss")},
                });
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName,new{prototype="meta-periods"});
            builder.WriteElement(BSharpSyntax.ClassExportDefinition,ElementName);
            builder.WriteElement(BSharpSyntax.ClassElementDefinition,ElementName);

		
			foreach (var g in pgroups) {
                builder.StartElement(BSharpSyntax.GroupedBlock,inlineattributes:new{category=g.Key});
				foreach (var gp in g.OrderBy(_=>_.BizId)) {
					Output(gp,builder);
				}
                builder.EndElement();
			}
            builder.StartElement(BSharpSyntax.GroupedBlock, inlineattributes: new { isfromula = true });
			foreach (var p in formulas.OrderBy(_=>_.BizId))
			{
				Output(p, builder);
			}
            builder.EndElement();
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

		private void Output(Period period,BSharpCodeBuilder sb) {
            sb.StartElement(ElementName,
                period.BizId.ToString(CultureInfo.InvariantCulture),
                period.Name,
                inlineattributes:
                new {
                    @short=period.ShortName,
                    months=period.MonthCount,
                    start = period.StartDate,
                    finish=period.EndDate,
                });
            if (period.IsFormula && !string.IsNullOrWhiteSpace(period.Formula)) {
                sb.WriteAttributesLined(new{formula=period.Formula});
            } 
            sb.EndElement();
		}
	}
}
