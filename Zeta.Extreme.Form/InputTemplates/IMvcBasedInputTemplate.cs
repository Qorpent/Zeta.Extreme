using System;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// ��������� ����� ���������� ��� ����, ��������������� �� ��������� ��������� � MVC
	/// </summary>
	//[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] 
	public interface IMvcBasedInputTemplate {
		/// <summary>
		/// ��������� ��� ��� MVC
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string Controller { get; set; }

		/// <summary>
		/// ������� ������������� �������������� ����
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] bool IsCustomView { get; }

		/// <summary>
		/// ������������� ���������������� ���
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string CustomView { get; set; }

		/// <summary>
		/// ������������� ��� �����������
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")] string CustomControllerType { get; set; }

		/// <summary>
		/// ��� �������
		/// </summary>
		[Obsolete("�� ����� ������ ��������� ��������� � ����� ����")]string TableView { get; set; }
	}
}