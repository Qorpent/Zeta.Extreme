using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Qorpent;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Приводит формулы к полному коду строки
    /// </summary>
    public class AccomodateFormulaRows :FormTaskBase
    {
        /// <summary>
        /// Индекс
        /// </summary>
        public const int INDEX = SetupTreeCodesTask.INDEX + 10;
        /// <summary>
        /// Формирует задачу посткомиляции для построения ZETA INDEX
        /// </summary>
        public AccomodateFormulaRows()
        {
            Phase = BSharpBuilderPhase.Build;
            Index = INDEX;
        }
        IDictionary<string,string> _globalIndex = new Dictionary<string, string>(); 
        /// <summary>
        /// Формирует полный индекс кодов
        /// </summary>
        /// <param name="forms"></param>
        protected override void InitializeForms(IEnumerable<IBSharpClass> forms)
        {
            foreach (var form in forms.AsParallel().WithDegreeOfParallelism(3)) {
                 foreach (var r in form.Compiled.Descendants()) {
                    if (null != r.Attribute("dbcode")) {
                        var bizcode = r.Attr("bizcode");
                        _globalIndex[r.Attr("dbcode")] = bizcode;
                        _globalIndex[r.Attr("bizcode")] = bizcode;
                       }
                 }
            }
            Project.Set("rowcodeindex",_globalIndex);
            Project.Log.Info(_globalIndex.Count/2+" rows indexed");
        }
        /// <summary>
        /// Конвертирует все коды строк в формулах к полным кодам
        /// </summary>
        /// <param name="compiled"></param>
        protected override void Execute(XElement compiled) {
            var formulas = compiled.Descendants("formula").ToArray();
           var localidx = new Dictionary<string, string>();
            foreach (var r in compiled.Descendants()) {
                if (null != r.Attribute("dbcode")) {
                    var bizcode = r.Attr("bizcode");
                    localidx[r.GetCode()] = bizcode;                    
                }
            }
            foreach (var f in formulas) {
                if (null != f.Attribute("noinput")) {
                    continue;
                }
                if (null == f.Attribute("formula")) {
                    f.Value = AccomodateFormula(f, f.Value, localidx);
                }
                else {
                    f.SetAttributeValue("formula", AccomodateFormula(f, f.Attr("formula"), localidx));
                }
            }
        }

        private string AccomodateFormula(XElement src,string formula, Dictionary<string, string> nameidx) {
            return Regex.Replace(formula, @"\$([^.?@\s]+)",
                                 m => {
                                     var code = m.Groups[1].Value;
                                     if (code.StartsWith("__")) return "$"+code;
                                     return "$" + AccomodateCode(src, m.Groups[1].Value, nameidx);
                                 });
        }

        private string AccomodateCode(XElement src,string value, Dictionary<string, string> localindex) {
            if (localindex.ContainsKey(value)) return localindex[value];
            if (_globalIndex.ContainsKey(value)) return _globalIndex[value];
            _context.RegisterError(new BSharpError{Level = ErrorLevel.Error,Xml=src, Message = "Формула "+src.Attr("bizcode")+" ссылается на недопустимый код "+value});
            src.SetAttributeValue("invalidformula",true);
            return "NOTRESOLVED_" + value;
        }
    }
}