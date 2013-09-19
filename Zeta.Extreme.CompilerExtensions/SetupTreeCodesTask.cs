using System.Xml.Linq;
using Qorpent.BSharp.Builder;
using Qorpent.Integration.BSharp.Builder.Tasks;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Адаптирует коды строк
    /// </summary>
    public class SetupTreeCodesTask : FormTaskBase {
        /// <summary>
        /// Индекс
        /// </summary>
        public const int INDEX = BuildZetaBizIndexTask.INDEX + 10;
        /// <summary>
        /// Формирует задачу посткомиляции для построения ZETA INDEX
        /// </summary>
        public SetupTreeCodesTask()
        {
            Phase = BSharpBuilderPhase.Build;
            Index = INDEX;
        }
        /// <summary>
        /// Выполняет установку полных и ДБ кодов к строкам
        /// </summary>
        /// <param name="compiled"></param>
        protected override void Execute(XElement compiled) {
            var root = GetRoot(compiled);
            if (null == root) return;
            var rootcode = compiled.Attr("formcode");
            var bizcode = compiled.Attr("bizcode");
            if (string.IsNullOrWhiteSpace(bizcode)) {
                bizcode ="0000"+ rootcode.ToUpper();
                compiled.SetAttributeValue("bizcode",bizcode);
            }
            
            
            ExecuteRow(compiled,root,rootcode,bizcode );
        }

        private void ExecuteRow(XElement src, XElement current, string rootcode, string bizcode) {
            var code = current.Attr("code");
            var dbcode = rootcode+code;
            if (code == rootcode) {
                dbcode = code;
            }
            current.SetAttributeValue("dbcode",dbcode);
            var rowbizcode = bizcode + "_" + code;
            current.SetAttributeValue("bizcode", rowbizcode);
            foreach (var c in current.Elements()) {
                ExecuteRow(src,c,rootcode,bizcode);
            }
        }
    }
}