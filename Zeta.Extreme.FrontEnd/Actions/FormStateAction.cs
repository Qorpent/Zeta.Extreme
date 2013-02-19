using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// ���������� ������ �����
	/// </summary>
	[Action("zefs.formstate")]
	public class FormStateAction : SessionAttachedActionBase
	{
		/// <summary>
		/// ���������� ������ ����� �� ����������
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			var isopen = _session.Template.IsOpen;
			var state = _session.Template.GetState(_session.Object, null);
			var editable = isopen && state=="0ISOPEN";
			var message =  _session.Template.CanSetState(_session.Object, null, "0ISBLOCK");
			var canblock = state == "0ISOPEN"  && string.IsNullOrWhiteSpace(message);
			return new
				{
					isopen,
					state,
					editable,
					canblock,
					message
				};
		}
	}
}