using System;
using System.Threading;

namespace Zeta.Extreme.Model.Querying {
	/// <summary>
	/// ��������������� ����� ������������ �������� ����� ���������� ������
	/// </summary>
	public static class SessionStatisticsExtensions {
		/// <summary>
		/// ����������� ���������� ������� ���������� SQL
		/// </summary>
		/// <param name="session"></param>
		/// <param name="timespan"></param>
		public static void StatIncStatBatchTime(this ISession session, TimeSpan timespan) {
			if(session.IsCollectStatistics()) {
				session.GetStatistics().BatchTime += timespan;
			}
		}

		/// <summary>
		/// ����������� ���������� "�����������" ��������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryCatched(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref session.GetStatistics().PrimaryCatched);
			}
		}	
		/// <summary>
		/// ����������� ���������� "�����������" ��������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryAffected(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().PrimaryAffected);
			}
		}
		/// <summary>
		/// ����������� ���������� ���������� ����������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncBatchCount(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().BatchCount);
			}
		}

		/// <summary>
		/// ����������� ���������� ���������� ���������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncQueryTypeFormula(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().QueryTypeFormula);
			}
		}

		/// <summary>
		/// ����������� ���������� ���������� ���������  ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncQueryTypePrimary(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().QueryTypePrimary);
			}
		}

		/// <summary>
		/// ����������� ���������� ���������� ��������  ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncQueryTypeSum(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().QueryTypeSum);
			}
		}
		/// <summary>
		/// ����������� ���������� ����������������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryIgnored(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryIgnored);
			}
		}
		/// <summary>
		/// ����������� ���������� ���������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryNew(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryNew);
			}
		}
		/// <summary>
		/// ����������� ���������� ������� �������������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryPreprocessed(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryPreprocessed);
			}
		}
		/// <summary>
		/// ����������� ���������� ��������� �������� �� ����������� ���-�����
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryResolvedByKey(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryResolvedByKey);
			}
		}
		/// <summary>
		/// ����������� ���������� ��������� �������� �� �������� �������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryResolvedByMapKey(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryResolvedByMapKey);
			}
		}

		/// <summary>
		/// ����������� ���������� ��������� �������� �� ���������� �����
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryResolvedByUid(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryResolvedByUid);
			}
		}
		/// <summary>
		/// ����������� ���������� ��������� �� �����������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryStarted(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryStarted);
			}
		}
		/// <summary>
		/// ����������� ���������� ���������������� ��������� �� �����������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryStartedUser(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryStartedUser);
			}
		}

		/// <summary>
		/// ����������� ���������� ���������������� ��������� �� �����������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRegistryUser(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RegistryUser);
			}
		}

		/// <summary>
		/// ���������� ��������� �����
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncRowRedirections(this ISession session)
		{
			if (session.IsCollectStatistics())
			{
				Interlocked.Increment(ref  session.GetStatistics().RowRedirections);
			}
		}

		/// <summary>
		/// ���������� ��������� �����
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncTimeTotal(this ISession session,TimeSpan span)
		{
			if (session.IsCollectStatistics()) {
				session.GetStatistics().TimeTotal += span;
			}
		}



	}
}