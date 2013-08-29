using Qorpent.Mvc;
using Zeta.Extreme.Developer.MetaStorage.Periods;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// ������� ����������� ���� ��������
	/// </summary>
	[Action("zdev.exportperiods",Arm="dev",Help="������������ ��������� ���� ��������", Role="DEVELOPER")]
	public class ExportPeriods : ActionBase {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return new PeriodsExporter().GeneratePeriods();
		}
	}
}