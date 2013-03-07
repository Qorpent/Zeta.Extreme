using System;
using System.IO;
using Qorpent.Mvc;
using Zeta.Extreme.BizProcess.Forms;

namespace Zeta.Extreme.FrontEnd {
	/// <summary>
	/// Обертка атача в виде дескриптора файла для выгрузки
	/// </summary>
	public class FormAttachmentFileDescriptor : IFileDescriptor {
		private FormAttachment _attach;
		private IFormAttachmentStorage _storage;

		/// <summary>
		/// Создает обертку с использованием 
		/// </summary>
		/// <param name="attach"></param>
		/// <param name="storage"></param>
		public FormAttachmentFileDescriptor(FormAttachment attach, IFormAttachmentStorage storage) {
			_attach = attach;
			_storage = storage;
		}

		/// <summary>
		/// 	Роль (список ролей), которые необходимы для доступа
		/// </summary>
		public string Role {
			get { return ""; }
			set { }
		}

		/// <summary>
		/// 	Имя ресурса возможно с путем
		/// </summary>
		public string Name {
			get {
				var srcname = _attach.Name.Replace("\"","_");
				var ext = Path.GetExtension(srcname);
				if (ext.Length<=1 || ext.Length>4) {
					return (srcname+ "." + _attach.Extension).Replace("..",".");
				}
				return srcname;
			}
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	Содержимое файла
		/// </summary>
		public string Content {
			get { return null; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	Mime-Type файла
		/// </summary>
		public string MimeType {
			get { return _attach.MimeType; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	Длина файла
		/// </summary>
		public int Length {
			get { return (int)_attach.Size; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// 	Время последнего изменения
		/// </summary>
		public DateTime LastWriteTime {
			get { return _attach.Version; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Признак того что надо использовать хидер расположения файла
		/// </summary>
		public bool NeedDisposition {
			get { return true; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Признак потокового файла
		/// </summary>
		public bool IsStream {
			get { return true; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Возвращает поток данных файла
		/// </summary>
		/// <returns></returns>
		public Stream GetStream() {
			return _storage.Open(_attach, FileAccess.Read);
		}
	}
}