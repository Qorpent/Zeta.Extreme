using System.Collections.Generic;
using System.IO;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.Form.DbfsAttachmentSource {
	/// <summary>
	/// ������� ��������� ��� ��������� ������� � ��������
	/// </summary>
	public abstract class BufferedAttachmentStorageBase : IAttachmentStorage {
		/// <summary>
		/// ���������� ������ ��� ���������� �������� � DBFS � ���� ������
		/// </summary>
		private class BufferedAttachmentStorageStream : DataCollectionStream
		{
			private BufferedAttachmentStorageBase _storage;
			private string _uid;

			/// <summary>
			/// ������� �������� ������ �� ������ ��� ������ ��� ���������� DBFS 
			/// </summary>
			/// <param name="uid">UID ������������ ����� </param>
			/// <param name="data"> ������ (��� �������� �� ������)</param>
			/// <param name="realstorage">������� ��������� Dbfs</param>
			public BufferedAttachmentStorageStream(string uid, byte[] data, BufferedAttachmentStorageBase realstorage)
				: base(data)
			{
				_storage = realstorage;
				_uid = uid;
			}
			/// <summary>
			/// �������� ����� ��� ���������� � �������� ������� - ���������� �������� ������ � ����
			/// </summary>
			/// <param name="data"></param>
			protected override void ProcessData(byte[] data)
			{
				_storage.PerformDataUpdate(_uid, data);
			}
		}
		/// <summary>
		/// ������������ ����� ����������� � ��������� ������ ������
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public abstract IEnumerable<Attachment> Find(Attachment query);

		/// <summary>
		/// ��������� ��������� � ���������
		/// </summary>
		/// <param name="attachment"></param>
		public abstract void Save(Attachment attachment);

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public Stream OpenWrite(Attachment attachment) {
			return new BufferedAttachmentStorageStream(attachment.Uid, null, this);
		}

		/// <summary>
		/// ��������� ����� �� ������ ��������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public Stream OpenRead(Attachment attachment) {
			byte[] data = DoRealLoadData(attachment);
			return new BufferedAttachmentStorageStream(attachment.Uid,data,this);
		}
		/// <summary>
		/// ��������� �������� �������� ������� ������ �� ���������
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		protected abstract byte[] DoRealLoadData(Attachment attachment);

		/// <summary>
		/// ��������� �������� ���������� ������ ������ � ��
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="data"></param>
		protected abstract void PerformDataUpdate(string uid, byte[] data);
	}
}