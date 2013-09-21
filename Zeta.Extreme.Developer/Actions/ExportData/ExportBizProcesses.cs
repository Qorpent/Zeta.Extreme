using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��� ���������
    /// </summary>
    [Action("zdev.exportbizprocesses", Arm = "dev", Help = "������������ ��������� ���� ������������", Role = "DEVELOPER")]
    public class ExportBizProcesses : ExportActionBase<BizProcessExporter> { }
}