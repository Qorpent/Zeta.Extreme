using Qorpent.Mvc;

namespace Zeta.Extreme.FrontEnd.Actions {
	/// <summary>
	/// 	��������, ������������ ������ �������� ����������
	/// </summary>
	[Action("zefs.restart")]
	public class FormServerRestart : ActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			FormServer.Default.Reload();
			return true;
		}
	}
}