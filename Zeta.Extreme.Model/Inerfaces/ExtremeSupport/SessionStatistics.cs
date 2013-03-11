using System;

namespace Zeta.Extreme.Model.Inerfaces {
	/// <summary>
	/// ������������� �������������� ������ �� ������
	/// </summary>
	public class SessionStatistics {
		
		/// <summary>
		/// 	���������� ������
		/// </summary>
		public int BatchCount;

		/// <summary>
		/// 	���������� ������� ������
		/// </summary>
		public TimeSpan BatchTime;

		/// <summary>
		/// 	���������� �������������� ��������
		/// </summary>
		public int PrimaryAffected;

		/// <summary>
		/// 	���������� ����������� �����
		/// </summary>
		public int PrimaryCatched;

		/// <summary>
		/// 	������� ������
		/// </summary>
		public int QueryTypeFormula;

		/// <summary>
		/// 	������� ��������� ��������
		/// </summary>
		public int QueryTypePrimary;

		/// <summary>
		/// 	������� ����
		/// </summary>
		public int QueryTypeSum;

		/// <summary>
		/// 	������� ������������ ��������
		/// </summary>
		public int RegistryIgnored;

		/// <summary>
		/// 	���������� ������������� ���������� �����������
		/// </summary>
		public int RegistryNew;

		/// <summary>
		/// 	���������� ������� �������������
		/// </summary>
		public int RegistryPreprocessed;

		/// <summary>
		/// 	���������� ����������� �� ����������� �����
		/// </summary>
		public int RegistryResolvedByKey;

		/// <summary>
		/// 	���������� ���������� ������������� �������� ��� ��������������
		/// </summary>
		public int RegistryResolvedByMapKey;

		/// <summary>
		/// 	���������� ����������� �� ������� � ����
		/// </summary>
		public int RegistryResolvedByUid;

		/// <summary>
		/// 	���������� ���������� ������� �����������
		/// </summary>
		public int RegistryStarted;

		/// <summary>
		/// 	���������� ��������������� �����������
		/// </summary>
		public int RegistryStartedUser;

		/// <summary>
		/// 	������� �������������� ���������� ��������
		/// </summary>
		public int RegistryUser;

		/// <summary>
		/// 	������� ��������� ������
		/// </summary>
		public int RowRedirections;

	

		/// <summary>
		/// 	���������� ������ ������� ����������
		/// </summary>
		public TimeSpan TimeTotal;
	}
}