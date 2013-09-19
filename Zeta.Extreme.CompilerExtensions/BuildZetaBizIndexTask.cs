using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Qorpent;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Integration.BSharp.Builder.Tasks;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Задача для
    /// </summary>
    public class BuildZetaBizIndexTask : BSharpBuilderTaskBase
    {
        private IBSharpContext _context;
        /// <summary>
        /// Индекс
        /// </summary>
        public const int INDEX = TaskConstants.CompileBSharpTaskIndex + 10;
        /// <summary>
        /// Формирует задачу посткомиляции для построения ZETA INDEX
        /// </summary>
        public BuildZetaBizIndexTask() {
            Phase = BSharpBuilderPhase.Build;
            Index = INDEX;
        }

        /// <summary>
        /// Трансформирует классы прототипа BIZINDEX в полноценные карты соотношения тем, блоков, подсистем
        /// </summary>
        /// <param name="context"></param>
        public override void Execute(IBSharpContext context) {
            _context = context;
            Project.Log.Info("BuildZetaBizIndexTask called");
            var bizindexes = context.ResolveAll("bizindex", null);
            foreach (var bSharpClass in bizindexes) {
                bSharpClass.Compiled.ReplaceNodes(CreateBizIndex(bSharpClass.Compiled));
                Project.Log.Info(bSharpClass.FullName+" processed");   
            }
        }

        private IEnumerable<XElement> CreateBizIndex(XElement src) {
            foreach (var subsystem in src.XPathSelectElements("./*[@prototype='subsystem']").ToArray()) {
                yield return BuildSubSystem(subsystem, src);
            }
        }

        private XElement BuildSubSystem(XElement subsystem, XElement src) {
            var result = new XElement(subsystem);
            var code = result.Name.LocalName;
            result.SetAttributeValue("class",code);
            result.Name = "subsystem";
            var bizcode = result.Attr("bizcode");
            if (string.IsNullOrWhiteSpace(bizcode)) {
                _context.RegisterError(new BSharpError{Level = ErrorLevel.Error,Message = "Подсистема "+code+" не имеет бизнес кода"});
                bizcode = "00";
                result.SetAttributeValue("bizcode",bizcode);
            }

            var blocks = src.XPathSelectElements("./*[@subsystem='"+subsystem.Name.LocalName+"']").ToArray();
            foreach (var block in blocks) {
                result.Add(BuildBlock(block,src,bizcode));
            }
            return result;
        }

        private XElement BuildBlock(XElement block, XElement src, string sysbizcode) {
            var result = new XElement(block);
            var code = result.Name.LocalName;
            result.SetAttributeValue("class", code);
            result.Name = "block";
            var bizcode = result.Attr("bizcode");
            if (string.IsNullOrWhiteSpace(bizcode))
            {
                _context.RegisterError(new BSharpError { Level = ErrorLevel.Error, Message = "Блок " + code + " не имеет бизнес кода" });
                bizcode = "00";
                result.SetAttributeValue("bizcode",bizcode);
            }
            var fullbizcode = sysbizcode + bizcode;
            result.SetAttributeValue("fullbizcode",fullbizcode);

            BuildForms(block, src, result,fullbizcode);
            return result;

        }

        private  void BuildForms(XElement block, XElement src, XElement result,string blockbizcode) {
            var themas =
                src.XPathSelectElements("./*[@prototype='formdef' and @block='" + block.Name.LocalName + "']").ToArray();
            foreach (var t in themas) {
                var form = new XElement(t);
                var formcode = form.Name.LocalName;
                var owncode = formcode.Split('.').Last();
                form.SetAttributeValue("class", formcode);
                form.SetAttributeValue("bizcode",owncode );
                form.SetAttributeValue("thematype", "primary");
                var formclass = _context.Get(formcode);
                formclass.Compiled.SetAttributeValue("bizcode",blockbizcode+owncode.ToUpper());
                form.Name = "thema";
                form.SetAttributeValue("code",owncode+"_prim");
                result.Add(form);
            }
        }
    }
}