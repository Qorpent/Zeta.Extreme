namespace Zeta.Extreme.Core {
	/// <summary>
	/// ������� ����� ��� �������� � �������, �������� �� ����������� ���-������
	/// </summary>
	public abstract class CacheKeyGeneratorBase {
		private string _cacheKey; //cached value of key

		/// <summary>
		/// ���������� ���-������ �������
		/// </summary>
		/// <returns></returns>
		public string GetCacheKey(bool save = true) {
			if(null!=_cacheKey) {
				return _cacheKey;
			}
			if(save) {
				return _cacheKey ?? (_cacheKey = EvalCacheKey());
			}
			return EvalCacheKey();
		}

		/// <summary>
		/// ���������� ������, ������� ������������ ������� ������.
		/// </summary>
		/// <returns>
		/// ������, �������������� ������� ������.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			//������ ������� ��� ������, �� �� ��������� ��!!!
			return GetCacheKey(false);
		}

		/// <summary>
		/// ���������� ���-������
		/// </summary>
		public void InvalidateCacheKey() {
			_cacheKey = null;
		}
		/// <summary>
		/// ������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns></returns>
		protected abstract string EvalCacheKey();
	}
}