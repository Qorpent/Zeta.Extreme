using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action("zdev.exportcolumns", Arm = "dev", Help = "������������ ��������� ���� �������", Role = "DEVELOPER")]
    public class ExportColumnsAction : ExportActionBase<ColumnExporter> {
    }
}