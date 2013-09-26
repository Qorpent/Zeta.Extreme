using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action("zdev.exportthemastructure", Arm = "dev", Help = "������������ ��������� ���� ��������� ���", Role = "DEVELOPER")]
    public class ExportThemaStructureAction : ExportActionBase<ThemaStructureExporter>
    {
    }
}