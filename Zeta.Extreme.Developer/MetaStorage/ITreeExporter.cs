using Zeta.Extreme.Model.Inerfaces;

namespace Zeta.Extreme.Developer.MetaStorage {
	/// <summary>
	/// 
	/// </summary>
	public interface ITreeExporter {
		/// <summary>
		/// ��������� ������� ������ � ������
		/// </summary>
		/// <param name="exportroot"></param>
		/// <param name="rootmode"></param>
		/// <returns></returns>
		string ProcessExport(IZetaRow exportroot, bool rootmode);
	}
}