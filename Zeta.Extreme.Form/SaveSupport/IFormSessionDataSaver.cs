using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zeta.Extreme.Form;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// ��������� ������ ��� ���������� ������ ����
	/// </summary>
	public interface IFormSessionDataSaver {
		/// <summary>
		/// ����� 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="savedata"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		Task<SaveResult> BeginSave(IFormSession session, XElement savedata);

		/// <summary>
		/// ������� ������ �������� ����������
		/// </summary>
		SaveStage Stage { get; set; }

		/// <summary>
		/// ��������� ��������� ������
		/// </summary>
		Exception Error { get; set; }
	}
}