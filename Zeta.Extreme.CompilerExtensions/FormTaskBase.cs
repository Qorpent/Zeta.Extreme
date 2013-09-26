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
            SelectedForms = null;
            Project.Log.Info("form task "+this.GetType().Name+" started");
            _context = context;
            AllForms = _context.ResolveAll("formdef");
            InitializeForms(AllForms);
            var selection = SelectedForms ?? AllForms;
            var parallel = selection.AsParallel().WithDegreeOfParallelism(4);
            parallel.ForAll(_=>Execute(_.Compiled));
            AfterFormProcessed(selection);
        }
        /// <summary>
        /// Выполняется по итогам выполнения операций
        /// </summary>
        /// <param name="selection"></param>
        protected virtual void AfterFormProcessed(IEnumerable<IBSharpClass> selection) {
            
        }

        /// <summary>
            /// Отобранные для работы формы
            /// </summary>
            protected IList<IBSharpClass> SelectedForms;
        /// <summary>
        /// Реестр всех форм
        /// </summary>
        protected IEnumerable<IBSharpClass> AllForms;

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