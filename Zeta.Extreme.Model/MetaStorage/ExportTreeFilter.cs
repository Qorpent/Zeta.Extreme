using Qorpent;
using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Model.MetaStorage {
	/// <summary>
	/// ��������� ���������� ������
	/// </summary>
	public class ExportTreeFilter {
		/// <summary>
		/// ���������� ��������� ���������� ����
		/// </summary>
		public string ExcludeRegex { get; set; }
		/// <summary>
		/// ������ ���� �� �������
		/// </summary>
		public ReplaceDescriptor CodeReplacer { get; set; }

		/// <summary>
		/// ��������� ���������� ������
		/// </summary>
		/// <param name="exportroot"></param>
		/// <returns></returns>
		public IZetaRow Execute(IZetaRow exportroot){
			throw new System.NotImplementedException();
		}
	}
}