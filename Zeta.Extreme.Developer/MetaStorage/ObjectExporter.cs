using System.Collections.Generic;
using System.Linq;
using Qorpent.BSharp;
using Zeta.Extreme.Model;
using Zeta.Extreme.Model.Deprecated;
using Zeta.Extreme.Model.SqlSupport;

namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Эксортер старших объектов
    /// </summary>
    public class ObjectExporter : IDataToBSharpExporter {
#pragma warning disable 612,618

        private BSharpCodeBuilder _builder;
        private Dictionary<int, Point> _points;
        private Dictionary<int, ObjectType> _objtypes;
        private Dictionary<int, Division> _divs;
        private Dictionary<int, Department> _deps;
        private IEnumerable<Obj> _objs;
        private Dictionary<int, Obj[]> _parentedObjects;
        private IEnumerable<Obj> _roots;
#pragma warning restore 612,618

        /// <summary>
        /// Признак использования внешних организаций
        /// </summary>
        public bool UseOutOrganization { get; set; }

        /// <summary>
        /// Признак использования внешних организаций
        /// </summary>
        public bool OnlyOwnOnRoot { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObjectExporter()
        {
            Namespace = "import";
            ClassName = "objects";
        }
        /// <summary>
        /// 
        /// </summary>
        public const string ObjElement = "obj";
        /// <summary>
        /// Формирует реестр старших объектов
        /// </summary>
        /// <returns></returns>
        public string Generate() {
            _builder = new BSharpCodeBuilder();
            var reader = new NativeZetaReader();
            _points = reader.ReadPoints().ToDictionary(_=>_.Id,_=>_);
            _objtypes = reader.ReadObjTypes().ToDictionary(_ => _.Id, _ => _);
            _divs = reader.ReadDivisions().ToDictionary(_ => _.Id, _ => _);
            _deps = reader.ReadDepartments().ToDictionary(_ => _.Id, _ => _);
            var cond = "TypeId is not null ";
            if (!UseOutOrganization) {
                cond += " and TypeId != (select id from zeta.normalobjtype where code = 'OUTORG')";
            }
            _objs = reader.ReadObjects(cond);
            _parentedObjects = _objs.Where(_ => _.ParentId != null)
                                    .GroupBy(_ => _.ParentId.Value)
                                    .ToDictionary(_ => _.Key, _ => _.ToArray());
            _roots = _objs.Where(_ => _.ParentId == null);
            var version = _objs.Select(_ => _.Version).Max();
            _builder.WriteCommentBlock("Экспорт старших объектов",new {version});
            _builder.StartNamespace(Namespace);
            _builder.StartClass(ClassName);
            _builder.WriteClassExport("obj");
            _builder.WriteClassElement("obj");
            foreach (var r in _roots) {
                WriteObject(r,true);
            }
            return _builder.ToString();
        }

        private void WriteObject(Obj o, bool isroot = false) {
            string type = null;
            if (null != o.ObjTypeId) type = _objtypes[o.ObjTypeId.Value].Code;
            string div = null;
            if (null != o.DivisionId) div = _divs[o.DivisionId.Value].Code;
            string dep = null;
            if (null != o.DepartmenId) dep = _deps[o.DepartmenId.Value].Code;
#pragma warning disable 612,618
            string point = null;
#pragma warning restore 612,618
            if (null != o.PointId) point = _points[o.PointId.Value].Code;
            if (isroot && OnlyOwnOnRoot) {
                if (type != "OWNORG") return;
            }
            _builder.StartElement(ObjElement,o.Id.ToString(),o.Name,inlineattributes:
            new{
               dbcode = o.Code,
               comment= o.Comment,
               tag = o.Tag,
               type,div,dep,point
            });
            if (_parentedObjects.ContainsKey(o.Id)) {
                foreach (var c in _parentedObjects[o.Id])
                {
                    WriteObject(c);
                }    
            }
            
            _builder.EndElement();
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