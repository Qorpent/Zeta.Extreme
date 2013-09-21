using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action("zdev.exportobjtypes", Arm = "dev", Help = "������������ ��������� ���� ����� ��������", Role = "DEVELOPER")]
    public class ExportObjTypesAction : ExportActionBase<ObjTypeExporter> { }
}