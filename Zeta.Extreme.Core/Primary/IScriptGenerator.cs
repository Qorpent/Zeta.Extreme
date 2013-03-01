#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : IScriptGenerator.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	��������� ���������� ��������
	/// </summary>
	public interface IScriptGenerator {
		/// <summary>
		/// 	������ SQL ������ � ������ ���������, � �� ���� "������" �������
		/// </summary>
		/// <param name="queries"> </param>
		/// <param name="prototype"> </param>
		/// <returns> </returns>
		string Generate(Query[] queries, PrimaryQueryPrototype prototype);
	}
}