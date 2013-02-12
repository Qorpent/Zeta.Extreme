#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : IMetaCache.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using Comdiv.Model.Interfaces;

namespace Zeta.Extreme {
	/// <summary>
	/// 	��������� ���������� ��������� ����������
	/// </summary>
	public interface IMetaCache {
		/// <summary>
		/// 	�������� ������ �� ���������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="id"> </param>
		/// <returns> </returns>
		T Get<T>(object id) where T : class, IEntityDataPattern;
		
		/// <summary>
		/// 	��������� ������ � ���������
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="item"> </param>
		/// <returns> </returns>
		IMetaCache Set<T>(T item) where T : class, IEntityDataPattern;
	}
}