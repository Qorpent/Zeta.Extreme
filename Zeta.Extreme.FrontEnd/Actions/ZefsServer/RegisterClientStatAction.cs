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
				var req = ((MvcContext) Context).NativeAspContext.Request;
				ClientStatisticsRegistry.RegisterClientStatistics(Data,req.UserHostAddress+" ("+req.UserHostName+")",req.UserAgent) ;
			}
			return true;
		}

		/// <summary>
		/// ��������� ������ ����������� ����
		/// </summary>
		[Bind] public string Data { get; set; }
	}
}