#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : CacheKeyGeneratorBase.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Zeta.Model.ExtremeSupport;

namespace Zeta.Extreme {
	/// <summary>
	/// 	������� ����� ��� �������� � �������, �������� �� ����������� ���-������
	/// </summary>
	public abstract class CacheKeyGeneratorBase : IWithCacheKey {
		/// <summary>
		/// 	���������� ���-������ �������
		/// </summary>
		/// <returns> </returns>
		public string GetCacheKey(bool save = true) {
			if (null != _cacheKey) {
				return _cacheKey;
			}
			if (save) {
				return _cacheKey ?? (_cacheKey = EvalCacheKey());
			}
			return EvalCacheKey();
		}

		/// <summary>
		/// 	���������� ������, ������� ������������ ������� ������.
		/// </summary>
		/// <returns> ������, �������������� ������� ������. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			//������ ������� ��� ������, �� �� ��������� ��!!!
			return GetCacheKey(false);
		}

		/// <summary>
		/// 	���������� ���-������
		/// </summary>
		public virtual void InvalidateCacheKey() {
			_cacheKey = null;
		}

		/// <summary>
		/// 	������� ����������������� ���������� ������� ������
		/// </summary>
		/// <returns> </returns>
		protected abstract string EvalCacheKey();

		private string _cacheKey; //cached value of key
	}
}