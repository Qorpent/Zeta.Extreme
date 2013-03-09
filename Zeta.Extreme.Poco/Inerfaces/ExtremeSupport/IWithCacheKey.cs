namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// ��������, ���������� ���������� ������������ �������
	/// </summary>
	public interface IWithCacheKey {
		/// <summary>
		/// 	���������� ���-������ �������
		/// </summary>
		/// <returns> </returns>
		string GetCacheKey(bool save = true);

		/// <summary>
		/// 	���������� ���-������
		/// </summary>
		void InvalidateCacheKey();
	}
}