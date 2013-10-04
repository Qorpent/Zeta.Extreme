using System;
using System.Linq;
using System.Xml.Linq;
using Qorpent;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Serialization;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Расширение проброса блоков и подсистем в форме по стандартному ThemaIndex
    /// </summary>
    public class SetupBlockExtensions : IBSharpCompilerExtension {
        /// <summary>
        /// Признак активности расширения
        /// </summary>
        public bool Active = false;
        /// <summary>
        /// Исходный класс
        /// </summary>
        public string From ;
        /// <summary>
        /// Целевой прототип
        /// </summary>
        public string To;

        private XElement _source = null;
        private bool _isSourceSetUp =false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public SetupBlockExtensions(SetupBlockExtensions parent) {
            From = parent.From;
            To = parent.To;
            _source = parent._source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        public SetupBlockExtensions(IBSharpProject project) {
            var mye = project.SrcClass.Compiled.Element("Zeta.BlockSetup");
            if (null != mye) {
                Active = true;
                From = mye.Attr("from");
                To = mye.Attr("into");
            }

        }
        /// <summary>
        /// Выполняет пересчет бизнес-кодов подсистем и блоков и пробрасывает их в целевые формы
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="context"></param>
        /// <param name="cls"></param>
        /// <param name="phase"></param>
        public void Execute(IBSharpCompiler compiler, IBSharpContext context, IBSharpClass cls, BSharpCompilePhase phase) {
            
            if (phase == BSharpCompilePhase.PreSimpleInclude && !cls.Is(BSharpClassAttributes.Abstract) && cls.Prototype == To ) {
                if (!_isSourceSetUp)
                {
                    lock (this)
                    {
                        if (!_isSourceSetUp) {
                            SetupSource(context, compiler);
                        }
                    }
                }    
               new SetupBlockExtensions(this).InternalExecute(compiler, context, cls);
            }
        }

        private void InternalExecute(IBSharpCompiler compiler, IBSharpContext context, IBSharpClass cls) {
            if (null == _source) return;
            var xml = cls.Compiled;
            var formcode = xml.Attr("formcode");
            if (string.IsNullOrWhiteSpace(formcode)) {
                context.RegisterError(new BSharpError {
                    Level = ErrorLevel.Error,
                    Message = "Form has not formcode " + cls.FullName
                });
                return;
            }
            if (formcode.Length != 4 || !formcode.IsLiteral(EscapingType.JsonLiteral)) {
                context.RegisterError(new BSharpError {
                    Level = ErrorLevel.Error,
                    Message = "Form formcode not match format" + cls.FullName
                });
                return;
            }
            var root = _source.Descendants("root").FirstOrDefault(_ => _.GetCode() == formcode);
            if (null == root) {
                context.RegisterError(new BSharpError {
                    Level = ErrorLevel.Warning,
                    Message = "Form not registered in blocks" + cls.FullName
                });
                return;
            }


            var bizcode = formcode.ToUpper();
            var block = root.Parent;
            if (string.IsNullOrWhiteSpace(block.Attr("bizcode"))) {
                return;
            }
            var subsystem = block.Parent;
            if (string.IsNullOrWhiteSpace(subsystem.Attr("bizcode"))) {
                return;
            }
            bizcode = subsystem.Attr("bizcode") + block.Attr("bizcode") + formcode.ToUpper();
            xml.SetAttributeValue("bizcode", bizcode);
            xml.SetAttributeValue("subsystem", subsystem.Attr("bizcode"));
            xml.SetAttributeValue("block", block.Attr("bizcode"));
            xml.SetAttributeValue("subsystemcode", subsystem.GetCode());
            xml.SetAttributeValue("blockcode", block.GetCode());

            var thema = root.Elements("thema").FirstOrDefault(_ => null == _.Attribute("noprimary"));
            if (null == thema) {
                context.RegisterError(new BSharpError {
                    Level = ErrorLevel.Hint,
                    Message = "No default thema found for form" + cls.FullName
                });
                return;
            }
            xml.SetAttributeValue("defaultformthema", thema.GetCode());
        }

        /// <summary>
        /// Настраивает индекс блоков
        /// </summary>
        /// <param name="context"></param>
        /// <param name="compiler"></param>
        private void SetupSource(IBSharpContext context, IBSharpCompiler compiler) {
            _isSourceSetUp = true;
            var _sourceCls = context.Get(From);
            if (null == _sourceCls) {
                context.RegisterError(new BSharpError {
                    Level = ErrorLevel.Error,
                    Message = "Cannot find source themaindex " + From
                });
            }
            else {
                BSharpClassBuilder.Build(BuildPhase.Compile, compiler, _sourceCls, context);
                _source = _sourceCls.Compiled;    
                foreach (var e in _source.Descendants()) {
                    if (e.Name.LocalName == "subsystem" || e.Name.LocalName == "block") {
                        var bizcode = e.Attr("bizcode");
                        if (string.IsNullOrWhiteSpace(bizcode)) {
                            context.RegisterError(new BSharpError { Level = ErrorLevel.Error, Message = "No bizcode for " + e.Name.LocalName+" "+e.GetCode() });
                        }
                        else if (bizcode.Length != 2 || !bizcode.IsLiteral(EscapingType.JsonLiteral)) {
                            context.RegisterError(new BSharpError { Level = ErrorLevel.Error, Message = "Invalid bizcode "+bizcode+" for " + e.Name.LocalName + " " + e.GetCode() });
                        }
                    }
                }
            }
            
        }
    }
}