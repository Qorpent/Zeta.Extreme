#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IQueryDimension.cs
// Project: Zeta.Extreme.Poco
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using Comdiv.Model.Interfaces;

namespace Comdiv.Zeta.Model.ExtremeSupport {
	/// <summary>
	/// 	��������� ����������� ��������� �������� Zeta
	/// </summary>
	/// <typeparam name="TItem"> </typeparam>
	public interface IQueryDimension<TItem> : IWithCacheKey, IZetaQueryDimension
		where TItem : class, IWithCode, IWithId, IWithNewTags {
		/// <summary>
		/// 	����� ����� ��������
		/// </summary>
		string[] Codes { get; set; }

		/// <summary>
		/// 	������ �� �������� ������
		/// </summary>
		TItem Native { get; set; }

		/// <summary>
		/// 	������������� ����� ���������������
		/// </summary>
		int[] Ids { get; set; }

		/// <summary>
		/// 	��� �������
		/// </summary>
		string FormulaType { get; set; }

		/// <summary>
		/// 	��������� ����������� �������� �������
		/// </summary>
		/// <returns> </returns>
		bool IsPrimary();

		/// <summary>
		/// 	����������� ������ ����
		/// </summary>
		/// <param name="session"> </param>
		/// <exception cref="NotImplementedException"></exception>
		void Normalize(ISession session);
		}
}