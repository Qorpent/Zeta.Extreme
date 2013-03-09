using System.IO;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Специальный поток, буфер, заполняет внутренний буфер памяти данными,
	/// при End вызывает специальный метод, который при перекрытии дает реакцию
	/// применения данных к цели (позволяет в виде потока выразить непоточные хранилища
	/// типа varbinary(max) в SQL)
	/// </summary>
	public abstract class DataCollectionStream : Stream {
		private MemoryStream _internalstream;
		private bool _canread;
	
		/// <summary>
		/// Создает поток с указанными данными (опционально) 
		/// </summary>
		protected DataCollectionStream(byte[] initialData = null) {
			_internalstream = initialData!=null ? new MemoryStream(initialData) : new MemoryStream();
			_canread = initialData != null;
		}

		/// <summary>
		/// При переопределении в производном классе очищает все буферы данного потока и вызывает запись данных буферов в базовое устройство.
		/// </summary>
		/// <exception cref="T:System.IO.IOException">Ошибка ввода-вывода.</exception><filterpriority>2</filterpriority>
		public override void Flush() {
			_flushed = true;
			_internalstream.Flush();
		}

		/// <summary>
		/// При переопределении в производном классе задает позицию в текущем потоке.
		/// </summary>
		/// <returns>
		/// Новая позиция в текущем потоке.
		/// </returns>
		/// <param name="offset">Смещение в байтах относительно параметра <paramref name="origin"/>.</param><param name="origin">Значение типа <see cref="T:System.IO.SeekOrigin"/> определяет точку ссылки, которая используется для получения новой позиции.</param><exception cref="T:System.IO.IOException">Ошибка ввода-вывода.</exception><exception cref="T:System.NotSupportedException">Поток не поддерживает поиск, если поток создан на основе канала или вывода консоли.</exception><exception cref="T:System.ObjectDisposedException">Методы были вызваны после закрытия потока.</exception><filterpriority>1</filterpriority>
		public override long Seek(long offset, SeekOrigin origin) {
			return _internalstream.Seek(offset, origin);
		}

		/// <summary>
		/// При переопределении в производном классе задает длину текущего потока.
		/// </summary>
		/// <param name="value">Необходимая длина текущего потока в байтах.</param><exception cref="T:System.IO.IOException">Ошибка ввода-вывода.</exception><exception cref="T:System.NotSupportedException">Поток не поддерживает ни поиск, ни запись, например, если поток создан на основе канала или вывода консоли.</exception><exception cref="T:System.ObjectDisposedException">Методы были вызваны после закрытия потока.</exception><filterpriority>2</filterpriority>
		public override void SetLength(long value) {
			_internalstream.SetLength(value);
		}

		/// <summary>
		/// При переопределении в производном классе считывает последовательность байтов из текущего потока и перемещает позицию в потоке на число считанных байтов.
		/// </summary>
		/// <returns>
		/// Общее количество байтов, считанных в буфер. Это число может быть меньше количества запрошенных байтов, если столько байтов в настоящее время недоступно, а также равняться нулю (0), если был достигнут конец потока.
		/// </returns>
		/// <param name="buffer">Массив байтов. После завершения выполнения данного метода буфер содержит указанный массив байтов, в котором значения в интервале между <paramref name="offset"/> и (<paramref name="offset"/> + <paramref name="count"/> - 1) заменены байтами, считанными из текущего источника.</param><param name="offset">Смещение байтов (начиная с нуля) в <paramref name="buffer"/>, с которого начинается сохранение данных, считанных из текущего потока.</param><param name="count">Максимальное количество байтов, которое должно быть считано из текущего потока.</param><exception cref="T:System.ArgumentException">Сумма значений параметров <paramref name="offset"/> и <paramref name="count"/> больше длины буфера.</exception><exception cref="T:System.ArgumentNullException">Параметр <paramref name="buffer"/> имеет значение null.</exception><exception cref="T:System.ArgumentOutOfRangeException">Значение параметра <paramref name="offset"/> или <paramref name="count"/> является отрицательным.</exception><exception cref="T:System.IO.IOException">Ошибка ввода-вывода.</exception><exception cref="T:System.NotSupportedException">Поток не поддерживает чтение.</exception><exception cref="T:System.ObjectDisposedException">Методы были вызваны после закрытия потока.</exception><filterpriority>1</filterpriority>
		public override int Read(byte[] buffer, int offset, int count) {
			return _internalstream.Read(buffer, offset, count);
		}

		/// <summary>
		/// При переопределении в производном классе записывает последовательность байтов в текущий поток и перемещает текущую позицию в нем вперед на число записанных байтов.
		/// </summary>
		/// <param name="buffer">Массив байтов. Этот метод копирует байты <paramref name="count"/> из параметра <paramref name="buffer"/> в текущий поток.</param><param name="offset">Смещение байтов (начиная с нуля) в <paramref name="buffer"/>, с которого начинается копирование байтов в текущий поток.</param><param name="count">Количество байтов, которое необходимо записать в текущий поток.</param><filterpriority>1</filterpriority>
		public override void Write(byte[] buffer, int offset, int count) {
			_internalstream.Write(buffer,offset,count);
		}

		/// <summary>
		/// При переопределении в производном классе возвращает значение, показывающее, поддерживает ли текущий поток возможность чтения.
		/// </summary>
		/// <returns>
		/// Значение true, если поток поддерживает чтение; в противном случае — значение false.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override bool CanRead {
			get { return _canread; }
		}

		/// <summary>
		/// При переопределении в производном классе возвращает значение, которое показывает, поддерживается ли в текущем потоке возможность поиска.
		/// </summary>
		/// <returns>
		/// Значение true, если поток поддерживает поиск; в противном случае — значение false.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override bool CanSeek {
			get { return _canread; }
		}

		/// <summary>
		/// При переопределении в производном классе возвращает значение, которое показывает, поддерживает ли текущий поток возможность записи.
		/// </summary>
		/// <returns>
		/// Значение true, если поток поддерживает запись; в противном случае — значение false.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public override bool CanWrite {
			get { return !_canread; }
		}

		/// <summary>
		/// При переопределении в производном классе получает длину потока в байтах.
		/// </summary>
		/// <returns>
		/// Длинное значение, представляющее длину потока в байтах.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">Класс, созданный на основе класса Stream, не поддерживает возможность поиска.</exception><exception cref="T:System.ObjectDisposedException">Методы были вызваны после закрытия потока.</exception><filterpriority>1</filterpriority>
		public override long Length {
			get { return _internalstream.Length; }
		}

		/// <summary>
		/// При переопределении в производном классе получает или задает позицию в текущем потоке.
		/// </summary>
		/// <returns>
		/// Текущее положение в потоке.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">Ошибка ввода-вывода.</exception><exception cref="T:System.NotSupportedException">Этот поток не поддерживает поиск.</exception><exception cref="T:System.ObjectDisposedException">Методы были вызваны после закрытия потока.</exception><filterpriority>1</filterpriority>
		public override long Position {
			get { return _internalstream.Position; }
			set { _internalstream.Position = value; }
		}

		private bool _closewascalled = false;
		private bool _flushed;

		/// <summary>
		/// Закрывает текущий поток и отключает все ресурсы (например, сокеты и файловые дескрипторы), связанные с текущим потоком. Вместо вызова данного метода, убедитесь в том, что поток надлежащим образом ликвидирован.
		/// </summary>
		/// <filterpriority>1</filterpriority>
		public override void Close() {
			base.Close();
			FinishStream();
		}
		/// <summary>
		/// Основной метод для реализации в дочерних классах - применение собрыннх данных к цели
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
		/// Освобождает неуправляемые ресурсы, используемые объектом <see cref="T:System.IO.Stream"/>, а при необходимости освобождает также управляемые ресурсы.
		/// </summary>
		/// <param name="disposing">Значение true позволяет освободить управляемые и неуправляемые ресурсы; значение false позволяет освободить только неуправляемые ресурсы.</param>
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