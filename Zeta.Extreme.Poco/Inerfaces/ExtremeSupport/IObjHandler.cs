using Comdiv.Olap.Model;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ����������� ��������� ��������� ������� Obj
	/// </summary>
	public interface IObjHandler : IQueryDimension<IZoneElement>
	{
		/// <summary>
		/// 	��� ����
		/// </summary>
		ObjType Type { get; set; }

		/// <summary>
		/// ����� ������ � �������� �� ������ ��������� ��������, �� ��������� NONE - ����� �������� �� ��������
		/// </summary>
		DetailMode DetailMode { get; set; }

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� � �����������
		/// </summary>
		bool IsForObj { get; }

		/// <summary>
		/// 	������ ��� ������� �������� ��� ���� ���� �� � �����������
		/// </summary>
		bool IsNotForObj { get; }

		/// <summary>
		/// 	������ �� �������� ��������� �������� �������
		/// </summary>
		IZetaMainObject ObjRef { get; }

		/// <summary>
		/// 	������� ����� ����
		/// </summary>
		/// <returns> </returns>
		IObjHandler Copy();
	}
}