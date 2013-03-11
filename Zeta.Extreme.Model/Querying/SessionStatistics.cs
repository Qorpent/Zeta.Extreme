using System;
using Qorpent.Serialization;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ������������� �������������� ������ �� ������
	/// </summary>
	[Serialize]
	public class SessionStatistics {
		
		/// <summary>
		/// 	���������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public int BatchCount;

		/// <summary>
		/// 	���������� ������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public TimeSpan BatchTime;

		/// <summary>
		/// 	���������� �������������� ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int PrimaryAffected;

		/// <summary>
		/// 	���������� ����������� �����
		/// </summary>
		[SerializeNotNullOnly]
		public int PrimaryCatched;

		/// <summary>
		/// 	������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypeFormula;

		/// <summary>
		/// 	������� ��������� ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypePrimary;

		/// <summary>
		/// 	������� ����
		/// </summary>
		[SerializeNotNullOnly]
		public int QueryTypeSum;

		/// <summary>
		/// 	������� ������������ ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryIgnored;

		/// <summary>
		/// 	���������� ������������� ���������� �����������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryNew;

		/// <summary>
		/// 	���������� ������� �������������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryPreprocessed;

		/// <summary>
		/// 	���������� ����������� �� ����������� �����
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByKey;

		/// <summary>
		/// 	���������� ���������� ������������� �������� ��� ��������������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByMapKey;

		/// <summary>
		/// 	���������� ����������� �� ������� � ����
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryResolvedByUid;

		/// <summary>
		/// 	���������� ���������� ������� �����������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryStarted;

		/// <summary>
		/// 	���������� ��������������� �����������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryStartedUser;

		/// <summary>
		/// 	������� �������������� ���������� ��������
		/// </summary>
		[SerializeNotNullOnly]
		public int RegistryUser;

		/// <summary>
		/// 	������� ��������� ������
		/// </summary>
		[SerializeNotNullOnly]
		public int RowRedirections;

	

		/// <summary>
		/// 	���������� ������ ������� ����������
		/// </summary>
		[SerializeNotNullOnly]
		public TimeSpan TimeTotal;
	}
}