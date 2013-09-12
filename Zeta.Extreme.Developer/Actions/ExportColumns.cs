using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action("zdev.exportcolumns", Arm = "dev", Help = "������������ ��������� ���� �������", Role = "DEVELOPER")]
    public class ExportColumns : ActionBase
    {

        /// <summary>
        /// ��� ������
        /// </summary>
        [Bind(Default = "columns")]
        public string ClassName { get; set; }
        /// <summary>
        /// ������������ ����
        /// </summary>
        [Bind(Default = "import")]
        public string Namespace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            return new ColumnExporter().GenerateBSharp(Namespace, ClassName);
        }
    }
}