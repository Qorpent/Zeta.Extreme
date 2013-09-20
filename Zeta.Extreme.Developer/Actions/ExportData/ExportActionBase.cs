using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// Базовый класс действия для экспорта данных из БД в BSharp
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExportActionBase<T> :ActionBase where T:IDataToBSharpExporter,new() {
        /// <summary>
        /// Имя класса
        /// </summary>
        [Bind]
        public string ClassName { get; set; }
        /// <summary>
        /// Пространство имен
        /// </summary>
        [Bind(Default = "import")]
        public string Namespace { get; set; }

        private T _exporter;
        /// <summary>
        /// Подготавливает экспортер к вызову
        /// </summary>
        protected override void Prepare()
        {
            if (string.IsNullOrWhiteSpace(ClassName)) {
                ClassName = GetDefaultClassName();
            }
            if (string.IsNullOrWhiteSpace(Namespace)) {
                Namespace = "import";
            }
            _exporter = InitializeExporter();
            base.Prepare();
        }
        /// <summary>
        /// Метод формирования имени класса по умолчанию
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultClassName() {
            return GetType().Name.ToLower().Replace("export", "").Replace("action", "");
        }
        /// <summary>
        /// Инициализирует экспортер
        /// </summary>
        /// <returns></returns>
        protected virtual T InitializeExporter() {
            var result = new T {Namespace = Namespace, ClassName = ClassName};
            return result;
        }
        /// <summary>
        /// Возвращает результат экспорта
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            return _exporter.Generate();
        }
    }
}