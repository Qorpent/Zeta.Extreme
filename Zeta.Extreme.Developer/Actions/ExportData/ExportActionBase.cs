using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����� �������� ��� �������� ������ �� �� � BSharp
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExportActionBase<T> :ActionBase where T:IDataToBSharpExporter,new() {
        /// <summary>
        /// ��� ������
        /// </summary>
        [Bind]
        public string ClassName { get; set; }
        /// <summary>
        /// ������������ ����
        /// </summary>
        [Bind(Default = "import")]
        public string Namespace { get; set; }

        private T _exporter;
        /// <summary>
        /// �������������� ��������� � ������
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
        /// ����� ������������ ����� ������ �� ���������
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDefaultClassName() {
            return GetType().Name.ToLower().Replace("export", "").Replace("action", "");
        }
        /// <summary>
        /// �������������� ���������
        /// </summary>
        /// <returns></returns>
        protected virtual T InitializeExporter() {
            var result = new T {Namespace = Namespace, ClassName = ClassName};
            return result;
        }
        /// <summary>
        /// ���������� ��������� ��������
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            return _exporter.Generate();
        }
    }
}