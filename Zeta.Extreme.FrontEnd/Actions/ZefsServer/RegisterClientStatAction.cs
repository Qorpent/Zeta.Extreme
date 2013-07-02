using Qorpent.IoC;
using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd.Actions.ZefsServer {
	///<summary>
	///	���������� ����������� ������� ����
	///</summary>
	[Action("zefs.registerclientstat",Role = "DEFAULT")]
	public class RegisterClientStatAction : ActionBase {
		
		/// <summary>
		/// ���������� ������������ �������
		/// </summary>
		[Inject]
		public IClientStatisticsRegistry ClientStatisticsRegistry { get; set; }

		/// <summary>
		/// 	���������� ������ ���������� ������� ���� � ��������� ��������
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			if (null != ClientStatisticsRegistry) {
				ClientStatisticsRegistry.RegisterClientStatistics(Data,Context.UserHostAddress+" ("+Context.UserHostName+")",Context.UserAgent) ;
			}
			return true;
		}

		/// <summary>
		/// ��������� ������ ����������� ����
		/// </summary>
		[Bind] public string Data { get; set; }
	}
}