

using Qorpent.Model;

namespace Zeta.Extreme.BizProcess.Reports{
	/// <summary>
	/// ��������� ������������ ������
	/// </summary>
    public interface ISavedReportParameter :IWithId, IWithName, IWithVersion{
		/// <summary>
		/// ������ �� �����
		/// </summary>
        ISavedReport Report { get; set; }
		/// <summary>
		/// ��������
		/// </summary>
        string Value { get; set; }
    }
}