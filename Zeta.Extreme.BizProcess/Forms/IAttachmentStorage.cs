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
// PROJECT ORIGIN: Zeta.Extreme.BizProcess/IAttachmentStorage.cs
#endregion
using System;
using System.Collections.Generic;
using System.IO;

namespace Zeta.Extreme.BizProcess.Forms {
	/// <summary>
	/// Хранилище присоединенного контента
	/// </summary>
	public interface IAttachmentStorage {
		/// <summary>
		/// Осуществляет поиск аттачментов с указанной маской поиска
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		IEnumerable<Attachment> Find(Attachment query);
		/// <summary>
		/// Сохраняет аттачмент в хранилище
		/// </summary>
		/// <param name="attachment"></param>
		void Save(Attachment attachment);
		/// <summary>
		/// Удаляет атачмент
		/// </summary>
		/// <param name="attachment"></param>
		void Delete(Attachment attachment);
		/// <summary>
		/// Открывает поток на запись контента
		/// </summary>
		/// <param name="attachment"></param>
		/// <param name="mode">режим доступа к файлу </param>
		/// <returns></returns>
		Stream Open(Attachment attachment,FileAccess mode);
	}


}