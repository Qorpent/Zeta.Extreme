using Qorpent.Utils.Extensions;
using Zeta.Extreme.BizProcess.Forms;
using Zeta.Extreme.Model.Querying;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ������� ������ ���� ��� �������� ���������� �� ���� � ���������
	/// </summary>
	public class FormSessionDataSessionPropertySoruce : ISessionPropertySource {
		private readonly IFormSession _session;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		public FormSessionDataSessionPropertySoruce(IFormSession session) {
			_session = session;
		}

		/// <summary>
		/// ����� ��������� ��������� �� �����
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object Get(string name) {
			if (null == _session.Template) return null;
			if (_session.Template.Parameters.ContainsKey(name)) {
				return _session.Template.Parameters[name].ToStr();
			}
			var result = _session.Template.Thema.GetParameter(name).ToStr();
			return result;
		}
	}
}