using System.Linq;
using Qorpent.BSharp;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Экспорт географических объектов
    /// </summary>
    public class GeoExporter : IDataToBSharpExporter {
        private const string ZoneElement = "zone";
        private const string RegionElement = "region";
        private const string PointElement = "point";
        /// <summary>
        /// 
        /// </summary>
        public GeoExporter() {
            Namespace = "import";
            ClassName = "geo";
        }
        /// <summary>
        /// Формирует 3-х уровневое дерево географии
        /// </summary>
        /// <returns></returns>
        public string Generate() {
            var builder = new BSharpCodeBuilder();
            var reader = new NativeZetaReader();
            var zones = reader.ReadZones();
            var regions = reader.ReadRegions();
            var points = reader.ReadPoints();
            var version = new[]{zones.Select(_=>_.Version).Max(),regions.Select(_=>_.Version).Max(),points.Select(_=>_.Version).Max()}.Max();
            builder.WriteCommentBlock("Объекты географии Zeta", new {version});
            builder.StartNamespace(Namespace);
            builder.StartClass(ClassName);
            builder.WriteClassElement(PointElement);
            foreach (var z in zones) {
                builder.StartElement(ZoneElement,z.Code,z.Name,inlineattributes:new{comment=z.Comment,tag=z.Tag});
                foreach (var r in regions.Where(_ => _.ZoneId == z.Id)) {
                    builder.StartElement(RegionElement, r.Code, r.Name, inlineattributes: new { comment = r.Comment, tag = r.Tag });
                    foreach (var p in points.Where(_ => _.RegionId == r.Id)) {
                        builder.WriteElement(PointElement, p.Code, p.Name, inlineattributes: new { comment = p.Comment, tag = p.Tag });
                    }
                    builder.EndElement();
                }
                builder.EndElement();
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

    }
}