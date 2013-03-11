using System;
using System.Threading;

namespace Zeta.Extreme.Model.Inerfaces {
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
				session.GetStatistics().Stat_Batch_Time += timespan;
			}
		}

		/// <summary>
		/// ����������� ���������� "�����������" ��������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryCatched(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref session.GetStatistics().Stat_Primary_Catched);
			}
		}	
		/// <summary>
		/// ����������� ���������� "�����������" ��������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncPrimaryAffected(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().Stat_Primary_Affected);
			}
		}
		/// <summary>
		/// ����������� ���������� ���������� ����������� ��������
		/// </summary>
		/// <param name="session"></param>
		public static void StatIncBatchCount(this ISession session)
		{
			if(session.IsCollectStatistics()) {
				Interlocked.Increment(ref  session.GetStatistics().Stat_Batch_Count);
			}
		}	
	}
}