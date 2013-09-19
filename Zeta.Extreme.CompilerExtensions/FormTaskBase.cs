using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Qorpent.BSharp;
using Qorpent.BSharp.Builder;
using Qorpent.Utils.Extensions;

namespace Zeta.Extreme.CompilerExtensions {
    /// <summary>
    /// Базис для задач над формами
    /// </summary>
    public abstract class FormTaskBase : BSharpBuilderTaskBase {
        /// <summary>
        /// Текущий контекст
        /// </summary>
        protected IBSharpContext _context;
        /// <summary>
        /// Имена элементов со строками
        /// </summary>
        protected static string[] rownames = new[] {"title", "sum", "primary", "formula", "controlpoint","ref"};

        /// <summary>
        /// Устанавливает полные коды строк
        /// </summary>
        /// <param name="context"></param>
        public override void Execute(IBSharpContext context) {
            Project.Log.Info("form task "+this.GetType().Name+" started");
            _context = context;
            var forms = _context.ResolveAll("formdef",null);
            InitializeForms(forms);
            var parallel = forms.AsParallel().WithDegreeOfParallelism(4);
            parallel.DoForEach(_ => Execute(_.Compiled));
            
        }
        /// <summary>
        /// Метод, позволяющий подготовить задачу для выполнения в целом по формам
        /// </summary>
        /// <param name="forms"></param>
        protected virtual void InitializeForms(IEnumerable<IBSharpClass> forms) {
            
        }

        /// <summary>
        /// Выполняется относитеьно XML класса формы
        /// </summary>
        /// <param name="compiled"></param>
        protected abstract void Execute(XElement compiled);
        /// <summary>
        /// Нахождение элемента с корнем
        /// </summary>
        /// <param name="compiled"></param>
        /// <returns></returns>
        protected XElement GetRoot(XElement compiled) {
            return compiled.Elements().FirstOrDefault(_ => -1 != Array.IndexOf(rownames, _.Name.LocalName));
        }
    }
}