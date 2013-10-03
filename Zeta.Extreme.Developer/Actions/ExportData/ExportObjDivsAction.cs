using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action(DeveloperConstants.ExportObjdivsCommand, Arm = "dev", Help = "������������ ��������� ���� ����������", Role = "DEVELOPER")]
    public class ExportObjDivsAction : ExportActionBase<ObjDivExporter> { }
}