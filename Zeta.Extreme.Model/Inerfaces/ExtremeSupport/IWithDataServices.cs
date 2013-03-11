using Zeta.Extreme.Model.Inerfaces;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// ��������������� ��������� ��� ����� ��� ������� � �������� ������
	/// </summary>
	public interface IWithDataServices {
		/// <summary>
		/// 	��������� ��������� ������
		/// </summary>
		IPrimarySource PrimarySource { get; set; }

		/// <summary>
		/// 	��������� ��� ��������� ������
		/// </summary>
		IMetaCache MetaCache { get; set; }

		/// <summary>
		/// ������� ���������� �����, ��������� � ���������� �������
		/// </summary>
		/// <param name="timeout"></param>
		void WaitPrimarySource(int timeout = -1);
	}
}