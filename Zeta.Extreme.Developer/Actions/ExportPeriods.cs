using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.Developer.MetaStorage.Periods;

namespace Zeta.Extreme.Developer.Actions {
	/// <summary>
	/// ������� ����������� ���� ��������
	/// </summary>
	[Action("zdev.exportperiods",Arm="dev",Help="������������ ��������� ���� ��������", Role="DEVELOPER")]
	public class ExportPeriods : ActionBase {

		/// <summary>
		/// ��� ������
		/// </summary>
		[Bind(Default="periods")]
		public string ClassName { get; set; }
		/// <summary>
		/// ������������ ����
		/// </summary>
		[Bind(Default="import")]
		public string Namespace { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return new PeriodsExporter{Namespace = Namespace,ClassName = ClassName}
				.GeneratePeriods();
		}
	}
}