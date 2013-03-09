#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : PrimaryQueryGroup.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

namespace Zeta.Extreme.Primary {
	/// <summary>
	/// 	������ ��������� ��������
	/// </summary>
	public class PrimaryQueryGroup {
		/// <summary>
		/// 	������� � ������
		/// </summary>
		public Query[] Queries { get; set; }

		/// <summary>
		/// 	�������� ���������� �������
		/// </summary>
		public PrimaryQueryPrototype Prototype { get; set; }

		/// <summary>
		/// 	��������� ��������
		/// </summary>
		public IScriptGenerator ScriptGenerator { get; set; }

		/// <summary>
		/// 	������ SQL ������
		/// </summary>
		/// <returns> </returns>
		public string GenerateSqlScript() {
			return ScriptGenerator.Generate(Queries, Prototype);
		}
	}
}