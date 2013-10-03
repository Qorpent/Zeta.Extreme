using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage;

namespace Zeta.Extreme.Developer.Actions {
    /// <summary>
	/// ������� ����������� ���� ��������
	/// </summary>
	[Action(DeveloperConstants.ExportPeriodsCommand,Arm="dev",Help="������������ ��������� ���� ��������", Role="DEVELOPER")]
    public class ExportPeriodsAction : ExportActionBase<PeriodsExporter>{}
}