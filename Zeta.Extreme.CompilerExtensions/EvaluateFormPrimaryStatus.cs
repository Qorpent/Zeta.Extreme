using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Qorpent;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Utils.Extensions;
using Zeta.Extreme.Developer.MetaStorage.Tree;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Проверяет степень связанности форм по формулам и соответствие уровня первичности
    /// </summary>
    public class EvaluateFormPrimaryStatus : FormTaskBase {
        /// <summary>
        /// Индекс
        /// </summary>
        public const int INDEX = UpdateRowCodes.INDEX + 10;
        /// <summary>
        /// 
        /// </summary>
        public EvaluateFormPrimaryStatus() {
            Phase= BSharpBuilderPhase.Build;
            Index = INDEX;
        }
        /// <summary>
        /// Проверяет правильность назначения признака первичности и отбирает формы для дальнейшей проверки на соответствие первичности
        /// </summary>
        /// <param name="forms"></param>
        protected override void InitializeForms(IEnumerable<IBSharpClass> forms)
        {
            SelectedForms = new List<IBSharpClass>();
            foreach (var form in forms) {
                var hasprimary = form.Compiled.Descendants("primary").Any();
                if (!hasprimary) {
                    form.Compiled.SetAttributeValue("primarycompatibility", PrimaryCompatibility.NoPrimary);
                }
                else {
                    //резервируем значение по умолчанию
                    form.Compiled.SetAttributeValue("primarycompatibility", PrimaryCompatibility.Ignored);
                }
                var needprimary = form.Compiled.Attr("needprimary").ToBool();
                if (!hasprimary && needprimary) {
                    _context.RegisterError(new BSharpError{Level = ErrorLevel.Error, Message = string.Format("форма {0} отмечена как первичная, хотя не имеет первичных строк",form.Name)});
                    continue;
                }
                if (hasprimary) {
                    SelectedForms.Add(form);
                }
            }
        }
        /// <summary>
        /// Проверяет форму на уровень первичности
        /// </summary>
        /// <param name="compiled"></param>
        protected override void Execute(XElement compiled) {
            PrimaryCompatibility c = EvaluateCompatibility(compiled);
            compiled.SetAttributeValue("primarycompatibility",c);
           
            var needprimary = compiled.Attr("needprimary").ToBool();
            if (needprimary) {
                if (0 != (PrimaryCompatibility.ErrorPrimary & c))
                {
                    _context.RegisterError(new BSharpError { Level = ErrorLevel.Error, Message = string.Format("форма {0} имеет неверный статус первичности {1}", compiled.GetCode(), c) });
                }
                else if (0 != (PrimaryCompatibility.WarnPrimary & c))
                {
                    _context.RegisterError(new BSharpError { Level = ErrorLevel.Warning, Message = string.Format("форма {0} имеет ненадежный статус первичности {1}", compiled.GetCode(), c) });
                }
            }
            else {
                _context.RegisterError(new BSharpError { Level = ErrorLevel.Warning, Message = string.Format("форма {0} имеет первичные строки, но как первичная не отмечена, тип {1}", compiled.GetCode(),c) });    
            }
            
        }

        private PrimaryCompatibility EvaluateCompatibility(XElement compiled) {
            var bizcode = compiled.Attr("bizcode").Substring(0, 4);
            var subsystem = bizcode.Substring(0, 2);
            var formulas = compiled.Descendants("formula").ToArray();
            var refs = compiled.Descendants("ref").ToArray();
            var exrefs = compiled.Descendants().Where(_ => null != _.Attribute("exref")).ToArray();
            var grpsums = compiled.Descendants("sum").Where(_ => _.Attr("groups").ToBool()).ToArray();
            if (0 == formulas.Length && 0 == refs.Length && 0 == exrefs.Length && 0 == grpsums.Length) {
                return PrimaryCompatibility.FullPrimary;
            }
            var current = PrimaryCompatibility.SelfPrimary;
            foreach (var f in formulas) {
                var references = f.Attr("refs").SmartSplit();
                foreach (var reference in references) {
                    var refbizcode = reference.Substring(0, 4);
                    if (bizcode != refbizcode) {
                        var refss = refbizcode.Substring(0, 2);
                        if (refss != subsystem) {
                            return PrimaryCompatibility.CrossSubsystem;
                        }
                        current =PrimaryCompatibility.CrossBlock;
                    }
                }
            }

            foreach (var @ref in refs)
            {
                var refbizcode = @ref.Attr("ref").Substring(0, 4);
                if (bizcode != refbizcode)
                {
                    var refss = refbizcode.Substring(0, 2);
                    if (refss != subsystem)
                    {
                        {
                            return PrimaryCompatibility.CrossSubsystem;
                        }
                    }
                    current = PrimaryCompatibility.CrossBlock;
                }
            }
            foreach (var @ref in exrefs)
            {
                var refbizcode = @ref.Attr("exref").Substring(0, 4);
                if (bizcode != refbizcode)
                {
                    var refss = refbizcode.Substring(0, 2);
                    if (refss != subsystem)
                    {
                        {
                            return PrimaryCompatibility.CrossSubsystem;
                        }
                    }
                    current = PrimaryCompatibility.CrossBlock;
                }
            }

            return current;
        }
    }
}