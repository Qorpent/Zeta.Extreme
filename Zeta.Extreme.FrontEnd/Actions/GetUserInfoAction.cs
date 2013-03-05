using Qorpent.Mvc;
using Qorpent.Mvc.Binding;
using Zeta.Extreme.FrontEnd.Helpers;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// 	���������� ������ ��������� ��������
	/// </summary>
	[Action("zefs.getuserinfo")]
	public class GetUserInfoAction : FormServerActionBase
	{
		/// <summary>
		/// 	processing of execution - main method of action
		/// </summary>
		/// <returns> </returns>
		protected override object MainProcess()
		{
			MyFormServer.MetaCacheLoad.Wait();
			return new UserInfoHelper().GetUserInfo(login);
		}

		/// <summary>
		/// �����, �� �������� ������������� ������ ������������
		/// </summary>
		[Bind(Required = true, ValidatePattern = @"^[\w\d]+\\[\w\d]+$")] protected string login;
	}
}