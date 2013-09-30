using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.Analyzers;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��� ���������
    /// </summary>
    [Action(DeveloperConstants.ExportBizprocessesCommand, Arm = "dev", Help = "������������ ��������� ���� ������������", Role = "DEVELOPER")]
    public class ExportBizProcesses : ExportActionBase<BizProcessExporter> {
        [Bind]
        bool primaryonly { get; set; }
        /// <summary>
        /// ������������ �������� primaryonly
        /// </summary>
        /// <returns></returns>
        protected override BizProcessExporter InitializeExporter()
        {
            var result = base.InitializeExporter();
            result.PrimaryOnly = primaryonly;
            return result;
        }
    }
}