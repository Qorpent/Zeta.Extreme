using System;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ������������� �������������� ������ �� ������
	/// </summary>
	public class SessionStatistics {
		
		/// <summary>
		/// 	���������� ������
		/// </summary>
		public int Stat_Batch_Count;

		/// <summary>
		/// 	���������� ������� ������
		/// </summary>
		public TimeSpan Stat_Batch_Time;

		/// <summary>
		/// 	���������� �������������� ��������
		/// </summary>
		public int Stat_Primary_Affected;

		/// <summary>
		/// 	���������� ����������� �����
		/// </summary>
		public int Stat_Primary_Catched;

		/// <summary>
		/// 	������� ������
		/// </summary>
		public int Stat_QueryType_Formula;

		/// <summary>
		/// 	������� ��������� ��������
		/// </summary>
		public int Stat_QueryType_Primary;

		/// <summary>
		/// 	������� ����
		/// </summary>
		public int Stat_QueryType_Sum;

		/// <summary>
		/// 	������� ������������ ��������
		/// </summary>
		public int Stat_Registry_Ignored;

		/// <summary>
		/// 	���������� ������������� ���������� �����������
		/// </summary>
		public int Stat_Registry_New;

		/// <summary>
		/// 	���������� ������� �������������
		/// </summary>
		public int Stat_Registry_Preprocessed;

		/// <summary>
		/// 	���������� ����������� �� ����������� �����
		/// </summary>
		public int Stat_Registry_Resolved_By_Key;

		/// <summary>
		/// 	���������� ���������� ������������� �������� ��� ��������������
		/// </summary>
		public int Stat_Registry_Resolved_By_Map_Key;

		/// <summary>
		/// 	���������� ����������� �� ������� � ����
		/// </summary>
		public int Stat_Registry_Resolved_By_Uid;

		/// <summary>
		/// 	���������� ���������� ������� �����������
		/// </summary>
		public int Stat_Registry_Started;

		/// <summary>
		/// 	���������� ��������������� �����������
		/// </summary>
		public int Stat_Registry_Started_User;

		/// <summary>
		/// 	������� �������������� ���������� ��������
		/// </summary>
		public int Stat_Registry_User;

		/// <summary>
		/// 	������� ��������� ������
		/// </summary>
		public int Stat_Row_Redirections;

		/// <summary>
		/// 	���������� ��������� ���-������
		/// </summary>
		public int Stat_SubSession_Count;

		/// <summary>
		/// 	���������� ������ ������� ����������
		/// </summary>
		public TimeSpan Stat_Time_Total;
	}
}