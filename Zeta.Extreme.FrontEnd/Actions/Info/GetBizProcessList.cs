using Qorpent.Mvc;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// ������� ������ ������ ��������� ���
	/// </summary>
	[Action("zefs.bizprocesslist")]
	public class  GetBizProcessList : FormServerActionBase {
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess() {
			return new BizProcessEnumerator().GetAllBizProcesses(Context.User);
		}

		/// <summary>
		/// ������ �������� ����� ������������ ������ 304
		/// </summary>
		/// <returns></returns>
		protected override bool GetSupportNotModified()
		{
			return true;
		}

		/// <summary>
		/// Etag �������� � ������������
		/// </summary>
		/// <returns></returns>
		protected override string EvalEtag()
		{
			MyFormServer.MetaCacheLoad.Wait();
			return FormServer.Default.GetUserETag();
		}

		/// <summary>
		/// 	override if Yr action provides 304 state  and return Last-Modified-State header
		/// </summary>
		/// <returns> </returns>
		protected override System.DateTime EvalLastModified()
		{
			MyFormServer.MetaCacheLoad.Wait();
			return FormServer.Default.GetCommonLastModified();
		}
	}
}