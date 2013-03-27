#region LICENSE
// Copyright 2007-2013 Qorpent Team - http://github.com/Qorpent
// Supported by Media Technology LTD 
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/BufferedAttachmentStorageBase.cs
#endregion
using System;
using System.Collections.Generic;
using System.IO;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Базовое хранилище для буферного доступа к контенту
	/// </summary>
	public abstract class BufferedAttachmentStorageBase : IAttachmentStorage {
		/// <summary>
		/// Реализация потока для обрамления запросов к DBFS в виде буфера
		/// </summary>
		private class BufferedAttachmentStorageStream : DataCollectionStream
		{
			private BufferedAttachmentStorageBase _storage;
			private string _uid;

			/// <summary>
			/// Создает акцептор потока на чтение или запись для контейнера DBFS 
			/// </summary>
			/// <param name="uid">UID сохраняемого файла </param>
			/// <param name="data"> данные (при открытии на чтение)</param>
			/// <param name="realstorage">целевое хранилище Dbfs</param>
			public BufferedAttachmentStorageStream(string uid, byte[] data, BufferedAttachmentStorageBase realstorage)
				: base(data)
			{
				_storage = realstorage;
				_uid = uid;
			}
			/// <summary>
			/// Основной метод для реализации в дочерних классах - применение собрыннх данных к цели
			/// </summary>
			/// <param name="data"></param>
			protected override void ProcessData(byte[] data)
			{
				_storage.PerformDataUpdate(_uid, data);
			}
		}
		/// <summary>
		/// Осуществляет поиск аттачментов с указанной маской поиска
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public abstract IEnumerable<Attachment> Find(Attachment query);

		/// <summary>
		/// Сохраняет аттачмент в хранилище
		/// </summary>
		/// <param name="attachment"></param>
		public abstract void Save(Attachment attachment);

		/// <summary>
		/// Удаляет атачмент
		/// </summary>
		/// <param name="attachment"></param>
		public abstract void Delete(Attachment attachment);

		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <param name="mode">режим доступа к файлу </param>
		/// <returns></returns>
		public Stream Open(Attachment attachment, FileAccess mode) {
			if(mode==FileAccess.Read) return OpenRead(attachment);
			if(mode==FileAccess.Write) return OpenWrite(attachment);
			throw new Exception("not supported accesss mode "+mode);
		}

		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		protected Stream OpenWrite(Attachment attachment) {
			return new BufferedAttachmentStorageStream(attachment.Uid, null, this);
		}

		/// <summary>
		/// Открывает поток на чтение контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		protected Stream OpenRead(Attachment attachment) {
			byte[] data = DoRealLoadData(attachment);
			return new BufferedAttachmentStorageStream(attachment.Uid,data,this);
		}
		/// <summary>
		/// Выполняет реальную загрузку массива байтов из хранилища
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		protected abstract byte[] DoRealLoadData(Attachment attachment);

		/// <summary>
		/// Выполняет реальное сохранение потока данных в БД
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="data"></param>
		protected abstract void PerformDataUpdate(string uid, byte[] data);
	}
}