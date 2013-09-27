using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
    /// ������� ����������� ���� ��������
    /// </summary>
    [Action(DeveloperConstants.ExportGeoCommand, Arm = "dev", Help = "������������ ��������� ���� ��������������� ���������", Role = "DEVELOPER")]
    public class ExportGeoAction : ExportActionBase<GeoExporter> { }
}