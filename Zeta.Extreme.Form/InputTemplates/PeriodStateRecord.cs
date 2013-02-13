using System;
using Comdiv.Extensions;

namespace Comdiv.Zeta.Web.InputTemplates {
	/// <summary>
	/// ������ ������� �������
	/// </summary>
	public sealed class PeriodStateRecord
	{
		/// <summary>
		///������� ����������� ������
		/// </summary>
		public PeriodStateRecord()
		{
			DeadLine = DateExtensions.Begin;
		}

		/// <summary>
		/// ���
		/// </summary>
		public int Year;
		/// <summary>
		/// ������
		/// </summary>
		public int Period;
		/// <summary>
		/// ������
		/// </summary>
		public bool State;
		/// <summary>
		/// �������
		/// </summary>
		public DateTime DeadLine;
		/// <summary>
		/// ����� �������
		/// </summary>
		public DateTime UDeadLine;
	}
}