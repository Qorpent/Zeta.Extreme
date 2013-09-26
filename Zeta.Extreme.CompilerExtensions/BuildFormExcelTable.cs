using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Integration.BSharp.Builder.Tasks;
using Qorpent.Utils.Extensions;
using Qorpent.Utils.Extensions.Html;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Проверяет степень связанности форм по формулам и соответствие уровня первичности
    /// </summary>
    public class BuildFormExcelTable : FormTaskBase {
        private XElement _xls;
        private XElement _table;

        /// <summary>
        /// Индекс
        /// </summary>
        public const int INDEX = TaskConstants.WriteWorkingOutputTaskIndex + 10;

        private const string TITLE = "Отчет о настройках форм ввода";

        /// <summary>
        /// 
        /// </summary>
        public BuildFormExcelTable()
        {
            Phase = BSharpBuilderPhase.PostProcess;
            Index = INDEX;
        }
        /// <summary>
        /// Подготавливает XElement для записи в него данных форм
        /// </summary>
        /// <param name="forms"></param>
        protected override void InitializeForms(IEnumerable<IBSharpClass> forms) {
            _xls = XmlHtmlExtensions.CreateNewHtml();
            _xls.HtmlAddDefaultStyles();
            _xls.HtmlSetDocumentTitle(TITLE);
            _xls.HtmlAddHead1(TITLE);
            _table = _xls.HtmlAddTable(
                cls : "forminfo",
                head : new {
                    number = "№",
                    thema = "Код темы формы",
                    root = "Корень",
                    block = "Блок",
                    subsystem = "Подсистема",
                    mstatus = "Вмененный статус",
                    rstatus = "Реальный статус",
                    oldgroup = "Группа (устар)",
                    oldroleth = "Префикс роли (по th)",
                    oldrole = "Префикс роли (по biz)",
                    
                }
             );
        }

        private volatile int _number = 1;
        /// <summary>
        /// Записывает на диск таблицусведений о формах
        /// </summary>
        /// <param name="compiled"></param>
        protected override void Execute(XElement compiled) {
            var bizcode = compiled.Attr("bizcode");
            _table.HtmlAddTableRow(cells:
            new object[] {
                _number++,
                compiled.Attr("oldthemacode"),
                compiled.Attr("formcode"),
                bizcode.Substring(2,2),
                bizcode.Substring(0,2),
                compiled.Attr("usingtype"),
                compiled.Attr("primarycompatibility"),
                compiled.Attr("oldgroup"),
                compiled.Attr("oldroleth"),
                compiled.Attr("oldrole"),           
            });
        }

        /// <summary>
        /// Выполняет запись HTML таблицы с расширением XLS
        /// </summary>
        /// <param name="selection"></param>
        protected override void AfterFormProcessed(IEnumerable<IBSharpClass> selection) {
            var path = Path.Combine(Project.GetOutputDirectory(),"zeta.forminfo.xls");
            File.WriteAllText(path,_xls.ToString());
        }
    }
}