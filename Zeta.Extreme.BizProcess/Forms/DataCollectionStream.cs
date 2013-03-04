using System.IO;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// ����������� �����, �����, ��������� ���������� ����� ������ �������,
	/// ��� End �������� ����������� �����, ������� ��� ���������� ���� �������
	/// ���������� ������ � ���� (��������� � ���� ������ �������� ���������� ���������
	/// ���� varbinary(max) � SQL)
	/// </summary>
	public abstract class DataCollectionStream : Stream {
		private MemoryStream _internalstream;
		private bool _canread;
	
		/// <summary>
		/// ������� ����� � ���������� ������� (�����������) 
		/// </summary>
		protected DataCollectionStream(byte[] initialData = null) {
			_internalstream = initialData!=null ? new MemoryStream(initialData) : new MemoryStream();
			_canread = initialData != null;
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ������� ��� ������ ������� ������ � �������� ������ ������ ������� � ������� ����������.
		/// </summary>
		/// <exception cref="T:System.IO.IOException">������ �����-������.</exception><filterpriority>2</filterpriority>
		public override void Flush() {
			_flushed = true;
			_internalstream.Flush();
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ������ ������� � ������� ������.
		/// </summary>
		/// <returns>
		/// ����� ������� � ������� ������.
		/// </returns>
		/// <param name="offset">�������� � ������ ������������ ��������� <paramref name="origin"/>.</param><param name="origin">�������� ���� <see cref="T:System.IO.SeekOrigin"/> ���������� ����� ������, ������� ������������ ��� ��������� ����� �������.</param><exception cref="T:System.IO.IOException">������ �����-������.</exception><exception cref="T:System.NotSupportedException">����� �� ������������ �����, ���� ����� ������ �� ������ ������ ��� ������ �������.</exception><exception cref="T:System.ObjectDisposedException">������ ���� ������� ����� �������� ������.</exception><filterpriority>1</filterpriority>
		public override long Seek(long offset, SeekOrigin origin) {
			return _internalstream.Seek(offset, origin);
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ������ ����� �������� ������.
		/// </summary>
		/// <param name="value">����������� ����� �������� ������ � ������.</param><exception cref="T:System.IO.IOException">������ �����-������.</exception><exception cref="T:System.NotSupportedException">����� �� ������������ �� �����, �� ������, ��������, ���� ����� ������ �� ������ ������ ��� ������ �������.</exception><exception cref="T:System.ObjectDisposedException">������ ���� ������� ����� �������� ������.</exception><filterpriority>2</filterpriority>
		public override void SetLength(long value) {
			_internalstream.SetLength(value);
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ��������� ������������������ ������ �� �������� ������ � ���������� ������� � ������ �� ����� ��������� ������.
		/// </summary>
		/// <returns>
		/// ����� ���������� ������, ��������� � �����. ��� ����� ����� ���� ������ ���������� ����������� ������, ���� ������� ������ � ��������� ����� ����������, � ����� ��������� ���� (0), ���� ��� ��������� ����� ������.
		/// </returns>
		/// <param name="buffer">������ ������. ����� ���������� ���������� ������� ������ ����� �������� ��������� ������ ������, � ������� �������� � ��������� ����� <paramref name="offset"/> � (<paramref name="offset"/> + <paramref name="count"/> - 1) �������� �������, ���������� �� �������� ���������.</param><param name="offset">�������� ������ (������� � ����) � <paramref name="buffer"/>, � �������� ���������� ���������� ������, ��������� �� �������� ������.</param><param name="count">������������ ���������� ������, ������� ������ ���� ������� �� �������� ������.</param><exception cref="T:System.ArgumentException">����� �������� ���������� <paramref name="offset"/> � <paramref name="count"/> ������ ����� ������.</exception><exception cref="T:System.ArgumentNullException">�������� <paramref name="buffer"/> ����� �������� null.</exception><exception cref="T:System.ArgumentOutOfRangeException">�������� ��������� <paramref name="offset"/> ��� <paramref name="count"/> �������� �������������.</exception><exception cref="T:System.IO.IOException">������ �����-������.</exception><exception cref="T:System.NotSupportedException">����� �� ������������ ������.</exception><exception cref="T:System.ObjectDisposedException">������ ���� ������� ����� �������� ������.</exception><filterpriority>1</filterpriority>
		public override int Read(byte[] buffer, int offset, int count) {
			return _internalstream.Read(buffer, offset, count);
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ���������� ������������������ ������ � ������� ����� � ���������� ������� ������� � ��� ������ �� ����� ���������� ������.
		/// </summary>
		/// <param name="buffer">������ ������. ���� ����� �������� ����� <paramref name="count"/> �� ��������� <paramref name="buffer"/> � ������� �����.</param><param name="offset">�������� ������ (������� � ����) � <paramref name="buffer"/>, � �������� ���������� ����������� ������ � ������� �����.</param><param name="count">���������� ������, ������� ���������� �������� � ������� �����.</param><filterpriority>1</filterpriority>
		public override void Write(byte[] buffer, int offset, int count) {
			_internalstream.Write(buffer,offset,count);
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ���������� ��������, ������������, ������������ �� ������� ����� ����������� ������.
		/// </summary>
		/// <returns>
		/// �������� true, ���� ����� ������������ ������; � ��������� ������ � �������� false.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override bool CanRead {
			get { return _canread; }
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ���������� ��������, ������� ����������, �������������� �� � ������� ������ ����������� ������.
		/// </summary>
		/// <returns>
		/// �������� true, ���� ����� ������������ �����; � ��������� ������ � �������� false.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override bool CanSeek {
			get { return _canread; }
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ ���������� ��������, ������� ����������, ������������ �� ������� ����� ����������� ������.
		/// </summary>
		/// <returns>
		/// �������� true, ���� ����� ������������ ������; � ��������� ������ � �������� false.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override bool CanWrite {
			get { return !_canread; }
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ �������� ����� ������ � ������.
		/// </summary>
		/// <returns>
		/// ������� ��������, �������������� ����� ������ � ������.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">�����, ��������� �� ������ ������ Stream, �� ������������ ����������� ������.</exception><exception cref="T:System.ObjectDisposedException">������ ���� ������� ����� �������� ������.</exception><filterpriority>1</filterpriority>
		public override long Length {
			get { return _internalstream.Length; }
		}

		/// <summary>
		/// ��� ��������������� � ����������� ������ �������� ��� ������ ������� � ������� ������.
		/// </summary>
		/// <returns>
		/// ������� ��������� � ������.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">������ �����-������.</exception><exception cref="T:System.NotSupportedException">���� ����� �� ������������ �����.</exception><exception cref="T:System.ObjectDisposedException">������ ���� ������� ����� �������� ������.</exception><filterpriority>1</filterpriority>
		public override long Position {
			get { return _internalstream.Position; }
			set { _internalstream.Position = value; }
		}

		private bool _closewascalled = false;
		private bool _flushed;

		/// <summary>
		/// ��������� ������� ����� � ��������� ��� ������� (��������, ������ � �������� �����������), ��������� � ������� �������. ������ ������ ������� ������, ��������� � ���, ��� ����� ���������� ������� ������������.
		/// </summary>
		/// <filterpriority>1</filterpriority>
		public override void Close() {
			base.Close();
			FinishStream();
		}
		/// <summary>
		/// �������� ����� ��� ���������� � �������� ������� - ���������� �������� ������ � ����
		/// </summary>
		/// <param name="data"></param>
		protected abstract void ProcessData(byte[] data);

		private byte[] CollectData() {
			_internalstream.Seek(0, SeekOrigin.Begin);
			var buffer = new byte[_internalstream.Length];
			_internalstream.Read(buffer, 0, buffer.Length);
			return buffer;
		}

		/// <summary>
		/// ����������� ������������� �������, ������������ �������� <see cref="T:System.IO.Stream"/>, � ��� ������������� ����������� ����� ����������� �������.
		/// </summary>
		/// <param name="disposing">�������� true ��������� ���������� ����������� � ������������� �������; �������� false ��������� ���������� ������ ������������� �������.</param>
		protected override void Dispose(bool disposing) {
			//	Close();
			FinishStream();
			base.Dispose(disposing);
		}

		private void FinishStream() {
			if (!_closewascalled && _flushed) {
				_closewascalled = true;
				byte[] data = CollectData();
				ProcessData(data);
			}
		}
	}
}