using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action("zdev.exportobjdivs", Arm = "dev", Help = "������������ ��������� ���� ����������", Role = "DEVELOPER")]
    public class ExportObjDivsAction : ExportActionBase<ObjDivExporter> { }
}