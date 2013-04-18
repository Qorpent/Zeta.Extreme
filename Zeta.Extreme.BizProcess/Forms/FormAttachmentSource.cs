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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/FormAttachmentSource.cs
#endregion
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qorpent.IoC;
using Zeta.Extreme.Model.MetaCaches;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	///Служба для работы с аттачами формы
	/// </summary>
	public class FormAttachmentSource : IFormAttachmentStorage {
		private IAttachmentStorage _storage;

        /// <summary>
        /// 
        /// </summary>
        [Inject(Name = "attachment.source")]
		public IAttachmentStorage InternalStorage  {
            get { return _storage; }
            set { _storage = value; }
        }

		/// <summary>
		/// Устанавливает службу хранения
		/// </summary>
		/// <param name="storage"></param>
		public void SetStorage(IAttachmentStorage storage) {
			_storage = storage;
		}

		/// <summary>
		/// Найти все присоедиеннные файлы, реферирующиеся с данной сессией
		/// </summary>
		/// <returns></returns>
		public IEnumerable<FormAttachment> GetAttachments(IFormSession session) {
			var allperiods = Periods.Eval(session.Year, session.Period, -11).Periods;
			foreach (var period in allperiods) {
				var query = new FormAttachment(session, null, AttachedFileType.Default, false);
				query.Period = period;
				var subresult = _storage.Find(query).Select(_ => new FormAttachment(session, _, AttachedFileType.Default));
				foreach (var attachment in subresult) {
					yield return attachment;
				}
			}
			
		}

		/// <summary>
		/// Сохраняет аттачмент, связанный с текущей сессией
		/// </summary>
		/// <param name="session">целевая сессия </param>
		/// <param name="attachment">присоединенный контент</param>
		public FormAttachment SaveAttachment(IFormSession session, Attachment attachment) {
			var realattach = new FormAttachment(session, attachment, AttachedFileType.Default, false) {User = session.Usr};
			_storage.Save(realattach);
			return realattach;
		}

		/// <summary>
		/// Удаляет присоединенный элемент
		/// </summary>
		/// <param name="attachment"></param>
		public void Delete(FormAttachment attachment) {
			_storage.Delete(attachment);
		}

		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <param name="mode"> </param>
		/// <returns></returns>
		public Stream Open(FormAttachment attachment,FileAccess mode) {
			return _storage.Open(attachment,mode);
		}

		
	}
}