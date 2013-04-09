using System;
using System.Collections.Generic;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ��������� ���� �����
	/// </summary>
	public interface IFormChatProvider {
		/// <summary>
		/// ����� ��������� � ������� ��������� ����
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		IEnumerable<FormChatItem> GetSessionItems(IFormSession session);
		/// <summary>
		/// ���������� ������ ���������
		/// </summary>
		/// <param name="session"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		FormChatItem AddMessage(IFormSession session, string message);

		/// <summary>
		/// �������� ��������� � ��������� ��������������� ��� ���������� �������������
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="user"></param>
		void Archive(string uid, string user);

		/// <summary>
		/// �������� ��������� � ��������� ��������������� ��� ���������� �������������
		/// </summary>
		/// <param name="user"></param>
		void SetHaveRead(string user);

		/// <summary>
		/// ���������� ���� ��������� ������� � ���������
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		DateTime GetLastRead(string user);

		/// <summary>
		///     ��������� ������� ��������� � ���� ���������
		/// </summary>
		/// <param name="user"></param>
		/// <returns>
		/// </returns>
		long GetUpdatesCount(string user);

		/// <summary>
		/// </summary>
		/// <param name="user"></param>
		/// <param name="startdate"></param>
		/// <param name="objids"></param>
		/// <param name="types"></param>
		/// <param name="includeArchived"></param>
		/// <returns>
		/// </returns>
		IEnumerable<FormChatItem> FindAll(string user, DateTime startdate, int[] objids, string[] types,
		                                                  bool includeArchived);
	}
}