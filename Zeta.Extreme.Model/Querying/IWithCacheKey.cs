#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IWithCacheKey.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// 	��������, ���������� ���������� ������������ �������
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