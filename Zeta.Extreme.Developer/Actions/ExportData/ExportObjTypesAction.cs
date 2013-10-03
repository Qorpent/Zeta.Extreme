using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action(DeveloperConstants.ExportObjtypesCommand, Arm = "dev", Help = "������������ ��������� ���� ����� ��������", Role = "DEVELOPER")]
    public class ExportObjTypesAction : ExportActionBase<ObjTypeExporter> { }
}