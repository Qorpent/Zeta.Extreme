using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.Form.Themas {
	/// <summary>
	/// ��������� ���������� ���� ��� Extreme
	/// </summary>
	public interface IExtremeFormProvider {
		/// <summary>
		/// �������������� ������������ �������
		/// </summary>
		void Reload();

		/// <summary>
		/// �������� ������ �� ����
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		IInputTemplate Get(string code);
	}
}