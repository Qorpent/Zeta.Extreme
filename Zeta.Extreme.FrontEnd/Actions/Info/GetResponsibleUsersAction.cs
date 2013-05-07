using System.Linq;
using Qorpent.Mvc;
using Zeta.Extreme.Form.Meta;
using Zeta.Extreme.FrontEnd.Helpers;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.FrontEnd.Actions.Info {
	/// <summary>
	/// ���������� ������ �������������, ������� ����� ��������� ������ �����
	/// </summary>
	[Action("zefs.responsibleusers")]
	public class GetResponsibleUsersAction : FormSessionActionBase {
		private IZetaUser[] _allusers;
		private IZetaUser[] _responsibleUsers;
		private SimpleUserInfo[] _responsibleUsersRecords;

		/// <summary>
		/// �������������� ������ ������������
		/// </summary>
		protected override void Prepare()
		{
			base.Prepare();
			_allusers = UserOrgDataMapper.GetUsersForObject(MySession.Object);
			_responsibleUsers = _allusers.Where(_ => _.Active && Roles.IsInRole(_.Login, MySession.Template.Role)).OrderBy(_=>_.Name).ToArray();
			var helper = new UserInfoHelper();
			_responsibleUsersRecords = _responsibleUsers.Select(_ =>helper.GetUserInfo(_,true)).ToArray();
		}
		/// <summary>
		/// ���������� ������ ���������� �������������
		/// </summary>
		/// <returns></returns>
		protected override object MainProcess() {
			return _responsibleUsersRecords;
		}
	}
}